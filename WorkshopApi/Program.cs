using Microsoft.EntityFrameworkCore;
using WorkshopApi.Models.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WorkShopDbContext>(options => options.UseSqlServer("Data Source=AC-ADEMASI\\SQLEXPRESS;Initial Catalog=FormazioneDb;Integrated Security=True;TrustServerCertificate=true;"));
builder.Services.AddHttpClient().AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
