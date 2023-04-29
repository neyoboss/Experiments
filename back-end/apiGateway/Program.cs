using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddAuthentication().AddJwtBearer("Auth0",options =>
{
    options.Authority = "https://dev-0ck6l5pnflrq01jd.eu.auth0.com";
    options.Audience ="https://tender-auth/";
    options.RequireHttpsMetadata = false;
});



var app = builder.Build();
await app.UseOcelot();

app.Run();
