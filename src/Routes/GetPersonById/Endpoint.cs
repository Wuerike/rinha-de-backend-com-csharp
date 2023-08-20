using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Infra;
using StackExchange.Redis;

namespace RinhaBackend.Routes.GetPersonById;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder GetPersonById(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/pessoas/{id}", Handler);

        return endpoints;
    }

    private static async Task<string?> Handler(
        Guid id,
        HttpContext http,
        [FromServices] IConnectionMultiplexer cache
    )
    {
        var result = await cache.GetRecordAsync(id.ToString());

        http.Response.StatusCode = 404;
        if (result == null) return "Not Found";

        http.Response.StatusCode = 200;
        return result;
    }
}
