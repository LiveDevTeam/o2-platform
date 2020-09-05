using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopWebMvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
         

        [HttpPost]
        public IActionResult AddToCart()
        {
            string name ="Secured Method";
            return View(name);

        }
        //public async Task WriteOutIdentityInfo()
        //{
        //    var identityToken =
        //        await HttpContext.Authentication.
        //         GetAuthenticateInfoAsync(OpenIdConnectParameterNames.IdToken);
        //    Debug.WriteLine($"Identity Token: {identityToken}");
        //    foreach (var claim in User.Claims)
        //    {
        //        Debug.WriteLine($"Claim Type: {claim.Type} - Claim Value : {claim.Value}");
        //    }

        //}
    }
}