CREATE TABLE [dbo].[TreeNode] (
    [ID]         INT          IDENTITY (1, 1) NOT NULL,
    [ParentID]   INT          NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [Sort]       INT          NOT NULL,
    [IsEnable]   BIT          NOT NULL,
    [createDate] DATETIME     NOT NULL,
    [UpdateDate] DATETIME     NULL,
    CONSTRAINT [codigo_TreeNode] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [TreeNode_TreeNode_fk] FOREIGN KEY ([ParentID]) REFERENCES [dbo].[TreeNode] ([ID])
);