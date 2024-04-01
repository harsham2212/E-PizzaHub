using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Interfaces
{
    public interface IItemService : IService<Item>
    {
        IEnumerable<ItemModel> GetItems();
    }
}
