using CwkSocial.Api.Extensions;
using CwkSocial.Api.Options;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices(typeof(Program));

var app = builder.Build();
builder.RegisterPipelineComponents(typeof(Program));

app.Run();
