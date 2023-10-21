CREATE PROCEDURE dbo.up_operacion_cuota_comision_listado
(
	@p_codigo_detalle_cronograma	INT
	,@p_codigo_usuario				VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

DECLARE
	@v_codigo_perfil_usuario	INT,
	@v_modificacion				INT = 0

	IF (UPPER(@p_codigo_usuario) <> 'ROOT')
	BEGIN
		SET @v_codigo_perfil_usuario = (select top 1 codigo_perfil_usuario from usuario where codigo_usuario = @p_codigo_usuario)

		SET @v_modificacion = CASE WHEN EXISTS(select * from item_tipo_acceso where codigo_perfil_usuario = @v_codigo_perfil_usuario and estado_registro = 1 and codigo_tipo_acceso_item in (24)) THEN 1 ELSE 0 END

		IF (@v_modificacion = 0)
		BEGIN
			SET @v_modificacion = CASE WHEN EXISTS(select * from item_tipo_acceso where codigo_perfil_usuario = @v_codigo_perfil_usuario and estado_registro = 1 and codigo_tipo_acceso_item in (23)) THEN NULL ELSE 0 END
		END
	END
	ELSE
	BEGIN
		SET @v_modificacion = NULl
	END

	SELECT 
		occ.codigo_operacion_cuota_comision
		,toc.nombre as nombre_operacion
		,occ.motivo_movimiento as observacion
		,CONVERT(VARCHAR, occ.fecha_movimiento, 103) + ' ' + CONVERT(VARCHAR, occ.fecha_movimiento, 108) AS fecha_operacion
		,CASE WHEN occ.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS nombre_estado
		,dbo.fn_obtener_nombre_usuario(occ.usuario_registra) as usuario
		,
		CASE WHEN occ.codigo_tipo_operacion_cuota = 6 THEN
		(SELECT TOP 1 convert(varchar, dcl.monto_neto)FROM dbo.detalle_cronograma_log dcl WHERE dcl.codigo_detalle = occ.codigo_detalle_cronograma and CONVERT(VARCHAR, dcl.fecha_log, 103) + ' ' + CONVERT(VARCHAR, dcl.fecha_log, 108) = CONVERT(VARCHAR, occ.fecha_movimiento, 103) + ' ' + CONVERT(VARCHAR, occ.fecha_movimiento, 108) )
		ELSE '' END as valor_original
	FROM 
		dbo.operacion_cuota_comision occ 
	INNER JOIN dbo.tipo_operacion_cuota toc 
		ON toc.codigo_tipo_operacion_cuota = occ.codigo_tipo_operacion_cuota
	WHERE 
		occ.codigo_detalle_cronograma = @p_codigo_detalle_cronograma
		AND ( (@v_modificacion IS NULL) OR (@v_modificacion IS NOT NULL AND (@v_modificacion = 1 AND occ.codigo_tipo_operacion_cuota = 6)) )
	ORDER BY
		occ.fecha_registra DESC;

	SET NOCOUNT OFF
END;