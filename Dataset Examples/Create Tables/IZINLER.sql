USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_IZINLER]    Script Date: 12/08/2024 16:28:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_IZINLER](
	[izinID] [int] IDENTITY(1,1) NOT NULL,
	[izinPersonelID] [int] NULL,
	[izinBaslangicTarihi] [datetime] NULL,
	[izinBitisTarihi] [datetime] NULL,
	[izinOnay] [bit] NULL,
	[izinTalepTarihi] [datetime] NULL,
	[izinRed] [bit] NULL,
	[izinGunuSayisi] [int] NULL,
	[izinDegerlendirmeTarihi] [datetime] NULL,
 CONSTRAINT [PK_TBL_IZINLER] PRIMARY KEY CLUSTERED 
(
	[izinID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TBL_IZINLER]  WITH CHECK ADD  CONSTRAINT [FK_TBL_IZINLER_TBL_PERSONELLER] FOREIGN KEY([izinPersonelID])
REFERENCES [dbo].[TBL_PERSONELLER] ([personelID])
GO

ALTER TABLE [dbo].[TBL_IZINLER] CHECK CONSTRAINT [FK_TBL_IZINLER_TBL_PERSONELLER]
GO


