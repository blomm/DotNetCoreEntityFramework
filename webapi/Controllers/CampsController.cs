using Api.Database;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers{

    [Route("api/[controller]")]
    public class CampsController : Controller{

        private ICampRepository _repo;

        public CampsController(ICampRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet("")]
        public IActionResult Get(){
            var camps = this._repo.GetAllCamps();
            
            return Ok(camps);
            //return Ok(new {name = "mikeyyyyy blommm", favouriteColour = "blue"});
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, bool includeSpeakers = false){
            
            try{
                var camp = includeSpeakers? this._repo.GetCampWithSpeakers(id) : this._repo.GetCamp(id);

                if (camp == null) return NotFound($"Camp {id} was not found");

                return Ok(camp);
            }
            catch{
                return BadRequest();
            }
            
        }

        

    }
}