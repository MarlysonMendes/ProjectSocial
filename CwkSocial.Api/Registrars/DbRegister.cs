﻿using CwkSocial.Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Api.Registrars
{
    public class DbRegister : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>( options =>
            {
                options.UseSqlServer(cs);
            });
            builder.Services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
