using epizzahub.API.Filters;
using epizzahub.Models;
using epizzahub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace epizzahub.API.Controllers
{
    [CustomAuthorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        IItemService _itemService;
        public CatalogController (IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IEnumerable<ItemModel> GetAll()
        {
            return _itemService.GetItems();
        }
    }
}
