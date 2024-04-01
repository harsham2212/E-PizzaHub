using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Interfaces;
using epizzahub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Implementation
{
    public class ItemService : Service<Item>, IItemService
    {
        private readonly IRepostiory<Item> _itemRepo;
        public ItemService(IRepostiory<Item> itemRepo) : base(itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public IEnumerable<ItemModel> GetItems()
        {
            var data = _itemRepo.GetAll().OrderBy(i => i.CategoryId).Select(i => new ItemModel
            {
                Id = i.Id,
                Name = i.Name,
                CategoryId = i.CategoryId,
                Description = i.Description,
                ImageUrl = i.ImageUrl,
                ItemTypeId = i.ItemTypeId,
                UnitPrice = i.UnitPrice
            });
            return data;
        }
    }
}
