using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Infra;
using StackExchange.Redis;
using System.Text.Json;

namespace RinhaBackend.Routes.AddPerson;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder AddPerson(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/pessoas", Handler);

        return endpoints;
    }

    private static async Task Handler(
        HttpContext http,
        [FromBody] AddPersonRequest person,
        [FromServices] AddPersonProducer producer,
        [FromServices] IConnectionMultiplexer cache
    )
    {
        http.Response.StatusCode = 422;

        var p = person.ToPerson();
        if (p == null) return;

        var result = await cache.GetRecordAsync(p.Id.ToString());
        if (result != null) return;

        await producer.PublishAsync(p);

        await cache.SetRecordAsync(
            p.Id.ToString(), 
            JsonSerializer.Serialize(p)
        );

        http.Response.StatusCode = 201;
        http.Response.Headers.Location = $"/pessoas/{p.Id}";
        return;
    }
}
