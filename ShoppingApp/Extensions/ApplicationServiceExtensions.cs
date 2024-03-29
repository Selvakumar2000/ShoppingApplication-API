﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Data;
using ShoppingApp.Helpers;
using ShoppingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //For Database Operation
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("ShopingAppCon"));
                //options.UseNpgsql(config.GetConnectionString("DatingAppCon"));
            });

            //For Jwt Token Creation and Handling
            services.AddScoped<TokenService>();
     
            //For Cloudinary Settings
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ProductPhotoService>();

            //For Repositories
            services.AddScoped<UserRepository>();
            services.AddScoped<ProductsRepository>();
            services.AddScoped<OrderManagementRepository>();

            return services;

        }
    }
}
