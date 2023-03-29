using System.Net;
using System.Net.Http.Json;

namespace ChatGpt.Shared;

public class ApiClient
{
    private readonly HttpClient HttpClient;

    public ApiClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public void SetToken(string token)
    {
        // httpclient是否存在token存在即删除
        if (HttpClient.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
        {
            HttpClient.DefaultRequestHeaders.Remove("Authorization");
        }
        // 添加token
        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public async Task<DALLEMoDto> CreateDALLEClient(string url, object value)
    {
        var message = await HttpClient.PostAsJsonAsync(url, value);

        // 如果是401可能是token设置有问题，或者token失效
        if (message.StatusCode == HttpStatusCode.Unauthorized)
        {
            // 权限不足
            throw new Exception(message: "token可能错误或者无法使用");
        }
        else if (message.IsSuccessStatusCode)
        {
            // 发送成功解析结构
            return await message.Content.ReadFromJsonAsync<DALLEMoDto>();
        }
        else
        {
            throw new Exception(message: $"发送严重错误：" + await message.Content.ReadAsStringAsync());
        }
    }

    public async Task<GetChatGPTDto> CreateChatGptClient(string url, object value)
    {
        var message = await HttpClient.PostAsJsonAsync(url, value);

        // 如果是401可能是token设置有问题，或者token失效
        if (message.StatusCode == HttpStatusCode.Unauthorized)
        {
            // 权限不足
            throw new Exception(message: "token可能错误或者无法使用");
        }
        else if (message.IsSuccessStatusCode)
        {
            // 发送成功解析结构
            return await message.Content.ReadFromJsonAsync<GetChatGPTDto>();
        }
        else
        {
            throw new Exception(message: $"发送严重错误：" + await message.Content.ReadAsStringAsync());
        }
    }

    public async Task<DALLEMoDto> CreateDALLEClient(Uri url, object value)
    {
        throw new NotImplementedException();
    }

    public async Task<GetChatGPTDto> CreateChatGptClient(Uri url, object value)
    {
        throw new NotImplementedException();
    }
}
