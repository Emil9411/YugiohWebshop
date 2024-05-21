using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Yugioh.Server.Context;
using Yugioh.Server.Services.Api;
using Yugioh.Server.Services.BusinessLogic;
using Yugioh.Server.Services.CardRepository;
using Yugioh.Server.Services.JsonProcess;
using Yugioh.Server.Utilities;

var builder = WebApplication.CreateBuilder(args);

AddServices();
ConfigureSwagger();
AddDbContext();

var app = builder.Build();

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

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IBusinessAllCard, Business>();
    builder.Services.AddScoped<IBusinessSingleCard, Business>();
    builder.Services.AddScoped<ICardRepoAllCard, CardRepo>();
    builder.Services.AddScoped<ICardRepoSingleCard, CardRepo>();
    builder.Services.AddScoped<IRandomRowSelector, RandomRowSelector>();
    builder.Services.AddSingleton<IYugiohApiAllCard, YugiohApi>();
    builder.Services.AddSingleton<IYugiohApiSingleCard, YugiohApi>();
    builder.Services.AddSingleton<IJsonProcessAllCard, YugiohJsonProcessor>();
    builder.Services.AddSingleton<IJsonProcessSingleCard, YugiohJsonProcessor>();
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen();
}
void AddDbContext()
{
    var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
    builder.Services.AddDbContext<CardsContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
}
