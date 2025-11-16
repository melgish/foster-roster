namespace FosterRoster.Infrastructure;

interface IRepository;

public static class RepositoryExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRepositoriesFromAssemblyContaining<T>()
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
}
