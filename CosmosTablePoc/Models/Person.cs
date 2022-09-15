using Azure;
using Azure.Data.Tables;

namespace CosmosTablePoc.Models
{
    public record Person : ITableEntity
    {
        public string RowKey { get; set; } = default!;

        public string PartitionKey { get; set; } = default!;

        public string Discriminator { get; set; } = default!;

        public string Name { get; set; } = default!;

        public DateTime Dob { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = default!;

        public ETag ETag { get; set; } = default!;
    }
}
