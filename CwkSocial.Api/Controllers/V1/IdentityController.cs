using AutoMapper;
using CwkSocial.Api.Contracts.Identity;
using CwkSocial.Api.Extensions;
using CwkSocial.Api.Filters;
using CwkSocial.Application.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CwkSocial.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.Registration)]
        [ValidateModel]
        public async Task<IActionResult> Register ([FromBody]UserRegistration registration)
        {
            var command = _mapper.Map<RegisterIdentityCommand>(registration);
            var result = await _mediator.Send(command);

            var handlerError = new HandlerError();
            if (result.IsError) return handlerError.HandleErrorResponse(result.Errors);

            var authenticationResult = new AuthenticationResult() { Token = result.Payload };

            return Ok(authenticationResult);
        }

        [HttpPost]
        [Route(ApiRoutes.Identity.login)]
        [ValidateModel]
        public async Task<IActionResult> Login(Login login)
        {
            var command = _mapper.Map<LoginIdentityCommand>(login);
            var result = await _mediator.Send(command);

            var handlerError = new HandlerError();
            if (result.IsError) return handlerError.HandleErrorResponse(result.Errors);

            var authenticationResult = new AuthenticationResult() { Token = result.Payload };

            return Ok(authenticationResult);
        }

        [HttpDelete]
        [Route(ApiRoutes.Identity.IdentityById)]
        [ValidateGuid("identityUserId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAccount(string identityUserId, CancellationToken token)
        {
            var identityUserGuid = Guid.Parse(identityUserId);
            var requestorGuid = HttpContext.GetIdentityIdClaimValue();
            var command = new RemoveAccountCommand
            {
                IdentityUserId = identityUserGuid,
                RequestorGuid = requestorGuid
            };
            var result = await _mediator.Send(command, token);
            var handlerError = new HandlerError();

            if (result.IsError) return handlerError.HandleErrorResponse(result.Errors);

            return NoContent();
        }

    }
}
