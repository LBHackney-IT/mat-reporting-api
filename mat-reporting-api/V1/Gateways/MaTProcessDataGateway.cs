using Dapper;
using MaTReportingAPI.V1.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MaTReportingAPI.V1.Gateways
{
    public class MaTProcessDataGateway : IMaTProcessDataGateway
    {
        public JObject GetHomeCheckAnswersByInteractionIDs(List<string> interactionIDs)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();

            try
            {
                var connectionString = Environment.GetEnvironmentVariable("MaTProcessDbConnectionString");

                List<Guid> queryResults = new List<Guid>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    queryResults = connection.Query<Guid>(
                        "SELECT [OutSystemsSession].[TenencyManagementProcessId]" +
                        "FROM [SchemaData]" +
                        "INNER JOIN [SchemaDataValues] ON [SchemaData].[SchemaDataValueId] = [SchemaDataValues].[SchemaDataValueId]" +
                        "AND [SchemaData].[BitValue] = 1" +
                        "AND [SchemaDataValues].[TaskPageId] = 1130" +
                        "INNER JOIN [OutSystemsSession] ON [OutSystemsSession].[OutSystemsSessionId] = [SchemaDataValues].[OutSystemsSessionId]").ToList();
                }

                foreach (var e in interactionIDs)
                {
                    results.Add(e, queryResults.Contains(new Guid(e)) ? "Yes" : "no");
                }
            }
            catch
            {
                throw new MaTProcessDbException("Unable to get home check details from MaT Process database");
            }

            return JObject.FromObject(results);
        }
    }
}
