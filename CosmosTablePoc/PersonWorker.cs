using Azure.Data.Tables;
using CosmosTablePoc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net;

namespace CosmosTablePoc
{
    public class PersonWorker
    {
        protected readonly ILogger<PersonWorker> _logger;
        protected readonly IConfiguration _configuration;

        public PersonWorker(
            ILogger<PersonWorker> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        internal async Task RunAsync()
        {
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration["AzureCosmosStorage:ConnectionString"]);
            TableClient tableClient = tableServiceClient.GetTableClient(tableName: "people");
            await tableClient.CreateIfNotExistsAsync();

            var person = new Person
            {
                RowKey = "pete.mitchell@a.com.au",
                PartitionKey = "Mitchell",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1974-01-01"), DateTimeKind.Utc),
                Name = "Pete (Maverick) Mitchell",
                Discriminator = "Pilot"
            };

            //  This seems to fail silently!!!
            //var entry = await tableClient.GetEntityAsync<Person>(
            //    rowKey: person.RowKey,
            //    partitionKey: person.PartitionKey);
            var search = tableClient.Query<Person>(p => p.RowKey == person.RowKey).FirstOrDefault();

            if (search == null)
            {
                var response = await tableClient.AddEntityAsync<Person>(person);
                if(response.Status == (int)HttpStatusCode.NoContent)
                {
                    _logger.LogInformation($"Person successfully created: {response.Status}.");
                    return;
                }
                _logger.LogInformation($"Person successfully created: {response.Status}.");
            }
        }
    }
}
