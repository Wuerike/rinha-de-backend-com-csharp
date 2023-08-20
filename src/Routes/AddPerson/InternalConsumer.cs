using System.Threading.Channels;
using RinhaBackend.Entities;

namespace RinhaBackend.Routes.AddPerson;

public class InternalConsumer : BackgroundService
{
    private readonly AddPersonRespository _repo;
    private readonly ChannelReader<Person> _channel;

    public InternalConsumer(
        AddPersonRespository repo,
        Channel<Person> channel
    )
    {
        _repo = repo;
        _channel = channel.Reader;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);

            try
            {
                var persons = await _channel.ReadBatchAsync(1000, cancellationToken);
                await _repo.AddPersonBatchASync(persons);
            }
            catch (Exception)
            {
                Console.WriteLine("Xabu no channel consumer");
            }

        }
    }
}
