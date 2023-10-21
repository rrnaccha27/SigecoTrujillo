CREATE TABLE [dbo].[pbcatedt] (
    [pbe_name] VARCHAR (30)  NOT NULL,
    [pbe_edit] VARCHAR (254) NULL,
    [pbe_type] SMALLINT      NOT NULL,
    [pbe_cntr] INT           NULL,
    [pbe_seqn] SMALLINT      NOT NULL,
    [pbe_flag] INT           NULL,
    [pbe_work] CHAR (32)     NULL
);
GO
CREATE UNIQUE CLUSTERED INDEX [pbcatedt_idx]
    ON [dbo].[pbcatedt]([pbe_name] ASC, [pbe_seqn] ASC);