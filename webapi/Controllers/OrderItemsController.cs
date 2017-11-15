using DutchTreat.Database.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers{

    [Route("api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController:Controller{
        
        private readonly IDutchRepository _repo;
        public OrderItemsController(IDutchRepository repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult Get(int orderid){           
            var items = _repo.GetOrderItems(User.Identity.Name, orderid);
            if (items !=null) return Ok(items);
            else return NotFound();
        }
    }

}