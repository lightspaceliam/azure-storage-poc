using CosmosTablePoc.CosmosTableServices.Abstract;
using CosmosTablePoc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CosmosTablePoc
{
    public class PersonWorker
    {
        protected readonly ILogger<PersonWorker> _logger;
        protected readonly IConfiguration _configuration;
        private readonly ICosmosTableEntityService<Person> _service = default!;

        public PersonWorker(
            ICosmosTableEntityService<Person> service,
            ILogger<PersonWorker> logger,
            IConfiguration configuration)
        {
            _service = service;
            _logger = logger;
            _configuration = configuration;
        }

        internal async Task RunAsync()
        {
            var person = new Person
            {
                RowKey = "pete.mitchell@topgun.com.au",
                PartitionKey = "Mitchell",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1974-01-01"), DateTimeKind.Utc),
                Name = "Pete (Maverick) Mitchell",
                Discriminator = "Pilot"
            };

            //  Read
            var searchResult = _service.Find(p => p.RowKey == person.RowKey && p.PartitionKey == person.PartitionKey);

            // Create

            if (searchResult == null || !searchResult.Any())
            {
                var statusCode = await _service.CreateAsync(person);
                
                _logger.LogInformation($"Person create operation HTTP Status Code: {statusCode}.");
            } 
            else
            {
                _logger.LogInformation($"Person {person.Name} already exists.");
            }

            //  Delete
            var deleteStatusCode = await _service.DeleteAsync("doesnt.exist@topgun.com.au", "Mitchell");
            _logger.LogInformation($"Person delete operation HTTP Status Code: {deleteStatusCode}.");

            //  Update
            if(searchResult != null && searchResult.Any())
            {
                var personEntry = searchResult.First();

                var updatedPerson = new Person
                {
                    RowKey = personEntry.RowKey,
                    PartitionKey = personEntry.PartitionKey,
                    Dob = personEntry.Dob,
                    Name = personEntry.Name,
                    Discriminator = "Ace",
                    Timestamp = personEntry.Timestamp,
                    ETag = personEntry.ETag,
                };

                var updateStatus = await _service.UpdateAsync(updatedPerson);

                _logger.LogInformation($"Person: {updatedPerson.Name} Type: {personEntry.Discriminator} has been updated to: {updatedPerson.Discriminator}. HTTPS Status: {updateStatus}");
            }
        }
    }
}
