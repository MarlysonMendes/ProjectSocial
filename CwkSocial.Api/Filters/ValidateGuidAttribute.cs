using CwkSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CwkSocial.Api.Filters
{
    public class ValidateGuidAttribute : ActionFilterAttribute
    {
        private readonly string _key;
        public ValidateGuidAttribute(string key)
        {
            _key = key;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue(_key, out var value)) return;
            if (Guid.TryParse(value?.ToString(), out var guid)) return;

            var apiError = new ErrorResponse();
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad request";
            apiError.Timestamp = DateTime.Now;
            context.Result = new ObjectResult(apiError);

            apiError.Errors.Add($"the identifier for {_key} is not a correct Guid format");
            context.Result = new ObjectResult(apiError);
        }
    }
}
