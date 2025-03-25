namespace Services.Identity.Services.Auth;

public class CustomTokenOption
{
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;

    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }

    public string SecurityKey { get; set; } = default!;
}