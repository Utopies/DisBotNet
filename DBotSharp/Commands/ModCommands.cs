using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace DBotSharp.Commands;

public class ModCommands : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("mute", "Отправить пользователя в Time-Out")]
    [RequireUserPermission(GuildPermission.MuteMembers)]
    public async Task Mute(
        [Summary("Юзер", "Какого юзера надо надо замьютить")] SocketGuildUser user, 
        [Summary("Время", "Сколько время человек будет наказан(в минутах)")] int t = 30, 
        [Summary("Причина")] string reason = "не указана")
    {
        var answer = new EmbedBuilder()
            .WithDescription($"{user.Mention} был замьючен.")
            .AddField("Модератор: ", $"{Context.User.Mention}", true)
            .AddField("Длительность: ", $"на {t.ToString()} минут", true)
            .AddField("Причина: ", $"{reason}", true)
            .WithColor(Color.Red);
        
        await user.SetTimeOutAsync(TimeSpan.FromMinutes(t));
        await RespondAsync(embed: answer.Build());
    }

    [SlashCommand("unmute", "Снять Time-Out с юзера")]
    [RequireUserPermission(GuildPermission.MuteMembers)]
    public async Task Unmute(
        [Summary("Юзер", "Кому вернуть голос")] SocketGuildUser user,
        [Summary("Причина")] string reason = "не указана")
    {
        var answer = new EmbedBuilder()
            .WithDescription($"{user.Mention} был размьючен модерацией.")
            .AddField("Модератор: ", $"{Context.User.Mention}", true)
            .AddField("Причина: ", $"{reason}", true)
            .WithColor(new Color(237,223,178));
        
        await user.RemoveTimeOutAsync();
        await RespondAsync(embed: answer.Build());
    }
}