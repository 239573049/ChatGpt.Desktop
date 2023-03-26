using ChatGpt.Shared.Interop;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatGpt(this IServiceCollection services)
    {
        services.AddMasaBlazor();
        services.AddScoped<StorageJsInterop>();
        services.AddScoped<ChatGptJsInterop>();
        return services;
    }
}