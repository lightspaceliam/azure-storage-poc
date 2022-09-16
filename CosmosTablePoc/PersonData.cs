using CosmosTablePoc.Models;

namespace CosmosTablePoc
{
    public static class PersonData
    {
        public static List<Person> People = new List<Person> {
            new Person
            {
                RowKey = "pete.mitchell@topgun.com.au",
                PartitionKey = "Mitchell",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1964-01-01"), DateTimeKind.Utc),
                Name = "Pete (Maverick) Mitchell",
                Discriminator = "Pilot"
            },
            new Person
            {
                RowKey = "tom.kazansky@topgun.com.au",
                PartitionKey = "Kazansky",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1961-01-01"), DateTimeKind.Utc),
                Name = "Tom (Iceman) Kazansky",
                Discriminator = "Admiral"
            },
            new Person
            {
                RowKey = "bradley.bradshaw@topgun.com.au",
                PartitionKey = "Bradley",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1991-01-01"), DateTimeKind.Utc),
                Name = "Bradley (Rooster) Bradshaw",
                Discriminator = "Pilot"
            },
            new Person
            {
                RowKey = "jake.seresin@topgun.com.au",
                PartitionKey = "Seresin",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1990-01-01"), DateTimeKind.Utc),
                Name = "Jake (Hangman) Seresin",
                Discriminator = "Pilot"
            },
            new Person
            {
                RowKey = "beau.simpson@topgun.com.au",
                PartitionKey = "Simpson",
                Dob = DateTime.SpecifyKind(DateTime.Parse("1980-01-01"), DateTimeKind.Utc),
                Name = "Beau (Cyclone) Simpson",
                Discriminator = "Air Boss"
            },
        };
    }
}
