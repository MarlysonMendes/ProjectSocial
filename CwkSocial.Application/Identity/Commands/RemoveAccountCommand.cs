using CwkSocial.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Identity.Commands
{
    public class RemoveAccountCommand : IRequest<OperationResult<bool>>
    {
        public Guid IdentityUserId { get; set; }
        public Guid RequestorGuid { get; set; }
    }
}
