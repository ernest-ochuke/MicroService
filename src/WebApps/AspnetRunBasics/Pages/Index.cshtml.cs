using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogService e_CatalogService;
        private readonly IBasketService e_BasketService;

        public IndexModel(IBasketService basketService, ICatalogService catalogService)
        {
            e_BasketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            e_CatalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await e_CatalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            var product = await e_CatalogService.GetCatalog(productId);
            var userName = "swn";

            var basket = await e_BasketService.GetBasket(userName);
            basket.Items.Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Quantity = 1,
                Color = "Black"
            });
            var basketUpdated = await e_BasketService.UpdateBasket(basket);

           
            return RedirectToPage("Cart");
        }
    }
}
