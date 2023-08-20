using RinhaBackend.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace RinhaBackend.Routes.SearchPerson;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder SearchPerson(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/pessoas", Handler);

        return endpoints;
    }

    private static async Task<IResult> Handler(
        string t,
        [FromServices] ConcurrentDictionary<string, Person> inMemoryDb
    )
    {
        var result = inMemoryDb
            .Where(p => p.Key.Contains(t))
            .Take(50)
            .Select(p => p.Value)
            .ToList();
        return Results.Ok(result);
    }
}
