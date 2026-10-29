USE [Nombre_BD]
GO
/****** Object:  StoredProcedure [ct].[sp_Actualizar_Relacion_Cuenta_Requisitos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---

CREATE   PROCEDURE [ct].[sp_Actualizar_Relacion_Cuenta_Requisitos]
    @CuentaRequisitosID INT,
    @IdCuenta FLOAT,
    @IdRequisito INT,
    @Estado BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- 1. Validar que el registro exista
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] WITH (NOLOCK) WHERE CuentaRequisitosID = @CuentaRequisitosID)
        BEGIN
            RAISERROR('La relación cuenta-requisito a modificar no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar existencia de la cuenta y requisito nuevos
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[cuentas] WITH (NOLOCK) WHERE id_cuenta = @IdCuenta)
        BEGIN
            RAISERROR('La cuenta especificada no existe.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK) WHERE id_requisitos = @IdRequisito)
        BEGIN
            RAISERROR('El requisito especificado no existe.', 16, 1);
            RETURN;
        END

        -- 3. Validar duplicados si se cambia a otra combinación ya existente en otro ID
        IF EXISTS (
            SELECT 1 
            FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] WITH (NOLOCK)
            WHERE id_cuenta = @IdCuenta AND id_requisito = @IdRequisito AND CuentaRequisitosID <> @CuentaRequisitosID
        )
        BEGIN
            RAISERROR('Ya existe otra relación registrada con esta misma cuenta y requisito.', 16, 1);
            RETURN;
        END

        -- 4. Actualización
        BEGIN TRANSACTION;

        UPDATE [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos]
        SET id_cuenta = @IdCuenta,
            id_requisito = @IdRequisito,
            estado = @Estado
        WHERE CuentaRequisitosID = @CuentaRequisitosID;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Relación actualizada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Actualizar_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--

CREATE   PROCEDURE [ct].[sp_Actualizar_Requisito]
    @RequisitosID INT,
    @RequisitoDescripcion NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK) WHERE RequisitosID = @RequisitosID)
        BEGIN
            RAISERROR('El requisito que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        IF @RequisitoDescripcion IS NULL OR LTRIM(RTRIM(@RequisitoDescripcion)) = ''
        BEGIN
            RAISERROR('La descripción del requisito no puede estar vacía.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        UPDATE [SIGOB_CONTRALORIA].[ct].[requisitos]
        SET REQUISITOS = LTRIM(RTRIM(@RequisitoDescripcion))
        WHERE RequisitosID = @RequisitosID;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Requisito actualizado exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Asignar_Cuenta_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CREATE OR ALTER PROCEDURE [ct].[sp_Obtener_Relacion_Cuenta_Requisitos]
--    @IdCuenta FLOAT = NULL,
--    @IdRequisito INT = NULL,
--    @Estado BIT = NULL
--AS
--BEGIN
--    SET NOCOUNT ON;

--    SELECT 
--        r.CuentaRequisitosID,
--        r.id_cuenta,
--        c.codigo_cuenta,
--        c.descripcion_cuenta,
--        r.id_requisito,
--        req.REQUISITOS AS descripcion_requisito,
--        r.estado
--    FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] r WITH (NOLOCK)
--    INNER JOIN [SIGOB_CONTRALORIA].[ct].[cuentas] c WITH (NOLOCK) 
--        ON c.id_cuenta = r.id_cuenta
--    INNER JOIN [SIGOB_CONTRALORIA].[ct].[requisitos] req WITH (NOLOCK) 
--        ON req.id_requisitos = r.id_requisito
--    WHERE 
--        (@IdCuenta IS NULL OR r.id_cuenta = @IdCuenta)
--        AND (@IdRequisito IS NULL OR r.id_requisito = @IdRequisito)
--        AND (@Estado IS NULL OR r.estado = @Estado)
--    ORDER BY c.codigo_cuenta ASC, req.REQUISITOS ASC
--    OPTION (RECOMPILE);
--END;
--GO

---

CREATE   PROCEDURE [ct].[sp_Asignar_Cuenta_Requisito]
    @IdCuenta FLOAT,
    @IdRequisito INT,
    @Estado BIT = 1 -- Por defecto activo al crear
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- 1. Validar existencia de la cuenta
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[cuentas] WITH (NOLOCK) WHERE id_cuenta = @IdCuenta)
        BEGIN
            RAISERROR('La cuenta especificada no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar existencia del requisito
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK) WHERE id_requisitos = @IdRequisito)
        BEGIN
            RAISERROR('El requisito especificado no existe.', 16, 1);
            RETURN;
        END

        -- 3. Validar que la relación no exista previamente
        IF EXISTS (
            SELECT 1 
            FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] WITH (NOLOCK)
            WHERE id_cuenta = @IdCuenta AND id_requisito = @IdRequisito
        )
        BEGIN
            RAISERROR('Esta cuenta ya tiene asignado este requisito.', 16, 1);
            RETURN;
        END

        -- 4. Inserción
        BEGIN TRANSACTION;

        INSERT INTO [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] (id_cuenta, id_requisito, estado)
        VALUES (@IdCuenta, @IdRequisito, @Estado);

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Relación de cuenta y requisito creada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Crear_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--

CREATE   PROCEDURE [ct].[sp_Crear_Requisito]
    @RequisitoDescripcion NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF @RequisitoDescripcion IS NULL OR LTRIM(RTRIM(@RequisitoDescripcion)) = ''
        BEGIN
            RAISERROR('La descripción del requisito es obligatoria.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        -- Insertamos en la tabla temporalmente habilitando IDENTITY_INSERT para emparejar id_requisitos con RequisitosID
        DECLARE @NuevoID INT;
        
        -- Si id_requisitos no es identity sino que depende o se llena igual, calculamos el siguiente ID
        SELECT @NuevoID = ISNULL(MAX(RequisitosID), 0) + 1 FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (TABLOCKX);

        SET IDENTITY_INSERT [SIGOB_CONTRALORIA].[ct].[requisitos] ON;

        INSERT INTO [SIGOB_CONTRALORIA].[ct].[requisitos] (RequisitosID, id_requisitos, REQUISITOS)
        VALUES (@NuevoID, @NuevoID, LTRIM(RTRIM(@RequisitoDescripcion)));

        SET IDENTITY_INSERT [SIGOB_CONTRALORIA].[ct].[requisitos] OFF;

        COMMIT TRANSACTION;

        SELECT @NuevoID AS RequisitosID, 'Requisito creado exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Asegurar apagar IDENTITY_INSERT en caso de error
        BEGIN TRY
            SET IDENTITY_INSERT [SIGOB_CONTRALORIA].[ct].[requisitos] OFF;
        END TRY
        BEGIN CATCH END CATCH

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [ct].[sp_CuentasCt_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [ct].[sp_CuentasCt_Actualizar]
    @CuentaID INT,
    @id_cuenta FLOAT = NULL,
    @codigo_cuenta NVARCHAR(255),
    @descripcion_cuenta NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [ct].[cuentas]
    SET [id_cuenta] = @id_cuenta,
        [codigo_cuenta] = @codigo_cuenta,
        [descripcion_cuenta] = @descripcion_cuenta
    WHERE [CuentaID] = @CuentaID;
END
GO
/****** Object:  StoredProcedure [ct].[sp_CuentasCt_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [ct].[sp_CuentasCt_Eliminar]
    @CuentaID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [ct].[cuentas]
    WHERE [CuentaID] = @CuentaID;
END
GO
/****** Object:  StoredProcedure [ct].[sp_CuentasCt_Insertar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [ct].[sp_CuentasCt_Insertar]
    @id_cuenta FLOAT = NULL,
    @codigo_cuenta NVARCHAR(255),
    @descripcion_cuenta NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [ct].[cuentas] ([id_cuenta], [codigo_cuenta], [descripcion_cuenta])
    VALUES (@id_cuenta, @codigo_cuenta, @descripcion_cuenta);

    SELECT SCOPE_IDENTITY() AS CuentaID;
END
GO
/****** Object:  StoredProcedure [ct].[sp_CuentasCt_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [ct].[sp_CuentasCt_Obtener]
    @CuentaID INT = NULL,
    @busqueda NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [CuentaID],
        [id_cuenta],
        RTRIM([codigo_cuenta]) AS [codigo_cuenta],
        RTRIM([descripcion_cuenta]) AS [descripcion_cuenta]
    FROM [ct].[cuentas]
    WHERE (@CuentaID IS NULL OR [CuentaID] = @CuentaID)
      AND (@busqueda IS NULL 
           OR [codigo_cuenta] LIKE '%' + @busqueda + '%' 
           OR [descripcion_cuenta] LIKE '%' + @busqueda + '%')
    ORDER BY [codigo_cuenta] ASC;
END
GO
/****** Object:  StoredProcedure [ct].[sp_Eliminar_Relacion_Cuenta_Requisitos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---

CREATE   PROCEDURE [ct].[sp_Eliminar_Relacion_Cuenta_Requisitos]
    @CuentaRequisitosID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] WITH (NOLOCK) WHERE CuentaRequisitosID = @CuentaRequisitosID)
        BEGIN
            RAISERROR('La relación que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos]
        WHERE CuentaRequisitosID = @CuentaRequisitosID;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Relación eliminada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Eliminar_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--

CREATE   PROCEDURE [ct].[sp_Eliminar_Requisito]
    @RequisitosID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK) WHERE RequisitosID = @RequisitosID)
        BEGIN
            RAISERROR('El requisito que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        -- Validar integridad referencial: No permitir borrar si está asociado a cuentas
        IF EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] WITH (NOLOCK) WHERE id_requisito = @RequisitosID)
        BEGIN
            RAISERROR('No se puede eliminar el requisito porque se encuentra asociado a una o más cuentas.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [SIGOB_CONTRALORIA].[ct].[requisitos]
        WHERE RequisitosID = @RequisitosID;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Requisito eliminado exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Obtener_Catalogo_ctCuentas]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Obtener catálogo de cuentas para dropdown
CREATE   PROCEDURE [ct].[sp_Obtener_Catalogo_ctCuentas]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_cuenta, codigo_cuenta, descripcion_cuenta
    FROM [SIGOB_CONTRALORIA].[ct].[cuentas] WITH (NOLOCK)
    ORDER BY codigo_cuenta ASC;
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Obtener_Catalogo_ctRequisitos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Obtener catálogo de requisitos para dropdown
CREATE   PROCEDURE [ct].[sp_Obtener_Catalogo_ctRequisitos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id_requisitos, REQUISITOS
    FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK)
    ORDER BY REQUISITOS ASC;
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Obtener_Relacion_Cuenta_Requisitos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [ct].[sp_Obtener_Relacion_Cuenta_Requisitos]
    @IdCuenta FLOAT = NULL,
    @IdRequisito INT = NULL,
    @Estado BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.CuentaRequisitosID,
        r.id_cuenta,
        c.codigo_cuenta,
        c.descripcion_cuenta,
        r.id_requisito,
        req.REQUISITOS AS descripcion_requisito,
        r.estado
    FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] r WITH (NOLOCK)
    INNER JOIN [SIGOB_CONTRALORIA].[ct].[cuentas] c WITH (NOLOCK) 
        ON c.id_cuenta = r.id_cuenta
    INNER JOIN [SIGOB_CONTRALORIA].[ct].[requisitos] req WITH (NOLOCK) 
        ON req.id_requisitos = r.id_requisito
    WHERE 
        (@IdCuenta IS NULL OR r.id_cuenta = @IdCuenta)
        AND (@IdRequisito IS NULL OR r.id_requisito = @IdRequisito)
        AND (@Estado IS NULL OR r.estado = @Estado)
    ORDER BY c.codigo_cuenta ASC, req.REQUISITOS ASC
    OPTION (RECOMPILE);
END;
GO
/****** Object:  StoredProcedure [ct].[sp_Obtener_Requisitos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Store Procedures de Requisitos

CREATE   PROCEDURE [ct].[sp_Obtener_Requisitos]
    @Busqueda NVARCHAR(MAX) = NULL -- Filtro opcional por descripción del requisito
AS
BEGIN
    SET NOCOUNT ON;

    SET @Busqueda = NULLIF(LTRIM(RTRIM(@Busqueda)), '');

    SELECT 
        RequisitosID,
        id_requisitos,
        REQUISITOS
    FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK)
    WHERE 
        (@Busqueda IS NULL OR REQUISITOS LIKE '%' + @Busqueda + '%')
    ORDER BY RequisitosID ASC
    OPTION (RECOMPILE);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Actualizar_Relacion_Contrato_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----

CREATE   PROCEDURE [dbo].[sp_Actualizar_Relacion_Contrato_Requisito]
    @Id INT,
    @CodigoClasificacion INT,
    @CodigoRequisito INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- 1. Validar que el registro exista
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] WITH (NOLOCK) WHERE id = @Id)
        BEGIN
            RAISERROR('La relación contrato-requisito que intenta modificar no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar existencia de la clasificación y requisito nuevos
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[dbo].[clasificacion_contratos] WITH (NOLOCK) WHERE codigo_clasificacion = @CodigoClasificacion)
        BEGIN
            RAISERROR('La clasificación de contrato especificada no existe.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[dbo].[requisitos_contratos] WITH (NOLOCK) WHERE codigo_requisito = @CodigoRequisito)
        BEGIN
            RAISERROR('El requisito especificado no existe.', 16, 1);
            RETURN;
        END

        -- 3. Validar duplicados en otro ID
        IF EXISTS (
            SELECT 1 
            FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] WITH (NOLOCK)
            WHERE codigo_clasificacion = @CodigoClasificacion AND codigo_requisito = @CodigoRequisito AND id <> @Id
        )
        BEGIN
            RAISERROR('Ya existe otra relación registrada con esta misma clasificación y requisito.', 16, 1);
            RETURN;
        END

        -- 4. Actualización
        BEGIN TRANSACTION;

        UPDATE [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito]
        SET codigo_clasificacion = @CodigoClasificacion,
            codigo_requisito = @CodigoRequisito
        WHERE id = @Id;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Relación actualizada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Asignar_Contrato_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----

CREATE   PROCEDURE [dbo].[sp_Asignar_Contrato_Requisito]
    @CodigoClasificacion INT,
    @CodigoRequisito INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- 1. Validar existencia de la clasificación de contrato
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[dbo].[clasificacion_contratos] WITH (NOLOCK) WHERE codigo_clasificacion = @CodigoClasificacion)
        BEGIN
            RAISERROR('La clasificación de contrato especificada no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar existencia del requisito de contrato
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[dbo].[requisitos_contratos] WITH (NOLOCK) WHERE codigo_requisito = @CodigoRequisito)
        BEGIN
            RAISERROR('El requisito especificado no existe.', 16, 1);
            RETURN;
        END

        -- 3. Validar que la relación no exista previamente
        IF EXISTS (
            SELECT 1 
            FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] WITH (NOLOCK)
            WHERE codigo_clasificacion = @CodigoClasificacion AND codigo_requisito = @CodigoRequisito
        )
        BEGIN
            RAISERROR('Esta clasificación de contrato ya tiene asignado este requisito.', 16, 1);
            RETURN;
        END

        -- 4. Inserción
        BEGIN TRANSACTION;

        INSERT INTO [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] (codigo_clasificacion, codigo_requisito)
        VALUES (@CodigoClasificacion, @CodigoRequisito);

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Relación de contrato y requisito creada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Asignar_Supervisor_Analista]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Asignar_Supervisor_Analista]
    @DespachoAnalista VARCHAR(20),
    @DespachoSupervisor VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON; -- Si ocurre un error, cancela y revierte automáticamente la transacción

    BEGIN TRY
        -- 1. Validaciones previas de parámetros de entrada
        IF @DespachoAnalista IS NULL OR LTRIM(RTRIM(@DespachoAnalista)) = ''
        BEGIN
            RAISERROR('El código de despacho del analista es obligatorio.', 16, 1);
            RETURN;
        END

        IF @DespachoSupervisor IS NULL OR LTRIM(RTRIM(@DespachoSupervisor)) = ''
        BEGIN
            RAISERROR('El código de despacho del supervisor es obligatorio.', 16, 1);
            RETURN;
        END

        -- 2. Validar que el analista NO tenga un supervisor ya asignado
        IF EXISTS (
            SELECT 1 
            FROM [SIGOB_Contraloria].[dbo].[analista_supervisor] WITH (NOLOCK)
            WHERE despacho_analista = @DespachoAnalista
        )
        BEGIN
            RAISERROR('El analista ya tiene un supervisor asignado. Utilice la función de Reasignar/Cambiar.', 16, 1);
            RETURN;
        END

        -- 3. Inserción con transacción explícita
        BEGIN TRANSACTION;

        INSERT INTO [SIGOB_Contraloria].[dbo].[analista_supervisor] (despacho_analista, despacho_supervisor)
        VALUES (@DespachoAnalista, @DespachoSupervisor);

        COMMIT TRANSACTION;

        -- Retorno explícito para confirmación en aplicación
        SELECT 1 AS CodigoResultado, 'Supervisor asignado exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_capitulo_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_capitulo_Actualizar]
    @id INT,
    @codigo_capitulo VARCHAR(10),
    @capitulo VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar si el ID existe
        IF NOT EXISTS (SELECT 1 FROM [dbo].[capitulo] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('El registro que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        -- Validar que el código no esté en uso por otro registro
        IF EXISTS (SELECT 1 FROM [dbo].[capitulo] WITH (NOLOCK) WHERE [codigo_capitulo] = @codigo_capitulo AND [id] <> @id)
        BEGIN
            RAISERROR('El código de capítulo asignado ya está siendo usado por otro registro.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        UPDATE [dbo].[capitulo]
        SET 
            [codigo_capitulo] = @codigo_capitulo,
            [capitulo] = UPPER(TRIM(@capitulo))
        WHERE [id] = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_capitulo_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_capitulo_Eliminar]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[capitulo] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('El registro que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [dbo].[capitulo]
        WHERE [id] = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_capitulo_Insertar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_capitulo_Insertar]
    @codigo_capitulo VARCHAR(10),
    @capitulo VARCHAR(200),
    @id_generado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar si ya existe el código
        IF EXISTS (SELECT 1 FROM [dbo].[capitulo] WITH (NOLOCK) WHERE [codigo_capitulo] = @codigo_capitulo)
        BEGIN
            RAISERROR('El código de capítulo ya se encuentra registrado.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        INSERT INTO [dbo].[capitulo] ([codigo_capitulo], [capitulo])
        VALUES (@codigo_capitulo, UPPER(TRIM(@capitulo)));

        SET @id_generado = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_capitulo_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_capitulo_Obtener]
    @id INT = NULL,
    @codigo VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Obtener por ID
    IF @id IS NOT NULL
    BEGIN
        SELECT 
            [id],
            [codigo_capitulo],
            [capitulo]
        FROM [dbo].[capitulo] WITH (NOLOCK)
        WHERE [id] = @id;

        RETURN;
    END

    -- 2. Obtener por código exacto
    IF @codigo IS NOT NULL
    BEGIN
        SELECT 
            [id],
            [codigo_capitulo],
            [capitulo]
        FROM [dbo].[capitulo] WITH (NOLOCK)
        WHERE [codigo_capitulo] = @codigo;

        RETURN;
    END

    -- 3. Si no se pasa ningún parámetro, listar TODOS
    SELECT 
        [id],
        [codigo_capitulo],
        [capitulo]
    FROM [dbo].[capitulo] WITH (NOLOCK)
    ORDER BY [codigo_capitulo] ASC;
END;

GO
/****** Object:  StoredProcedure [dbo].[sp_clasificacion_contratos_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----
-----

CREATE PROCEDURE [dbo].[sp_clasificacion_contratos_Actualizar]
    @codigo_clasificacion INT,
    @contrato_tipo VARCHAR(50) = NULL,
    @tipo_operacion VARCHAR(50) = NULL,
    @contrato_proceso_seleccion VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar existencia del registro
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[clasificacion_contratos] WITH (NOLOCK) 
            WHERE [codigo_clasificacion] = @codigo_clasificacion
        )
        BEGIN
            RAISERROR('El registro de clasificación que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        -- Validar duplicidad con otro registro diferente al actual
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
            WHERE ISNULL([contrato_tipo], '') = ISNULL(@contrato_tipo, '')
              AND ISNULL([tipo_operacion], '') = ISNULL(@tipo_operacion, '')
              AND ISNULL([contrato_proceso_seleccion], '') = ISNULL(@contrato_proceso_seleccion, '')
              AND [codigo_clasificacion] <> @codigo_clasificacion
        )
        BEGIN
            RAISERROR('Ya existe otra clasificación registrada con esta misma combinación de datos.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        UPDATE [dbo].[clasificacion_contratos]
        SET 
            [contrato_tipo] = NULLIF(TRIM(@contrato_tipo), ''),
            [tipo_operacion] = NULLIF(TRIM(@tipo_operacion), ''),
            [contrato_proceso_seleccion] = NULLIF(TRIM(@contrato_proceso_seleccion), '')
        WHERE [codigo_clasificacion] = @codigo_clasificacion;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_clasificacion_contratos_Crear]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----
-----

CREATE PROCEDURE [dbo].[sp_clasificacion_contratos_Crear]
    @contrato_tipo VARCHAR(50) = NULL,
    @tipo_operacion VARCHAR(50) = NULL,
    @contrato_proceso_seleccion VARCHAR(50) = NULL,
    @codigo_generado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar si ya existe un registro idéntico
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
            WHERE ISNULL([contrato_tipo], '') = ISNULL(@contrato_tipo, '')
              AND ISNULL([tipo_operacion], '') = ISNULL(@tipo_operacion, '')
              AND ISNULL([contrato_proceso_seleccion], '') = ISNULL(@contrato_proceso_seleccion, '')
        )
        BEGIN
            RAISERROR('Ya existe una combinación con los mismos datos de clasificación.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        INSERT INTO [dbo].[clasificacion_contratos] (
            [contrato_tipo],
            [tipo_operacion],
            [contrato_proceso_seleccion]
        )
        VALUES (
            NULLIF(TRIM(@contrato_tipo), ''),
            NULLIF(TRIM(@tipo_operacion), ''),
            NULLIF(TRIM(@contrato_proceso_seleccion), '')
        );

        SET @codigo_generado = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_clasificacion_contratos_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----
-----

CREATE PROCEDURE [dbo].[sp_clasificacion_contratos_Eliminar]
    @codigo_clasificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[clasificacion_contratos] WITH (NOLOCK) 
            WHERE [codigo_clasificacion] = @codigo_clasificacion
        )
        BEGIN
            RAISERROR('El registro de clasificación que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [dbo].[clasificacion_contratos]
        WHERE [codigo_clasificacion] = @codigo_clasificacion;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_clasificacion_contratos_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_clasificacion_contratos_Obtener]
    @codigo_clasificacion INT = NULL,
    @contrato_tipo VARCHAR(50) = NULL,
    @tipo_operacion VARCHAR(50) = NULL,
    @contrato_proceso_seleccion VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Obtener registro puntual por ID
    IF @codigo_clasificacion IS NOT NULL
    BEGIN
        SELECT 
            [codigo_clasificacion],
            [contrato_tipo],
            [tipo_operacion],
            [contrato_proceso_seleccion]
        FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
        WHERE [codigo_clasificacion] = @codigo_clasificacion;

        RETURN;
    END

    -- 2. Listar todos o filtrar por campos
    SELECT 
        [codigo_clasificacion],
        [contrato_tipo],
        [tipo_operacion],
        [contrato_proceso_seleccion]
    FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
    WHERE (@contrato_tipo IS NULL OR [contrato_tipo] = @contrato_tipo)
      AND (@tipo_operacion IS NULL OR [tipo_operacion] = @tipo_operacion)
      AND (@contrato_proceso_seleccion IS NULL OR [contrato_proceso_seleccion] = @contrato_proceso_seleccion)
    ORDER BY [codigo_clasificacion] DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_clasificacion_contratos_ObtenerOpciones]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----
-----

CREATE PROCEDURE [dbo].[sp_clasificacion_contratos_ObtenerOpciones]
    @tipo_opcion VARCHAR(20) -- 'CONTRATO_TIPO' o 'TIPO_OPERACION'
AS
BEGIN
    SET NOCOUNT ON;

    IF @tipo_opcion = 'CONTRATO_TIPO'
    BEGIN
        SELECT DISTINCT 
            [contrato_tipo] AS [valor],
            [contrato_tipo] AS [texto]
        FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
        WHERE [contrato_tipo] IS NOT NULL 
          AND TRIM([contrato_tipo]) <> ''
        ORDER BY [contrato_tipo] ASC;

        RETURN;
    END

    IF @tipo_opcion = 'TIPO_OPERACION'
    BEGIN
        SELECT DISTINCT 
            [tipo_operacion] AS [valor],
            [tipo_operacion] AS [texto]
        FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
        WHERE [tipo_operacion] IS NOT NULL 
          AND TRIM([tipo_operacion]) <> ''
        ORDER BY [tipo_operacion] ASC;

        RETURN;
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_cta_institucion_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_cta_institucion_Actualizar]
    @ID INT,
    @ins_codigo NVARCHAR(100),
    @Institucion NVARCHAR(255),
    @cta_cta_banco NVARCHAR(50),
    @cta_descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE [dbo].[t_cta_institucion]
        SET 
            [ins_codigo] = @ins_codigo,
            [Institucion] = @Institucion,
            [cta_cta_banco] = @cta_cta_banco,
            [cta_descripcion] = @cta_descripcion
        WHERE 
            [ID] = @ID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_cta_institucion_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_cta_institucion_Eliminar]
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM [dbo].[t_cta_institucion]
        WHERE [ID] = @ID;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_cta_institucion_Insertar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_cta_institucion_Insertar]
    @ins_codigo NVARCHAR(100),    -- Estructura
    @Institucion NVARCHAR(255),   -- Unidad Ejecutora
    @cta_cta_banco NVARCHAR(50),
    @cta_descripcion NVARCHAR(255),
    @NuevoID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO [dbo].[t_cta_institucion]
        (
            [ins_codigo],
            [Institucion],
            [cta_cta_banco],
            [cta_descripcion]
        )
        VALUES
        (
            @ins_codigo,
            @Institucion,
            @cta_cta_banco,
            @cta_descripcion
        );

        -- Retorna el ID recién generado
        SET @NuevoID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_cta_institucion_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_cta_institucion_Obtener]
    @ID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [ID],
        [ins_codigo],
        [Institucion],
        [cta_cta_banco],
        [cta_descripcion]
    FROM 
        [dbo].[t_cta_institucion]
    WHERE 
        (@ID IS NULL OR [ID] = @ID);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_cta_institucion_ObtenerPorEstructura]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_cta_institucion_ObtenerPorEstructura]
    @ins_codigo NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.[ID],
        c.[ins_codigo],
        c.[Institucion],
        c.[cta_cta_banco],
        c.[cta_descripcion]
    FROM 
        [dbo].[t_cta_institucion] c
    WHERE 
        c.[ins_codigo] = @ins_codigo;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Cuentas_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cuentas_Actualizar]
    @id_cuenta INT,
    @descripcion_cuenta NVARCHAR(255),
    @tipo_tramite INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Validar que el registro exista en dbo.cuentas
    IF NOT EXISTS (SELECT 1 FROM [dbo].[cuentas] WHERE [id_cuenta] = @id_cuenta)
    BEGIN
        RAISERROR('El registro de la cuenta especificada no existe.', 16, 1);
        RETURN;
    END

    -- 2. Validar que el tipo_tramite exista en dbo.tipo_tramite_pago
    IF NOT EXISTS (SELECT 1 FROM [dbo].[tipo_tramite_pago] WHERE [id] = @tipo_tramite)
    BEGIN
        RAISERROR('El tipo de trámite seleccionado no es válido.', 16, 1);
        RETURN;
    END

    -- 3. Actualizar la descripción y el trámite (manteniendo código inmutable)
    UPDATE [dbo].[cuentas]
    SET 
        [descripcion_cuenta] = @descripcion_cuenta,
        [tipo_tramite] = @tipo_tramite
    WHERE [id_cuenta] = @id_cuenta;

    -- Retornar el registro actualizado
    SELECT 
        c.[id_cuenta],
        c.[codigo_cuenta],
        c.[descripcion_cuenta],
        c.[tipo_tramite],
        t.[tipo] AS tipo_tramite_nombre
    FROM [dbo].[cuentas] c
    INNER JOIN [dbo].[tipo_tramite_pago] t ON c.[tipo_tramite] = t.[id]
    WHERE c.[id_cuenta] = @id_cuenta;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cuentas_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cuentas_Eliminar]
    @id_cuenta INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM [dbo].[cuentas] WHERE [id_cuenta] = @id_cuenta)
    BEGIN
        RAISERROR('El registro a eliminar no existe.', 16, 1);
        RETURN;
    END

    DELETE FROM [dbo].[cuentas]
    WHERE [id_cuenta] = @id_cuenta;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cuentas_Insertar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cuentas_Insertar]
    @id_cuenta INT,
    @descripcion_cuenta NVARCHAR(255) = NULL,
    @tipo_tramite INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @codigoCuenta NVARCHAR(255);
    DECLARE @descripcionOriginal NVARCHAR(255);

    -- 1. Validar que el tipo_tramite exista en dbo.tipo_tramite_pago
    IF NOT EXISTS (SELECT 1 FROM [dbo].[tipo_tramite_pago] WHERE [id] = @tipo_tramite)
    BEGIN
        RAISERROR('El tipo de trámite seleccionado no existe.', 16, 1);
        RETURN;
    END

    -- 2. Obtener los datos base de la cuenta desde [ct].[cuentas]
    SELECT TOP 1 
        @codigoCuenta = [codigo_cuenta],
        @descripcionOriginal = [descripcion_cuenta]
    FROM [ct].[cuentas]
    WHERE CAST([id_cuenta] AS INT) = @id_cuenta;

    IF @codigoCuenta IS NULL
    BEGIN
        RAISERROR('La cuenta seleccionada no existe en el catálogo base (ct.cuentas).', 16, 1);
        RETURN;
    END

    -- 3. Si no se envía una descripción personalizada, usar la de ct.cuentas
    IF @descripcion_cuenta IS NULL OR RTRIM(@descripcion_cuenta) = ''
    BEGIN
        SET @descripcion_cuenta = @descripcionOriginal;
    END

    -- 4. Inserción en dbo.cuentas
    INSERT INTO [dbo].[cuentas] (
        [codigo_cuenta],
        [descripcion_cuenta],
        [tipo_tramite]
    )
    VALUES (
        @codigoCuenta,
        @descripcion_cuenta,
        @tipo_tramite
    );

    -- Retornar la fila recién insertada
    DECLARE @nuevoId INT = SCOPE_IDENTITY();

    SELECT 
        c.[id_cuenta],
        c.[codigo_cuenta],
        c.[descripcion_cuenta],
        c.[tipo_tramite],
        t.[tipo] AS tipo_tramite_nombre
    FROM [dbo].[cuentas] c
    INNER JOIN [dbo].[tipo_tramite_pago] t ON c.[tipo_tramite] = t.[id]
    WHERE c.[id_cuenta] = @nuevoId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cuentas_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Cuentas_Obtener]
    @id_cuenta INT = NULL,
    @tipo_tramite INT = NULL,
    @busqueda NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.[id_cuenta],
        c.[codigo_cuenta],
        c.[descripcion_cuenta],
        c.[tipo_tramite],
        RTRIM(t.[tipo]) AS tipo_tramite_nombre
    FROM [dbo].[cuentas] c
    INNER JOIN [dbo].[tipo_tramite_pago] t ON c.[tipo_tramite] = t.[id]
    WHERE (@id_cuenta IS NULL OR c.[id_cuenta] = @id_cuenta)
      AND (@tipo_tramite IS NULL OR c.[tipo_tramite] = @tipo_tramite)
      AND (@busqueda IS NULL OR c.[codigo_cuenta] LIKE '%' + @busqueda + '%' OR c.[descripcion_cuenta] LIKE '%' + @busqueda + '%')
    ORDER BY c.[codigo_cuenta] ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_daf_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_daf_Actualizar]
    @id INT,
    @id_sub_capitulo INT,
    @codigo_daf VARCHAR(10),
    @daf VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- 1. Validar si existe el registro a actualizar
        IF NOT EXISTS (SELECT 1 FROM [dbo].[daf] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('El DAF que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar existencia del subcapitulo padre
        IF NOT EXISTS (SELECT 1 FROM [dbo].[sub_capitulo] WITH (NOLOCK) WHERE [id] = @id_sub_capitulo)
        BEGIN
            RAISERROR('El subcapítulo especificado no existe.', 16, 1);
            RETURN;
        END

        -- 3. Validar duplicidad de código en el mismo subcapítulo
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[daf] WITH (NOLOCK) 
            WHERE [id_sub_capitulo] = @id_sub_capitulo 
              AND [codigo_daf] = @codigo_daf 
              AND [id] <> @id
        )
        BEGIN
            RAISERROR('El código DAF asignado ya se encuentra utilizado en este subcapítulo.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        UPDATE [dbo].[daf]
        SET 
            [id_sub_capitulo] = @id_sub_capitulo,
            [codigo_daf] = @codigo_daf,
            [daf] = UPPER(TRIM(@daf))
        WHERE [id] = @id;

        COMMIT TRANSACTION;

        -- DEVOLVER EL REGISTRO ACTUALIZADO PARA EF CORE
        SELECT [id], [id_sub_capitulo], [codigo_daf], [daf]
        FROM [dbo].[daf] WITH (NOLOCK)
        WHERE [id] = @id;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_daf_Crear]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_daf_Crear]
    @id_sub_capitulo INT,
    @codigo_daf VARCHAR(10),
    @daf VARCHAR(200),
    @id_generado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- 1. Validar existencia del subcapitulo padre
        IF NOT EXISTS (SELECT 1 FROM [dbo].[sub_capitulo] WITH (NOLOCK) WHERE [id] = @id_sub_capitulo)
        BEGIN
            RAISERROR('El subcapítulo especificado no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar duplicidad de codigo_daf dentro del mismo subcapitulo
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[daf] WITH (NOLOCK) 
            WHERE [id_sub_capitulo] = @id_sub_capitulo 
              AND [codigo_daf] = @codigo_daf
        )
        BEGIN
            RAISERROR('El código DAF ya se encuentra registrado para este subcapítulo.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        INSERT INTO [dbo].[daf] ([id_sub_capitulo], [codigo_daf], [daf])
        VALUES (@id_sub_capitulo, @codigo_daf, UPPER(TRIM(@daf)));

        SET @id_generado = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_daf_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_daf_Eliminar]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[daf] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('El DAF que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        -- Validar si existen unidades ejecutoras asociadas antes de borrar
        IF EXISTS (SELECT 1 FROM [dbo].[unidad_ejecutora] WITH (NOLOCK) WHERE [id_daf] = @id)
        BEGIN
            RAISERROR('No se puede eliminar el DAF porque tiene unidades ejecutoras asociadas.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [dbo].[daf]
        WHERE [id] = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_daf_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_daf_Obtener]
    @id INT = NULL,
    @id_sub_capitulo INT = NULL,
    @codigo_daf VARCHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Obtener por ID único
    IF @id IS NOT NULL
    BEGIN
        SELECT 
            d.[id],
            d.[id_sub_capitulo],
            sc.[codigo_sub_capitulo],
            sc.[sub_capitulo],
            d.[codigo_daf],
            d.[daf]
        FROM [dbo].[daf] d WITH (NOLOCK)
        INNER JOIN [dbo].[sub_capitulo] sc WITH (NOLOCK) ON d.[id_sub_capitulo] = sc.[id]
        WHERE d.[id] = @id;

        RETURN;
    END

    -- 2. Obtener por Código DAF exacto (opcionalmente dentro de un Subcapítulo específico)
    IF @codigo_daf IS NOT NULL
    BEGIN
        SELECT 
            d.[id],
            d.[id_sub_capitulo],
            sc.[codigo_sub_capitulo],
            sc.[sub_capitulo],
            d.[codigo_daf],
            d.[daf]
        FROM [dbo].[daf] d WITH (NOLOCK)
        INNER JOIN [dbo].[sub_capitulo] sc WITH (NOLOCK) ON d.[id_sub_capitulo] = sc.[id]
        WHERE d.[codigo_daf] = @codigo_daf
          AND (@id_sub_capitulo IS NULL OR d.[id_sub_capitulo] = @id_sub_capitulo);

        RETURN;
    END

    -- 3. Obtener por Subcapítulo o Listar Todos si no se pasan parámetros
    SELECT 
        d.[id],
        d.[id_sub_capitulo],
        sc.[codigo_sub_capitulo],
        sc.[sub_capitulo],
        d.[codigo_daf],
        d.[daf]
    FROM [dbo].[daf] d WITH (NOLOCK)
    INNER JOIN [dbo].[sub_capitulo] sc WITH (NOLOCK) ON d.[id_sub_capitulo] = sc.[id]
    WHERE (@id_sub_capitulo IS NULL OR d.[id_sub_capitulo] = @id_sub_capitulo)
    ORDER BY sc.[sub_capitulo] ASC, d.[daf] ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Eliminar_Asignacion_Analista]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Eliminar_Asignacion_Analista]
    @DespachoAnalista VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF @DespachoAnalista IS NULL OR LTRIM(RTRIM(@DespachoAnalista)) = ''
        BEGIN
            RAISERROR('El código de despacho del analista es obligatorio.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (
            SELECT 1 
            FROM [SIGOB_Contraloria].[dbo].[analista_supervisor] WITH (NOLOCK)
            WHERE despacho_analista = @DespachoAnalista
        )
        BEGIN
            RAISERROR('El analista no posee un supervisor asignado para eliminar.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [SIGOB_Contraloria].[dbo].[analista_supervisor]
        WHERE despacho_analista = @DespachoAnalista;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Asignación eliminada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Eliminar_Relacion_Contrato_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----

CREATE   PROCEDURE [dbo].[sp_Eliminar_Relacion_Contrato_Requisito]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] WITH (NOLOCK) WHERE id = @Id)
        BEGIN
            RAISERROR('La relación que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito]
        WHERE id = @Id;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Relación eliminada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_FondoEspeciales_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_FondoEspeciales_Actualizar]
    @id INT,
    @id_institucion INT,
    @fondo VARCHAR(50),
    @descripcion VARCHAR(500) = NULL, -- Nuevo parámetro para actualización
    @tipo_tramite VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @estructuraEncontrada VARCHAR(50);
    DECLARE @nombreUnidadEjecutora VARCHAR(255);
    DECLARE @descripcionFinal VARCHAR(500);

    SELECT 
        @estructuraEncontrada = [estructura],
        @nombreUnidadEjecutora = [unidad_ejecutora]
    FROM [dbo].[v_institucion]
    WHERE [id_institucion] = @id_institucion;

    IF @estructuraEncontrada IS NULL
    BEGIN
        RAISERROR('La institución seleccionada no es válida o no existe.', 16, 1);
        RETURN;
    END

    SET @descripcionFinal = ISNULL(NULLIF(RTRIM(@descripcion), ''), @nombreUnidadEjecutora);

    UPDATE [dbo].[Fondo_especiales]
    SET 
        [Fondo] = @fondo,
        [estructura_institucion] = @estructuraEncontrada,
        [descripcion] = @descripcionFinal,
        [tipo_tramite] = ISNULL(NULLIF(RTRIM(@tipo_tramite), ''), 'Pendiente')
    WHERE [id] = @id;

    SELECT 
        [Id],
        [Fondo],
        [estructura_institucion],
        [descripcion],
        [tipo_tramite]
    FROM [dbo].[Fondo_especiales]
    WHERE [Id] = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_FondoEspeciales_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_FondoEspeciales_Eliminar]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Fondo_especiales]
    WHERE [id] = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_FondoEspeciales_Insertar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_FondoEspeciales_Insertar]
    @id_institucion INT,            
    @fondo VARCHAR(50),             
    @descripcion VARCHAR(500) = NULL,  -- Nuevo parámetro para la descripción manual
    @tipo_tramite VARCHAR(100) = NULL      
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @estructuraEncontrada VARCHAR(50);
    DECLARE @nombreUnidadEjecutora VARCHAR(255);
    DECLARE @tramiteFinal VARCHAR(100);
    DECLARE @descripcionFinal VARCHAR(500);
    DECLARE @nuevoId INT;

    -- Asignar 'Pendiente' por defecto si no se especifica tipo de trámite
    SET @tramiteFinal = ISNULL(NULLIF(RTRIM(@tipo_tramite), ''), 'Pendiente');

    SELECT 
        @estructuraEncontrada = [estructura],
        @nombreUnidadEjecutora = [unidad_ejecutora]
    FROM [dbo].[v_institucion]
    WHERE [id_institucion] = @id_institucion;

    IF @estructuraEncontrada IS NULL
    BEGIN
        RAISERROR('La institución seleccionada no es válida o no existe.', 16, 1);
        RETURN;
    END

    -- Usar la descripción ingresada manualmente; si viene vacía, usar el nombre de la institución
    SET @descripcionFinal = ISNULL(NULLIF(RTRIM(@descripcion), ''), @nombreUnidadEjecutora);

    SELECT @nuevoId = ISNULL(MAX([Id]), 0) + 1 FROM [dbo].[Fondo_especiales];

    INSERT INTO [dbo].[Fondo_especiales] (
        [Id],
        [Fondo],
        [estructura_institucion],
        [descripcion],
        [tipo_tramite]
    )
    VALUES (
        @nuevoId,
        @fondo,
        @estructuraEncontrada,
        @descripcionFinal,
        @tramiteFinal
    );

    SELECT 
        [Id],
        [Fondo],
        [estructura_institucion],
        [descripcion],
        [tipo_tramite]
    FROM [dbo].[Fondo_especiales]
    WHERE [Id] = @nuevoId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_FondoEspeciales_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_FondoEspeciales_Obtener]
    @filtroFondo VARCHAR(50) = NULL,
    @filtroEstructura VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [id],
        [Fondo],
        [estructura_institucion],
        [descripcion],
        [tipo_tramite]
    FROM [dbo].[Fondo_especiales]
    WHERE (@filtroFondo IS NULL OR [Fondo] LIKE '%' + @filtroFondo + '%')
      AND (@filtroEstructura IS NULL OR [estructura_institucion] = @filtroEstructura)
	ORDER BY [tipo_tramite] Desc;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Obtener_Catalogo_Clasificacion_Contratos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----
-----

-- Obtener catálogo de clasificación de contratos para dropdown
CREATE   PROCEDURE [dbo].[sp_Obtener_Catalogo_Clasificacion_Contratos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT codigo_clasificacion, contrato_tipo, tipo_operacion, contrato_proceso_seleccion
    FROM [SIGOB_CONTRALORIA].[dbo].[clasificacion_contratos] WITH (NOLOCK)
    ORDER BY contrato_tipo ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Obtener_Catalogo_Proceso_Seleccion]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Obtener_Catalogo_Proceso_Seleccion]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        id,
        proceso
    FROM [SIGOB_CONTRALORIA].[dbo].[proceso_seleccion] WITH (NOLOCK)
    ORDER BY proceso ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Obtener_Catalogo_Requisitos_Contratos]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Obtener catálogo de requisitos de contratos para dropdown
CREATE   PROCEDURE [dbo].[sp_Obtener_Catalogo_Requisitos_Contratos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT codigo_requisito, descripcion_requisito, codigo_documento_tre
    FROM [SIGOB_CONTRALORIA].[dbo].[requisitos_contratos] WITH (NOLOCK)
    ORDER BY descripcion_requisito ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Obtener_Mantenimiento_Analistas_Supervisores]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_Obtener_Mantenimiento_Analistas_Supervisores]
    @NombreAnalista NVARCHAR(100) = NULL,      -- Filtro por nombre o apellido del analista
    @EstadoAnalista VARCHAR(30) = NULL,         -- 'ASIGNADO' o 'PENDIENTE DE ASIGNACION'
    @DespachoSupervisor VARCHAR(20) = NULL     -- Filtro por un supervisor en específico
AS
BEGIN
    SET NOCOUNT ON;

    -- Limpieza de espacios en parámetros
    SET @NombreAnalista = NULLIF(LTRIM(RTRIM(@NombreAnalista)), '');
    SET @EstadoAnalista = NULLIF(LTRIM(RTRIM(@EstadoAnalista)), '');
    SET @DespachoSupervisor = NULLIF(LTRIM(RTRIM(@DespachoSupervisor)), '');

    SELECT 
        -- Datos del Analista
        A.codigo_despa AS Despacho_Analista,
        CONCAT(DA.nombre, ' ', DA.apellido) AS Nombre_Analista,
        
        -- Estado operativo
        CASE 
            WHEN REL.despacho_supervisor IS NOT NULL THEN 'ASIGNADO'
            ELSE 'PENDIENTE DE ASIGNACION'
        END AS Estado_Analista,

        -- Datos del Supervisor Asignado (si aplica)
        REL.despacho_supervisor AS Despacho_Supervisor,
        CONCAT(DS.nombre, ' ', DS.apellido) AS Nombre_Supervisor

    FROM (
        -- Universo único de analistas con actividades de analista
        SELECT DISTINCT codigo_despa
        FROM [SIGOB].[dbo].[responsable_actividad] WITH (NOLOCK)
        WHERE codigo_actividad IN (6930018, 6930039, 6930046, 6930057, 6930077)
    ) A

    -- Datos personales del Analista
    INNER JOIN [SIGOB].[dbo].[v_Despachos] DA WITH (NOLOCK) 
        ON DA.codigo_despa = A.codigo_despa

    -- Relación de Asignación actual
    LEFT JOIN [SIGOB_Contraloria].[dbo].[analista_supervisor] REL WITH (NOLOCK) 
        ON REL.despacho_analista = A.codigo_despa

    -- Datos personales del Supervisor
    LEFT JOIN [SIGOB].[dbo].[v_Despachos] DS WITH (NOLOCK) 
        ON DS.codigo_despa = REL.despacho_supervisor

    WHERE 
        -- Filtro opcional: Por nombre/apellido del analista (búsqueda parcial)
        (@NombreAnalista IS NULL OR CONCAT(DA.nombre, ' ', DA.apellido) LIKE '%' + @NombreAnalista + '%')
        
        -- Filtro opcional: Por estado ('ASIGNADO' o 'PENDIENTE DE ASIGNACION')
        AND (
            @EstadoAnalista IS NULL 
            OR (@EstadoAnalista = 'ASIGNADO' AND REL.despacho_supervisor IS NOT NULL)
            OR (@EstadoAnalista = 'PENDIENTE DE ASIGNACION' AND REL.despacho_supervisor IS NULL)
        )

        -- Filtro opcional: Por supervisor asignado
        AND (@DespachoSupervisor IS NULL OR REL.despacho_supervisor = @DespachoSupervisor)

    ORDER BY 
        -- Orden por defecto: Primero los pendientes de asignación, luego por nombre de analista
        CASE WHEN REL.despacho_supervisor IS NULL THEN 0 ELSE 1 END ASC,
        Nombre_Analista ASC
    OPTION (RECOMPILE); -- Mantiene el plan de ejecución optimizado ante parámetros dinámicos/variables
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Obtener_Relacion_Contrato_Requisito]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_Obtener_Relacion_Contrato_Requisito]
    @CodigoClasificacion INT = NULL,
    @CodigoRequisito INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.id,
        r.codigo_clasificacion,
        c.contrato_tipo,
        c.tipo_operacion,
        c.contrato_proceso_seleccion,
        r.codigo_requisito,
        req.descripcion_requisito,
        req.codigo_documento_tre
    FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] r WITH (NOLOCK)
    INNER JOIN [SIGOB_CONTRALORIA].[dbo].[clasificacion_contratos] c WITH (NOLOCK) 
        ON c.codigo_clasificacion = r.codigo_clasificacion
    INNER JOIN [SIGOB_CONTRALORIA].[dbo].[requisitos_contratos] req WITH (NOLOCK) 
        ON req.codigo_requisito = r.codigo_requisito
    WHERE 
        (@CodigoClasificacion IS NULL OR r.codigo_clasificacion = @CodigoClasificacion)
        AND (@CodigoRequisito IS NULL OR r.codigo_requisito = @CodigoRequisito)
    ORDER BY r.codigo_clasificacion ASC, req.descripcion_requisito ASC
    OPTION (RECOMPILE);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Obtener_Supervisores_Disponibles]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Obtener_Supervisores_Disponibles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        ra.codigo_despa AS Despacho_Supervisor,
        CONCAT(d.nombre, ' ', d.apellido) AS Nombre_Supervisor
    FROM [SIGOB].[dbo].[responsable_actividad] ra WITH (NOLOCK)
    INNER JOIN [SIGOB].[dbo].[v_Despachos] d WITH (NOLOCK) 
        ON d.codigo_despa = ra.codigo_despa
    WHERE ra.codigo_actividad IN (6930019, 6930040, 6930049, 6930060, 6930080)
    ORDER BY Nombre_Supervisor ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_Reasignar_Supervisor_Analista]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Reasignar_Supervisor_Analista]
    @DespachoAnalista VARCHAR(20),
    @NuevoDespachoSupervisor VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- 1. Validaciones de entrada
        IF @DespachoAnalista IS NULL OR LTRIM(RTRIM(@DespachoAnalista)) = ''
        BEGIN
            RAISERROR('El código de despacho del analista es obligatorio.', 16, 1);
            RETURN;
        END

        IF @NuevoDespachoSupervisor IS NULL OR LTRIM(RTRIM(@NuevoDespachoSupervisor)) = ''
        BEGIN
            RAISERROR('El código del nuevo supervisor es obligatorio.', 16, 1);
            RETURN;
        END

        -- 2. Validar que el analista realmente tenga una asignación activa
        IF NOT EXISTS (
            SELECT 1 
            FROM [SIGOB_Contraloria].[dbo].[analista_supervisor] WITH (NOLOCK)
            WHERE despacho_analista = @DespachoAnalista
        )
        BEGIN
            RAISERROR('El analista no tiene ninguna asignación previa. Utilice la función de Asignar.', 16, 1);
            RETURN;
        END

        -- 3. Validar que el nuevo supervisor no sea el mismo que el actual
        IF EXISTS (
            SELECT 1 
            FROM [SIGOB_Contraloria].[dbo].[analista_supervisor] WITH (NOLOCK)
            WHERE despacho_analista = @DespachoAnalista AND despacho_supervisor = @NuevoDespachoSupervisor
        )
        BEGIN
            RAISERROR('El analista ya se encuentra asignado a este mismo supervisor.', 16, 1);
            RETURN;
        END

        -- 4. Actualización con transacción explícita
        BEGIN TRANSACTION;

        UPDATE [SIGOB_Contraloria].[dbo].[analista_supervisor]
        SET despacho_supervisor = @NuevoDespachoSupervisor
        WHERE despacho_analista = @DespachoAnalista;

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Supervisor reasignado exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_requisitos_contratos_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_requisitos_contratos_Actualizar]
    @codigo_requisito INT,
    @descripcion_requisito NVARCHAR(300),
    @codigo_documento_tre INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[requisitos_contratos] WITH (NOLOCK) WHERE [codigo_requisito] = @codigo_requisito)
        BEGIN
            RAISERROR('El requisito de contrato que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        UPDATE [dbo].[requisitos_contratos]
        SET 
            [descripcion_requisito] = LTRIM(RTRIM(@descripcion_requisito)),
            [codigo_documento_tre] = @codigo_documento_tre
        WHERE [codigo_requisito] = @codigo_requisito;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_requisitos_contratos_Crear]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_requisitos_contratos_Crear]
    @descripcion_requisito NVARCHAR(300),
    @codigo_documento_tre INT = NULL,
    @codigo_requisito INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Genera el correlativo automático de manera segura
        SELECT @codigo_requisito = ISNULL(MAX([codigo_requisito]), 0) + 1 
        FROM [dbo].[requisitos_contratos] WITH (UPDLOCK, HOLDLOCK);

        INSERT INTO [dbo].[requisitos_contratos] ([codigo_requisito], [descripcion_requisito], [codigo_documento_tre])
        VALUES (@codigo_requisito, LTRIM(RTRIM(@descripcion_requisito)), @codigo_documento_tre);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_requisitos_contratos_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_requisitos_contratos_Eliminar]
    @codigo_requisito INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[requisitos_contratos] WITH (NOLOCK) WHERE [codigo_requisito] = @codigo_requisito)
        BEGIN
            RAISERROR('El requisito de contrato que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        DELETE FROM [dbo].[requisitos_contratos]
        WHERE [codigo_requisito] = @codigo_requisito;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_requisitos_contratos_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_requisitos_contratos_Obtener]
    @codigo_requisito INT = NULL,
    @busqueda NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [codigo_requisito],
        [descripcion_requisito],
        [codigo_documento_tre]
    FROM [dbo].[requisitos_contratos]
    WHERE (@codigo_requisito IS NULL OR [codigo_requisito] = @codigo_requisito)
      AND (@busqueda IS NULL 
           OR CAST([codigo_requisito] AS NVARCHAR(50)) LIKE '%' + @busqueda + '%' 
           OR [descripcion_requisito] LIKE '%' + @busqueda + '%'
           OR CAST([codigo_documento_tre] AS NVARCHAR(50)) LIKE '%' + @busqueda + '%')
    ORDER BY [codigo_requisito] ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_subcapitulo_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_subcapitulo_Actualizar]
    @id INT,
    @id_capitulo INT,
    @codigo_sub_capitulo VARCHAR(10),
    @sub_capitulo VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- 1. Validar si existe el registro a actualizar
        IF NOT EXISTS (SELECT 1 FROM [dbo].[sub_capitulo] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('El subcapítulo que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar existencia del capitulo padre
        IF NOT EXISTS (SELECT 1 FROM [dbo].[capitulo] WITH (NOLOCK) WHERE [id] = @id_capitulo)
        BEGIN
            RAISERROR('El capítulo especificado no existe.', 16, 1);
            RETURN;
        END

        -- 3. Validar duplicidad de código en el mismo capítulo (excluyendo el registro actual)
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[sub_capitulo] WITH (NOLOCK) 
            WHERE [id_capitulo] = @id_capitulo 
              AND [codigo_sub_capitulo] = @codigo_sub_capitulo 
              AND [id] <> @id
        )
        BEGIN
            RAISERROR('El código de subcapítulo asignado ya se encuentra utilizado en este capítulo.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        UPDATE [dbo].[sub_capitulo]
        SET 
            [id_capitulo] = @id_capitulo,
            [codigo_sub_capitulo] = @codigo_sub_capitulo,
            [sub_capitulo] = UPPER(TRIM(@sub_capitulo))
        WHERE [id] = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_subcapitulo_Crear]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_subcapitulo_Crear]
    @id_capitulo INT,
    @codigo_sub_capitulo VARCHAR(10),
    @sub_capitulo VARCHAR(200),
    @id_generado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- 1. Validar existencia del capitulo padre
        IF NOT EXISTS (SELECT 1 FROM [dbo].[capitulo] WITH (NOLOCK) WHERE [id] = @id_capitulo)
        BEGIN
            RAISERROR('El capítulo especificado no existe.', 16, 1);
            RETURN;
        END

        -- 2. Validar duplicidad de codigo_sub_capitulo dentro del mismo capitulo
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[sub_capitulo] WITH (NOLOCK) 
            WHERE [id_capitulo] = @id_capitulo 
              AND [codigo_sub_capitulo] = @codigo_sub_capitulo
        )
        BEGIN
            RAISERROR('El código de subcapítulo ya se encuentra registrado para este capítulo.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        INSERT INTO [dbo].[sub_capitulo] ([id_capitulo], [codigo_sub_capitulo], [sub_capitulo])
        VALUES (@id_capitulo, @codigo_sub_capitulo, UPPER(TRIM(@sub_capitulo)));

        SET @id_generado = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_subcapitulo_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_subcapitulo_Eliminar]
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[sub_capitulo] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('El subcapítulo que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        DELETE FROM [dbo].[sub_capitulo]
        WHERE [id] = @id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_subcapitulo_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_subcapitulo_Obtener]
    @id INT = NULL,
    @id_capitulo INT = NULL,
    @buscar VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Obtener por ID único
    IF @id IS NOT NULL
    BEGIN
        SELECT 
            sc.[id],
            sc.[id_capitulo],
            c.[codigo_capitulo],
            c.[capitulo],
            sc.[codigo_sub_capitulo],
            sc.[sub_capitulo]
        FROM [dbo].[sub_capitulo] sc WITH (NOLOCK)
        INNER JOIN [dbo].[capitulo] c WITH (NOLOCK) ON sc.[id_capitulo] = c.[id]
        WHERE sc.[id] = @id;

        RETURN;
    END

    -- Consultar / Listar con filtros opcionales
    SELECT 
        sc.[id],
        sc.[id_capitulo],
        c.[codigo_capitulo],
        c.[capitulo],
        sc.[codigo_sub_capitulo],
        sc.[sub_capitulo]
    FROM [dbo].[sub_capitulo] sc WITH (NOLOCK)
    INNER JOIN [dbo].[capitulo] c WITH (NOLOCK) ON sc.[id_capitulo] = c.[id]
    WHERE (@id_capitulo IS NULL OR sc.[id_capitulo] = @id_capitulo)
      AND (@buscar IS NULL 
           OR sc.[codigo_sub_capitulo] LIKE '%' + @buscar + '%' 
           OR sc.[sub_capitulo] LIKE '%' + @buscar + '%'
           OR c.[capitulo] LIKE '%' + @buscar + '%')
    ORDER BY c.[codigo_capitulo] ASC, sc.[codigo_sub_capitulo] ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TipoTramitePago_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_TipoTramitePago_Obtener]
    @id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [id],
        RTRIM([tipo]) AS [tipo]
    FROM [dbo].[tipo_tramite_pago]
    WHERE (@id IS NULL OR [id] = @id)
    ORDER BY [id] ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_unidad_ejecutora_Actualizar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_unidad_ejecutora_Actualizar]
    @id INT,
    @id_padre INT = NULL,
    @id_daf INT,
    @codigo_unidad_ejecutora VARCHAR(10),
    @unidad_ejecutora VARCHAR(250),
    @rnc VARCHAR(20) = NULL,
    @estado BIT,
    @portal_compra BIT,
    @usuario VARCHAR(100),
    @id_categoria INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[unidad_ejecutora] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('La unidad ejecutora que intenta actualizar no existe.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM [dbo].[daf] WITH (NOLOCK) WHERE [id] = @id_daf)
        BEGIN
            RAISERROR('La DAF especificada no existe.', 16, 1);
            RETURN;
        END

        IF EXISTS (
            SELECT 1 
            FROM [dbo].[unidad_ejecutora] WITH (NOLOCK) 
            WHERE [id_daf] = @id_daf 
              AND [codigo_unidad_ejecutora] = @codigo_unidad_ejecutora 
              AND [id] <> @id
        )
        BEGIN
            RAISERROR('El código asignado ya pertenece a otra unidad ejecutora dentro de la misma DAF.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        -- 1. Actualizar la unidad ejecutora principal
        UPDATE [dbo].[unidad_ejecutora]
        SET 
            [id_padre] = @id_padre,
            [id_daf] = @id_daf,
            [codigo_unidad_ejecutora] = @codigo_unidad_ejecutora,
            [unidad_ejecutora] = UPPER(TRIM(@unidad_ejecutora)),
            [rnc] = @rnc,
            [estado] = @estado,
            [portal_compra] = @portal_compra,
            [modificado] = GETDATE(),
            [usuario_modificacion] = @usuario
        WHERE [id] = @id;

        -- 2. Actualizar o crear la relación de categoría en contrato_sigob
        IF @id_categoria IS NOT NULL
        BEGIN
            IF EXISTS (SELECT 1 FROM [contrato_sigob].[relacion_institucion_categoria] WHERE [id_institucion] = @id)
            BEGIN
                -- SOLO actualizamos la categoría. La estructura SE MANTIENE INTACTA tal como se creó.
                UPDATE [contrato_sigob].[relacion_institucion_categoria]
                SET [id_categoria] = @id_categoria
                WHERE [id_institucion] = @id;
            END
            ELSE
            BEGIN
                -- Si por alguna razón no existía la relación, la creamos calculando su estructura por primera vez
                DECLARE @estructura VARCHAR(50);

                SELECT TOP 1 
                    @estructura = IIF(
                        @codigo_unidad_ejecutora NOT LIKE '%SE%', 
                        (c.codigo_capitulo + sc.codigo_sub_capitulo + daf.codigo_daf + @codigo_unidad_ejecutora), 
                        @codigo_unidad_ejecutora
                    )
                FROM dbo.daf AS daf WITH (NOLOCK)
                INNER JOIN dbo.sub_capitulo AS sc WITH (NOLOCK) ON daf.id_sub_capitulo = sc.id
                INNER JOIN dbo.capitulo AS c WITH (NOLOCK) ON sc.id_capitulo = c.id
                WHERE daf.id = @id_daf;

                INSERT INTO [contrato_sigob].[relacion_institucion_categoria] (
                    [id_institucion],
                    [id_categoria],
                    [estrutura],
                    [estado]
                )
                VALUES (
                    @id,
                    @id_categoria,
                    @estructura,
                    1
                );
            END
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_unidad_ejecutora_Crear]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_unidad_ejecutora_Crear]
    @id_padre INT = NULL,
    @id_daf INT,
    @codigo_unidad_ejecutora VARCHAR(10),
    @unidad_ejecutora VARCHAR(250),
    @rnc VARCHAR(20) = NULL,
    @estado BIT = 1,
    @portal_compra BIT = 0,
    @usuario VARCHAR(100),
    @id_categoria INT = NULL,
    @id_generado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[daf] WITH (NOLOCK) WHERE [id] = @id_daf)
        BEGIN
            RAISERROR('La DAF especificada no existe.', 16, 1);
            RETURN;
        END

        IF EXISTS (
            SELECT 1 
            FROM [dbo].[unidad_ejecutora] WITH (NOLOCK) 
            WHERE [id_daf] = @id_daf 
              AND [codigo_unidad_ejecutora] = @codigo_unidad_ejecutora
        )
        BEGIN
            RAISERROR('El código de unidad ejecutora ya existe para esta DAF.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        INSERT INTO [dbo].[unidad_ejecutora] (
            [id_padre],
            [id_daf],
            [codigo_unidad_ejecutora],
            [unidad_ejecutora],
            [rnc],
            [estado],
            [creado],
            [modificado],
            [usuario_creacion],
            [usuario_modificacion],
            [portal_compra]
        )
        VALUES (
            @id_padre,
            @id_daf,
            @codigo_unidad_ejecutora,
            UPPER(TRIM(@unidad_ejecutora)),
            @rnc,
            @estado,
            GETDATE(),
            GETDATE(),
            @usuario,
            @usuario,
            @portal_compra
        );

        SET @id_generado = SCOPE_IDENTITY();

        UPDATE [dbo].[unidad_ejecutora]
        SET [id_padre] = @id_generado
        WHERE [id] = @id_generado;

        -- Guardar la relación con la Categoría en contrato_sigob
        IF @id_categoria IS NOT NULL
        BEGIN
            -- Calcular la estructura dinámicamente igual que en la vista v_institucion
            DECLARE @estructura VARCHAR(50);

            SELECT TOP 1 
                @estructura = IIF(
                    @codigo_unidad_ejecutora NOT LIKE '%SE%', 
                    (c.codigo_capitulo + sc.codigo_sub_capitulo + daf.codigo_daf + @codigo_unidad_ejecutora), 
                    @codigo_unidad_ejecutora
                )
            FROM dbo.daf AS daf WITH (NOLOCK)
            INNER JOIN dbo.sub_capitulo AS sc WITH (NOLOCK) ON daf.id_sub_capitulo = sc.id
            INNER JOIN dbo.capitulo AS c WITH (NOLOCK) ON sc.id_capitulo = c.id
            WHERE daf.id = @id_daf;

            INSERT INTO [contrato_sigob].[relacion_institucion_categoria] (
                [id_institucion],
                [id_categoria],
                [estrutura], -- Mantiene el nombre exacto de la columna en la BD
                [estado]
            )
            VALUES (
                @id_generado,
                @id_categoria,
                @estructura,
                1
            );
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_unidad_ejecutora_Eliminar]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_unidad_ejecutora_Eliminar]
    @id INT,
    @borrado_logico BIT = 1,
    @usuario VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM [dbo].[unidad_ejecutora] WITH (NOLOCK) WHERE [id] = @id)
        BEGIN
            RAISERROR('La unidad ejecutora que intenta eliminar no existe.', 16, 1);
            RETURN;
        END

        BEGIN TRANSACTION;

        IF @borrado_logico = 1
        BEGIN
            -- Desactivación lógica en unidad_ejecutora
            UPDATE [dbo].[unidad_ejecutora]
            SET 
                [estado] = 0,
                [modificado] = GETDATE(),
                [usuario_modificacion] = ISNULL(@usuario, [usuario_modificacion])
            WHERE [id] = @id;

            -- Desactivación lógica de la relación
            UPDATE [contrato_sigob].[relacion_institucion_categoria]
            SET [estado] = 0
            WHERE [id_institucion] = @id;
        END
        ELSE
        BEGIN
            -- Borrado físico de la relación primero por integridad/limpieza
            DELETE FROM [contrato_sigob].[relacion_institucion_categoria]
            WHERE [id_institucion] = @id;

            -- Borrado físico de la unidad ejecutora
            DELETE FROM [dbo].[unidad_ejecutora]
            WHERE [id] = @id;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_unidad_ejecutora_Obtener]    Script Date: 19/10/2026 11:45:18 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_unidad_ejecutora_Obtener]
    @id INT = NULL,
    @codigo_unidad_ejecutora VARCHAR(10) = NULL,
    @id_daf INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Obtener por ID único
    IF @id IS NOT NULL
    BEGIN
        SELECT 
            ue.[id],
            ue.[id_padre],
            ue.[id_daf],
            d.[codigo_daf],
            d.[daf],
            ue.[codigo_unidad_ejecutora],
            ue.[unidad_ejecutora],
            ue.[rnc],
            ue.[estado],
            ue.[creado],
            ue.[modificado],
            ue.[usuario_creacion],
            ue.[usuario_modificacion],
            ue.[portal_compra],
            ric.[id_categoria] AS [id_categoria],
            cat.[descripcion] AS [descripcion], -- <--- EF Core busca 'descripcion'
            ric.[estrutura] AS [estructura]
        FROM [dbo].[unidad_ejecutora] ue WITH (NOLOCK)
        INNER JOIN [dbo].[daf] d WITH (NOLOCK) ON ue.[id_daf] = d.[id]
        LEFT JOIN [contrato_sigob].[relacion_institucion_categoria] ric WITH (NOLOCK) ON ue.[id] = ric.[id_institucion]
        LEFT JOIN [SIGOB].[dbo].[categoria_tramite] cat WITH (NOLOCK) ON ric.[id_categoria] = cat.[codigo_categoria]
        WHERE ue.[id] = @id;

        RETURN;
    END

    -- 2. Obtener por Código exacto
    IF @codigo_unidad_ejecutora IS NOT NULL
    BEGIN
        SELECT 
            ue.[id],
            ue.[id_padre],
            ue.[id_daf],
            d.[codigo_daf],
            d.[daf],
            ue.[codigo_unidad_ejecutora],
            ue.[unidad_ejecutora],
            ue.[rnc],
            ue.[estado],
            ue.[creado],
            ue.[modificado],
            ue.[usuario_creacion],
            ue.[usuario_modificacion],
            ue.[portal_compra],
            ric.[id_categoria] AS [id_categoria],
            cat.[descripcion] AS [descripcion], -- <--- EF Core busca 'descripcion'
            ric.[estrutura] AS [estructura]
        FROM [dbo].[unidad_ejecutora] ue WITH (NOLOCK)
        INNER JOIN [dbo].[daf] d WITH (NOLOCK) ON ue.[id_daf] = d.[id]
        LEFT JOIN [contrato_sigob].[relacion_institucion_categoria] ric WITH (NOLOCK) ON ue.[id] = ric.[id_institucion]
        LEFT JOIN [SIGOB].[dbo].[categoria_tramite] cat WITH (NOLOCK) ON ric.[id_categoria] = cat.[codigo_categoria]
        WHERE ue.[codigo_unidad_ejecutora] = @codigo_unidad_ejecutora
          AND (@id_daf IS NULL OR ue.[id_daf] = @id_daf);

        RETURN;
    END

    -- 3. Obtener por DAF o Listar Todas
    SELECT 
        ue.[id],
        ue.[id_padre],
        ue.[id_daf],
        d.[codigo_daf],
        d.[daf],
        ue.[codigo_unidad_ejecutora],
        ue.[unidad_ejecutora],
        ue.[rnc],
        ue.[estado],
        ue.[creado],
        ue.[modificado],
        ue.[usuario_creacion],
        ue.[usuario_modificacion],
        ue.[portal_compra],
        ric.[id_categoria] AS [id_categoria],
        cat.[descripcion] AS [descripcion], -- <--- EF Core busca 'descripcion'
        ric.[estrutura] AS [estructura]
    FROM [dbo].[unidad_ejecutora] ue WITH (NOLOCK)
    INNER JOIN [dbo].[daf] d WITH (NOLOCK) ON ue.[id_daf] = d.[id]
    LEFT JOIN [contrato_sigob].[relacion_institucion_categoria] ric WITH (NOLOCK) ON ue.[id] = ric.[id_institucion]
    LEFT JOIN [SIGOB].[dbo].[categoria_tramite] cat WITH (NOLOCK) ON ric.[id_categoria] = cat.[codigo_categoria]
    WHERE (@id_daf IS NULL OR ue.[id_daf] = @id_daf)
    ORDER BY d.[daf] ASC, ue.[codigo_unidad_ejecutora] ASC;
END;
GO

---------------------------------
--Stores responsable_actividad
---------------------------------
-- Obtener Catálogo de Moldes

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Catalogo_Moldes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        codigo_tre,
        arbol_asociado,
        nombre_tre
    FROM [SIGOB].[dbo].[tre] WITH (NOLOCK)
    ORDER BY nombre_tre ASC;
END;
GO

-- Obtener Subtipos por Molde

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Subtipos_Por_Molde]
    @CodigoTre INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH SubtiposRank AS (
        SELECT 
            codigo_tipo_tre,
            codigo_tre,
            descripcion,
            abreviatura,
            -- Numera los registros duplicados según su ID (del más antiguo al más nuevo)
            ROW_NUMBER() OVER (PARTITION BY descripcion, abreviatura ORDER BY codigo_tipo_tre ASC) AS version_num,
            -- Cuenta el total de duplicados que tiene ese grupo
            COUNT(*) OVER (PARTITION BY descripcion, abreviatura) AS total_duplicados
        FROM [SIGOB].[dbo].[tipos_tre] WITH (NOLOCK)
        WHERE codigo_tre = @CodigoTre 
          AND activo = 1
    )
    SELECT 
        codigo_tipo_tre,
        codigo_tre,
        descripcion + 
            CASE 
                -- Si no tiene duplicados, se queda tal cual
                WHEN total_duplicados = 1 THEN ''
                -- El registro más nuevo (último ID) siempre es el Actual
                WHEN version_num = total_duplicados THEN ' (Flujo Nuevo)'
                -- Si exactamente son 2 registros, al primero se le llama simplemente (Anterior)
                WHEN total_duplicados = 2 AND version_num = 1 THEN ' (Flujo Viejo)'
                -- Si hay 3 o más, al penúltimo se le llama (Anterior 1)
                WHEN version_num = total_duplicados - 1 THEN ' (Flujo Viejo No.1)'
                -- Si hay más hacia atrás, se numeran de forma progresiva
                ELSE ' (Flujo Viejo No.' + CAST(total_duplicados - version_num AS VARCHAR(10)) + ')'
            END AS descripcion,
        abreviatura
    FROM SubtiposRank
    ORDER BY descripcion ASC, codigo_tipo_tre ASC;
END;

-- Obtener Usuarios por Molde

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Usuarios_Por_Molde]
    @CodigoTre INT -- Recibe el código del molde seleccionado (ej. 1)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT 
        da.codigo_despa,
        da.NOMBRE,
        da.INSTITUCION,
        da.descarea,
        da.cargo,
        da.email
    FROM [SIGOB].[dbo].[v_DespachoAreaActivos] da WITH (NOLOCK)
    INNER JOIN [SIGOB].[dbo].[usuario_tre] ut WITH (NOLOCK) 
        ON ut.codigo_despa = da.codigo_despa
    WHERE ut.codigo_tre = @CodigoTre
    ORDER BY da.NOMBRE ASC;
END;
GO

-- Obtener Actividades Disponibles por Subtipo(s)

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Actividades_Por_Subtipos]
    @CodigosSubtipos VARCHAR(MAX) -- Ej: '1088,1098,1099'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @T_Subtipos TABLE (codigo_tipo_tre INT);
    
    INSERT INTO @T_Subtipos (codigo_tipo_tre)
    SELECT TRY_CAST(value AS INT)
    FROM STRING_SPLIT(@CodigosSubtipos, ',')
    WHERE TRY_CAST(value AS INT) IS NOT NULL;

    SELECT DISTINCT 
        att.codigo_actividad,
        att.codigo_tipo_tre,
        a.descripcion AS descripcion_actividad,
        tt.descripcion AS descripcion_subtipo
    FROM [SIGOB].[dbo].[actividad_tipo_tre] att WITH (NOLOCK)
    INNER JOIN [SIGOB].[dbo].[actividad] a WITH (NOLOCK) 
        ON a.codigo_actividad = att.codigo_actividad
    INNER JOIN [SIGOB].[dbo].[tipos_tre] tt WITH (NOLOCK) 
        ON tt.codigo_tipo_tre = att.codigo_tipo_tre
    WHERE att.codigo_tipo_tre IN (SELECT codigo_tipo_tre FROM @T_Subtipos)
	-- Filtros de exclusión solicitados
      AND a.descripcion NOT LIKE 'RI%'
      AND a.descripcion <> 'Inicio trámite'
    ORDER BY descripcion_actividad ASC;
END;
GO

-- Consultar Actividades Actuales del Usuario

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Actividades_Usuario]
    @CodigoDespa CHAR(5)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ra.codigo_despa,
        ra.codigo_actividad,
        a.descripcion AS descripcion_actividad
    FROM [SIGOB].[dbo].[responsable_actividad] ra WITH (NOLOCK)
    INNER JOIN [SIGOB].[dbo].[actividad] a WITH (NOLOCK) 
        ON a.codigo_actividad = ra.codigo_actividad
    WHERE ra.codigo_despa = @CodigoDespa
    ORDER BY a.descripcion ASC;
END;
GO

-- Gestionar / Sincronizar Actividades del Responsable

CREATE OR ALTER PROCEDURE [dbo].[sp_Gestionar_Responsable_Actividades]
    @CodigoDespa CHAR(5),
    @ListaCodigosActividades VARCHAR(MAX) -- Lista separada por comas de actividades seleccionadas, ej: '968440,1878226,2083535'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Validar existencia del usuario en despacho
        IF NOT EXISTS (SELECT 1 FROM [SIGOB].[dbo].[v_DespachoAreaActivos] WITH (NOLOCK) WHERE codigo_despa = @CodigoDespa)
        BEGIN
            RAISERROR('El código de despacho del usuario no es válido.', 16, 1);
            RETURN;
        END

        -- Parsear la lista de actividades entrantes en una tabla temporal
        DECLARE @T_NuevasActividades TABLE (codigo_actividad INT);
        
        IF @ListaCodigosActividades IS NOT NULL AND LTRIM(RTRIM(@ListaCodigosActividades)) <> ''
        BEGIN
            INSERT INTO @T_NuevasActividades (codigo_actividad)
            SELECT TRY_CAST(value AS INT)
            FROM STRING_SPLIT(@ListaCodigosActividades, ',')
            WHERE TRY_CAST(value AS INT) IS NOT NULL;
        END

        BEGIN TRANSACTION;

        -- 1. Eliminar las actividades que el usuario tenía antes pero ya no fueron seleccionadas en la nueva lista
        DELETE FROM [SIGOB].[dbo].[responsable_actividad]
        WHERE codigo_despa = @CodigoDespa
          AND codigo_actividad NOT IN (SELECT codigo_actividad FROM @T_NuevasActividades);

        -- 2. Insertar las nuevas actividades que no existían previamente para este usuario
        INSERT INTO [SIGOB].[dbo].[responsable_actividad] (codigo_despa, codigo_actividad)
        SELECT @CodigoDespa, na.codigo_actividad
        FROM @T_NuevasActividades na
        WHERE NOT EXISTS (
            SELECT 1 
            FROM [SIGOB].[dbo].[responsable_actividad] ra 
            WHERE ra.codigo_despa = @CodigoDespa 
              AND ra.codigo_actividad = na.codigo_actividad
        );

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Asignación de actividades actualizada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

-- Obtener_Usuarios_Por_Actividad

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Usuarios_Por_Actividad]
    @CodigoActividad INT,
    @CodigoTre INT,
    @FiltroCargo VARCHAR(100) = NULL,
    @FiltroArea VARCHAR(100) = NULL,
    @BusquedaGeneral VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT 
        da.codigo_despa,
        da.NOMBRE AS Nombre,
        da.INSTITUCION AS Institucion,
        da.descarea AS DescArea,
        da.cargo AS Cargo,
        da.email AS Email,
        CASE 
            WHEN ra.codigo_actividad IS NOT NULL THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT)
        END AS EstaAsignado
    FROM [SIGOB].[dbo].[v_DespachoAreaActivos] da WITH (NOLOCK)
    INNER JOIN [SIGOB].[dbo].[usuario_tre] ut WITH (NOLOCK) 
        ON ut.codigo_despa = da.codigo_despa
    LEFT JOIN [SIGOB].[dbo].[responsable_actividad] ra WITH (NOLOCK)
        ON ra.codigo_despa = da.codigo_despa AND ra.codigo_actividad = @CodigoActividad
    WHERE ut.codigo_tre = @CodigoTre
      AND (@FiltroCargo IS NULL OR da.cargo LIKE '%' + @FiltroCargo + '%')
      AND (@FiltroArea IS NULL OR da.descarea LIKE '%' + @FiltroArea + '%')
      AND (@BusquedaGeneral IS NULL OR da.NOMBRE LIKE '%' + @BusquedaGeneral + '%' OR da.codigo_despa LIKE '%' + @BusquedaGeneral + '%')
    ORDER BY da.NOMBRE ASC;
END;
GO

-- Store para gestionar la asignación inversa (Una Actividad -> Muchos Usuarios)
CREATE OR ALTER PROCEDURE [dbo].[sp_Gestionar_Actividad_Usuarios]
    @CodigoActividad INT,
    @ListaCodigosDespa VARCHAR(MAX) -- Lista separada por comas, ej: 'D0001,D0005'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        DECLARE @T_Usuarios TABLE (codigo_despa CHAR(5));
        
        IF @ListaCodigosDespa IS NOT NULL AND LTRIM(RTRIM(@ListaCodigosDespa)) <> ''
        BEGIN
            INSERT INTO @T_Usuarios (codigo_despa)
            SELECT LTRIM(RTRIM(value))
            FROM STRING_SPLIT(@ListaCodigosDespa, ',')
            WHERE LTRIM(RTRIM(value)) <> '';
        END

        BEGIN TRANSACTION;

        -- 1. Eliminar la asignación de esta actividad a los usuarios que ya no están en la lista seleccionada
        DELETE FROM [SIGOB].[dbo].[responsable_actividad]
        WHERE codigo_actividad = @CodigoActividad
          AND codigo_despa NOT IN (SELECT codigo_despa FROM @T_Usuarios);

        -- 2. Insertar la actividad a los nuevos usuarios seleccionados que no la tenían
        INSERT INTO [SIGOB].[dbo].[responsable_actividad] (codigo_despa, codigo_actividad)
        SELECT tu.codigo_despa, @CodigoActividad
        FROM @T_Usuarios tu
        WHERE NOT EXISTS (
            SELECT 1 
            FROM [SIGOB].[dbo].[responsable_actividad] ra 
            WHERE ra.codigo_actividad = @CodigoActividad 
              AND ra.codigo_despa = tu.codigo_despa
        );

        COMMIT TRANSACTION;

        SELECT 1 AS CodigoResultado, 'Asignación de usuarios a la actividad actualizada exitosamente.' AS Mensaje;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE(),
                @ErrorSeverity INT = ERROR_SEVERITY(),
                @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

USE SIGOB
GO

-- Obtener Flujos por Molde Seleccionado
CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Flujos_Por_Molde]
    @CodigoTre INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT 
        fc.nombre_flujo AS NombreFlujo
    FROM [SIGOB_CONTRALORIA].[dbo].[flujo_actividades_config] fac WITH (NOLOCK)
    INNER JOIN [SIGOB_CONTRALORIA].[dbo].[flujos_catalogo] fc WITH (NOLOCK) 
        ON fc.id_flujo = fac.id_flujo
    WHERE fac.codigo_tre = @CodigoTre
      AND fc.activo = 1
    ORDER BY fc.nombre_flujo ASC;
END;
GO

-- Obtener Actividades por Molde y Flujo Seleccionado

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Actividades_Por_Flujo]
    @CodigoTre INT,
    @NombreFlujo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT 
        att.codigo_actividad,
        att.codigo_tipo_tre,
        a.descripcion AS descripcion_actividad,
        tt.descripcion AS descripcion_subtipo,
        @NombreFlujo AS flujo_correspondiente
    FROM [SIGOB_CONTRALORIA].[dbo].[flujo_actividades_config] fac WITH (NOLOCK)
    INNER JOIN [SIGOB_CONTRALORIA].[dbo].[flujos_catalogo] fc WITH (NOLOCK) 
        ON fc.id_flujo = fac.id_flujo
    -- Cruzamos directamente con la actividad configurada para este molde y flujo
    INNER JOIN [SIGOB].[dbo].[actividad_tipo_tre] att WITH (NOLOCK) 
        ON att.codigo_actividad = fac.codigo_actividad
    INNER JOIN [SIGOB].[dbo].[actividad] a WITH (NOLOCK) 
        ON a.codigo_actividad = att.codigo_actividad
    INNER JOIN [SIGOB].[dbo].[tipos_tre] tt WITH (NOLOCK) 
        ON tt.codigo_tipo_tre = att.codigo_tipo_tre
    WHERE fac.codigo_tre = @CodigoTre
      AND fc.nombre_flujo = @NombreFlujo
      -- Validamos que el tipo pertenezca al molde actual
      AND tt.codigo_tre = @CodigoTre
      -- Reglas de exclusión del sistema
      AND a.descripcion NOT LIKE 'RI%'
      AND a.descripcion <> 'Inicio trámite'
    ORDER BY descripcion_actividad ASC;
END;
GO