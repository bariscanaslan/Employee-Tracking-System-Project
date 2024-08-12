USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_PERSONELLER]    Script Date: 12/08/2024 16:28:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_PERSONELLER](
	[personelID] [int] IDENTITY(1,1) NOT NULL,
	[personelAdSoyad] [nvarchar](50) NULL,
	[personelKullaniciAdi] [nvarchar](50) NULL,
	[personelParola] [nvarchar](50) NULL,
	[personelFoto] [nvarchar](max) NULL,
	[personelBirimID] [int] NULL,
	[personelYetkiTurID] [int] NULL,
	[yeniPersonel] [bit] NULL,
	[aktiflik] [bit] NULL,
	[personelCreationDate] [datetime] NULL,
	[personelDeletionDate] [datetime] NULL,
	[personelDogumTarihi] [date] NULL,
	[mailAdresi] [nvarchar](200) NULL,
	[telefonNumarasi] [nvarchar](50) NULL,
	[personelIzinGun] [int] NULL,
 CONSTRAINT [PK_TBL_PERSONELLER] PRIMARY KEY CLUSTERED 
(
	[personelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TBL_PERSONELLER]  WITH CHECK ADD  CONSTRAINT [FK_TBL_PERSONELLER_TBL_BIRIMLER] FOREIGN KEY([personelBirimID])
REFERENCES [dbo].[TBL_BIRIMLER] ([birimID])
GO

ALTER TABLE [dbo].[TBL_PERSONELLER] CHECK CONSTRAINT [FK_TBL_PERSONELLER_TBL_BIRIMLER]
GO

ALTER TABLE [dbo].[TBL_PERSONELLER]  WITH CHECK ADD  CONSTRAINT [FK_TBL_PERSONELLER_TBL_YETKITURLER] FOREIGN KEY([personelYetkiTurID])
REFERENCES [dbo].[TBL_YETKITURLER] ([yetkiTurID])
GO

ALTER TABLE [dbo].[TBL_PERSONELLER] CHECK CONSTRAINT [FK_TBL_PERSONELLER_TBL_YETKITURLER]
GO


