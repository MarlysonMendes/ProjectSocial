using AutoMapper;
using CwkSocial.Api.Contracts.UserProfile.Requests;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Application.Enums;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Application.UserProfiles.Queries;
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
        private readonly IMapper _mapper;
        public UserProfilesController(IMediator mediator, IMapper mapper)
        { 
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            var query = new GetAllUserProfileQuery();
            var response = await _mediator.Send(query);
            var profiles = _mapper.Map<List<UserProfileResponse>>(response);
            return Ok(profiles);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUserProfiles([FromBody] UserProfileCreateUpdate profile)
        {
            var command = _mapper.Map<CreateUserCommand>(profile);
            var response = await _mediator.Send(command);
            var userProfile = _mapper.Map<UserProfileResponse>(response);

            return CreatedAtAction(nameof(GetUserProfileById), new { id = response.UserProfileId }, userProfile);
        }



        [Route(ApiRoutes.UserProfiles.IdRoute)]
        [HttpGet]
        public async Task<IActionResult> GetUserProfileById(string id)
        {
            var query = new GetUserProfileByIdQuery { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(query);
            var profile = _mapper.Map<UserProfileResponse>(response);
            
            if(response == null) return NotFound($"No user with profile Id {id} found");
            
            return Ok(profile);
        }

        [HttpPatch]
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate updatedProfile)
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfo>(updatedProfile);
            command.UserProfileId = Guid.Parse(id);
            var response = await _mediator.Send(command);

            if(response.IsErro)
            {
                if (response.Erros.Any(e => e.Code == ErrorCodes.NotFound))
                {
                    var error = response.Erros.FirstOrDefault(e => e.Code == ErrorCodes.NotFound);

                    return NotFound(error.Message) ;
                }
                if(response.Erros.Any(e => e.Code == ErrorCodes.ServerError))
                {
                    var error = response.Erros.FirstOrDefault(e => e.Code == ErrorCodes.ServerError);

                    return StatusCode(500, error.Message);
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        public async Task<IActionResult> DeleteUserProfile(string id)
        {
            var command = new DeleteUserProfile() { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(command);
            return NoContent();
        }
    }
}
