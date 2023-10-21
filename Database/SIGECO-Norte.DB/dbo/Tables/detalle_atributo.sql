CREATE TABLE [dbo].[detalle_atributo] (
    [codigo_detalle_atributo] INT           IDENTITY (1, 1) NOT NULL,
    [codigo_detalle_entidad]  INT           NOT NULL,
    [nombre_atributo]         VARCHAR (50)  NOT NULL,
    [valor_atributo]          VARCHAR (250) NOT NULL,
    CONSTRAINT [codigo_detalle_atributo] PRIMARY KEY CLUSTERED ([codigo_detalle_atributo] ASC),
    CONSTRAINT [detalle_entidad_detalle_atributo_fk] FOREIGN KEY ([codigo_detalle_entidad]) REFERENCES [dbo].[detalle_entidad] ([codigo_detalle_entidad])
);