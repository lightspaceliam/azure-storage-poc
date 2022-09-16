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
            //  Delete existing data.
            foreach (var person in PersonData.People
                    .OrderByDescending(p => p.Dob)
                    .ToList())
            {
                await _service.DeleteAsync(person.RowKey, person.PartitionKey);
            }

            var maverick = PersonData.People
                .First(p => p.RowKey == "pete.mitchell@topgun.com.au" && p.PartitionKey == "Mitchell");

            //  Read
            var searchResult = _service.Find(p => p.RowKey == maverick.RowKey && p.PartitionKey == maverick.PartitionKey);

            // Create

            if (searchResult == null || !searchResult.Any())
            {
                var statusCode = await _service.CreateAsync(maverick);

                _logger.LogInformation($"Person create operation HTTP Status Code: {statusCode}.");
            }
            else
            {
                _logger.LogInformation($"Person {maverick.Name} already exists.");
            }

            //  Delete
            var deleteStatusCode = await _service.DeleteAsync("doesnt.exist@topgun.com.au", "Mitchell");
            _logger.LogInformation($"Person delete operation HTTP Status Code: {deleteStatusCode}.");

            //  Update
            if (searchResult != null && searchResult.Any())
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

                await _service.DeleteAsync(maverick.RowKey, maverick.PartitionKey);

                //  Bulk add
                foreach (var person in PersonData.People
                    .OrderByDescending(p => p.Dob)
                    .ToList())
                {
                    await _service.CreateAsync(person);
                }

                var from = DateTime.SpecifyKind(DateTime.Parse("1964-01-01"), DateTimeKind.Utc);
                var to = DateTime.SpecifyKind(DateTime.Parse("1990-01-01"), DateTimeKind.Utc);

                _logger.LogInformation($"Get a range of People, filtering by date range from: {from.ToString("yyyy-MM-dd")} to: {to.ToString("yyyy-MM-dd")}  and descriminator type: Pilot.");

                
                var pilots = _service.Find(p => (p.Dob >= from && p.Dob <= to) && p.Discriminator == "Pilot");

                foreach(var pilot in pilots)
                {
                    _logger.LogInformation($"Pilot: {pilot.Name}, Dob: {pilot.Dob}");
                }
            }
        }
    }
}
