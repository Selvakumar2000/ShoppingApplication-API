using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ShoppingApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Interfaces
{
    public interface IProductPhotoService
    {
        public ImageUploadResult AddProductPhoto(IFormFile file);
    }
}
