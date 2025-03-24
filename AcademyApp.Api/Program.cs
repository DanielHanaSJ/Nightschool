using AcademyApp.Api.Contracts;
using AcademyApp.Api.Database.Dapper;
using AcademyApp.Api.Database.EfCore;
using AcademyApp.Api.Helpers;
using AcademyApp.Api.Middleware;
using AcademyApp.Api.Models;
using AcademyApp.Api.Services.Dapper;
using AcademyApp.Api.Services.EfCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme, e.g. \"bearer {token} \"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Nightschool API", Version = "v1" });
    x.EnableAnnotations();
});

builder.Services.AddScoped<IListService, DapperListService>();
builder.Services.AddScoped<IAuthService, DapperAuthService>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
if (appSettings == null)
{
    throw new InvalidOperationException("AppSettings configuration is missing.");
}

if (connectionStrings == null)
{
    throw new InvalidOperationException("ConnectionStrings configuration is missing.");
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(appSettings);
builder.Services.AddSingleton(connectionStrings);

builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(connectionStrings.NightschoolDB);
});

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(appSettings.TokenSecret)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
