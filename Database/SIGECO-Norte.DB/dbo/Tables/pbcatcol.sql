CREATE TABLE [dbo].[pbcatcol] (
    [pbc_tnam] CHAR (30)     NULL,
    [pbc_tid]  INT           NULL,
    [pbc_ownr] CHAR (30)     NULL,
    [pbc_cnam] CHAR (30)     NULL,
    [pbc_cid]  SMALLINT      NULL,
    [pbc_labl] VARCHAR (254) NULL,
    [pbc_lpos] SMALLINT      NULL,
    [pbc_hdr]  VARCHAR (254) NULL,
    [pbc_hpos] SMALLINT      NULL,
    [pbc_jtfy] SMALLINT      NULL,
    [pbc_mask] VARCHAR (31)  NULL,
    [pbc_case] SMALLINT      NULL,
    [pbc_hght] SMALLINT      NULL,
    [pbc_wdth] SMALLINT      NULL,
    [pbc_ptrn] VARCHAR (31)  NULL,
    [pbc_bmap] CHAR (1)      NULL,
    [pbc_init] VARCHAR (254) NULL,
    [pbc_cmnt] VARCHAR (254) NULL,
    [pbc_edit] VARCHAR (31)  NULL,
    [pbc_tag]  VARCHAR (254) NULL
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [pbcatcol_idx]
    ON [dbo].[pbcatcol]([pbc_tnam] ASC, [pbc_ownr] ASC, [pbc_cnam] ASC);