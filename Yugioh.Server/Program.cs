using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using Yugioh.Server.Context;
using Yugioh.Server.Model.UserModels;
using Yugioh.Server.Services.Api;
using Yugioh.Server.Services.AuthServices.Models;
using Yugioh.Server.Services.AuthServices.Seeder;
using Yugioh.Server.Services.AuthServices.Services;
using Yugioh.Server.Services.BusinessLogic;
using Yugioh.Server.Services.CardRepository;
using Yugioh.Server.Services.JsonProcess;
using Yugioh.Server.Services.UserRepository;
using Yugioh.Server.Utilities;

var builder = WebApplication.CreateBuilder(args);

AddServices();
ConfigureSwagger();
AddDbContext();
AddIdentity();
AddAuthentication();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var authSeeder = scope.ServiceProvider.GetRequiredService<AuthSeeder>();
authSeeder.AddRoles();
authSeeder.AddAmin();
authSeeder.AddTestUser();

app.UseCors();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "YugiohPics")),
    RequestPath = "/YugiohPics"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddLogging();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddScoped<IBusinessAllCard, Business>();
    builder.Services.AddScoped<IBusinessSingleCard, Business>();
    builder.Services.AddScoped<ICardRepoAllCard, CardRepo>();
    builder.Services.AddScoped<ICardRepoSingleCard, CardRepo>();
    builder.Services.AddScoped<IRandomRowSelector, RandomRowSelector>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserRepoSingle, UserRepo>();
    builder.Services.AddScoped<IUserRepoMultiple, UserRepo>();
    builder.Services.AddScoped<AuthSeeder>();
    builder.Services.AddSingleton<IYugiohApiAllCard, YugiohApi>();
    builder.Services.AddSingleton<IYugiohApiSingleCard, YugiohApi>();
    builder.Services.AddSingleton<IJsonProcessAllCard, YugiohJsonProcessor>();
    builder.Services.AddSingleton<IJsonProcessSingleCard, YugiohJsonProcessor>();
    // Add CORS services
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                   .WithHeaders(HeaderNames.ContentType, "x-custom-header")
                   .WithMethods("PUT", "DELETE", "GET", "PATCH", "POST");
        });
    });
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}
void AddDbContext()
{
    var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
    builder.Services.AddDbContext<CardsContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
    builder.Services.AddDbContext<UsersContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UsersContext>();
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "Authorization";
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
            var issuerSigningKey = builder.Configuration.GetSection("IssuerSigningKey").Value ?? "";
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.ContainsKey("Authorization"))
                    {
                        context.Token = context.Request.Cookies["Authorization"];
                    }
                    return Task.CompletedTask;
                }
            };
        });
}
