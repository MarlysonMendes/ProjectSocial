using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Dal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Identity.Handlers
{
    public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<string>>
    {
        private readonly DataContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;


        public RegisterIdentityCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }
        public Task<OperationResult<string>> Handle(RegisterIdentityCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
