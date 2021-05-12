using System;
using System.Threading.Tasks;
using discordVoicePlayer.Player;

namespace discordVoicePlayer
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var discordVoice = new DiscordVoice("token", true);
            var code = await discordVoice.CreateInvite("channelid",
                DiscordVoiceMode.YoutubeMode);
            
            Console.WriteLine($"Created: {code}");
            Console.ReadKey();
        }
    }
}