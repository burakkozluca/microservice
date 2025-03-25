namespace Services.Identity.Models.User;

public record UserDto(string Id,string name, string surName, string email, DateTime dateOfBirth, string phoneNumber);