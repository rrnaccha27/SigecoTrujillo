CREATE PROCEDURE [dbo].[up_personal_actualizar_estado]
(
	@p_codigo_personal		INT
	,@p_usuario_modifica		VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_codigo_registro				INT
		,@v_indice						INT
		,@v_total						INT
		,@v_codigo_detalle_cronograma	INT

	DECLARE 
		@t_comision TABLE
	(
		id							INT identity(1, 1)
		,codigo_detalle_cronograma	INT
	)

	UPDATE dbo.personal
	SET  
		estado_registro = 0
		,usuario_modifica = @p_usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_personal = @p_codigo_personal

	UPDATE 
		dbo.personal_canal_grupo
	SET  
		estado_registro = 0
		,usuario_modifica = @p_usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_personal = @p_codigo_personal

	SELECT TOP 1
		@v_codigo_registro = codigo_registro
	FROM
		dbo.personal_canal_grupo
	WHERE
		codigo_personal = @p_codigo_personal
		AND estado_registro = 0
	ORDER BY 
		fecha_registra DESC

	INSERT INTO	
		@t_comision
	SELECT 
		dc.codigo_detalle
	FROM
		dbo.cronograma_pago_comision cpc
	INNER JOIN
		dbo.detalle_cronograma dc
		ON dc.codigo_cronograma = cpc.codigo_cronograma
	WHERE
		cpc.codigo_personal_canal_grupo = @v_codigo_registro
		and dc.codigo_estado_cuota = 1

	SELECT @v_total = MAX(id), @v_indice = 1 FROM @t_comision 

	WHILE (@v_indice <= @v_total)
	BEGIN
		SELECT
			@v_codigo_detalle_cronograma = codigo_detalle_cronograma
		FROM
			@t_comision
		WHERE
			id = @v_indice

		EXEC dbo.up_planilla_anular_pago_comision @v_codigo_detalle_cronograma, @p_usuario_modifica, 'POR DESACTIVACION DE PERSONAL.'

		SET @v_indice = @v_indice + 1
	END

	INSERT INTO 
		dbo.comision_personal_inactivo
		(
			codigo_detalle_cronograma
			,liquidado
			,fecha_registra
			,usuario_registra
		)
	SELECT
		codigo_detalle_cronograma
		,0
		,GETDATE()
		,@p_usuario_modifica
	FROM
		@t_comision

	SET NOCOUNT OFF
END;