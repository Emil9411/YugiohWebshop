namespace Yugioh.Server.Services.AuthServices.Models
{
    public class JwtSettings
    {
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
    }
}
