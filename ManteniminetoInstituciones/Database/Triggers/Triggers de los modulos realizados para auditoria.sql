USE [Nombre_BD];
GO

-- =========================================================================
-- 1. TRIGGER AUDITORÍA: capitulo
-- =========================================================================
CREATE OR ALTER TRIGGER [dbo].[TR_capitulo_Auditoria]
ON [dbo].[capitulo]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'capitulo';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.id AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.id = d.id;
    END
END;
GO

-- =========================================================================
-- 2. TRIGGER AUDITORÍA: sub_capitulo
-- =========================================================================
CREATE OR ALTER TRIGGER [dbo].[TR_sub_capitulo_Auditoria]
ON [dbo].[sub_capitulo]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'sub_capitulo';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.id AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.id = d.id;
    END
END;
GO

-- =========================================================================
-- 3. TRIGGER AUDITORÍA: daf
-- =========================================================================
CREATE OR ALTER TRIGGER [dbo].[TR_daf_Auditoria]
ON [dbo].[daf]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'daf';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.id AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.id = d.id;
    END
END;
GO

-- =========================================================================
-- 4. TRIGGER AUDITORÍA: t_cta_institucion
-- =========================================================================
CREATE OR ALTER TRIGGER [dbo].[TR_t_cta_institucion_Auditoria]
ON [dbo].[t_cta_institucion]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 't_cta_institucion';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.ID AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.ID AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.ID AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.ID = d.ID;
    END
END;
GO

-- =========================================================================
-- 5. TRIGGER AUDITORÍA: Fondo_especiales
-- =========================================================================
CREATE OR ALTER TRIGGER [dbo].[TR_Fondo_especiales_Auditoria]
ON [dbo].[Fondo_especiales]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'Fondo_especiales';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.Id AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.Id AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.Id AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.Id = d.Id;
    END
END;
GO

-- =========================================================================
-- 6. TRIGGER AUDITORÍA: analista_supervisor
-- =========================================================================
CREATE OR ALTER TRIGGER [dbo].[TR_analista_supervisor_Auditoria]
ON [dbo].[analista_supervisor]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'analista_supervisor';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.Id AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.Id AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.Id AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.Id = d.Id;
    END
END;
GO

-- =========================================================================
-- 7. TRIGGER AUDITORÍA: unidad_ejecutora
-- =========================================================================

CREATE OR ALTER TRIGGER [dbo].[TR_unidad_ejecutora_Auditoria]
ON [dbo].[unidad_ejecutora]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Previene que los UPDATE ejecutados por otros triggers generen duplicados
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
        RETURN;

    DECLARE @tabla VARCHAR(128) = 'unidad_ejecutora';
    DECLARE @operacion CHAR(6);
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();

    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
        SET @operacion = 'UPDATE';
    ELSE IF EXISTS (SELECT 1 FROM inserted)
        SET @operacion = 'INSERT';
    ELSE
        SET @operacion = 'DELETE';

    -------------------------------------------------------------------------
    -- INSERT
    -------------------------------------------------------------------------
    IF @operacion = 'INSERT'
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla,
            CAST(i.id AS VARCHAR(100)),
            @operacion,
            -- Prioriza el usuario_creacion guardado por la app; si viene NULL/vacío usa SUSER_SNAME()
            ISNULL(NULLIF(i.usuario_creacion, ''), SUSER_SNAME()),
            @app_name,
            @host_name,
            NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END

    -------------------------------------------------------------------------
    -- DELETE
    -------------------------------------------------------------------------
    ELSE IF @operacion = 'DELETE'
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla,
            CAST(d.id AS VARCHAR(100)),
            @operacion,
            -- En borrados toma la última persona que modificó el registro o el usuario de conexión
            ISNULL(NULLIF(d.usuario_modificacion, ''), SUSER_SNAME()),
            @app_name,
            @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL
        FROM deleted d;
    END

    -------------------------------------------------------------------------
    -- UPDATE
    -------------------------------------------------------------------------
    ELSE IF @operacion = 'UPDATE'
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla,
            CAST(i.id AS VARCHAR(100)),
            @operacion,
            -- Prioriza el usuario_modificacion enviado por la app; si viene NULL/vacío usa SUSER_SNAME()
            ISNULL(NULLIF(i.usuario_modificacion, ''), SUSER_SNAME()),
            @app_name,
            @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.id = d.id;
    END
