using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Repositories.Interfaces
{
    public interface IUserRepository : IRepostiory<User>
    {
        bool CreateUser(User user, string role);
        UserModel ValidateUser(string Email, string Password);
    }

}
