// spell-checker: ignore npgsql
using FosterRoster.Components;
using FosterRoster.Components.Account;
using FosterRoster.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<FosterRosterDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddSingleton<TimeProvider, TexasTimeProvider>();
builder.Services.AddValidatorsFromAssemblyContaining<Feline>();
builder.Services.AddValidatorsFromAssemblyContaining<App>();

builder.Services.AddScoped<ICommentRepository, ServerCommentRepository>();
builder.Services.AddScoped<IFelineRepository, ServerFelineRepository>();
builder.Services.AddScoped<IFostererRepository, ServerFostererRepository>();
builder.Services.AddScoped<ISourceRepository, ServerSourceRepository>();
builder.Services.AddScoped<IWeightRepository, ServerWeightRepository>();

builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
    {
        options.Name = "RadzenTheme";
        options.Duration = TimeSpan.FromDays(30);
    });

builder.Services.AddOutputCache();
builder.Services.AddResponseCaching();

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
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.UseResponseCaching();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();