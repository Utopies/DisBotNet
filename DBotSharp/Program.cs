using Discord;
using Discord.WebSocket;

namespace DBotSharp;

class Program
{
    private static DiscordSocketClient _client = new DiscordSocketClient();
    private static String _token = "";

    public static async Task Main()
    {
        await RegToken();
        await  _client.LoginAsync(TokenType.Bot, _token);
        
        _client.Log += Loger;
        
        await _client.StartAsync();
        await Task.Delay(-1);
    }
    
    private static Task Loger(LogMessage msg)
    {
        EnsureLogFileExists();
        
        File.AppendAllText("Logs.txt", $"[{DateTime.Now}] - {msg.ToString()}");
        Console.WriteLine($"[{DateTime.Now}] - {msg.ToString()}");      
        
        return Task.CompletedTask;
    }
    
    private static void EnsureLogFileExists()
    {
        if (File.Exists("Logs.txt")) return;
        using (File.CreateText("Logs.txt"));
    } 
    
    private static async Task RegToken()
        => _token = (await ReadLinesFileAsync("token.txt").FirstOrDefaultAsync(_token))?.Trim() ?? ErrorRegToken();

    private static string ErrorRegToken()
    {
        Console.WriteLine("Token is not found");
        return "Token is not found";
    }
    
    private static  IAsyncEnumerable<string?> ReadLinesFileAsync(string path)
        => File.ReadLinesAsync(path);

}