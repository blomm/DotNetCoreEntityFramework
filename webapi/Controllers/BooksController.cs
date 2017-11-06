using Books.Database;
//using BookStore.Database;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers{
    
    public class BooksController: Controller{


        [HttpGet("api/books")]
        public IActionResult Get(){

            using(var context = new BooksContext())
            {

            }

            return Ok(true);
        }
    }
}

