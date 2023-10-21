CREATE TABLE [dbo].[piso_pabellon] (
    [codigo_piso_pabellon] INT           IDENTITY (1, 1) NOT NULL,
    [nombre_piso_pabellon] VARCHAR (100) NOT NULL,
    [estado_registro]      BIT           DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([codigo_piso_pabellon] ASC)
);