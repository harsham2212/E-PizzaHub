using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Interfaces
{
    public interface IAuthService: IService<User>
    {
        bool CreateUser(User user, string role);
        UserModel ValidateUser(string email, string password);
    }
}
