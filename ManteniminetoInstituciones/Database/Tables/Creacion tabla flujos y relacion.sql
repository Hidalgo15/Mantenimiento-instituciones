USE SIGOB_CONTRALORIA
GO

-- 1. Tabla maestra de Flujos
CREATE TABLE [dbo].[flujos_catalogo] (
    [id_flujo] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre_flujo] VARCHAR(100) NOT NULL UNIQUE, -- Ej: 'Flujo Auditor Normal', 'Flujo Auditor Tecnico'
    [activo] BIT NOT NULL DEFAULT 1
);
GO

-- 2. Tabla de relación: Qué actividades pertenecen a cada flujo y molde
CREATE TABLE [dbo].[flujo_actividades_config] (
    [id_config] INT IDENTITY(1,1) PRIMARY KEY,
    [codigo_tre] INT NOT NULL,               -- Código del Molde (ej. 2033)
    [id_flujo] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[flujos_catalogo]([id_flujo]),
    [codigo_actividad] INT NOT NULL,         -- Código de la Actividad
    CONSTRAINT UQ_Molde_Flujo_Actividad UNIQUE ([codigo_tre], [id_flujo], [codigo_actividad])
);
GO