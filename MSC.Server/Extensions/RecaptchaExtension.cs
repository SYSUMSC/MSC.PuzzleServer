using Microsoft.Extensions.Configuration;
using MSC.Server.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSC.Server.Extensions
{
    public interface IRecaptchaExtension
    {
        Task<bool> VerifyAsync(string token, string ip);
    }

    public class RecaptchaExtension : IRecaptchaExtension
    {
        private IConfiguration _configuration { get; }
        private static string GoogleSecretKey { get; set; }
        private static string GoogleRecaptchaVerifyApi { get; set; }
        private static decimal RecaptchaThreshold { get; set; }
        public RecaptchaExtension()
        {
            _configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .Build();

            GoogleRecaptchaVerifyApi = _configuration.GetSection("GoogleRecaptcha").GetSection("VefiyAPIAddress").Value ?? "";
            GoogleSecretKey = _configuration.GetSection("GoogleRecaptcha").GetSection("Secretkey").Value ?? "";

            var hasThresholdValue = decimal.TryParse(_configuration.GetSection("RecaptchaThreshold").Value ?? "", out var threshold);
            if (hasThresholdValue)
                RecaptchaThreshold = threshold;
        }
        public async Task<bool> VerifyAsync(string token, string ip)
        {
            if (RecaptchaThreshold == 0)
                return true;
            if (string.IsNullOrEmpty(token))
                return false;
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync($"{GoogleRecaptchaVerifyApi}?secret={GoogleSecretKey}&response={token}&remoteip={ip}");
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(response);
                if (!tokenResponse.Success || tokenResponse.Score < RecaptchaThreshold)
                    return false;
            }
            return true;
        }
    }
}
