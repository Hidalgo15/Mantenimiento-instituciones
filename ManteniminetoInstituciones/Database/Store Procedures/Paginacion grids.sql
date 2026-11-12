USE SIGOB_CONTRALORIA
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_unidad_ejecutora_ObtenerPaginado]
    @id INT = NULL,
    @codigo_unidad_ejecutora VARCHAR(10) = NULL,
    @id_daf INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT
        ue.id,
        ue.id_padre,
        ue.id_daf,
        d.codigo_daf,
        d.daf,
        ue.codigo_unidad_ejecutora,
        ue.unidad_ejecutora,
        ue.rnc,
        ue.estado,
        ue.creado,
        ue.modificado,
        ue.usuario_creacion,
        ue.usuario_modificacion,
        ue.portal_compra,
        ric.id_categoria AS id_categoria,
        cat.descripcion AS descripcion,
        ric.estrutura AS estructura
    INTO #Rows
    FROM [dbo].[unidad_ejecutora] ue WITH (NOLOCK)
    INNER JOIN [dbo].[daf] d WITH (NOLOCK) ON ue.id_daf = d.id
    LEFT JOIN [contrato_sigob].[relacion_institucion_categoria] ric WITH (NOLOCK) ON ue.id = ric.id_institucion
    LEFT JOIN [SIGOB].[dbo].[categoria_tramite] cat WITH (NOLOCK) ON ric.id_categoria = cat.codigo_categoria
    WHERE
        (@id IS NOT NULL AND ue.id = @id)
        OR
        (@id IS NULL AND @codigo_unidad_ejecutora IS NOT NULL
            AND ue.codigo_unidad_ejecutora = @codigo_unidad_ejecutora
            AND (@id_daf IS NULL OR ue.id_daf = @id_daf))
        OR
        (@id IS NULL AND @codigo_unidad_ejecutora IS NULL
            AND (@id_daf IS NULL OR ue.id_daf = @id_daf));

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_daf, codigo_unidad_ejecutora, id, id_categoria) AS RowNumber, *
        FROM #Rows
    )
    SELECT id, id_padre, id_daf, codigo_daf, daf, codigo_unidad_ejecutora, unidad_ejecutora,
           rnc, estado, creado, modificado, usuario_creacion, usuario_modificacion, portal_compra,
           id_categoria, descripcion, estructura
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [ct].[sp_CuentasCt_ObtenerPaginado]
    @CuentaID INT = NULL,
    @busqueda NVARCHAR(255) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT CuentaID, id_cuenta,
           RTRIM(codigo_cuenta) AS codigo_cuenta,
           RTRIM(descripcion_cuenta) AS descripcion_cuenta
    INTO #Rows
    FROM [ct].[cuentas]
    WHERE (@CuentaID IS NULL OR CuentaID = @CuentaID)
      AND (@busqueda IS NULL OR codigo_cuenta LIKE '%' + @busqueda + '%'
           OR descripcion_cuenta LIKE '%' + @busqueda + '%');

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_cuenta, CuentaID) AS RowNumber, *
        FROM #Rows
    )
    SELECT CuentaID, id_cuenta, codigo_cuenta, descripcion_cuenta
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [ct].[sp_Obtener_Requisitos_Paginado]
    @Busqueda NVARCHAR(MAX) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Busqueda = NULLIF(LTRIM(RTRIM(@Busqueda)), '');
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT RequisitosID, id_requisitos, REQUISITOS
    INTO #Rows
    FROM [SIGOB_CONTRALORIA].[ct].[requisitos] WITH (NOLOCK)
    WHERE (@Busqueda IS NULL OR REQUISITOS LIKE '%' + @Busqueda + '%')
    OPTION (RECOMPILE);

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY RequisitosID) AS RowNumber, *
        FROM #Rows
    )
    SELECT RequisitosID, id_requisitos, REQUISITOS
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [ct].[sp_Obtener_Relacion_Cuenta_Requisitos_Paginado]
    @IdCuenta FLOAT = NULL,
    @IdRequisito INT = NULL,
    @Estado BIT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT r.CuentaRequisitosID, r.id_cuenta, c.codigo_cuenta, c.descripcion_cuenta,
           r.id_requisito, req.REQUISITOS AS descripcion_requisito, r.estado
    INTO #Rows
    FROM [SIGOB_CONTRALORIA].[ct].[relacion_cuenta_requisitos] r WITH (NOLOCK)
    INNER JOIN [SIGOB_CONTRALORIA].[ct].[cuentas] c WITH (NOLOCK) ON c.id_cuenta = r.id_cuenta
    INNER JOIN [SIGOB_CONTRALORIA].[ct].[requisitos] req WITH (NOLOCK) ON req.id_requisitos = r.id_requisito
    WHERE (@IdCuenta IS NULL OR r.id_cuenta = @IdCuenta)
      AND (@IdRequisito IS NULL OR r.id_requisito = @IdRequisito)
      AND (@Estado IS NULL OR r.estado = @Estado)
    OPTION (RECOMPILE);

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_cuenta, descripcion_requisito, CuentaRequisitosID) AS RowNumber, *
        FROM #Rows
    )
    SELECT CuentaRequisitosID, id_cuenta, codigo_cuenta, descripcion_cuenta,
           id_requisito, descripcion_requisito, estado
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Cuentas_Obtener_Paginado]
    @id_cuenta INT = NULL,
    @tipo_tramite INT = NULL,
    @busqueda NVARCHAR(255) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT c.id_cuenta, c.codigo_cuenta, c.descripcion_cuenta, c.tipo_tramite,
           RTRIM(t.tipo) AS tipo_tramite_nombre
    INTO #Rows
    FROM [dbo].[cuentas] c
    INNER JOIN [dbo].[tipo_tramite_pago] t ON c.tipo_tramite = t.id
    WHERE (@id_cuenta IS NULL OR c.id_cuenta = @id_cuenta)
      AND (@tipo_tramite IS NULL OR c.tipo_tramite = @tipo_tramite)
      AND (@busqueda IS NULL OR c.codigo_cuenta LIKE '%' + @busqueda + '%'
           OR c.descripcion_cuenta LIKE '%' + @busqueda + '%');

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_cuenta, id_cuenta) AS RowNumber, *
        FROM #Rows
    )
    SELECT id_cuenta, codigo_cuenta, descripcion_cuenta, tipo_tramite, tipo_tramite_nombre
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_requisitos_contratos_Obtener_Paginado]
    @codigo_requisito INT = NULL,
    @busqueda NVARCHAR(255) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT codigo_requisito, descripcion_requisito, codigo_documento_tre
    INTO #Rows
    FROM [dbo].[requisitos_contratos]
    WHERE (@codigo_requisito IS NULL OR codigo_requisito = @codigo_requisito)
      AND (@busqueda IS NULL
           OR CAST(codigo_requisito AS NVARCHAR(50)) LIKE '%' + @busqueda + '%'
           OR descripcion_requisito LIKE '%' + @busqueda + '%'
           OR CAST(codigo_documento_tre AS NVARCHAR(50)) LIKE '%' + @busqueda + '%');

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_requisito) AS RowNumber, *
        FROM #Rows
    )
    SELECT codigo_requisito, descripcion_requisito, codigo_documento_tre
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_clasificacion_contratos_Obtener_Paginado]
    @codigo_clasificacion INT = NULL,
    @contrato_tipo VARCHAR(50) = NULL,
    @tipo_operacion VARCHAR(50) = NULL,
    @contrato_proceso_seleccion VARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT codigo_clasificacion, contrato_tipo, tipo_operacion, contrato_proceso_seleccion
    INTO #Rows
    FROM [dbo].[clasificacion_contratos] WITH (NOLOCK)
    WHERE
        (@codigo_clasificacion IS NOT NULL AND codigo_clasificacion = @codigo_clasificacion)
        OR
        (@codigo_clasificacion IS NULL
            AND (@contrato_tipo IS NULL OR contrato_tipo = @contrato_tipo)
            AND (@tipo_operacion IS NULL OR tipo_operacion = @tipo_operacion)
            AND (@contrato_proceso_seleccion IS NULL OR contrato_proceso_seleccion = @contrato_proceso_seleccion));

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_clasificacion DESC) AS RowNumber, *
        FROM #Rows
    )
    SELECT codigo_clasificacion, contrato_tipo, tipo_operacion, contrato_proceso_seleccion
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Obtener_Relacion_Contrato_Requisito_Paginado]
    @CodigoClasificacion INT = NULL,
    @CodigoRequisito INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @PageNumber = CASE WHEN @PageNumber < 1 THEN 1 ELSE @PageNumber END;
    SET @PageSize = CASE WHEN @PageSize < 1 THEN 10 WHEN @PageSize > 100 THEN 100 ELSE @PageSize END;

    SELECT r.id, r.codigo_clasificacion, c.contrato_tipo, c.tipo_operacion,
           c.contrato_proceso_seleccion, r.codigo_requisito,
           req.descripcion_requisito, req.codigo_documento_tre
    INTO #Rows
    FROM [SIGOB_CONTRALORIA].[dbo].[relacion_contrato_requisito] r WITH (NOLOCK)
    INNER JOIN [SIGOB_CONTRALORIA].[dbo].[clasificacion_contratos] c WITH (NOLOCK)
        ON c.codigo_clasificacion = r.codigo_clasificacion
    INNER JOIN [SIGOB_CONTRALORIA].[dbo].[requisitos_contratos] req WITH (NOLOCK)
        ON req.codigo_requisito = r.codigo_requisito
    WHERE (@CodigoClasificacion IS NULL OR r.codigo_clasificacion = @CodigoClasificacion)
      AND (@CodigoRequisito IS NULL OR r.codigo_requisito = @CodigoRequisito)
    OPTION (RECOMPILE);

    SELECT @TotalCount = COUNT(*) FROM #Rows;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 1
        ELSE CEILING(CONVERT(FLOAT, @TotalCount) / @PageSize) END;
    IF @PageNumber > @TotalPages SET @PageNumber = @TotalPages;

    ;WITH Numbered AS
    (
        SELECT ROW_NUMBER() OVER (ORDER BY codigo_clasificacion, descripcion_requisito, id) AS RowNumber, *
        FROM #Rows
    )
    SELECT id, codigo_clasificacion, contrato_tipo, tipo_operacion, contrato_proceso_seleccion,
           codigo_requisito, descripcion_requisito, codigo_documento_tre
    FROM Numbered
    WHERE RowNumber BETWEEN (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize + 1
                        AND CONVERT(BIGINT, @PageNumber) * @PageSize
    ORDER BY RowNumber;
END
GO
