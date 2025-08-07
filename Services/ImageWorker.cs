using System.IO;

namespace Pozdravlyator.Services;
public interface IImageWorker
{
    public string CreateImageFile(ref IFormFile PhotoFile)
    {
        return "";
    }

    public void UpdateFile(ref IFormFile PhotoFile, string oldPhotoFile) { }

    private async void PerformCreation(IFormFile PhotoFile, string filePath) { }

    private void PerformDeleteon(string filePath) { }

    public bool isTheSameFiles(string filePath1, string filePath2)
    {
        return true;
    }

}


public class ImageWorker : IImageWorker
{

    private string pathToImagedir;
    private readonly IWebHostEnvironment _env;

    public ImageWorker(IWebHostEnvironment env)
    {
        _env = env;
        pathToImagedir = Path.Combine(_env.WebRootPath, "images");

        if (!Directory.Exists(pathToImagedir))
        {
            Directory.CreateDirectory(pathToImagedir);
        }
    }


    public string CreateImageFile(ref IFormFile PhotoFile)
    {
        var fileName = Path.GetFileName(PhotoFile.FileName);
        var newUniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
        var filePath = Path.Combine(pathToImagedir, newUniqueFileName);
        PerformCreation(PhotoFile, filePath);
        return newUniqueFileName;
    }

    public void UpdateFile(ref IFormFile PhotoFile, string oldPhotoFile)
    {
        //PerformDeleteon(oldPhotoFile);
        CreateImageFile(ref PhotoFile);
    }

    private async void PerformCreation(IFormFile PhotoFile, string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await PhotoFile.CopyToAsync(stream);
        }
    }

    private void PerformDeleteon(string filePath)
    {
        File.Delete(filePath);
    }

    public bool isTheSameFiles(string filePath1, string filePath2)
    {
        return filePath1.Equals(filePath2);
    }

}
