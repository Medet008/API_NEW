using API_NEW.Data;
using API_NEW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace API_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {

        protected ApiResponse _response;  
        private readonly ApplicationDbContext _db;
        public ShoppingCartController(ApplicationDbContext db) {
            _db = db;
            _response = new ();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updateQuantityBy)
        {

            // Корзина покупок будет иметь одну запись на каждый идентификатор пользователя, даже если у пользователя много товаров в корзине.
            // Cart items будут содержать все товары в корзине покупок для пользователя.
            // updatequantityby будет содержать количество, на которое нужно обновить количество товара.
            // Если это -1, это означает, что мы уменьшаем количество; если это 5, это означает, что нужно добавить 5 к существующему количеству.
            // Если updatequantityby равен 0, товар будет удален.


            // когда пользователь добавляет новый товар в новую корзину покупок в первый раз
            // когда пользователь добавляет новый товар в уже существующую корзину покупок (у пользователя уже есть другие товары в корзине)
            // когда пользователь обновляет количество существующего товара
            // когда пользователь удаляет существующий товар

            ShoppingCart shoppingCart = _db.ShoppingCarts.Include(u =>u.CartItems).FirstOrDefault(u => u.UserId == userId);
            MenuItem menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == menuItemId);  

            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response); 
            }
            if(shoppingCart == null && updateQuantityBy > 0) 
            {
                ShoppingCart newCart = new() { UserId = userId }; 
                _db.ShoppingCarts.Add(newCart);
                _db.SaveChanges();

                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = shoppingCart.Id, 
                    MenuItem=null
                }; 

                _db.CartItems.Add(newCartItem);
                _db.SaveChanges();
            }
            else
            {
                
            }
            return _response;
        }


    }
}
