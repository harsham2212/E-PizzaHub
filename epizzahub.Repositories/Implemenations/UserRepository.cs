using epizzahub.Entitites;
using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Repositories.Implemenations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDBContext db) :base(db) 
        {

        }
        public bool CreateUser(User user, string role)
        {
            try
            {
                Role roles = db.Roles.Where(r => r.Name == role).FirstOrDefault();
                if (role != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);  
                    user.Roles.Add(roles);
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex) 
            {
                throw ex; 
            }
            return false;
        }

        public UserModel ValidateUser(string Email, string Password)
        {
            User user = db.Users.Include(u => u.Roles).Where(u => u.Email == Email).FirstOrDefault();
            if (user != null)
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(Password, user.Password);
                if (!isValid)
                {
                    UserModel model = new UserModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = user.Roles.Select(r => r.Name).ToArray(),
                    };
                    return model;
                }
            }
            return null;
        }
    }
}
