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
    /**
     * <summary>
     * Class <c>DiscordVoice</c> provides a light Management about Invitations.
     * </summary>
     */
    public class DiscordVoice
    {

        private readonly HttpClient _httpClient = new();

        /**
         * <summary>
         * The Ctor <c>DiscordVoice</c> is for creating a new Management
         * corresponding to the Token.
         *
         * <param name="token">A <c>string</c> which can be a Bot- or an User-Token as well</param>
         * <param name="bot">A <c>bool</c> which is the State set to true when the Token relates to a Bot</param>
         * </summary>
         */
        public DiscordVoice(string token, bool bot)
        {
            var requestHeaders = this._httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Authorization", bot ? $"Bot {token}" : token);
        }

        /**
         * <summary>
         * The Method <c>CreateInvite</c> creates an Invite with the associated VoiceMode.
         *
         * <param name="channelSnowflake">A <c>string</c> which serves as Id of a Voice-Channel to create the Invitation at</param>
         * <param name="voiceMode">A <c>string</c> which serves as the VoiceMode to the corresponding Voice-Channel</param>
         *
         * <returns>Returns an asynchronous <c>Task of a string</c> which contains the Invitation-Code, when everything was done</returns>
         * </summary>
         */
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
            
            if (!(response.IsSuccessStatusCode))
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
            if (!(response.IsSuccessStatusCode))
                return false;

            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            return jObject.HasValues && jObject["type"]?.ToObject<int>() == 2;
        }

    }
}