namespace Backend.Service;

public class UploadImageService
{
    public async Task<string> UploadImage(IFormFile file)
    {
        List<string> validExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!validExtensions.Contains(extension))
        {
            throw new Exception("Invalid file extension");
        }
        
        if (!file.ContentType.StartsWith("image/"))
        {
            throw new Exception("Invalid file content type");
        }
        
        if (file.Length > 5 * 1024 * 1024) // 5 MB limit
        {
            throw new Exception("File is too large");
        }
        
        string fileName = Guid.NewGuid() + extension;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
            
        string fullPath = Path.Combine(filePath, fileName);
        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
            
        return "/Images/" + fileName;
    }
}