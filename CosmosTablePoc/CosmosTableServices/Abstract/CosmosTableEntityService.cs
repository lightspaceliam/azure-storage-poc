using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CosmosTablePoc.CosmosTableServices.Abstract
{
    public abstract class CosmosTableEntityService<T> : ICosmosTableEntityService<T>
        where T : class, ITableEntity, new()
    {
        protected readonly ILogger<CosmosTableEntityService<T>> Logger;
        private readonly IConfiguration _configuration;
        protected TableClient TableClient = default!;

        public CosmosTableEntityService(
            ILogger<CosmosTableEntityService<T>> logger,
            IConfiguration configuration) 
        { 
            Logger = logger;
            _configuration = configuration;
        }

        protected void CreateTableClient(string tableName)
        {
            TableServiceClient tableServiceClient = new TableServiceClient(_configuration["AzureCosmosStorage:ConnectionString"]);
            TableClient = tableServiceClient.GetTableClient(tableName: tableName);

            TableClient.CreateIfNotExists();
        }

        public virtual async Task<int?> CreateAsync(T tableEntity)
        {
            var response = await TableClient.AddEntityAsync<T>(tableEntity);

            return response?.Status;
        }

        public virtual Pageable<T>? Find(Expression<Func<T, bool>> filter)
        {   
            var tableEntities = TableClient.Query<T>(filter);

            return tableEntities;
        }

        public virtual async Task<int?> UpdateAsync(T tableEntity)
        {
            var response = await TableClient.UpdateEntityAsync<T>(tableEntity, ETag.All, TableUpdateMode.Merge);

            return response?.Status;
        }

        public virtual async Task<int?> DeleteAsync(string rowKey, string partitionKey)
        {
            try
            {
                var response = await TableClient.DeleteEntityAsync(partitionKey, rowKey);
                return response?.Status;
            }
            catch(Exception ex)
            {
                Logger.LogError("Something went wrong.", ex);
                throw;
            }
        }
    }
}
