using CwkSocial.Api.Extensions;
using CwkSocial.Api.Options;
using Microsoft.AspNetCore.Builder;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices(typeof(Program));

var app = builder.Build();
app.RegisterPipelineComponents(typeof(Program));

app.Run();
