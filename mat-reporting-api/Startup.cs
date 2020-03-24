using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MaTReportingAPI.UseCase.V1;
using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Gateways;
using MaTReportingAPI.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using FluentValidation.AspNetCore;
using FluentValidation;
using MaTReportingAPI.V1.Validators;
using System.Net.Http.Headers;
using MaTReportingAPI.V1.Infrastructure;

namespace MaTReportingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static List<ApiVersionDescription> _apiVersions { get; set; }

        private const string ApiName = "MaT Reporting API";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddFluentValidation()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //suppress filter so custom responses can be used
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //add validators
            services.AddTransient<IValidator<ListInteractionsRequest>, ListInteractionsRequestValidator>();
            services.AddTransient<IValidator<ListETRAMeetingsRequest>, ListETRAMeetingsRequestValidator>();
            services.AddTransient<IValidator<ListInteractionsAndChildInteractionsRequest>, ListInteractionsAndChildInteractionsRequestValidator>();

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the url segment header)
            });
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Token", Enumerable.Empty<string>()}
                });

                //Looks at the APIVersionAttribute [ApiVersion("x")] on controllers and decides whether or not
                //to include it in that version of the swagger document
                //Controllers must have this [ApiVersion("x")] to be included in swagger documentation!!
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions).ToList();

                    var any = versions.Any(v => $"{v.GetFormattedApiVersion()}" == docName);
                    return any;
                });

                //Get every ApiVersion attribute specified and create swagger docs for them
                foreach (var apiVersion in _apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new Info
                    {
                        Title = $"{ApiName}-api {version}",
                        Version = version,
                        Description = $"{ApiName} version {version}. Please check older versions for depreceted endpoints."
                    });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            ConfigureDbContext(services);
            RegisterGateWays(services);
            RegisterUseCases(services);

            //use HttpClientFactory for more efficient http client management

            //client for accessing CRM token service
            services.AddHttpClient<ICRMTokenGateway, CRMTokenGateway>(client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("GetCRM365AccessTokenURL"));
            });

            //client for generic CRM access
            services.AddHttpClient<ICRMGateway, CRMGateway>(client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CRMAPIBaseURL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");
            });

            //client for accessing MaT process API
            services.AddHttpClient<IMaTProcessAPIGateway, MaTProcessAPIGateway>(client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MaTProcessAPIURL"));
            });
        }

        private static void RegisterGateWays(IServiceCollection services)
        {
            services.AddSingleton<ICRMTokenGateway, CRMTokenGateway>();
            services.AddSingleton<ICRMGateway, CRMGateway>();
            services.AddSingleton<IETRAMeetingsGateway, ETRAMeetingsGateway>();
            services.AddSingleton<IInteractionsGateway, InteractionsGateway>();
            services.AddSingleton<IProcessDataGateway, ProcessDataGateway>();
        }

        private static void RegisterUseCases(IServiceCollection services)
        {
            services.AddSingleton<IListETRAMeetings, ListETRAMeetingsUseCase>();
            services.AddSingleton<IListInteractions, ListInteractionsUseCase>();
            services.AddSingleton<IListInteractionsAndChildInteractions, ListInteractionsAndChildInteractionsUseCase>();
        }

        private static void ConfigureDbContext(IServiceCollection services)
        {
            services.Configure<ConnectionSettings>(options =>
            {
                options.ConnectionString
                    = Environment.GetEnvironmentVariable("DocumentDbConnString");
                options.Database
                    = Environment.GetEnvironmentVariable("DatabaseName");
                options.CollectionName
                    = Environment.GetEnvironmentVariable("CollectionName");
            });

            services.AddSingleton<IProcessDbContext, ProcessDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //Get All ApiVersions,
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            //Get All ApiVersions,
            _apiVersions = api.ApiVersionDescriptions.Select(s => s).ToList();
            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in _apiVersions)
                {
                    //Create a swagger endpoint for each swagger version
                    c.SwaggerEndpoint($"{apiVersionDescription.GetFormattedApiVersion()}/swagger.json",
                        $"{ApiName}-api {apiVersionDescription.GetFormattedApiVersion()}");
                }
            });

            app.UseSwagger();

            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
