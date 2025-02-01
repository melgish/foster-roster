using FosterRoster.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ICommentRepository, ClientCommentRepository>();
builder.Services.AddScoped<IFelineRepository, ClientFelineRepository>();
builder.Services.AddScoped<IFostererRepository, ClientFostererRepository>();
builder.Services.AddScoped<ISourceRepository, ClientSourceRepository>();
builder.Services.AddScoped<IWeightRepository, ClientWeightRepository>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddValidatorsFromAssemblyContaining<Feline>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientFelineRepository>();

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();