using MSC.Server.Models;
using System.Text.Json;

namespace MSC.Server.Extensions;

public interface IRecaptchaExtension
{
    Task<bool> VerifyAsync(string? token, string? ip);
}

public class RecaptchaExtension : IRecaptchaExtension
{
    private IConfiguration Configuration { get; }
    private string GoogleSecretKey { get; set; }
    private string GoogleRecaptchaVerifyApi { get; set; }
    private decimal RecaptchaThreshold { get; set; }

    public RecaptchaExtension(IConfiguration configuration)
    {
        Configuration = configuration;

        GoogleRecaptchaVerifyApi = Configuration.GetSection("GoogleRecaptcha").GetSection("VefiyAPIAddress").Value ?? "";
        GoogleSecretKey = Configuration.GetSection("GoogleRecaptcha").GetSection("Secretkey").Value ?? "";

        var hasThresholdValue = decimal.TryParse(Configuration.GetSection("RecaptchaThreshold").Value ?? "", out var threshold);
        if (hasThresholdValue)
            RecaptchaThreshold = threshold;
    }

    public async Task<bool> VerifyAsync(string? token, string? ip)
    {
        if (RecaptchaThreshold == 0)
            return true;
        if (string.IsNullOrEmpty(token))
            return false;
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync($"{GoogleRecaptchaVerifyApi}?secret={GoogleSecretKey}&response={token}&remoteip={ip}");
            var tokenResponse = JsonSerializer.Deserialize<TokenResponseModel>(response);
            if (tokenResponse is null || !tokenResponse.Success || tokenResponse.Score < RecaptchaThreshold)
                return false;
        }
        return true;
    }
}
