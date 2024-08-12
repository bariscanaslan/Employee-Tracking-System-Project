USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_BIRIMLER]    Script Date: 12/08/2024 16:25:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_BIRIMLER](
	[birimID] [int] IDENTITY(1,1) NOT NULL,
	[birimAd] [nvarchar](50) NULL,
	[aktiflik] [bit] NULL,
	[birimCreationDate] [datetime] NULL,
	[birimDeletionDate] [datetime] NULL,
 CONSTRAINT [PK_TBL_BIRIMLER] PRIMARY KEY CLUSTERED 
(
	[birimID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


