using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSC.Server.Models
{
    /// <summary>
    /// Response Model from Google Recaptcha V3 Verify API
    /// </summary>
    public class TokenResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public decimal Score { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
