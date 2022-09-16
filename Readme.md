# Azure Storage POC

## Azure Cosmos Table Storage

```
CosmosTablePoc
    CosmosTableServices
        Abstract
            ICosmosTableEntityService.cs
            CosmosTableEntityService.cs
        PersonCosmosTableEntityService.cs
    Models
        Person.cs
    PersonWorker.cs
    Program.cs
```

CosmosTableServices is designed to simulate a generic decoupled service that could be consumed by multiple subscribers.

### Setup

Client secrets configuration is required. Assuming you are using Visual Studio 19 or higher:

1. Clone code base from: https://github.com/lightspaceliam/azure-storage-poc.git
2. Right click the CosmosTablePoc and select `Manage User Secrets`
3. Add:
```json
{
  "AzureCosmosStorage": {
    "ConnectionString": "<YOUR-AZURE-COSMOS-TABLE-STORAGE-CONNECTION-STRING>"
  }
}
```
4. Build
5. Run

### Use Cases

I want to perform the following data operation over the Person TableEntity.

- Create
- Read
- Update
- Delete
- Read a range of records TODO

### References

- [Quickstart: Azure Cosmos DB Table API for .NET](https://docs.microsoft.com/en-us/azure/cosmos-db/table/create-table-dotnet?tabs=azure-portal%2Cwindows)
- [Retrieving large numbers of entities from a query](https://docs.microsoft.com/en-us/azure/storage/tables/table-storage-design-patterns#retrieving-large-numbers-of-entities-from-a-query)
- [Table design patterns](https://docs.microsoft.com/en-us/azure/storage/tables/table-storage-design-patterns#retrieving-large-numbers-of-entities-from-a-query)
- https://microsoft.github.io/AzureTipsAndTricks/blog/tip86.html