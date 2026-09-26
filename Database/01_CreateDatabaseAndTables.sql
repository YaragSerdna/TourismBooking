IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Experiences] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(2000) NOT NULL,
    [Destination] nvarchar(150) NOT NULL,
    [PricePerPerson] decimal(18,2) NOT NULL,
    [MaxCapacity] int NOT NULL,
    [Status] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Experiences] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_Experiences_MaxCapacity_Positive] CHECK ([MaxCapacity] > 0),
    CONSTRAINT [CK_Experiences_PricePerPerson_Positive] CHECK ([PricePerPerson] > 0)
);
GO

CREATE TABLE [Bookings] (
    [Id] int NOT NULL IDENTITY,
    [ExperienceId] int NOT NULL,
    [ActivityDate] date NOT NULL,
    [PassengerCount] int NOT NULL,
    [CustomerName] nvarchar(150) NOT NULL,
    [CustomerEmail] nvarchar(254) NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Status] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_Bookings_PassengerCount_Positive] CHECK ([PassengerCount] > 0),
    CONSTRAINT [CK_Bookings_TotalAmount_NonNegative] CHECK ([TotalAmount] >= 0),
    CONSTRAINT [FK_Bookings_Experiences_ExperienceId] FOREIGN KEY ([ExperienceId]) REFERENCES [Experiences] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Bookings_ExperienceId_ActivityDate_Status] ON [Bookings] ([ExperienceId], [ActivityDate], [Status]);
GO

CREATE INDEX [IX_Experiences_Status] ON [Experiences] ([Status]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260926022724_InitialCreate', N'8.0.31');
GO

COMMIT;
GO

