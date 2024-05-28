using Microsoft.EntityFrameworkCore;
using CrawlerProject_API.Models;
using Microsoft.OpenApi.Models;
using CrawlerProject_API.Data;
using CrawlerProject_API.Repository.IRepository;
using CrawlerProject_API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("CrawlerProjectDb"));
});

builder.Services.AddScoped<IConferenceRepository, ConferenceRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Crawler Project V1",
        Description = "API to manage Crawler Project",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CrawlerProject_V1");
        options.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
