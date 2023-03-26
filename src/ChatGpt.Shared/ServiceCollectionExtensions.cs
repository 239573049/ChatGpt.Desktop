using ChatGpt.Shared.Interop;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatGpt(this IServiceCollection services)
    {
        services.AddMasaBlazor();
        services.AddScoped<StorageJsInterop>();
        services.AddScoped<ChatGptJsInterop>();
        services.AddScoped((_) =>
        {
            var message = new HttpClientHandler();
            message.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
            return new HttpClient(message);
        });
        return services;
    }
}