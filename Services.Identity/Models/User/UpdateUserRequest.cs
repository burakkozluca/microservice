namespace Services.Identity.Models.User.Requests;

public record UpdateUserRequest(string name , string surName , string email , DateTime dateOfBirth , string phoneNumber);

