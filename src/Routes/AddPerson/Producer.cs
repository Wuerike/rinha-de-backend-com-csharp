using System.Threading.Channels;
using RinhaBackend.Entities;

namespace RinhaBackend.Routes.AddPerson;

public class AddPersonProducer
{
    private readonly ChannelWriter<Person> _channel;

    public AddPersonProducer(Channel<Person> channel)
    {
        _channel = channel.Writer;
    }

    public async Task PublishAsync(Person person)
    {
        await _channel.WriteAsync(person);
        return;
    }
}
