USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_DUYURULAR]    Script Date: 12/08/2024 16:27:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_DUYURULAR](
	[duyuruID] [int] IDENTITY(1,1) NOT NULL,
	[duyuruBaslik] [varchar](max) NULL,
	[duyuruIcerik] [varchar](max) NULL,
	[duyuruTarih] [datetime] NULL,
	[duyuruOlusturan] [int] NULL,
	[duyuruAktiflik] [bit] NULL,
	[duyuruCreationDate] [datetime] NULL,
	[duyuruDeletionDate] [datetime] NULL,
	[duyuruOlusturanBirim] [int] NULL,
 CONSTRAINT [PK_TBL_DUYURULAR] PRIMARY KEY CLUSTERED 
(
	[duyuruID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TBL_DUYURULAR]  WITH CHECK ADD  CONSTRAINT [FK_TBL_DUYURULAR_TBL_PERSONELLER] FOREIGN KEY([duyuruOlusturan])
REFERENCES [dbo].[TBL_PERSONELLER] ([personelID])
GO

ALTER TABLE [dbo].[TBL_DUYURULAR] CHECK CONSTRAINT [FK_TBL_DUYURULAR_TBL_PERSONELLER]
GO


