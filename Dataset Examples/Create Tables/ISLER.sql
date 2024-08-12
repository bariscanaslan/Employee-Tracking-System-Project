USE [isTakipDB]
GO

/****** Object:  Table [dbo].[TBL_ISLER]    Script Date: 12/08/2024 16:27:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TBL_ISLER](
	[isID] [int] IDENTITY(1,1) NOT NULL,
	[isBaslik] [nvarchar](max) NULL,
	[isAciklama] [nvarchar](max) NULL,
	[isPersonelID] [int] NULL,
	[iletilenTarih] [datetime] NULL,
	[yapilanTarih] [datetime] NULL,
	[isDurumID] [int] NULL,
	[isYorum] [nvarchar](max) NULL,
	[isCreationDate] [datetime] NULL,
	[isDeletionDate] [datetime] NULL,
	[isAktiflik] [bit] NULL,
	[isKabulEdilenTarih] [datetime] NULL,
	[isReddedilenTarih] [datetime] NULL,
 CONSTRAINT [PK_TBL_ISLER] PRIMARY KEY CLUSTERED 
(
	[isID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TBL_ISLER]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ISLER_TBL_DURUMLAR] FOREIGN KEY([isDurumID])
REFERENCES [dbo].[TBL_DURUMLAR] ([durumID])
GO

ALTER TABLE [dbo].[TBL_ISLER] CHECK CONSTRAINT [FK_TBL_ISLER_TBL_DURUMLAR]
GO

ALTER TABLE [dbo].[TBL_ISLER]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ISLER_TBL_PERSONELLER] FOREIGN KEY([isPersonelID])
REFERENCES [dbo].[TBL_PERSONELLER] ([personelID])
GO

ALTER TABLE [dbo].[TBL_ISLER] CHECK CONSTRAINT [FK_TBL_ISLER_TBL_PERSONELLER]
GO


