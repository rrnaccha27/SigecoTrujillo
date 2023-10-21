CREATE TABLE [dbo].[log_clr_exception] (
    [id]         INT            IDENTITY (1, 1) NOT NULL,
    [createdate] DATETIME       DEFAULT (getdate()) NULL,
    [numatcard]  VARCHAR (100)  NULL,
    [empresa]    NVARCHAR (100) NULL,
    [docentry]   NVARCHAR (100) NULL,
    [user]       VARCHAR (250)  NULL,
    [exception]  VARCHAR (MAX)  NULL
);