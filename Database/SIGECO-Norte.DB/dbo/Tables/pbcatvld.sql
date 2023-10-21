CREATE TABLE [dbo].[pbcatvld] (
    [pbv_name] VARCHAR (30)  NOT NULL,
    [pbv_vald] VARCHAR (254) NOT NULL,
    [pbv_type] SMALLINT      NOT NULL,
    [pbv_cntr] INT           NULL,
    [pbv_msg]  VARCHAR (254) NULL
);
GO
CREATE UNIQUE CLUSTERED INDEX [pbcatvld_idx]
    ON [dbo].[pbcatvld]([pbv_name] ASC);