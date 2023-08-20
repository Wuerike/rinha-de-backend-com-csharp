using RinhaBackend.Entities;
using RinhaBackend.Extensions;

namespace RinhaBackend.Routes.AddPerson;

public record AddPersonRequest
{
    public string? Apelido { get; set; }

    public string? Nome { get; set; }

    public DateOnly Nascimento { get; set; }

    public IEnumerable<string>? Stack { get; set; } = Enumerable.Empty<string>();



    public Person? ToPerson()
    {
        if (string.IsNullOrEmpty(Nome) || Nome.Length > 100 || string.IsNullOrEmpty(Apelido) || Apelido.Length > 32)
            return default;

        foreach (var s in Stack ?? Enumerable.Empty<string>())
            if (s.Length > 32 || s.Length == 0)
                return default;

        return new Person(
            Id: Apelido!.ToGuidV5(),
            Apelido: Apelido!,
            Nome: Nome!,
            Nascimento: Nascimento,
            Stack: Stack
        );
    }
}
