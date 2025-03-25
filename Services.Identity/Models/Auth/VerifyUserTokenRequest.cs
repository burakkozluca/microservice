namespace Services.Identity.Models.Auth.Requests;

public record VerifyUserToken(string token, string email);