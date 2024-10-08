USE [master]
GO
/****** Object:  Database [eMeterDB]    Script Date: 19-Jul-24 22:52:15 ******/
CREATE DATABASE [eMeterDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'eMeterDB', FILENAME = N'C:\DB\eMeterDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'eMeterDB_log', FILENAME = N'C:\DB\eMeterDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [eMeterDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eMeterDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [eMeterDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [eMeterDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [eMeterDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [eMeterDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [eMeterDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [eMeterDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [eMeterDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [eMeterDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [eMeterDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [eMeterDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [eMeterDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [eMeterDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [eMeterDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [eMeterDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [eMeterDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [eMeterDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [eMeterDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [eMeterDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [eMeterDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [eMeterDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [eMeterDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [eMeterDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [eMeterDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [eMeterDB] SET  MULTI_USER 
GO
ALTER DATABASE [eMeterDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [eMeterDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [eMeterDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [eMeterDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [eMeterDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [eMeterDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [eMeterDB] SET QUERY_STORE = OFF
GO
USE [eMeterDB]
GO
/****** Object:  Table [dbo].[BillDetails]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetails](
	[billID] [int] NOT NULL,
	[billDetailsID] [int] IDENTITY(1,1) NOT NULL,
	[slabID] [int] NULL,
	[slabRate] [decimal](18, 2) NULL,
	[unitsApplied] [int] NULL,
	[slabAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_BillDetails] PRIMARY KEY CLUSTERED 
(
	[billID] ASC,
	[billDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillMain]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillMain](
	[billID] [int] IDENTITY(1,1) NOT NULL,
	[meterID] [int] NOT NULL,
	[customerID] [int] NOT NULL,
	[issueDate] [date] NULL,
	[dueDate] [date] NULL,
	[statusID] [int] NULL,
	[billMonth] [date] NULL,
	[readingDate] [date] NULL,
	[unitsConsumed] [int] NULL,
	[createdBy] [varchar](100) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_BillMain] PRIMARY KEY CLUSTERED 
(
	[billID] ASC,
	[meterID] ASC,
	[customerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[fullName] [varchar](100) NULL,
	[emailAddress] [varchar](100) NULL,
	[mobileNo] [char](11) NULL,
	[cnicNo] [char](13) NULL,
	[fullAddress] [varchar](150) NULL,
	[cityName] [varchar](15) NULL,
	[activeInd] [bit] NULL,
	[createdBy] [varchar](50) NULL,
	[createdAt] [datetime] NULL,
	[customerID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[customerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeterDeails]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeterDeails](
	[meterID] [int] IDENTITY(1,1) NOT NULL,
	[customerID] [int] NOT NULL,
	[meterNo] [varchar](15) NULL,
	[refNo] [varchar](15) NULL,
	[oldRefNo] [varchar](15) NULL,
	[connectionDate] [date] NULL,
	[statusID] [int] NULL,
	[meterLoad] [int] NULL,
	[activeInd] [bit] NULL,
	[createdBy] [varchar](100) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_MeterDeails] PRIMARY KEY CLUSTERED 
(
	[meterID] ASC,
	[customerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeterType]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeterType](
	[typeID] [int] IDENTITY(1,1) NOT NULL,
	[typeName] [varchar](50) NULL,
	[activeInd] [bit] NULL,
	[createdBy] [varchar](100) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_MeterType] PRIMARY KEY CLUSTERED 
(
	[typeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registration]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registration](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [varchar](100) NULL,
	[password] [varchar](200) NULL,
	[customerID] [int] NULL,
	[activeInd] [bit] NULL,
	[activeDate] [datetime] NULL,
	[createdBy] [varchar](100) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SlabCodes]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SlabCodes](
	[slabID] [int] IDENTITY(1,1) NOT NULL,
	[slabName] [varchar](50) NULL,
	[slabFrom] [int] NULL,
	[slabTo] [int] NULL,
	[slabUnitRate] [decimal](18, 2) NULL,
	[activeInd] [bit] NULL,
	[createdBy] [varchar](100) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_SlabCodes] PRIMARY KEY CLUSTERED 
(
	[slabID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusCodes]    Script Date: 19-Jul-24 22:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusCodes](
	[statusID] [int] IDENTITY(1,1) NOT NULL,
	[statusName] [varchar](50) NULL,
	[activeInd] [bit] NULL,
	[createdBy] [varchar](100) NULL,
	[createdAt] [datetime] NULL,
 CONSTRAINT [PK_StatusCodes] PRIMARY KEY CLUSTERED 
(
	[statusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [eMeterDB] SET  READ_WRITE 
GO
