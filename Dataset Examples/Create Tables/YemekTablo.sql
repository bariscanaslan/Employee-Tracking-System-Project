USE [isTakipDB]
GO

/****** Object:  Table [dbo].[YemekTablo]    Script Date: 12/08/2024 16:29:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[YemekTablo](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[YemekAdi1] [nvarchar](100) NULL,
	[YemekAdi2] [nvarchar](100) NULL,
	[YemekAdi3] [nvarchar](100) NULL,
	[YemekAdi4] [nvarchar](100) NULL,
	[YemekAdi5] [nvarchar](100) NULL,
	[YemekAdi6] [nvarchar](100) NULL,
	[YemekAdi7] [nvarchar](100) NULL,
	[YemekAdi8] [nvarchar](100) NULL,
	[Tarih] [date] NULL,
	[Kalori1] [int] NULL,
	[Kalori2] [int] NULL,
	[Kalori3] [int] NULL,
	[Kalori4] [int] NULL,
	[Kalori5] [int] NULL,
	[Kalori6] [int] NULL,
	[Kalori7] [int] NULL,
	[Kalori8] [int] NULL,
 CONSTRAINT [PK_YemekTablo] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


