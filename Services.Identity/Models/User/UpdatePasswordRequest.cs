namespace Services.Identity.Models.User.Requests;

public record UpdatePasswordRequest(string OldPassword, string NewPassword, string ReNewPassword);