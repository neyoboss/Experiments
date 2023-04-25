using Auth0.AspNetCore.Authentication;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProfileService,ProfileService>();
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

// builder.Services.ConfigureSameSiteNoneCookies();

// builder.Services.AddAuth0WebAppAuthentication(options =>
// {
//     options.Domain = builder.Configuration["Auth0:Domain"];
//     options.ClientId = builder.Configuration["Auth0:ClientId"];
//     options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
// }).WithAccessToken(options => {
//     options.Audience=builder.Configuration["Auth0:Audience"];
//     options.UseRefreshTokens = true;
// });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // options.Authority = builder.Configuration["Auth0:Domain"];
    options.Authority = "https://dev-0ck6l5pnflrq01jd.eu.auth0.com";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.RequireHttpsMetadata = false;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
