namespace FosterRoster.Infrastructure;


interface IRepository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoriesFromAssemblyContaining<T>(this IServiceCollection services)
    {
        var types = typeof(T)
            .Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IRepository)));

        foreach (var type in types)
        {
            services.AddScoped(type);
        }

        return services;
    }
}
