using System.Collections.Concurrent;
using StackExchange.Redis;
using System.Text.Json;
using RinhaBackend.Entities;

namespace RinhaBackend.Routes.AddPerson;

public class ExternalConsumer: BackgroundService
{
    private readonly ConcurrentDictionary<string, Person> _inMemoryDb;
    private readonly IConnectionMultiplexer _multiplexer;

    public ExternalConsumer(
        ConcurrentDictionary<string, Person> inMemoryDb,
        IConnectionMultiplexer multiplexer)
    {
        _inMemoryDb = inMemoryDb;
        _multiplexer = multiplexer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _multiplexer.GetSubscriber().SubscribeAsync("rinha", async (channel, message) =>
        {
            _inMemoryDb.TryAdd(message, JsonSerializer.Deserialize<Person>(message));
        });
    }
}