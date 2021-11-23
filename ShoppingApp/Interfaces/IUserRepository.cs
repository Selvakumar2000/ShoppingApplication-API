using ShoppingApp.DTOs;
using ShoppingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Interfaces
{
    public interface IUserRepository
    {
        public string GetUsername(string username);
        public UserDetailsDto GetUsersDetails(string username);
        public int UpdateDetails(AppUser updatedDetails, int userId);
        public string GetPhotoUrl(int userId);
        public string GetPhotoId(int userId);
    }
}
