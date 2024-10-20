using FluentValidation;
using FosterRoster.Domain;
using FosterRoster.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.VisibleStateDuration = 2500;
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ICommentRepository, ClientCommentRepository>();
builder.Services.AddScoped<IFelineRepository, ClientFelineRepository>();
builder.Services.AddScoped<ISourceRepository, ClientSourceRepository>();
builder.Services.AddScoped<IWeightRepository, ClientWeightRepository>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddValidatorsFromAssemblyContaining<Feline>();

await builder.Build().RunAsync();
