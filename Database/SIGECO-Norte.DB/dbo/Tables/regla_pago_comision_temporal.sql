CREATE TABLE [dbo].[regla_pago_comision_temporal] (
    [id]                        UNIQUEIDENTIFIER NULL,
    [indice]                    INT              NOT NULL,
    [codigo_articulo]           INT              NOT NULL,
    [cantidad]                  INT              NOT NULL,
    [monto_comision]            DECIMAL (12, 4)  NOT NULL,
    [cantidad_cuotas_excepcion] INT              DEFAULT ((0)) NULL,
    [es_comision_manual]        BIT              DEFAULT ((0)) NOT NULL
);