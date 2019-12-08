using AnfoldTask.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnfoldTask.Services
{
    public interface IAuthenticationService
    {
        bool HasToken { get; }
        string GetToken();
        Task<string> Authenticate(Credential credential);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration config;
        private readonly string apiAddress;
        private readonly HttpClient httpClient;
        private string token;

        public AuthenticationService(IConfiguration config)
        {
            this.config = config;
            apiAddress = config.GetValue<string>("ApiSettings:Url") + config.GetValue<string>("ApiSettings:TokenPath");
            httpClient = new HttpClient();
        }

        public bool HasToken
            => token != null && token.Length != 0;

        public string GetToken()
        {
            if (HasToken)
                return token;
            else
                throw new InvalidOperationException("Authenticate with remote API to acquire a token");
        }

        public async Task<string> Authenticate(Credential credential)
        {
            var requestBody = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", credential.Email),
                    new KeyValuePair<string, string>("Password", credential.Password),
                    new KeyValuePair<string, string>("AccountName", "demo"),
                });

            var response = await httpClient.PostAsync(apiAddress, requestBody);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
                throw new HttpRequestException("Authentication faild at API endpoint");

            token = (await response.Content.ReadAsStringAsync()).Trim('"');
            return token;
        }
    }
}
