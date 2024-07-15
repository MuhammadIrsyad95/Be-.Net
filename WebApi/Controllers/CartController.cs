using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CartController : Controller
    {
        private readonly CartRepository _cartRepository;

        public CartController(CartRepository cartRepository)
        {
            this._cartRepository = cartRepository;
        }

        //[Authorize]
        [HttpGet("CartUser")]
        public IActionResult GetCheckoutByUserId()
        {
            //diganti dengan auth user
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            var carts = _cartRepository.GetCheckoutByUserId(Int32.Parse(userId));

            return Ok(carts);
        }

        //[Authorize]
        [HttpGet("SpecificCart/{idCourse}/{schedule}")]
        public IActionResult Get1CartByUserId(int idCourse, string schedule)
        {
            //diganti dengan auth user
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            var carts = _cartRepository.Get1CartByUserId(Int32.Parse(userId), idCourse, schedule);

            return Ok(carts);
        }

        [HttpPost()]
        public IActionResult MakeCart([FromBody] MakeCartDto data)
        {
            //diganti dengan auth user
            string userId = User.FindFirstValue(ClaimTypes.Sid);
            //string userId = "1";

            _cartRepository.CreateCart(Int32.Parse(userId), data.ProdukIds, data.schedule);
            return Ok(data);
        }

        [HttpDelete("{cartId}")]
        public IActionResult DeleteCart(int cartId)
        {
            //string userId = "1";

            _cartRepository.Delete(cartId);
            return Ok(_cartRepository.Delete(cartId));
        }
    }
}
