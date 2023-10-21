CREATE TABLE [dbo].[regla_calculo_bono_matriz] (
    [codigo_regla_calculo_bono] INT             NOT NULL,
    [porcentaje_meta]           DECIMAL (10, 2) NOT NULL,
    [monto_meta]                DECIMAL (10, 2) NOT NULL,
    [porcentaje_pago]           DECIMAL (10, 2) NOT NULL,
    CONSTRAINT [PK_regla_calculo_bono_matriz] PRIMARY KEY CLUSTERED ([codigo_regla_calculo_bono] ASC, [porcentaje_meta] ASC),
    CONSTRAINT [regla_calculo_bono_matriz_codigo_regla_calculo_bono_regla_calculo_bono_codigo_regla_calculo_bono] FOREIGN KEY ([codigo_regla_calculo_bono]) REFERENCES [dbo].[regla_calculo_bono] ([codigo_regla_calculo_bono])
);