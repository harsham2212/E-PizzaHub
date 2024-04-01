using epizzahub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ePizzaHub.WebApp.Areas.Admin.Controllers
{
    public class ItemController : BaseController
    {
        HttpClient _client;
        IConfiguration _config;
        public ItemController(IConfiguration configuration)
        {
            _config = configuration;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_config["ApiAddress"]);
        }

        public IActionResult Index()
        {
            IEnumerable<ItemModel> items = null;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.Token);
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Catalog/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync().Result;
                items = JsonSerializer.Deserialize<IEnumerable<ItemModel>>(strData);
            }
            return View(items);
        }
    }
}
