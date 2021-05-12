namespace discordVoicePlayer.Player
{
    
    /**
     * <summary>Class <c>DiscordVoiceMode</c> provides all the possible VoiceModes.</summary>>
     */
    public static class DiscordVoiceMode
    {

        public static string YoutubeMode => "755600276941176913";
        public static string BetrayIo => "773336526917861400";
        public static string FishingtonIo => "814288819477020702";
        public static string PokerNight => "755827207812677713";

        /**
         * <summary>Method <c>Contains</c> checks if any available VoiceMode is included.</summary>
         */
        internal static bool Contains(string voiceMode)
        {
            return FishingtonIo.Equals(voiceMode)
                   || BetrayIo.Equals(voiceMode)
                   || YoutubeMode.Equals(voiceMode)
                   || PokerNight.Equals(voiceMode);
        }
        
    }
}