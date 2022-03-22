using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            return View();
        }

    }
}
