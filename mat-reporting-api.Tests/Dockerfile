FROM microsoft/dotnet:2.2-sdk

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./MaTReportingAPI.sln ./
COPY ./mat-reporting-api/MaTReportingAPI.csproj ./mat-reporting-api/
COPY ./mat-reporting-api.Tests/MaTReportingAPI.Tests.csproj ./mat-reporting-api.Tests/

RUN dotnet restore ./mat-reporting-api/MaTReportingAPI.csproj
RUN dotnet restore ./mat-reporting-api.Tests/MaTReportingAPI.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out mat-reporting-api.Tests/MaTReportingAPI.Tests.csproj

CMD dotnet test