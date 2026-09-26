using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Application.DTOs;
using TourismBooking.Application.Services;
using TourismBooking.Domain.Entities;
using TourismBooking.Domain.Enums;
using TourismBooking.Infrastructure.Persistence;

namespace TourismBooking.Tests
{
    public class BookingServiceTests
    {
        /// <summary>
        /// Crea un contexto de base de datos en memoria para pruebas unitarias.
        /// </summary>
        /// <returns>El contexto de base de datos en memoria.</returns>
        private TourismBookingDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TourismBookingDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;

            return new TourismBookingDbContext(options);
        }

        /// <summary>
        /// Prueba unitaria para verificar que la creación de una reserva válida se realiza correctamente y que el valor total se calcula adecuadamente.
        /// </summary>
        /// <returns>El resultado de la creación de la reserva.</returns>
        [Fact]
        public async Task CreateAsync_ReservaValida_DebeCrearExitosamenteYCalcularValorTotal()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Experiences.Add(new Experience
            {
                Id = 1,
                Name = "Tour Monserrate",
                Description = "Recorrido por Monserrate",
                Destination = "Bogotá",
                PricePerPerson = 100000,
                MaxCapacity = 10,
                Status = ExperienceStatus.Active,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var service = new BookingService(context);
            var dto = new CreateBookingDto(1, DateTime.UtcNow.AddDays(1), 3, "Juan Garzón", "juan@test.com");

            // Act
            var result = await service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ExperienceId);
            Assert.Equal(3, result.PassengerCount);
            Assert.Equal(300000, result.TotalAmount); // $100.000 x 3 = $300.000
            Assert.Equal(BookingStatus.Confirmed, result.Status);
        }

        /// <summary>
        /// Prueba unitaria para verificar que al intentar crear una reserva que supera la capacidad máxima de la experiencia, se lanza una excepción adecuada.
        /// </summary>
        /// <returns>La excepción lanzada.</returns>
        [Fact]
        public async Task CreateAsync_SuperaCapacidadMaxima_DebeLanzarExcepcion()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Experiences.Add(new Experience
            {
                Id = 1,
                Name = "Parapente Suesca",
                Description = "Vuelo en parapente",
                Destination = "Suesca",
                PricePerPerson = 150000,
                MaxCapacity = 5,
                Status = ExperienceStatus.Active,
                CreatedAt = DateTime.UtcNow
            });

            // Simulamos que ya hay 4 cupos reservados para esa fecha
            context.Bookings.Add(new Booking
            {
                Id = 1,
                ExperienceId = 1,
                ActivityDate = DateTime.UtcNow.AddDays(2).Date,
                PassengerCount = 4,
                CustomerName = "Pedro Pérez",
                CustomerEmail = "pedro@test.com",
                TotalAmount = 600000,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var service = new BookingService(context);
            // Intentamos reservar 2 cupos más (4 + 2 = 6, lo cual supera los 5 permitidos)
            var dto = new CreateBookingDto(1, DateTime.UtcNow.AddDays(2), 2, "Juan Garzón", "juan@test.com");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
            Assert.Contains("La reserva supera la capacidad disponible", exception.Message);
        }

        /// <summary>
        /// Prueba unitaria para verificar que al intentar crear una reserva para una experiencia inactiva, se lanza una excepción adecuada.
        /// </summary>
        /// <returns>La excepción lanzada.</returns>
        [Fact]
        public async Task CreateAsync_ExperienciaInactiva_DebeLanzarExcepcion()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Experiences.Add(new Experience
            {
                Id = 1,
                Name = "Tour Inactivo",
                Description = "Tour no disponible",
                Destination = "Bogotá",
                PricePerPerson = 50000,
                MaxCapacity = 10,
                Status = ExperienceStatus.Inactive,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var service = new BookingService(context);
            var dto = new CreateBookingDto(1, DateTime.UtcNow.AddDays(1), 2, "Juan Garzón", "juan@test.com");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
            Assert.Contains("inactivas o inexistentes", exception.Message);
        }

        /// <summary>
        /// Prueba unitaria para verificar que al cancelar una reserva existente, su estado cambia a "Cancelada" correctamente.
        /// </summary>
        /// <returns>El resultado de la cancelación de la reserva.</returns>
        [Fact]
        public async Task CancelAsync_ReservaExistente_DebeCambiarEstadoACancelada()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Bookings.Add(new Booking
            {
                Id = 10,
                ExperienceId = 1,
                ActivityDate = DateTime.UtcNow.AddDays(3).Date,
                PassengerCount = 2,
                CustomerName = "Carlos Gómez",
                CustomerEmail = "carlos@test.com",
                TotalAmount = 200000,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var service = new BookingService(context);

            // Act
            var result = await service.CancelAsync(10);
            var bookingCancelada = await context.Bookings.FindAsync(10);

            // Assert
            Assert.True(result);
            Assert.NotNull(bookingCancelada);
            Assert.Equal(BookingStatus.Cancelled, bookingCancelada.Status);
        }
    }
}
