USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_YETKITURLER]    Script Date: 12/08/2024 16:28:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_YETKITURLER](
	[yetkiTurID] [int] IDENTITY(1,1) NOT NULL,
	[yetkiTurAd] [nvarchar](50) NULL,
	[yetkiTurCreationDate] [datetime] NULL,
	[yetkiTurDeletionDate] [datetime] NULL,
	[yetkiTurAktiflik] [bit] NULL,
 CONSTRAINT [PK_TBL_YETKITURLER] PRIMARY KEY CLUSTERED 
(
	[yetkiTurID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


