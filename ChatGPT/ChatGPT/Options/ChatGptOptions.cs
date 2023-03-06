using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatGPT.Helper;

namespace ChatGPT.Options;

public class ChatGptOptions
{
    public string Token { get; set; }

    /// <summary>
    /// ChatGpt 3.5 api
    /// 官方地址 http://api.openai.com/v1/chat/completions
    /// </summary>
    public string Gpt35ApiUrl = "http://server.tokengo.top:1800/v1/chat/completions";
    
    public async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(this);

        var directory = Path.Combine(AppContext.BaseDirectory, "Config");

        // 如果目录不存在则创建
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        // 将加密后的字节数组转换为字符串
        await using var fileStream =
            File.Create(Path.Combine(directory, "ChatGptOptions.json"));

        await fileStream.WriteAsync(Encoding.UTF8.GetBytes(DESHelper.Encrypt(json)));
    }

    /// <summary>
    /// 获取ChatGPT配置
    /// </summary>
    /// <returns></returns>
    public static ChatGptOptions? NewChatGptOptions()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Config", "ChatGptOptions.json");
        try
        {
            // 如果文件不存在则返回默认值
            if (!File.Exists(path))
            {
                return new ChatGptOptions();
            }

            // 读取加密后的字符串
            var encryptedString = File.ReadAllText(path);

            // 将解密后的字符串反序列化为对象
            return JsonSerializer.Deserialize<ChatGptOptions>(DESHelper.Decrypt(encryptedString));
        }
        catch (Exception e)
        {
            // 如果出现异常则删除文件并返回默认值
            File.Delete(path);
            return new ChatGptOptions();
        }
    }
}