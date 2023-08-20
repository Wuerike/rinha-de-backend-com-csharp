using Microsoft.AspNetCore.Mvc;

namespace RinhaBackend.Routes.GetPersonsCount;

internal static class Endpoint
{
    internal static IEndpointRouteBuilder GetPersonsCount(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/contagem-pessoas", Handler);

        return endpoints;
    }

    private static async Task<IResult> Handler(
        [FromServices] GetPersonsCountRespository repo
    )
    {
        var result = await repo.GetPersonsCountASync();
        return Results.Ok(result);
    }
}
