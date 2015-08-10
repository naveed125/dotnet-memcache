CREATE TABLE [dbo].[Customers] (
    [Id]      INT IDENTITY(1,1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [Email]   NVARCHAR (50) NULL,
    [Company] NVARCHAR (50) NULL,
    [Phone]   NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Orders] (
    [Id]         INT     IDENTITY(1,1)      NOT NULL,
    [CustomerId] INT           NOT NULL,
    [Price]      INT           NOT NULL,
    [OrderDate]  DATETIME      NOT NULL,
    [Comments]   NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

