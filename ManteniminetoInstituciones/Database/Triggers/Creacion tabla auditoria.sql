USE [Nombre_BD];
GO

CREATE TABLE [dbo].[auditoria_mantenimientos] (
        [id_auditoria]      BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [tabla_nombre]      VARCHAR(128) NOT NULL,
        [id_registro]       VARCHAR(100) NOT NULL,
        [operacion]         CHAR(6) NOT NULL, -- 'INSERT', 'UPDATE', 'DELETE'
        [usuario]           VARCHAR(128) NOT NULL,
        [fecha]             DATETIME2(3) NOT NULL CONSTRAINT DF_auditoria_mantenimientos_fecha DEFAULT GETDATE(),
        [app_name]          VARCHAR(256) NULL,
        [host_name]         VARCHAR(128) NULL,
        [datos_anteriores]  NVARCHAR(MAX) NULL, -- Estado ANTES de la modificación
        [datos_nuevos]      NVARCHAR(MAX) NULL -- Estado DESPUÉS de la modificación
    );
GO