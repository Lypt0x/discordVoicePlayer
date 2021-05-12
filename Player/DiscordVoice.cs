using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using discordVoicePlayer.Player.Entities;
using discordVoicePlayer.Player.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace discordVoicePlayer.Player
{
    public class DiscordVoice
    {

        private readonly HttpClient _httpClient = new();

        public DiscordVoice(string token, bool bot)
        {
            var requestHeaders = this._httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Authorization", bot ? $"Bot {token}" : token);
        }

        public async Task<string> CreateInvite(string channelSnowflake, string voiceMode)
        {
            if (!(DiscordVoiceMode.Contains(voiceMode)) || !(await this.IsChannelVoice(channelSnowflake)))
                return "is not a voice channel or voiceMode not valid";
            
            var inviteEntity = CreateInviteEntity(0, 0, 2, false, voiceMode);
            var stringContent = new StringContent(JsonConvert.SerializeObject(inviteEntity),
                Encoding.UTF8, "application/json");
                
            var response = await this._httpClient.PostAsync($"{DiscordEndpoint.EndpointUri}" +
                                                            $"{DiscordEndpoint.EndpointChannels}/" +
                                                            $"{channelSnowflake}" +
                                                            $"{DiscordEndpoint.EndpointInvites}", stringContent);
                
            if (!(this.IsResponseValid(response.StatusCode)))
                return $"invalid - status: {response.StatusCode}";
             
            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            return !(jObject.ContainsKey("code")) ? "could not get code" : jObject["code"]?.ToString();
        }

        private static InviteEntity CreateInviteEntity(int maxAge, int maxUses, int targetType, bool temporary, string applicationId)
        {
            return new()
            {
                MaxAge = maxAge,
                MaxUses = maxUses,
                Temporary = temporary,
                TargetType = targetType,
                ApplicationId = applicationId,
            };
        }

        private async Task<bool> IsChannelVoice(string channelSnowflake)
        {
            var response = await this._httpClient.GetAsync($"{DiscordEndpoint.EndpointUri}" +
                                                           $"{DiscordEndpoint.EndpointChannels}/" +
                                                           $"{channelSnowflake}");
            if (!(this.IsResponseValid(response.StatusCode)))
                return false;

            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            return jObject.HasValues && jObject["type"]?.ToObject<int>() == 2;
        }

        private bool IsResponseValid(HttpStatusCode statusCode) => statusCode.Equals(HttpStatusCode.OK);
    }
}