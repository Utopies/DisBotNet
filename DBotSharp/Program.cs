using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace DBotSharp;

class Program
{
    private static DiscordSocketClient _client;
    private static InteractionService _interactions;
    private static String _token = "";
    
    public static async Task Main()
    {
        await InitFields();
        
        await RegToken();
        await  _client.LoginAsync(TokenType.Bot, _token);
        
        _client.Log += Loger;
        _client.Ready += OnReady;
        _client.InteractionCreated += HandleInteraction;
        
        
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private static async Task InitFields()
    {
         _client = new (SoketConfig());
         _interactions = new (_client);
         await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), null);
    }
    
    private static async Task HandleInteraction(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(_client, interaction); 
        await _interactions.ExecuteCommandAsync(context, null);
    }
    
    private static Task Loger(LogMessage msg)
    {
        string log = $"{DateTime.Now:[yy.MM.dd] [H:mm:ss]} - [{msg.Severity.ToString()}] {msg.Message}{Environment.NewLine}";
        
        Console.Write(log);
        File.AppendAllTextAsync("Logs.txt", log);
        
        return Task.CompletedTask;
    }
    
    private static async Task OnReady()
        => await _interactions.RegisterCommandsToGuildAsync(1144352940044324865);
    
    private static async Task RegToken()
        => _token = (await ReadLinesFileAsync("token.txt").FirstOrDefaultAsync(_token))?.Trim() ?? ErrorRegToken();

    private static string ErrorRegToken()
    {
        Console.WriteLine("Token is not found" +
                          "Press any key to exit.");
        Console.ReadKey();
        Environment.Exit(1);
        return "Token is not found";
    }
    
    private static  IAsyncEnumerable<string?> ReadLinesFileAsync(string path)
        => File.ReadLinesAsync(path);
    
    
    private static DiscordSocketConfig SoketConfig()
        => new ()
        {
            GatewayIntents = GatewayIntents.All,
            AlwaysDownloadUsers = true
        };
}