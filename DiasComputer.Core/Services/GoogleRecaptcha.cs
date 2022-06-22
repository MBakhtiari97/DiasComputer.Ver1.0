using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DiasComputer.Core.Services
{
    public class GoogleRecaptcha:IGoogleRecaptcha
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public GoogleRecaptcha(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> IsUserValid()
        {
            var secretKey = _configuration.GetSection("GoogleRecaptcha")["SecretKey"];
            var response = _contextAccessor.HttpContext.Request.Form["g-recaptcha-response"];
            var http = new HttpClient();
            var result = await http.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}",null);

            if (result.IsSuccessStatusCode)
            {
                var googleResponse =
                    JsonConvert.DeserializeObject<RecaptchaResponse>(await result.Content.ReadAsStringAsync());

                return googleResponse.Success;
            }

            return false;
        }
    }

    public class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTimeOffset ChallengeTs { get; set; }
        [JsonProperty("hostname")]
        public string HostName { get; set; }
    }
}
