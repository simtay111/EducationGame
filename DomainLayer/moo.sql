

/****** Object:  Table [dbo].[AwardedPrize]    Script Date: 07/21/2014 20:01:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountPrize](
	[Id] [int] IDENTITY(1,1) NOT NULL,
		[IssueDate] [datetime] NULL,
	[PrizeName] [nvarchar](255) NULL,
	[PrizePoints] [int] NULL,
	[PrizeSku] [nvarchar](255) NULL,
	[TargetEmail] [nvarchar](255) NULL,
	[AccountId] [int] NULL,
	[AccountInformationId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



ALTER TABLE [dbo].[AccountPrize]  WITH CHECK ADD  CONSTRAINT [acctprizetoacctinfo] FOREIGN KEY([AccountInformationId])
REFERENCES [dbo].[AccountInformation] ([Id])
GO

ALTER TABLE [dbo].[AccountPrize] CHECK CONSTRAINT [acctprizetoacctinfo]
GO


alter table account add Points int default 0
alter table accountinformation add DateOfPrintedDaily datetime
alter table accountinformation add LastAccountSignedOn int default 0