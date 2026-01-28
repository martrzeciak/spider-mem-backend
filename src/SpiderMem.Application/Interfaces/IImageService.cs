using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SpiderMem.Application.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult> AddImageAsync(IFormFile file, string option);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}