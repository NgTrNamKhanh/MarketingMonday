using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace Comp1640_Final.Services
{
    public interface ICloudinaryService
    {
        string GetPublicIdFromImageUrl(string imageUrl);
        Task DeleteResource(string publicId);
        Task<ImageUploadResult> UploadImage(IFormFile file);
        Task<RawUploadResult> UploadDocument(IFormFile file);
        string GetPublicIdFromDocUrl(string docUrl);
        Task<ImageUploadResult> UploadAvatarImage(IFormFile imageFile);
    }
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            Account account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Image file is required.");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task DeleteResource(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                throw new ArgumentException("Public ID is required.");

            await _cloudinary.DeleteResourcesAsync(publicId);
        }

        public string GetPublicIdFromImageUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                throw new ArgumentException("Image URL is required.");

            var uri = new Uri(imageUrl);
            return Path.GetFileNameWithoutExtension(uri.Segments.Last());
        }

        public async Task<RawUploadResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Document file is required.");

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public string GetPublicIdFromDocUrl(string docUrl)
        {
            if (string.IsNullOrEmpty(docUrl))
                throw new ArgumentException("Document URL is required.");

            var uri = new Uri(docUrl);
            return Path.GetFileNameWithoutExtension(uri.Segments.Last());
        }
        public async Task<ImageUploadResult> UploadAvatarImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is required.");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream())
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}
