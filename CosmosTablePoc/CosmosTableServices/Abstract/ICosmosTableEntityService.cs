using Azure;
using System.Linq.Expressions;

namespace CosmosTablePoc.CosmosTableServices.Abstract
{
    public interface ICosmosTableEntityService<T>
    {
        Task<int?> CreateAsync(T tableEntity);
        Pageable<T>? Find(Expression<Func<T, bool>> filter);
        Task<int?> UpdateAsync(T tableEntity);
        Task<int?> DeleteAsync(string rowKey, string partitionKey);
    }
}
