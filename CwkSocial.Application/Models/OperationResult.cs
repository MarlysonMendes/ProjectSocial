using CwkSocial.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Models
{
    public class OperationResult<T>
    {
        public T Payload { get; set; }
        public bool IsError { get;private set; }
        public List<Error> Errors { get; } = new List<Error>();
        public void AddError(ErrorCode code, string message)
        {
            Errors.Add(new Error { Code = code, Message = message });
            IsError = true;
        }
        public void ResetIsErrorFlag()
        {
            IsError = false;
        }
    }
}
