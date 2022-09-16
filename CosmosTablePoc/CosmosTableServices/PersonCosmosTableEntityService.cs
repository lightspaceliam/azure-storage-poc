using CosmosTablePoc.CosmosTableServices.Abstract;
using CosmosTablePoc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CosmosTablePoc.CosmosTableServices
{
    public class PersonCosmosTableEntityService : CosmosTableEntityService<Person>
    {
        public PersonCosmosTableEntityService(
            ILogger<CosmosTableEntityService<Person>> logger,
            IConfiguration configuration) : base(logger, configuration)
        {
            CreateTableClient("people");
        }
    }
}
