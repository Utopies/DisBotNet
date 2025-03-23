using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace DBotSharp.Commands;

public class ModCommands : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("mute", "Отправить пользователя в Time-Out")]
    [RequireUserPermission(GuildPermission.MuteMembers)]
    public async Task Mute(
        [Summary("User", "Какого юзера надо надо замьютить")] SocketGuildUser user, 
        [Summary("Time", "Сколько время человек будет наказан(в минутах)")] int t = 20, 
        [Summary("Reason", "Причина наказания")] string reason = "[ - ]")
    {
        var answer = new EmbedBuilder()
            .WithDescription($"{user.Mention} is muted")
            .AddField("Moderator: ", $"{Context.User.Mention}", true)
            .AddField("Time: ", $"{t.ToString()} mins", true)
            .AddField("Reason: ", $"{reason}", true)
            .WithColor(Color.Red);
        
        await user.SetTimeOutAsync(TimeSpan.FromMinutes(t));
        await RespondAsync(embed: answer.Build());
    }

    [SlashCommand("unmute", "Снять Time-Out с юзера")]
    [RequireUserPermission(GuildPermission.MuteMembers)]
    public async Task Unmute(
        [Summary("User", "Кому вернуть голос")] SocketGuildUser user,
        [Summary("Reason")] string reason = "[ - ]")
    {
        var answer = new EmbedBuilder()
            .WithDescription($"{user.Mention} is unmuted.")
            .AddField("Moderator: ", $"{Context.User.Mention}", true)
            .AddField("Reason: ", $"{reason}", true)
            .WithColor(new Color(237,223,178));
        
        await user.RemoveTimeOutAsync();
        await RespondAsync(embed: answer.Build());
    }

    [SlashCommand("nometa", "ссылка на Nometa")]
    public async Task Nometa()
    {
        var answer = new EmbedBuilder()
            .WithTitle("Don't use NOMETA!")
            .AddField("EN", 
                "NOMETA questions are introductory prompts like \"Is anyone here familiar with...\". " +
                "When someone responds, they have to wait for your actual question \n**\\- THIS IS BAD** :bangbang: " +
                "\n\n\\- Please respect others' time and patience 🙏", 
                inline: false)
            .AddField("RU", 
                "Nometa вопросы - это вводные вопросы, типа \"Есть ли кто-то, кто разбирается в...\". " +
                "Когда кто-то откликается на такой вопрос, ему приходится ждать, пока вы сформулируете основной вопрос \n**\\- ЭТО ПЛОХО** :bangbang:" +
                "\n\n\\- Цените чужое время и нервы 🙏", 
                inline: false)
            .WithColor(new Color(237,223,178));
        
        await RespondAsync(embed: answer.Build());
    }

    [SlashCommand("clear", "очистить чат")]
    [RequireUserPermission(GuildPermission.ManageMessages)]
    public async Task Clear( [Summary("count" )] int count = 1)
    {
        var messages = await Context.Channel.GetMessagesAsync(count).FlattenAsync();
        foreach (var msg in messages)
            await msg.DeleteAsync();
        
        await RespondAsync($"{Context.User.Mention} deleted {count} messages.");
    }
}