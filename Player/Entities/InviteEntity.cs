using Newtonsoft.Json;

namespace discordVoicePlayer.Player.Entities
{
    public class InviteEntity
    {
        [JsonProperty("max_age")]
        public int MaxAge { get; internal set; }
        
        [JsonProperty("max_uses")]
        public int MaxUses { get; internal set; }
        
        [JsonProperty("temporary")]
        public bool Temporary { get; internal set; }
        
        [JsonProperty("target_type")]
        public int TargetType { get; internal set; }
        
        [JsonProperty("target_application_id")]
        public string ApplicationId { get; internal set; }
    }
}