using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //  https://localhost:portnumber/api/
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]

        public IActionResult GetAllStudent()
        {
            string[] studentNames = { "Ram", "Shyam", "Gopal", "Jane", "John" };

            return Ok(studentNames);
        }
    }
}
