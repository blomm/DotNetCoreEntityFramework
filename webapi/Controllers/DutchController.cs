
using DutchTreat.Database.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace webapi.Controllers{

    [Route("api/[controller]")]
    public class DutchController:Controller{

        private readonly IDutchRepository _dutchRepo;
        private readonly ILogger<DutchController> _logger;
        public DutchController(IDutchRepository repo, ILogger<DutchController> logger )
        {
            _dutchRepo = repo;
            _logger = logger;
        }

        [HttpGet("products")]
        public IActionResult GetProducts(){
            try
            {
                return Ok(_dutchRepo.GetAllProducts());
            }
            catch (System.Exception ex)
            {               
                _logger.LogError($"Failed to retrieve Products: {ex}");
                return BadRequest("Failed to get Products");
            }
        }

        [HttpGet("products")]
        public IActionResult GetProducts(string category){
            try
            {
                return Ok(_dutchRepo.GetProductsByCategory(category));
            }
            catch (System.Exception ex)
            {               
                _logger.LogError($"Failed to retrieve Products by category: {ex}");
                return BadRequest("Failed to get Products by category");
            }
        }

    }
}