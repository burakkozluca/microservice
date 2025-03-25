namespace Services.Identity.Models.User.Requests;

public record CreateUserRequest(string Username, string email, string password);