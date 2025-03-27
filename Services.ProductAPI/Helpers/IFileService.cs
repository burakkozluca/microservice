namespace Services.ProductAPI.Helpers;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile imageFile);
    void DeleteFile(string fileNameWithExtension);
}