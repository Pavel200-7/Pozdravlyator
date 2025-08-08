using Pozdravlyator.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pozdravlyator.Services;
public interface IImageWorker
{
    public string CreateImageFile(ref IFormFile PhotoFile)
    {
        return "";
    }

    public void DeleteImageFile(string filePath) { }

    public string UpdateImageFile(ref IFormFile PhotoFile, string oldPhotoFile) 
    { 
        return ""; 
    }

    //public bool AreTheSameFiles(IFormFile? PhotoFile, string? currentFile)
    //{
    //    return true;
    //}

}


public class ImageWorker : IImageWorker
{
    private string pathToImagedir;
    private readonly IWebHostEnvironment _env;
    const int BYTES_TO_READ = sizeof(Int64);


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

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            PhotoFile.CopyTo(stream);
        }

        return newUniqueFileName;
    }

    public void DeleteImageFile(string fileName)
    {
        string filePath = Path.Combine(pathToImagedir, fileName);
        File.Delete(filePath);
    }

    public string UpdateImageFile(ref IFormFile PhotoFile, string currentFile)
    {
        DeleteImageFile(currentFile);
        return CreateImageFile(ref PhotoFile);
    }

    //public bool AreTheSameFiles(ref IFormFile PhotoFile, string currentFile)
    //{
    //}

}
