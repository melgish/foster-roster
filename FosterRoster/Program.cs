// spell-checker: ignore npgsql
using FluentValidation;

using FosterRoster.Client.Components;
using FosterRoster.Data;
using FosterRoster.Domain;
using FosterRoster.Services;

using Microsoft.EntityFrameworkCore;

using MudBlazor.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddDbContextFactory<FosterRosterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<IFelineRepository, ServerFelineRepository>();
builder.Services.AddScoped<IWeightRepository, ServerWeightRepository>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddValidatorsFromAssemblyContaining<FelineEditModelValidator>();


var app = builder.Build();

if (app.Environment.IsDevelopment() || builder.Configuration.GetValue("AutoMigrate", false))
{
    // always apply database migrations in development mode.
    using var scope = app.Services.CreateScope();
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FosterRosterDbContext>>();
    using var context = contextFactory.CreateDbContext();
    context.Database.MigrateAsync().Wait();
    FosterRosterDbContextSeedData.SeedAsync(context).Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode();

await app.RunAsync();
