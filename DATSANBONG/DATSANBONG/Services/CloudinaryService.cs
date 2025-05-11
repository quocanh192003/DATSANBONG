using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATSANBONG.Services.IServices;

namespace DATSANBONG.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration config)
        {
            var acc = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                var publicId = GetPublicIdFromUrl(imageUrl);
                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);
                return result.Result == "ok" || result.Result == "not found";
            }
            catch
            {
                return false;
            }
        }

        private string GetPublicIdFromUrl(string imageUrl)
        {
            // Ví dụ: https://res.cloudinary.com/demo/image/upload/v12345678/myimage.jpg
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            var fileName = segments.Last(); // myimage.jpg
            var publicId = Path.GetFileNameWithoutExtension(fileName);

            // Nếu bạn có folder trong Cloudinary thì sửa tại đây
            // Ví dụ: return $"myfolder/{publicId}";
            return publicId;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file.Length <= 0) return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "sanbong"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
    }
}
