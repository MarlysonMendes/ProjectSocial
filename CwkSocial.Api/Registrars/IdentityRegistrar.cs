﻿using CwkSocial.Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CwkSocial.Api.Registrars
{
    public class IdentityRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var jwtSettings = new JwtSettings();
            builder.Configuration.Bind(nameof(JwtSettings),jwtSettings);

            var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
            builder.Services.Configure<JwtSettings>(jwtSection);

            builder.Services.AddAuthentication(e =>
            {
                e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
            })
             .AddJwtBearer(jwt =>
             {
                 jwt.SaveToken = true;
                 jwt.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                     ValidateIssuer = true,
                     ValidIssuer = jwtSettings.Issuer,
                     ValidateAudience = true,
                     ValidAudiences = jwtSettings.Audiences,
                     RequireExpirationTime = false,
                     ValidateLifetime = true
                 };
                 jwt.Audience = jwtSettings.Audiences[0];
                 jwt.ClaimsIssuer = jwtSettings.Issuer;
             });

        }
    }
}
