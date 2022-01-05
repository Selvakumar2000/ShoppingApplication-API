using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShoppingApp.DTOs;
using ShoppingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Services
{
    public class ProductPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public ProductPhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
           (   //ordering matters
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public ImageUploadResult AddProductPhoto(IFormFile file)
        {
            var UploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                UploadResult = _cloudinary.Upload(uploadParams);
            }
            return UploadResult;
        }
    }
}
