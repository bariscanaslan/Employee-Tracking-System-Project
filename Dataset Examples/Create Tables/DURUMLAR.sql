USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_DURUMLAR]    Script Date: 12/08/2024 16:27:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_DURUMLAR](
	[durumID] [int] IDENTITY(1,1) NOT NULL,
	[durumAd] [nvarchar](50) NULL,
	[durumCreationDate] [datetime] NULL,
	[durumAktiflik] [bit] NULL,
	[durumDeletionDate] [datetime] NULL,
 CONSTRAINT [PK_TBL_DURUMLAR] PRIMARY KEY CLUSTERED 
(
	[durumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


