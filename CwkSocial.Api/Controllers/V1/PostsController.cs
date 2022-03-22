using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1
{
    [ApiVersion("1.0")]
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