END;
GO

-- =========================================================================
-- 8. TRIGGER AUDITORÍA: cuentas (CuentaMapeo)
-- =========================================================================

CREATE OR ALTER TRIGGER [dbo].[TR_cuentas_Auditoria]
ON [dbo].[cuentas]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'cuentas';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id_cuenta AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.id_cuenta AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id_cuenta AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.id_cuenta = d.id_cuenta;
    END
END;
GO

-- =========================================================================
-- 9. TRIGGER AUDITORÍA: cuentas (CuentaCt)
-- =========================================================================

CREATE OR ALTER TRIGGER [ct].[TR_ct_cuentas_Auditoria]
ON [ct].[cuentas]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'ct.cuentas';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.CuentaID AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.CuentaID AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.CuentaID AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.CuentaID = d.CuentaID;
    END
END;
GO

-- =========================================================================
-- 10. TRIGGER AUDITORÍA: requisitos
-- =========================================================================

CREATE OR ALTER TRIGGER [ct].[TR_ct_requisitos_Auditoria]
ON [ct].[requisitos]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'requisitos';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.RequisitosID AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.RequisitosID AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.RequisitosID AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.RequisitosID = d.RequisitosID;
    END
END;
GO

-- =========================================================================
-- 11. TRIGGER AUDITORÍA: relacion_cuenta_requisitos
-- =========================================================================

CREATE OR ALTER TRIGGER [ct].[TR_ct_relacion_cuenta_requisitos_Auditoria]
ON [ct].[relacion_cuenta_requisitos]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'relacion_cuenta_requisitos';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.CuentaRequisitosID AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.CuentaRequisitosID AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.CuentaRequisitosID AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.CuentaRequisitosID = d.CuentaRequisitosID;
    END
END;
GO

-- =========================================================================
-- 12. TRIGGER AUDITORÍA: requisitos_contratos
-- =========================================================================

CREATE OR ALTER TRIGGER [dbo].[TR_requisitos_contratos_Auditoria]
ON [dbo].[requisitos_contratos]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'requisitos_contratos';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.codigo_requisito AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.codigo_requisito AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.codigo_requisito AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.codigo_requisito = d.codigo_requisito;
    END
END;
GO

-- =========================================================================
-- 13. TRIGGER AUDITORÍA: clasificacion_contratos
-- =========================================================================

CREATE OR ALTER TRIGGER [dbo].[TR_clasificacion_contratos_Auditoria]
ON [dbo].[clasificacion_contratos]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'clasificacion_contratos';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.codigo_clasificacion AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.codigo_clasificacion AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.codigo_clasificacion AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.codigo_clasificacion = d.codigo_clasificacion;
    END
END;
GO

-- =========================================================================
-- 14. TRIGGER AUDITORÍA: relacion_contrato_requisito
-- =========================================================================

CREATE OR ALTER TRIGGER [dbo].[TR_relacion_contrato_requisito_Auditoria]
ON [dbo].[relacion_contrato_requisito]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF TRIGGER_NESTLEVEL() > 1 RETURN;
    IF NOT EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted) RETURN;

    DECLARE @tabla VARCHAR(128) = 'relacion_contrato_requisito';
    DECLARE @app_name VARCHAR(256) = APP_NAME();
    DECLARE @host_name VARCHAR(128) = HOST_NAME();
    DECLARE @usuario VARCHAR(128) = ISNULL(CAST(SESSION_CONTEXT(N'UsuarioApp') AS VARCHAR(128)), SUSER_SNAME());

    IF EXISTS (SELECT 1 FROM inserted) AND NOT EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'INSERT', @usuario, @app_name, @host_name, NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i;
    END
    ELSE IF EXISTS (SELECT 1 FROM deleted) AND NOT EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(d.id AS VARCHAR(100)), 'DELETE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER), NULL
        FROM deleted d;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO [dbo].[auditoria_mantenimientos] 
            ([tabla_nombre], [id_registro], [operacion], [usuario], [app_name], [host_name], [datos_anteriores], [datos_nuevos])
        SELECT 
            @tabla, CAST(i.id AS VARCHAR(100)), 'UPDATE', @usuario, @app_name, @host_name,
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM inserted i
        INNER JOIN deleted d ON i.id = d.id;
    END
END;
GO

---