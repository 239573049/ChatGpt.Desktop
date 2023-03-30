using System.Net;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json;
using Masa.Blazor.Presets;
using System.Diagnostics;

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

    public async Task CreateStream(string url, object value)
    {
        var message = await HttpClient.PostAsJsonAsync(url, value);
        message.Content.ReadAsStreamAsync();
    }

    public async Task CreateChatGptClient(string url, object value, Action<string?> result, Action<bool> complete)
    {
        var response = await HttpRequestRaw(url, value);

        var resultAsString = string.Empty;
        await using var stream = await response.Content.ReadAsStreamAsync();
        using StreamReader reader = new(stream);
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            resultAsString += line + Environment.NewLine;

            if (line.StartsWith("data:"))
                line = line.Substring("data:".Length);

            line = line.TrimStart();

            if (line == "[DONE]")
            {
                break;
            }
            else if (line.StartsWith(":"))
            {
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                var res = JsonSerializer.Deserialize<GetChatGPTDto>(line, new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                });
                result.Invoke(res?.choices[0].delta?.content);
            }
        }
        complete.Invoke(true);
    }

    public async Task<HttpResponseMessage> HttpRequestRaw(string url, object postData = null, bool streaming = true)
    {
        HttpResponseMessage response = null;
        string resultAsString = null;
        HttpRequestMessage req = new(HttpMethod.Post, url);

        if (postData != null)
        {
            if (postData is HttpContent)
            {
                req.Content = postData as HttpContent;
            }
            else
            {
                string jsonContent = JsonSerializer.Serialize(postData, new JsonSerializerOptions
                {
                    IgnoreNullValues = true
                });
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                req.Content = stringContent;
            }
        }
        response = await HttpClient.SendAsync(req, streaming ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }
        else
        {
            try
            {
                resultAsString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                resultAsString = "Additionally, the following error was thrown when attemping to read the response content: " + e;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException("OpenAI rejected your authorization, most likely due to an invalid API Key.  Try checking your API Key and see https://github.com/OkGoDoIt/OpenAI-API-dotnet#authentication for guidance.  Full API response follows: " + resultAsString);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException("OpenAI had an internal server error, which can happen occasionally.  Please retry your request. " + resultAsString);
            }
            else
            {
                throw new HttpRequestException(resultAsString);
            }
        }
    }

    public async Task<DALLEMoDto> CreateDALLEClient(Uri url, object value)
    {
        throw new NotImplementedException();
    }
}
