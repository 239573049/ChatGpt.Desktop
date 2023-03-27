using System.Text.Json;

namespace ChatGpt.Shared.Interop;

public class StorageJsInterop : JSModule
{
    public StorageJsInterop(IJSRuntime js) : base(js, "./_content/ChatGpt.Shared/js/storage-JsInterop.js")
    {
    }

    public async ValueTask SetValue<T>(string key, T value) where T : class
    {
        await SetValue(key, JsonSerializer.Serialize(value));
    }

    public async ValueTask SetValue(string key, string value)
    {
        await InvokeVoidAsync("setValue", key, value);
    }

    public async ValueTask<T?> GetValue<T>(string key) where T : class
    {
        var value = await GetValue(key);
        return string.IsNullOrEmpty(value) ? null : JsonSerializer.Deserialize<T>(value);
    }
    public async ValueTask<string> GetValue(string key)
    {
        return await InvokeAsync<string>("getValue", key);
    }

    public async ValueTask RemoveValue(string key)
    {
        await InvokeVoidAsync("removeValue", key);
    }

    public async ValueTask Clear()
    {
        await InvokeVoidAsync("clear");
    }
}