USE TourismBookingDb;
GO

INSERT INTO Experiences (Name, Description, Destination, PricePerPerson, MaxCapacity, Status, CreatedAt)
VALUES 
('Tour Candelaria y Monserrate', 'Recorrido histórico por Bogotá y ascenso al cerro de Monserrate', 'Bogotá', 85000.00, 10, 1, GETUTCDATE()),
('Pasadía Islas del Rosario', 'Excursión en lancha rápida con almuerzo típico caribeño incluido', 'Cartagena', 250000.00, 15, 1, GETUTCDATE()),
('Parapente en Suesca', 'Vuelo de aventura sobre la sabana con fotos incluidas', 'Suesca', 180000.00, 5, 2, GETUTCDATE()); -- Inactiva para pruebas
GO