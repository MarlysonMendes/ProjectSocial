using CwkSocial.Api.Contracts.Common;
using CwkSocial.Application.Enums;
using CwkSocial.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.Api.Controllers.V1
{
    public class HandlerError : ControllerBase
    {
        
        public IActionResult HandleErrorResponse(List<Error> errors)
        {
            var apiError = new ErrorResponse();


            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad request";
            apiError.Timestamp = DateTime.Now;
            errors.ForEach(e => apiError.Errors.Add(e.Message));
            return StatusCode(400, apiError);
        }
    }
}


