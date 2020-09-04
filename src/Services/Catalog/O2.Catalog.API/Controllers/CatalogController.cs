using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using O2.Catalog.API.Data;
using O2.Catalog.API.Domain;
using O2.Catalog.API.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace O2.Catalog.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Catalog")]
    public class CatalogController : Controller
    {
        private readonly CatalogContext _catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;

        public CatalogController(CatalogContext catalogContext,
            IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = catalogContext;
            _settings = settings;
            ((DbContext)catalogContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogContext.CatalogTypes.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _catalogContext.CatalogBrands.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogItems()
        {
            var items = await _catalogContext.CatalogItems.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}]")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if(id<=0)
            {
                return BadRequest();
            }

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);

            if(item!=null)
            {
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", _settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }

            return NotFound();
        }

        //GET api/Catalog/item[?pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize=6,[FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                .LongCountAsync();

            var itemOnPage = await _catalogContext.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemOnPage = ChangeUrlPlaceHolder(itemOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemOnPage);

            return Ok(model);
        }

        //GET api/Catalog/item/withname/Wonder?pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                .Where(c=>c.Name.StartsWith(name))
                .LongCountAsync();

            var itemOnPage = await _catalogContext.CatalogItems
                .Where(c => c.Name.StartsWith(name))
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemOnPage = ChangeUrlPlaceHolder(itemOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemOnPage);

            return Ok(model);
        }

        //GET api/Catalog/items/type/1/brand/null[?pageSize=4&pageIndex=0]]
        [HttpGet]
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        public async Task<IActionResult> Items(int? catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var root =(IQueryable<CatalogItem>)_catalogContext.CatalogItems;

            if (catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }

            if (catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemOnPage = await root
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemOnPage = ChangeUrlPlaceHolder(itemOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemOnPage);

            return Ok(model);
        }


        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct([FromBody]CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _catalogContext.CatalogItems.Add(item);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id });
        }

        [HttpPut]
        [Route("Items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await _catalogContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
                return NotFound(new { Message = $"item with id {productToUpdate.Id} not found." });

            catalogItem = productToUpdate;
            _catalogContext.CatalogItems.Update(catalogItem);

            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.Id });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _catalogContext.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if (product == null)
                return NotFound(new { Message = $"item with id {product.Id} not found." });

            _catalogContext.CatalogItems.Remove(product);
            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }

        private List<CatalogItem> ChangeUrlPlaceHolder(List<CatalogItem> items)
        {
           items.ForEach(x=>x.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", _settings.Value.ExternalCatalogBaseUrl));
           return items;
        }
    }
}
