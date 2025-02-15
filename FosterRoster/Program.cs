// spell-checker: ignore npgsql

using FosterRoster.Components;
using FosterRoster.Services;
using Radzen;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<FosterRosterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);


builder.Services.AddScoped<ICommentRepository, ServerCommentRepository>();
builder.Services.AddScoped<IFelineRepository, ServerFelineRepository>();
builder.Services.AddScoped<IFostererRepository, ServerFostererRepository>();
builder.Services.AddScoped<ISourceRepository, ServerSourceRepository>();
builder.Services.AddScoped<IWeightRepository, ServerWeightRepository>();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddValidatorsFromAssemblyContaining<Feline>();
builder.Services.AddValidatorsFromAssemblyContaining<App>();

builder.Services
    .AddRadzenComponents()
    .AddRadzenCookieThemeService(options =>
    {
        options.Name = "FosterRosterTheme";
        options.Duration = TimeSpan.FromDays(30);
    });

var app = builder.Build();

// Apply database migrations when requested by configuration.
if (builder.Configuration.GetValue("AutoMigrate", false))
{
    using var scope = app.Services.CreateScope();
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FosterRosterDbContext>>();
    await using var context = await factory.CreateDbContextAsync();
    string[] migrations = [.. await context.Database.GetPendingMigrationsAsync()];
    if (migrations.Length > 0) await context.Database.MigrateAsync();
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

app.UseAntiforgery();
app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();