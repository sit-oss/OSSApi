using System.Net.Http;
using System.Net.Http.Headers;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace OSSApi
{


    public class UserInfo
    {
        /// <summary>
        /// User id string
        /// </summary>
        public required string sub { get; set; } = "0";

        /// <summary>
        /// id
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int id => int.Parse(sub);

        /// <summary>
        /// User name
        /// </summary>
        public required string preferred_username { get; set; } = "";

        /// <summary>
        /// User normal name
        /// </summary>
        public required string name { get; set; } = "";

        /// <summary>
        /// User email
        /// </summary>
        public required string email { get; set; } = "";

        /// <summary>
        /// User email verified
        /// </summary>
        public bool email_verified { get; set; }
    }


    public interface IUserInfoClient
    {
        public Task<UserInfo> GetUserInfo(string token);
    }
    public class UserInfoClient : IUserInfoClient
    {
        private readonly HttpClient _httpClient;
        public UserInfoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfo> GetUserInfo(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var responseString = await _httpClient.GetStringAsync("/connect/userinfo");
            return System.Text.Json.JsonSerializer.Deserialize<UserInfo>(responseString) ?? new UserInfo
                { email = "", email_verified = false, name = "", preferred_username = "", sub = "" };
        }
    }
}
