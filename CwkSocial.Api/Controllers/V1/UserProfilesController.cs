using CwkSocial.Api.Contracts.UserProfile.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class UserProfilesController : Controller
    {
        private readonly IMediator _mediator;
        public UserProfilesController(IMediator mediator)
        { 
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            return await Task.FromResult(Ok());
        }
        [HttpPut]
        public async Task<IActionResult> CreateUserProfiles([FromBody] UserProfileCreate profile)
        {
            return await Task.FromResult(Ok());
        }
    }
}
