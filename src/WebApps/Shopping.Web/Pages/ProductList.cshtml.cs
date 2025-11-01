namespace Shopping.Web.Pages
{
    public class ProductListModel(ICatalogService catalogService, IBasketService basketService, ILogger<ProductListModel> logger)
        : PageModel
    {
        public IEnumerable<string> CategoryList { get; set; } = [];
        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string catagoryName)
        {
            var response = await catalogService.GetProducts();

            CategoryList = response.Products
                .SelectMany(p => p.Category)
                .Distinct()
                .OrderBy(c => c);

            if (!string.IsNullOrEmpty(catagoryName))
            {
                ProductList = response.Products
                    .Where(p => p.Category.Contains(catagoryName));
                SelectedCategory = catagoryName;
            }
            else
            {
                ProductList = response.Products;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            logger.LogInformation("Add to cart button clicked");
            var productResponse = await catalogService.GetProduct(productId);

            var basket = await basketService.LoadUserBasket();

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Products.Name,
                Price = productResponse.Products.Price,
                Quantity = 1,
                Color = "Black"
            });

            await basketService.StoreBasket(new StoreBasketRequest(basket));

            return RedirectToPage("Cart");
        }
    }
}
