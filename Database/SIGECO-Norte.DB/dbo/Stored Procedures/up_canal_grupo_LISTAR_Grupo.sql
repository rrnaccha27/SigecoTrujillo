CREATE PROCEDURE [dbo].[up_canal_grupo_LISTAR_Grupo]
@codigo_padre                                       int
AS
BEGIN
       SELECT 
          codigo_canal_grupo
          ,es_canal_grupo
          ,nombre
          ,codigo_padre
          ,estado_registro
          ,fecha_registra
          ,usuario_registra
          ,fecha_modifica
          ,usuario_modifica
       FROM canal_grupo WITH (NOLOCK)
       WHERE es_canal_grupo=0
          and codigo_padre = @codigo_padre
END