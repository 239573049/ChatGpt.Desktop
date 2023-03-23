namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatGpt(this IServiceCollection services)
    {
        services.AddMasaBlazor();
        return services;
    }
}