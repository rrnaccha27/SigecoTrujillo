CREATE TABLE [dbo].[tipo_reporte](
	[codigo_tipo_reporte] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](250) NOT NULL,
	[nomenclatura] [varchar](3) NOT NULL,
	[estado_registro] [bit] NOT NULL,
	[fecha_registra] [datetime] NOT NULL,
	[usuario_registra] [varchar](25) NOT NULL,
	[fecha_modifica] [datetime] NULL,
	[usuario_modifica] [varchar](25) NULL,
 CONSTRAINT [PK_regla_tipo_reporte] PRIMARY KEY CLUSTERED 
(
	[codigo_tipo_reporte] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]