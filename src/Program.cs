using System.Collections.Concurrent;
using System.Threading.Channels;
using JorgeSerrano.Json;
using Microsoft.AspNetCore.Http.Json;
using RinhaBackend.Entities;
using RinhaBackend.Infra;
using RinhaBackend.Routes.AddPerson;
using RinhaBackend.Routes.GetPersonById;
using RinhaBackend.Routes.GetPersonsCount;
using RinhaBackend.Routes.SearchPerson;
using RinhaBackend.Settings;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Build Settings
builder.Services.AddRouting();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);
builder.Services.Configure<JsonOptions>(opt =>
{
    opt.SerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
});


builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis") ?? "",
        t => {
            t.ConnectTimeout = 60000;
            t.AbortOnConnectFail = false;
        }
    )
);

// Backgroud Service
builder.Services.AddHostedService<ExternalConsumer>();
builder.Services.AddHostedService<InternalConsumer>();
builder.Services.AddSingleton(_ => new ConcurrentDictionary<string, Person>());
builder.Services.AddSingleton(Channel.CreateUnbounded<Person>(new UnboundedChannelOptions { SingleReader = true}));

// Services
builder.Services.AddSingleton<MongoDbClient>();
builder.Services.AddSingleton<AddPersonProducer>();
builder.Services.AddSingleton<AddPersonRespository>();
builder.Services.AddSingleton<GetPersonsCountRespository>();

// Environment Settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(MongoDbSettings.MongoDbSection));

// App Build
var app = builder.Build();

// App Settings
app.UseRouting();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints
        .AddPerson()
        .GetPersonById()
        .GetPersonsCount()
        .SearchPerson();
});
#pragma warning restore ASP0014

app.Run();
