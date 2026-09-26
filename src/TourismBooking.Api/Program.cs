using Microsoft.EntityFrameworkCore;
using TourismBooking.Api.Middlewares;
using TourismBooking.Application.Interfaces;
using TourismBooking.Application.Services;
using TourismBooking.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tourism Booking API", Version = "v1", Description = "API de reservas de experiencias turísticas para el proceso de selección de Grupo Aviatur" });
});

// Configuración de EF Core con SQL Server
builder.Services.AddDbContext<TourismBookingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de Dependencias de Servicios de Aplicación
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Middleware de Excepciones Global
app.UseMiddleware<ExceptionHandlingMiddleware>();

//Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tourism Booking API v1"); });
}

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
