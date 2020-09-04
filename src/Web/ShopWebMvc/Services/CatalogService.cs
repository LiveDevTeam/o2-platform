using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShopWebMvc.Infrastructure;
using ShopWebMvc.Models;

namespace ShopWebMvc.Services
{
    public class CatalogService: ICatalogService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public IHttpClient _apiClient { get; }
        public ILogger<CatalogService> _logger { get; }
        public string _remoteServiceBaseUrl { get; }

        public CatalogService(IOptionsSnapshot<AppSettings> setting, IHttpClient httpClient,
            ILogger<CatalogService> logger)
        {
            _settings = setting;
            _apiClient = httpClient;
            _logger = logger;
            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/catalog/";
        }

        public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
        {
            var allcatalogItemUri = ApiPaths.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, page,take,brand,type);
            var dataString = await _apiClient.GetStringAsync(allcatalogItemUri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            var getBrandsUri = ApiPaths.Catalog.GetAllBrands(_remoteServiceBaseUrl);
            var dataString = await _apiClient.GetStringAsync(getBrandsUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem(){Value=null, Text="Все", Selected=true}
            };

            var brands = JArray.Parse(dataString);
            foreach (var brand in brands.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("id"),
                    Text = brand.Value<string>("brand")

                });

            }
            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var getTypesUrl = ApiPaths.Catalog.GetAllTypes(_remoteServiceBaseUrl);

            var dataString = await _apiClient.GetStringAsync(getTypesUrl);

            var items = new List<SelectListItem>
            {
                new SelectListItem(){Value=null, Text="Все", Selected=true}
            };

            var types = JArray.Parse(dataString);
            foreach (var type in types.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = type.Value<string>("id"),
                    Text = type.Value<string>("type")

                });

            }
            return items;
        }
    }
}
