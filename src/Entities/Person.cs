namespace RinhaBackend.Entities;

public record Person(
    Guid Id,
    string Apelido,
    string Nome,
    DateOnly Nascimento,
    IEnumerable<string>? Stack
);
