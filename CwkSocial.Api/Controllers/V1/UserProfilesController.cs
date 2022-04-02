using AutoMapper;
using CwkSocial.Api.Contracts.Common;
using CwkSocial.Api.Contracts.UserProfile.Requests;
using CwkSocial.Api.Contracts.UserProfile.Responses;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.Application.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CwkSocial.Api.Controllers.V1;
using CwkSocial.Api.Filters;

namespace CwkSocial.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class UserProfilesController : ControllerBase
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
            throw new NotImplementedException("Method not implemented");


            var query = new GetAllUserProfileQuery();
            var response = await _mediator.Send(query);
            var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
            return Ok(profiles);
        }


        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateUserProfiles([FromBody] UserProfileCreateUpdate profile)
        {
            var command = _mapper.Map<CreateUserCommand>(profile);
            var response = await _mediator.Send(command);
            var handleError = new HandlerError();
           
            var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);

            return response.IsError? handleError.HandleErrorResponse(response.Errors)
                :CreatedAtAction(nameof(GetUserProfileById), new { id = userProfile.UserProfileId }, userProfile);
        }



        [Route(ApiRoutes.UserProfiles.IdRoute)]
        [HttpGet]
        [ValidateModel]
        public async Task<IActionResult> GetUserProfileById(string id)
        {
            var query = new GetUserProfileByIdQuery { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(query);
            var profile = _mapper.Map<UserProfileResponse>(response.Payload);
            var handleError = new HandlerError();
            if (response.IsError)
                return handleError.HandleErrorResponse(response.Errors);
            
            return Ok(profile);
        }

        [HttpPatch]
        [ValidateModel]
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate updatedProfile)
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfo>(updatedProfile);
            command.UserProfileId = Guid.Parse(id);
            var response = await _mediator.Send(command);

            var handleError = new HandlerError();

            return response.IsError ? handleError.HandleErrorResponse(response.Errors) : NoContent();
        }



        [HttpDelete]
        [ValidateModel]
        [Route(ApiRoutes.UserProfiles.IdRoute)]
        public async Task<IActionResult> DeleteUserProfile(string id)
        {
            var command = new DeleteUserProfile() { UserProfileId = Guid.Parse(id) };
            var response = await _mediator.Send(command);

            var handleError = new HandlerError();

            return response.IsError ? handleError.HandleErrorResponse(response.Errors) : NoContent();
        }
    }
}
