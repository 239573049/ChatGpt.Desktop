using Microsoft.Extensions.DependencyInjection;

namespace ChatGPT;

public static class MainApp
{
    private static IServiceProvider ServiceProvider;

    public static ServiceCollection CreateServiceCollection()
    {
        return new ServiceCollection();
    }

    public static IServiceProvider Build(this IServiceCollection services)
    {
        return ServiceProvider = services.BuildServiceProvider();
    }
    
    public static T GetService<T>()
    {
        if (ServiceProvider is null)
        {
            throw new ArgumentNullException(nameof(ServiceProvider));
        }
        return ServiceProvider.GetService<T>();
    }
    
    public static IEnumerable<T> GetServices<T>()
    {
        if (ServiceProvider is null)
        {
            throw new ArgumentNullException(nameof(ServiceProvider));
        }
        return ServiceProvider.GetServices<T>();
    }

    public static object? GetService(Type type)
    {
        if (ServiceProvider is null)
        {
            throw new ArgumentNullException(nameof(ServiceProvider));
        }
        return ServiceProvider.GetService(type);
    }
}