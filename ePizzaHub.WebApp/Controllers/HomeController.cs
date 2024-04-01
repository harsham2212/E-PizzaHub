using epizzahub.Models;
using epizzahub.Services.Interfaces;
using ePizzaHub.WebApp.Helpers;
using ePizzaHub.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace ePizzaHub.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemService _itemService;
        IMemoryCache _cache;
        IDistributedCache _cacheRedis;
        public HomeController(ILogger<HomeController> logger, IItemService itemService, IMemoryCache cache, IDistributedCache cacheRedis)
        {
            _logger = logger;
            _itemService = itemService;
            _cache = cache;
            _cacheRedis = cacheRedis;
        }

        public IActionResult Index()
        {
            //IEnumerable<ItemModel> items = _itemService.GetItems();
            string key = "Items";
            //var items = _cache.GetOrCreate(key, entry =>
            //{
            //    entry.AbsoluteExpiration = DateTime.Now.AddHours(24);
            //    entry.SlidingExpiration = TimeSpan.FromMinutes(5);
            //    return _itemService.GetItems();
            //});

            var items = _cacheRedis.GetRecordAsync<IEnumerable<ItemModel>>(key).Result;
            if (items == null)
            {
                items = _itemService.GetItems();
                _cacheRedis.SetRecordAsync(key, items).Wait();
            }
            try
            {
                int x = 0, y = 3;
                int z = y / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}