// spell-checker: ignore npgsql
using FosterRoster.Client.Components;
using FosterRoster.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.VisibleStateDuration = 2500;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


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


var app = builder.Build();

// Apply database migrations when requested by configuration.
if (builder.Configuration.GetValue("AutoMigrate", false))
{
    using var scope = app.Services.CreateScope();
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FosterRosterDbContext>>();
    await using var context = await factory.CreateDbContextAsync();
    await context.Database.MigrateAsync();
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
