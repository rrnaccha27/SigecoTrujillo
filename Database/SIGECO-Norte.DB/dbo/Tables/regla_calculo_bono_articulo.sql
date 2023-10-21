CREATE TABLE [dbo].[regla_calculo_bono_articulo] (
    [codigo_regla_calculo_bono] INT NOT NULL,
    [codigo_articulo]           INT NOT NULL,
    [cantidad]                  INT NOT NULL,
    CONSTRAINT [PK_regla_calculo_bono_articulo] PRIMARY KEY CLUSTERED ([codigo_regla_calculo_bono] ASC, [codigo_articulo] ASC),
    CONSTRAINT [regla_calculo_bono_articulo_codigo_articulo_articulo_codigo_articulo] FOREIGN KEY ([codigo_articulo]) REFERENCES [dbo].[articulo] ([codigo_articulo]),
    CONSTRAINT [regla_calculo_bono_articulo_codigo_regla_calculo_bono_regla_calculo_bono_codigo_regla_calculo_bono] FOREIGN KEY ([codigo_regla_calculo_bono]) REFERENCES [dbo].[regla_calculo_bono] ([codigo_regla_calculo_bono])
);