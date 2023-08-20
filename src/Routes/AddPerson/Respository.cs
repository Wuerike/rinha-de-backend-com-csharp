using RinhaBackend.Entities;
using RinhaBackend.Infra;

namespace RinhaBackend.Routes.AddPerson;

public record AddPersonRespository
{
    private readonly MongoDbClient _mongo;

    public AddPersonRespository(MongoDbClient mongo)
    {
        _mongo = mongo;
    }

    public async Task AddPersonBatchASync(IEnumerable<Person> p)
    {
        await _mongo
            .GetCollection<Person>(nameof(Person))
            .InsertManyAsync(p);
    }
}