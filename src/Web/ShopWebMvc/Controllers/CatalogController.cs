using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopWebMvc.Models;
using ShopWebMvc.Services;
using ShopWebMvc.ViewModels;

namespace ShopWebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService _catalogSvc;

        public CatalogController(ICatalogService catalogSvc) =>
            _catalogSvc = catalogSvc;

        public async Task<IActionResult> Index(int? brandFilterApplied, int? typesFilterApplied, int? page)
        {
            int itemsPage=10 ;
            var catalog = await _catalogSvc.GetCatalogItems(page ?? 0, itemsPage, brandFilterApplied, typesFilterApplied);
            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = catalog.Data,
                Brands = await _catalogSvc.GetBrands(),
                Types = await _catalogSvc.GetTypes(),
                BrandFilterApplied = brandFilterApplied ?? 0,
                TypesFilterApplied = typesFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsPage , //catalog.Data.Count,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)catalog.Count / itemsPage))
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        // public IActionResult Contact()
        // {
        //     ViewData["Message"] = "Your contact page.";
        //
        //     return View();
        // }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
