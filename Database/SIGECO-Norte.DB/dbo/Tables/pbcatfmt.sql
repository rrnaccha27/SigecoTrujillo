CREATE TABLE [dbo].[pbcatfmt] (
    [pbf_name] VARCHAR (30)  NOT NULL,
    [pbf_frmt] VARCHAR (254) NOT NULL,
    [pbf_type] SMALLINT      NOT NULL,
    [pbf_cntr] INT           NULL
);
GO
CREATE UNIQUE CLUSTERED INDEX [pbcatfmt_idx]
    ON [dbo].[pbcatfmt]([pbf_name] ASC);