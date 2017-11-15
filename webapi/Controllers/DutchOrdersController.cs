
using System;
using System.Threading.Tasks;
using DutchTreat.Database.Entities;
using DutchTreat.Database.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace webapi.Controllers{

    //https://wildermuth.com/2017/08/19/Two-AuthorizationSchemes-in-ASP-NET-Core-2
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/orders")]
    public class DutchOrdersController : Controller{

        private readonly IDutchRepository _repo;
        private readonly ILogger _logger;

        private readonly UserManager<DutchUser> _usrMgr;
        
        public DutchOrdersController(
            IDutchRepository repository, 
            ILogger<DutchOrdersController> logger,
            UserManager<DutchUser> usrMgr
            )
        {
            this._repo = repository;
            this._logger = logger;
            this._usrMgr = usrMgr;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order){

            try
            {
                //we need to get a User object (not just a name)
                var user = await _usrMgr.FindByNameAsync(User.Identity.Name);
                
                order.User = user;
                _repo.AddEntity(order);
                
                if(_repo.SaveAll()){
                    return Created($"/api/orders/{order.Id}",order);
                };
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Problem saving order: {ex}");
            }
            return BadRequest($"Problem saving new order");

            
        }

        
        [HttpGet("")]
        public IActionResult Get(){
            
            try
            {
                var username = User.Identity.Name;
                //return Ok(_repo.GetAllOrders());
                return Ok(_repo.GetAllOrdersByUser(username));

            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Failed to get Orders: {ex}");
                return BadRequest();
                
            }
            
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id){

            try
            {
                var username = User.Identity.Name;
                var order = _repo.GetOrderById(username, id);
                if(order !=null) return Ok(order); 
                else return NotFound();       
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to retrieve order with id {id}");
                return BadRequest(ex);
                
            }
            
        }
    }

}