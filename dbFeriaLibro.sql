USE [master]
GO
/****** Object:  Database [dbFeriaLibro]    Script Date: 29/1/2019 12:35:50 ******/
CREATE DATABASE [dbFeriaLibro]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbFeriaLibro', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\dbFeriaLibro.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dbFeriaLibro_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\dbFeriaLibro_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [dbFeriaLibro] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbFeriaLibro].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dbFeriaLibro] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET ARITHABORT OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dbFeriaLibro] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dbFeriaLibro] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET  DISABLE_BROKER 
GO
ALTER DATABASE [dbFeriaLibro] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dbFeriaLibro] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET RECOVERY FULL 
GO
ALTER DATABASE [dbFeriaLibro] SET  MULTI_USER 
GO
ALTER DATABASE [dbFeriaLibro] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dbFeriaLibro] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dbFeriaLibro] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dbFeriaLibro] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [dbFeriaLibro] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'dbFeriaLibro', N'ON'
GO
ALTER DATABASE [dbFeriaLibro] SET QUERY_STORE = OFF
GO
USE [dbFeriaLibro]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [dbFeriaLibro]
GO
/****** Object:  Table [dbo].[Pedido]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedido](
	[pedidoID] [int] IDENTITY(1,1) NOT NULL,
	[librosalaID] [int] NOT NULL,
	[clienteID] [int] NOT NULL,
	[vendedorID] [nvarchar](128) NULL,
	[estadopedidoID] [int] NOT NULL,
	[cantidadPedido] [int] NOT NULL,
	[subTotalPedido] [decimal](18, 2) NOT NULL,
	[ivaPedido] [decimal](18, 2) NOT NULL,
	[totalPedido] [decimal](18, 2) NOT NULL,
	[fechaInicioPedido] [datetime] NOT NULL,
	[fechaFinPedido] [datetime] NULL,
 CONSTRAINT [PK_Pedido_1] PRIMARY KEY CLUSTERED 
(
	[pedidoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Listar_Pedidos_Resumen]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_Listar_Pedidos_Resumen]
AS
	SELECT	
		Pedido.clienteID,
		Pedido.estadopedidoID,
		SUM(Pedido.subTotalPedido) as totalSubTotal,
		SUM(Pedido.ivaPedido) as totalIva,
		SUM(Pedido.totalPedido) as totalPedido,
		SUM(Pedido.cantidadPedido) as totalCantidadPedido,
		COUNT(Pedido.pedidoID) as totalPedidos
	FROM Pedido 
	GROUP BY Pedido.clienteID, Pedido.estadopedidoID
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[clienteID] [int] IDENTITY(4,1) NOT NULL,
	[clienteCodigo] [int] NULL,
	[clienteCI_RUC] [varchar](13) NOT NULL,
	[clienteNombre] [varchar](25) NOT NULL,
	[clienteApellido] [varchar](25) NULL,
	[clienteCorreo] [varchar](75) NOT NULL,
	[clienteTelefono] [nchar](10) NULL,
	[clienteFechaRegistro] [datetime] NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[clienteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Listar_Pedidos_PorLiquidar]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_Listar_Pedidos_PorLiquidar]
AS
	SELECT	
		Pedido.clienteID,
		SUM(Pedido.subTotalPedido) as totalSubTotal,
		SUM(Pedido.ivaPedido) as totalIva,
		SUM(Pedido.totalPedido) as totalPedido,
		SUM(Pedido.cantidadPedido) as totalCantidadPedido,
		COUNT(Pedido.pedidoID) as totalPedidos
	FROM Pedido 
	WHERE Pedido.estadopedidoID = 3
	GROUP BY Pedido.clienteID
GO
/****** Object:  View [dbo].[View_Listar_Pedidos_Por_Lq_Clientes]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Listar_Pedidos_Por_Lq_Clientes]
AS
SELECT        dbo.View_Listar_Pedidos_PorLiquidar.totalSubTotal, dbo.View_Listar_Pedidos_PorLiquidar.totalIva, dbo.View_Listar_Pedidos_PorLiquidar.totalPedido, dbo.View_Listar_Pedidos_PorLiquidar.totalPedidos, 
                         dbo.View_Listar_Pedidos_PorLiquidar.totalCantidadPedido, dbo.Cliente.clienteID, dbo.Cliente.clienteCodigo, dbo.Cliente.clienteCI_RUC, dbo.Cliente.clienteNombre, dbo.Cliente.clienteApellido, dbo.Cliente.clienteCorreo, 
                         dbo.Cliente.clienteTelefono, dbo.Cliente.clienteFechaRegistro
FROM            dbo.View_Listar_Pedidos_PorLiquidar INNER JOIN
                         dbo.Cliente ON dbo.View_Listar_Pedidos_PorLiquidar.clienteID = dbo.Cliente.clienteID
GO
/****** Object:  View [dbo].[View_Listar_Pedidos_ResumenCliente]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Listar_Pedidos_ResumenCliente]
AS
SELECT        dbo.View_Listar_Pedidos_Resumen.estadopedidoID, dbo.View_Listar_Pedidos_Resumen.totalSubTotal, dbo.View_Listar_Pedidos_Resumen.totalIva, dbo.View_Listar_Pedidos_Resumen.totalPedido, 
                         dbo.View_Listar_Pedidos_Resumen.totalCantidadPedido, dbo.View_Listar_Pedidos_Resumen.totalPedidos, dbo.Cliente.clienteID, dbo.Cliente.clienteCodigo, dbo.Cliente.clienteCI_RUC, dbo.Cliente.clienteNombre, 
                         dbo.Cliente.clienteApellido, dbo.Cliente.clienteCorreo, dbo.Cliente.clienteTelefono, dbo.Cliente.clienteFechaRegistro
FROM            dbo.View_Listar_Pedidos_Resumen INNER JOIN
                         dbo.Cliente ON dbo.View_Listar_Pedidos_Resumen.clienteID = dbo.Cliente.clienteID
GO
/****** Object:  Table [dbo].[LibroSala]    Script Date: 29/1/2019 12:35:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LibroSala](
	[librosalaID] [int] IDENTITY(1,1) NOT NULL,
	[salaID] [int] NOT NULL,
	[libroID] [int] NOT NULL,
	[cantidadLibroSala] [int] NOT NULL,
	[precioLibroSala] [decimal](18, 2) NOT NULL,
	[estadoLibroSala] [bit] NOT NULL,
 CONSTRAINT [PK_LibroSala_1] PRIMARY KEY CLUSTERED 
(
	[librosalaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Libro]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Libro](
	[libroID] [int] IDENTITY(1,1) NOT NULL,
	[libroMateria] [int] NOT NULL,
	[libroNombre] [varchar](100) NOT NULL,
	[libroAutor] [varchar](75) NOT NULL,
	[libroISBN] [varchar](13) NOT NULL,
	[libroIVA] [bit] NULL,
	[libroSinopsis] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Libro] PRIMARY KEY CLUSTERED 
(
	[libroID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vimporte_total]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Reporte 1--
create view [dbo].[Vimporte_total]
as
select libroNombre, precioLibroSala
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID
where Pedido.estadopedidoID = 3 and Pedido.estadopedidoID=2;

GO
/****** Object:  View [dbo].[suma]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[suma]
as
select sum(precioLibroSala) as Total
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID
where Pedido.estadopedidoID = 3 and Pedido.estadopedidoID=2;


--Reporte 2--
GO
/****** Object:  Table [dbo].[Vendedor]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendedor](
	[vendedorID] [int] IDENTITY(100,1) NOT NULL,
	[aspUserID] [nvarchar](128) NOT NULL,
	[vendedorEstado] [bit] NOT NULL,
 CONSTRAINT [PK_Vendedor_1] PRIMARY KEY CLUSTERED 
(
	[aspUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Vvendedores]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Vvendedores]
as
select TOP(3)  vendedorNombre, sum(totalPedido) as total  
from Vendedor inner join Pedido
on Vendedor.vendedorID = Pedido.vendedorID
group by Vendedor.vendedorNombre
order by total ASC 
 

 --Reporte 3--
GO
/****** Object:  View [dbo].[Vtitulos]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create view [dbo].[Vtitulos]
as
select libroNombre, count(*) as totalpedido
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID
group by libroNombre;

--Reporte 4--
GO
/****** Object:  Table [dbo].[Materia]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Materia](
	[materiaID] [int] IDENTITY(1,1) NOT NULL,
	[materiaNombre] [varchar](75) NOT NULL,
 CONSTRAINT [PK_Materia] PRIMARY KEY CLUSTERED 
(
	[materiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[VAutor]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create view [dbo].[VAutor]
as
select libroAutor, count(*) as numero
from libro inner join Materia
on libro.libroMateria=materiaID
group by libroAutor;

GO
/****** Object:  View [dbo].[VMaterias]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create view [dbo].[VMaterias]
as
select materiaNombre, count(*) as numero
from libro inner join Materia
on libro.libroMateria=materiaID
group by materiaNombre;

--Reporte 5--
GO
/****** Object:  View [dbo].[VDia]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 create view [dbo].[VDia]
as
select fechaInicioPedido, count(*) as NumeroPedido
from Pedido
group by fechaInicioPedido;

--Reporte 6--
GO
/****** Object:  View [dbo].[Vrecaudado_titulo]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[Vrecaudado_titulo]
as
select libroNombre, count(*) as libros, sum(precioLibroSala) as total
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID 
group by libroNombre;

--Reporte 7--
GO
/****** Object:  Table [dbo].[Libreria]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Libreria](
	[libreriaID] [int] IDENTITY(1,1) NOT NULL,
	[libreriaNombre] [varchar](100) NOT NULL,
	[libreriaRUC] [char](13) NOT NULL,
	[libreriaDireccion] [varchar](250) NOT NULL,
	[libreriaTelefono] [char](9) NOT NULL,
 CONSTRAINT [PK_Libreria] PRIMARY KEY CLUSTERED 
(
	[libreriaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sala]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sala](
	[salaID] [int] IDENTITY(1,1) NOT NULL,
	[salaLibreria] [int] NOT NULL,
	[salaNombre] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Sala] PRIMARY KEY CLUSTERED 
(
	[salaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Listar_Libros_Materia_Sala_Libreria]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[View_Listar_Libros_Materia_Sala_Libreria] AS
SELECT 
	Lbr.libroID,
	Lbr.libroNombre,
	Lbr.libroAutor,
	Lbr.libroISBN,
	Lbr.libroSinopsis,
	Lbr.libroIVA,
	Mtr.materiaID,
	Mtr.materiaNombre,
	Sla.salaID,
	Sla.salaNombre,
	Lbi.libreriaID,
	Lbi.libreriaNombre,
	LbSl.librosalaID,
	LbSl.cantidadLibroSala,
	LbSl.precioLibroSala,
	LbSl.estadoLibroSala
FROM LibroSala LbSl
INNER JOIN Libro Lbr ON Lbr.libroID = LbSl.libroID
INNER JOIN Materia Mtr ON Mtr.materiaID = Lbr.libroMateria
INNER JOIN Sala Sla ON Sla.salaID = LbSl.salaID
INNER JOIN Libreria Lbi ON Sla.salaLibreria = Lbi.libreriaID 
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoPedido]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoPedido](
	[estadopedidoID] [int] IDENTITY(1,1) NOT NULL,
	[estadopedidoNombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EstadoPedido] PRIMARY KEY CLUSTERED 
(
	[estadopedidoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Listar_Pedidos]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[View_Listar_Pedidos] AS
	SELECT 
		Pd.pedidoID, 
		Pd.librosalaID, 
		ls.libroID,
		lb.libroNombre,
		lb.libroAutor,
		lb.libroISBN,
		lb.libroMateria,
		mt.materiaNombre,
		ls.salaID,
		SL.salaNombre,
		SL.salaLibreria,
		lr.libreriaNombre,
		Pd.clienteID, 
		cl.clienteNombre,
		cl.clienteApellido,
		cl.clienteCI_RUC,
		Pd.vendedorID, 
		vd.vendedorEstado,
		vend.Name,
		vend.LastName,
		Pd.estadopedidoID, 
		ep.estadopedidoNombre,
		Pd.cantidadPedido, 
		Pd.subTotalPedido,
		Pd.ivaPedido,
		Pd.totalPedido,
		Pd.fechaInicioPedido,
		Pd.fechaFinPedido
	FROM Pedido Pd
	INNER JOIN LibroSala ls ON ls.librosalaID = Pd.librosalaID
	INNER JOIN Libro lb ON lb.libroID = ls.libroID
	INNER JOIN Sala sl ON sl.salaID = ls.salaID
	INNER JOIN Materia mt ON mt.materiaID = lb.libroMateria
	INNER JOIN Libreria lr ON lr.libreriaID = sl.salaLibreria
	INNER JOIN EstadoPedido ep ON ep.estadopedidoID = Pd.estadopedidoID
	INNER JOIN Cliente cl ON cl.clienteID = Pd.clienteID
	LEFT JOIN Vendedor vd ON vd.aspUserID = Pd.vendedorID
	LEFT JOIN AspNetUsers vend ON vend.Id = vd.aspUserID
GO
/****** Object:  View [dbo].[View_Listar_Libros_Materia]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[View_Listar_Libros_Materia] AS
SELECT 
	Lbr.libroID,
	Lbr.libroNombre,
	Lbr.libroAutor,
	Lbr.libroISBN,
	Lbr.libroSinopsis,
	Lbr.libroIVA,
	Mtr.materiaID,
	Mtr.materiaNombre
FROM Libro Lbr
INNER JOIN Materia Mtr ON Mtr.materiaID = Lbr.libroMateria
GO
/****** Object:  View [dbo].[View_Listar_Libros_Sala]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_Listar_Libros_Sala] AS
SELECT 
	LbSl.cantidadLibroSala,
	LbSl.precioLibroSala,
	LbSl.estadoLibroSala,
	LbSl.librosalaID,
	Sla.salaID,
	Sla.salaNombre,
	Lbr.libroID,
	Lbr.libroAutor,
	Lbr.libroISBN,
	Lbr.libroSinopsis,
	Lbr.libroNombre
FROM LibroSala LbSl
INNER JOIN Sala Sla ON Sla.salaID = LbSl.salaID
INNER JOIN Libro Lbr ON Lbr.libroID = LbSl.libroID
GO
/****** Object:  View [dbo].[View_Listar_Sala_Libreria]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Listar_Sala_Libreria] AS
SELECT 
	Sla.salaID,
	Sla.salaNombre,
	Lbi.libreriaID,
	Lbi.libreriaNombre,
	Lbi.libreriaRUC,
	Lbi.libreriaTelefono,
	Lbi.libreriaDireccion
FROM Sala Sla
INNER JOIN Libreria Lbi ON Sla.salaLibreria = Lbi.libreriaID 
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClienteLibreria]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClienteLibreria](
	[clienteID] [int] NOT NULL,
	[libreriaID] [int] NOT NULL,
	[clientelibreriaFechaRegistro] [date] NULL,
 CONSTRAINT [PK_ClienteLibreria] PRIMARY KEY CLUSTERED 
(
	[clienteID] ASC,
	[libreriaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Variables]    Script Date: 29/1/2019 12:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Variables](
	[variableID] [int] IDENTITY(1,1) NOT NULL,
	[variableNombre] [varchar](50) NOT NULL,
	[variableValorNumerico] [decimal](18, 2) NULL,
	[variableValorString] [varchar](50) NULL,
 CONSTRAINT [PK_Variables] PRIMARY KEY CLUSTERED 
(
	[variableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201812040539505_InitialCreate', N'slnLibreria.Models.ApplicationDbContext', 0x1F8B0800000000000400DD5C5B6FE3B6127E2F70FE83A0A7738AD4CAE5EC621BD82D5227E9099A1BD6D9E2BC2D68897688952855A2B2098AFEB23E9C9F74FE428712254BBCE8622BB6532CB088C8E137C3E1901C0E87FEFF9FFF1BFFF81CF8D6138E1312D2897D343AB42D4CDDD0237439B153B6F8EE83FDE30FFFF8667CE105CFD6AF05DD09A783963499D88F8C45A78E93B88F3840C928206E1C26E1828DDC307090173AC78787DF3B47470E06081BB02C6BFC31A58C0438FB80CF69485D1CB114F937A187FD449443CD2C43B56E51809308B97862273EBD26F318C7048D726ADB3AF309024966D85FD816A2346488819CA79F123C63714897B3080A90FFF01261A05B203FC142FED31579D7AE1C1EF3AE38AB8605949B262C0C7A021E9D08DD3872F3B5346C97BA03ED5D8096D90BEF75A6C1897DE5E1ACE863E883026486A7533FE6C413FBA664719644B7988D8A86A31CF23206B8AF61FC6554453CB03AB73B286DE97874C8FF1D58D3D467698C2714A72C46FE81759FCE7DE2FE825F1EC22F984E4E8EE68B930FEFDE23EFE4FDBFF1C9BB6A4FA1AF40572B80A2FB388C700CB2E145D97FDB72EAED1CB961D9ACD226D70AD8124C0BDBBA41CFD7982ED9234C98E30FB675499EB1579408E3FA4409CC2268C4E2143E6F53DF47731F97F54E234FFE7F03D7E377EF07E17A8B9EC8321B7A893F4C9C18E6D547EC67B5C92389F2E9551BEFCF82EC320E03FE5DB7AFBCF6F32C4C6397772634923CA07889595DBAB1B332DE4E26CDA18637EB0275FF4D9B4BAA9AB7969477689D9950B0D8F66C28E47D5DBE9D2DEE2C8A60F032D3E21A69323875B31A49AD0FAC0ACDCA748EBA9A0E852EFD9D57C28B00117F80A5B00317F04216240E70D9CB9F42303C447BCB7C8F92045602EF3F28796C101DFE1C40F41976D3180C74C65010BD3AB7FBC790E2DB349873BBDF1EAFC186E6E16B78895C16C61794B7DA18EF3A74BF8429BBA0DE3962F813730B40FEF94082EE00838873E6BA38492EC198B1370DC1C92E00AF283B39EE0DC717A85DBB22531F9140EF8B484BE9E78274E58FE829149FC440A6F34B9A44BD0E97847613B520358B9A53B48A2AC8FA8ACAC1BA492A28CD826604AD72E65483797AD9080DEFEA65B0FBEFEB6DB6799BD6828A1A67B042E29F31C5312C63DE3D620CC77435025DD68D5D380BD9F071A6AFBE37659C7E457E3A34ABB56643B6080C3F1B32D8FD9F0D999850FC443CEE9574380015C400DF895E7FB66A9F739264DB9E0EB56E6E9BF976D600D374394B92D025D92CD084BE44E0A22E3FF870567B1423EF8D1C09818E81A113BEE54109F4CD968DEA8E9E631F336C9DB97968708A121779AA1AA1435E0FC18A1D5523D82A225217EE5B8527583A8E7923C40F4109CC5442993A2D08754984FC562D492D3B6E61BCEF250FB9E61C47987286AD9AE8C25C1F00E102947CA44169D3D0D8A9585CB3211ABC56D398B7B9B0AB7157E2125BB1C916DFD96097C27F7B15C36CD6D8168CB359255D043006F37661A0E2ACD2D500E483CBBE19A874623218A870A9B662A0758DEDC040EB2A7973069A1F51BB8EBF745EDD37F3AC1F94B7BFAD37AA6B07B659D3C79E9966EE7B421B062D70AC9AE7F99C57E267A6399C819CE27C9608575736110E3EC3AC1EB259F9BB5A3FD46906918DA8097065682DA0E21A50015226540FE18A585EA374C28BE8015BC4DD1A61C5DA2FC1566C40C5AE5E875608CD97A6B271763A7D943D2BAD4131F24E87850A8EC620E4C5ABDEF10E4A31C56555C574F185FB78C3958E89C16850508BE76A5052D199C1B5549866BB96740E591F976C232D49EE93414B456706D792B0D17625699C821E6EC1462AAA6FE1034DB622D251EE3665DDD8C9B3A444C1D831A4538D6F501411BAACA45789126B96E7564DBF9BF54F3A0A720CC74D34B947A5B4252716C66889A55A600D925E923861E788A139E2719EA9172864DABDD5B0FC172CABDBA73A88C53E5050F3BFF3169ACBFBDA5EAB3A2302E3127A18708F260BA36BC65FDFDCE2E96EC847B126723F0DFD34A06607CBDC3ABFBFABB6CF4B5484B123C9AF38508AB61437B7AEFA4E03A34E8A8106A9F45FD61F28338449DD85F75955B8C92335A31401AA2A8A2968B5B381333932BD064BF611FB8F552BC2EBCC2B9198520510453D312AB90D0A58A5AE3B6A3DFDA48A59AFE98E28E5985421A5AA1E525633496A42562BD6C23368544FD19D839A3B5245576BBB236BB248AAD09AEA35B03532CB75DD5135892655604D7577EC55D689BC88EEF1CE653CB9ACBD75E587DBCDF62E03C6EBAC88C36C7D953BFC2A50A5B82796B8A557C044F95E5A93F184B7B635E5318DCDACC980615E796AB7DFF585A7F1CADE8C59BBD2AE2DEE4D57FA66BC7E36FBAA96A11CF06492927B79D0930E746371B86A7F44A39CB67212DB2AD4081BFB4BC27030E204A3D96FFED427982FE305C10DA264811396A771D8C78747C7D23B9CFD7913E32489E76B0EA7A68731F531DB4246167D42B1FB8862353F628377232B5025F47C453DFC3CB17FCF5A9D66510CFE57567C605D259F28F92D858A8738C5D61F6ABEE73079F4CD87AC3D7DF5D05DAB57FFFD9C373DB0EE629831A7D6A1A4CB7546B8FE16A2973479D30DA459FB85C4DB9D50B5E7075A546942ACFFDA604ED8202F0D0A29FF19A0E77FF5154DFB9A602344CD8B81A1F00651A1E945C03A58C6D7001E7CB2EC3540BFCEEA5F07AC239AF16500A1FDC1E47701DD97A1A2E50EB71ACD99681B4B52A6E7D6BCEA8D922C77BD3729E9D71B4D7435C5BA07DC0669D46B58C61BCB401E6C77D424180F86BD4BD37EF5ACE27D49245EA578EC367F789B29C30DD7427FAB4CE13DC86DD3E4EAEC3E1F78DBB6668AE3EE795265BFACDF3D333691C1B5FBDCDE6D1B9B29CCBBE7C6D62B8377CF6C6D57FBE78E2DADF316BAF37C5C35B5C8701FA38B05B7E5DBE6817338E1CF433082DCA3CC9F49EA13BC9A92535B18AE48CC4CCD9965326365E2287C158A66B6FDFA2A36FCC6CE0A9A66B6867CCC26DE62FD6FE42D689A791BB21C779129ACCD33D4656FB7AC634D49506F2933B8D6939644F4369FB5F172FD2D25020FA294DAEC31DC11BF9DBCDF415432E4D4E991E7AB5EF7C2DE59F96545D8BF13B25C41F0DF59A4D8ADED9A25CD155D84C5E62D49549048119A1BCC90075BEA59CCC802B90CAA798C397BE79DC5EDF84DC71C7B57F42E6551CAA0CB3898FBB5801777029AF867C9CC7599C77751F69325437401C4243C367F477F4A89EF95725F6A62420608EE5D88882E1F4BC623BBCB9712E936A41D8184FA4AA7E80107910F60C91D9DA127BC8E6C607ED77889DC975504D004D23E1075B58FCF095AC6284804C6AA3D7C820D7BC1F30F7F0173E3F23660540000, N'6.1.3-40302')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c81bc465-746c-4ef2-9528-42b6aa831b50', N'Administrador')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5a58e1c9-826d-4cd1-920e-5ece90ad7369', N'Caja')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'ff2bcb31-5a14-4efd-9d60-1e0b5bbd5b1c', N'Libreria')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'7ef4d4e1-0f49-4548-8cf1-68e1518e4759', N'Vendedor')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3562c1b3-e7f3-4175-9080-c653b34ab7b3', N'7ef4d4e1-0f49-4548-8cf1-68e1518e4759')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e32b1b6f-4a84-4b35-b135-64644c566b73', N'7ef4d4e1-0f49-4548-8cf1-68e1518e4759')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c854bae2-ac47-47ae-b77f-5aa05c70683a', N'c81bc465-746c-4ef2-9528-42b6aa831b50')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [LastName]) VALUES (N'3562c1b3-e7f3-4175-9080-c653b34ab7b3', N'vendedor1@ferialibro.com', 0, N'ANkGaLowJYCQfpkjs5KY8KNjB6B3xZ9Eg1uKbht6ZhRLlbHfeNDu90majVXiGRMS+Q==', N'28462431-9f9b-4b63-89ac-d3398f9b478f', NULL, 0, 0, NULL, 1, 0, N'vendedor1@ferialibro.com', N'Vendedor ', N'1')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [LastName]) VALUES (N'c854bae2-ac47-47ae-b77f-5aa05c70683a', N'juan.viana@udla.edu.ec', 0, N'AIXwVwtKEvhkMu2kqQy657+vRQ4yt0OOyHFRewM5TgZtEF6InUGYV9Msms/OmFmsBw==', N'72e55987-389e-4e46-96f6-d244e9b9dbd4', NULL, 0, 0, NULL, 1, 0, N'juan.viana@udla.edu.ec', N'Juan', N'Viana')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Name], [LastName]) VALUES (N'e32b1b6f-4a84-4b35-b135-64644c566b73', N'vendedor2@ferialibro.com', 0, N'AOnFFRGzAKpxtEiyvGRKx49ak4JVmqvIINrk9sKO+Zapj7WGf5OoiXK2A6pJ3X/5mw==', N'5e9faf23-28da-4398-b2fb-f7df3cd2ce1a', NULL, 0, 0, NULL, 1, 0, N'vendedor2@ferialibro.com', N'Vendedor', N'2')
SET IDENTITY_INSERT [dbo].[Cliente] ON 

INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (1, 100, N'1715737118', N'Juan', N'Viana', N'juan.viana@udla.edu.ec', N'0967790159', CAST(N'2019-01-28T02:41:25.737' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (2, 101, N'1714025333', N'Andres', N'Barrero', N'aviana.facturas@gmial.com', N'0999655671', CAST(N'2019-01-28T03:41:25.737' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (3, 106, N'1750301986', N'Nicole', N'Cerón', N'nceron@correo.com', N'0999985855', CAST(N'2019-01-28T05:20:45.187' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (4, 102, N'1714025333001', N'Andres', N'c.a.', N'andres@empresa.com', N'0999655674', CAST(N'2019-01-28T04:41:25.737' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (5, 103, N'1714040845', N'Anónimo', N'N. 00000103', N'anonimo@anonimo.com', N'0999999999', CAST(N'2019-01-28T04:49:13.083' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (6, 104, N'1714040847', N'Anónimo', N'N. 00000104', N'anonimo@anonimo.com', N'0999999999', CAST(N'2019-01-28T04:50:42.393' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (7, 105, N'1715737118001', N'Anónimo', N'N. 00000105', N'anonimo@anonimo.com', N'0999999999', CAST(N'2019-01-28T04:54:23.793' AS DateTime))
INSERT [dbo].[Cliente] ([clienteID], [clienteCodigo], [clienteCI_RUC], [clienteNombre], [clienteApellido], [clienteCorreo], [clienteTelefono], [clienteFechaRegistro]) VALUES (8, 107, N'1708001706', N'Anónimo', N'N. 00000107', N'anonimo@anonimo.com', N'0999999999', CAST(N'2019-01-28T05:57:49.600' AS DateTime))
SET IDENTITY_INSERT [dbo].[Cliente] OFF
INSERT [dbo].[ClienteLibreria] ([clienteID], [libreriaID], [clientelibreriaFechaRegistro]) VALUES (1, 3, NULL)
INSERT [dbo].[ClienteLibreria] ([clienteID], [libreriaID], [clientelibreriaFechaRegistro]) VALUES (1, 7, NULL)
INSERT [dbo].[ClienteLibreria] ([clienteID], [libreriaID], [clientelibreriaFechaRegistro]) VALUES (2, 4, NULL)
INSERT [dbo].[ClienteLibreria] ([clienteID], [libreriaID], [clientelibreriaFechaRegistro]) VALUES (3, 4, NULL)
SET IDENTITY_INSERT [dbo].[EstadoPedido] ON 

INSERT [dbo].[EstadoPedido] ([estadopedidoID], [estadopedidoNombre]) VALUES (1, N'En Curso')
INSERT [dbo].[EstadoPedido] ([estadopedidoID], [estadopedidoNombre]) VALUES (2, N'Por Despachar')
INSERT [dbo].[EstadoPedido] ([estadopedidoID], [estadopedidoNombre]) VALUES (3, N'Despachado')
INSERT [dbo].[EstadoPedido] ([estadopedidoID], [estadopedidoNombre]) VALUES (4, N'Finalizado')
INSERT [dbo].[EstadoPedido] ([estadopedidoID], [estadopedidoNombre]) VALUES (5, N'Cancelado')
SET IDENTITY_INSERT [dbo].[EstadoPedido] OFF
SET IDENTITY_INSERT [dbo].[Libreria] ON 

INSERT [dbo].[Libreria] ([libreriaID], [libreriaNombre], [libreriaRUC], [libreriaDireccion], [libreriaTelefono]) VALUES (2, N'Nuevo Mundo', N'1718040843001', N'Hernandez de Giron y Mañosca', N'023331470')
INSERT [dbo].[Libreria] ([libreriaID], [libreriaNombre], [libreriaRUC], [libreriaDireccion], [libreriaTelefono]) VALUES (3, N'Libreria Española', N'1750291784001', N'Replúbica del Salvador y Portugal', N'023330557')
INSERT [dbo].[Libreria] ([libreriaID], [libreriaNombre], [libreriaRUC], [libreriaDireccion], [libreriaTelefono]) VALUES (4, N'Nuevo Oriente', N'1714025333001', N'Marina de Jesús y 10 de Agosto', N'023331470')
INSERT [dbo].[Libreria] ([libreriaID], [libreriaNombre], [libreriaRUC], [libreriaDireccion], [libreriaTelefono]) VALUES (7, N'Libreria Nueva Esperanza', N'1714040845001', N'Cerca', N'023331470')
INSERT [dbo].[Libreria] ([libreriaID], [libreriaNombre], [libreriaRUC], [libreriaDireccion], [libreriaTelefono]) VALUES (8, N'Libreria Quito', N'1753426871001', N'Nueva Vda', N'022815203')
SET IDENTITY_INSERT [dbo].[Libreria] OFF
SET IDENTITY_INSERT [dbo].[Libro] ON 

INSERT [dbo].[Libro] ([libroID], [libroMateria], [libroNombre], [libroAutor], [libroISBN], [libroIVA], [libroSinopsis]) VALUES (2, 7, N'LibroPrueba', N'Pedro', N'AA78', 0, N'TEXTO')
INSERT [dbo].[Libro] ([libroID], [libroMateria], [libroNombre], [libroAutor], [libroISBN], [libroIVA], [libroSinopsis]) VALUES (3, 3, N'Physics For The IB Diploma', N'Tim Kirk', N'9780199151411', 1, N'This book should prove useful to nayone following a pre-universitary physics')
INSERT [dbo].[Libro] ([libroID], [libroMateria], [libroNombre], [libroAutor], [libroISBN], [libroIVA], [libroSinopsis]) VALUES (4, 4, N'Mathematics Standard Level', N'International Baccalaureate Organisation (IBO)', N'1876659173', 1, N'This book has been specifically written to meet the demands of the mathematics standard level')
INSERT [dbo].[Libro] ([libroID], [libroMateria], [libroNombre], [libroAutor], [libroISBN], [libroIVA], [libroSinopsis]) VALUES (5, 7, N'¡Vivir!', N'Ya Hua', N'9788432228735', 1, N'El titulo original: Huozhe 
Primera edición mayo 2010
Ya Hua 1993')
INSERT [dbo].[Libro] ([libroID], [libroMateria], [libroNombre], [libroAutor], [libroISBN], [libroIVA], [libroSinopsis]) VALUES (6, 3, N'Papita Frita', N'Salchichita', N'15181746', 1, N'Base de datos ')
SET IDENTITY_INSERT [dbo].[Libro] OFF
SET IDENTITY_INSERT [dbo].[LibroSala] ON 

INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (12, 3, 5, 309, CAST(28.65 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (13, 1, 2, 53, CAST(12.50 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (14, 2, 3, 296, CAST(120.52 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (16, 8, 3, 256, CAST(28.69 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (17, 9, 6, 20, CAST(400.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (18, 1, 6, 30, CAST(600.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (1017, 1, 6, 50, CAST(50.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[LibroSala] ([librosalaID], [salaID], [libroID], [cantidadLibroSala], [precioLibroSala], [estadoLibroSala]) VALUES (1018, 8, 6, 145, CAST(38.56 AS Decimal(18, 2)), 1)
SET IDENTITY_INSERT [dbo].[LibroSala] OFF
SET IDENTITY_INSERT [dbo].[Materia] ON 

INSERT [dbo].[Materia] ([materiaID], [materiaNombre]) VALUES (1, N'Inglés')
INSERT [dbo].[Materia] ([materiaID], [materiaNombre]) VALUES (3, N'Ciencias')
INSERT [dbo].[Materia] ([materiaID], [materiaNombre]) VALUES (4, N'Matemáticas Nueva')
INSERT [dbo].[Materia] ([materiaID], [materiaNombre]) VALUES (7, N'Drama')
SET IDENTITY_INSERT [dbo].[Materia] OFF
SET IDENTITY_INSERT [dbo].[Pedido] ON 

INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (3, 17, 2, NULL, 1, 1, CAST(400.00 AS Decimal(18, 2)), CAST(48.00 AS Decimal(18, 2)), CAST(448.00 AS Decimal(18, 2)), CAST(N'2019-01-15T21:06:00.937' AS DateTime), NULL)
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1012, 18, 1, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 5, 10, CAST(6000.00 AS Decimal(18, 2)), CAST(720.00 AS Decimal(18, 2)), CAST(6720.00 AS Decimal(18, 2)), CAST(N'2019-01-27T20:48:14.150' AS DateTime), CAST(N'2019-01-29T09:55:49.237' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1014, 12, 7, NULL, 5, 1, CAST(28.65 AS Decimal(18, 2)), CAST(3.43 AS Decimal(18, 2)), CAST(32.08 AS Decimal(18, 2)), CAST(N'2019-01-28T05:00:17.483' AS DateTime), NULL)
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1016, 12, 7, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 5, 3, CAST(85.95 AS Decimal(18, 2)), CAST(10.31 AS Decimal(18, 2)), CAST(96.26 AS Decimal(18, 2)), CAST(N'2019-01-28T05:02:00.567' AS DateTime), CAST(N'2019-01-29T09:53:11.120' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1017, 12, 3, NULL, 5, 2, CAST(57.30 AS Decimal(18, 2)), CAST(6.87 AS Decimal(18, 2)), CAST(64.17 AS Decimal(18, 2)), CAST(N'2019-01-28T05:41:04.683' AS DateTime), NULL)
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1018, 12, 3, NULL, 5, 4, CAST(114.60 AS Decimal(18, 2)), CAST(13.75 AS Decimal(18, 2)), CAST(128.35 AS Decimal(18, 2)), CAST(N'2019-01-28T05:41:28.683' AS DateTime), NULL)
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1022, 14, 3, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 5, 3, CAST(361.56 AS Decimal(18, 2)), CAST(43.38 AS Decimal(18, 2)), CAST(404.94 AS Decimal(18, 2)), CAST(N'2019-01-28T05:48:45.507' AS DateTime), CAST(N'2019-01-29T09:46:16.937' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1024, 14, 3, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 5, 5, CAST(602.60 AS Decimal(18, 2)), CAST(72.31 AS Decimal(18, 2)), CAST(674.91 AS Decimal(18, 2)), CAST(N'2019-01-28T05:53:58.923' AS DateTime), CAST(N'2019-01-29T09:47:27.683' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1025, 17, 8, N'e32b1b6f-4a84-4b35-b135-64644c566b73', 4, 2, CAST(800.00 AS Decimal(18, 2)), CAST(96.00 AS Decimal(18, 2)), CAST(896.00 AS Decimal(18, 2)), CAST(N'2019-01-28T05:58:00.510' AS DateTime), CAST(N'2019-01-29T10:09:18.447' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1026, 12, 3, NULL, 5, 5, CAST(143.25 AS Decimal(18, 2)), CAST(17.19 AS Decimal(18, 2)), CAST(160.44 AS Decimal(18, 2)), CAST(N'2019-01-28T05:59:01.297' AS DateTime), NULL)
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1027, 14, 3, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 5, 2, CAST(241.04 AS Decimal(18, 2)), CAST(28.92 AS Decimal(18, 2)), CAST(269.96 AS Decimal(18, 2)), CAST(N'2019-01-28T10:33:38.270' AS DateTime), CAST(N'2019-01-29T09:48:30.120' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (1028, 14, 3, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 4, 6, CAST(723.12 AS Decimal(18, 2)), CAST(86.77 AS Decimal(18, 2)), CAST(809.89 AS Decimal(18, 2)), CAST(N'2019-01-28T18:08:42.080' AS DateTime), CAST(N'2019-01-29T12:28:19.970' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (2014, 1018, 3, N'3562c1b3-e7f3-4175-9080-c653b34ab7b3', 5, 7, CAST(269.92 AS Decimal(18, 2)), CAST(32.39 AS Decimal(18, 2)), CAST(302.31 AS Decimal(18, 2)), CAST(N'2019-01-29T09:59:07.163' AS DateTime), CAST(N'2019-01-29T12:28:13.110' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (2015, 12, 3, N'3562c1b3-e7f3-4175-9080-c653b34ab7b3', 4, 7, CAST(200.55 AS Decimal(18, 2)), CAST(24.06 AS Decimal(18, 2)), CAST(224.61 AS Decimal(18, 2)), CAST(N'2019-01-29T09:59:36.300' AS DateTime), CAST(N'2019-01-29T12:28:19.973' AS DateTime))
INSERT [dbo].[Pedido] ([pedidoID], [librosalaID], [clienteID], [vendedorID], [estadopedidoID], [cantidadPedido], [subTotalPedido], [ivaPedido], [totalPedido], [fechaInicioPedido], [fechaFinPedido]) VALUES (2016, 1018, 8, NULL, 2, 5, CAST(192.80 AS Decimal(18, 2)), CAST(23.13 AS Decimal(18, 2)), CAST(215.93 AS Decimal(18, 2)), CAST(N'2019-01-29T10:13:57.683' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Pedido] OFF
SET IDENTITY_INSERT [dbo].[Sala] ON 

INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (1, 4, N'Oriente')
INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (2, 2, N'Modernidad en Persona')
INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (3, 3, N'Clasicos')
INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (4, 7, N'Sala Prueba')
INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (6, 2, N'Sala del Futuro')
INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (8, 3, N'El fin del mundo')
INSERT [dbo].[Sala] ([salaID], [salaLibreria], [salaNombre]) VALUES (9, 8, N'Quito 2')
SET IDENTITY_INSERT [dbo].[Sala] OFF
SET IDENTITY_INSERT [dbo].[Variables] ON 

INSERT [dbo].[Variables] ([variableID], [variableNombre], [variableValorNumerico], [variableValorString]) VALUES (1, N'IVA', CAST(12.00 AS Decimal(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[Variables] OFF
SET IDENTITY_INSERT [dbo].[Vendedor] ON 

INSERT [dbo].[Vendedor] ([vendedorID], [aspUserID], [vendedorEstado]) VALUES (103, N'3562c1b3-e7f3-4175-9080-c653b34ab7b3', 1)
INSERT [dbo].[Vendedor] ([vendedorID], [aspUserID], [vendedorEstado]) VALUES (101, N'c854bae2-ac47-47ae-b77f-5aa05c70683a', 1)
INSERT [dbo].[Vendedor] ([vendedorID], [aspUserID], [vendedorEstado]) VALUES (100, N'e32b1b6f-4a84-4b35-b135-64644c566b73', 1)
SET IDENTITY_INSERT [dbo].[Vendedor] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 29/1/2019 12:35:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 29/1/2019 12:35:51 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 29/1/2019 12:35:51 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleId]    Script Date: 29/1/2019 12:35:51 ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserId]    Script Date: 29/1/2019 12:35:51 ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 29/1/2019 12:35:51 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [AK_ClienteCI_RUC]    Script Date: 29/1/2019 12:35:51 ******/
ALTER TABLE [dbo].[Cliente] ADD  CONSTRAINT [AK_ClienteCI_RUC] UNIQUE NONCLUSTERED 
(
	[clienteCI_RUC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Libro] ADD  CONSTRAINT [DF_Libro_libroIVA]  DEFAULT ((1)) FOR [libroIVA]
GO
ALTER TABLE [dbo].[LibroSala] ADD  CONSTRAINT [DF_LibroSala_cantidadLibroSala]  DEFAULT ((0)) FOR [cantidadLibroSala]
GO
ALTER TABLE [dbo].[LibroSala] ADD  CONSTRAINT [DF_LibroSala_precioLibroSala]  DEFAULT ((0.00)) FOR [precioLibroSala]
GO
ALTER TABLE [dbo].[LibroSala] ADD  CONSTRAINT [DF_LibroSala_estadoLibroSala]  DEFAULT ((1)) FOR [estadoLibroSala]
GO
ALTER TABLE [dbo].[Vendedor] ADD  CONSTRAINT [DF_Vendedor_vendedorEstado]  DEFAULT ((1)) FOR [vendedorEstado]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ClienteLibreria]  WITH CHECK ADD  CONSTRAINT [FK_ClienteLibreria_Cliente] FOREIGN KEY([clienteID])
REFERENCES [dbo].[Cliente] ([clienteID])
GO
ALTER TABLE [dbo].[ClienteLibreria] CHECK CONSTRAINT [FK_ClienteLibreria_Cliente]
GO
ALTER TABLE [dbo].[ClienteLibreria]  WITH CHECK ADD  CONSTRAINT [FK_ClienteLibreria_Libreria] FOREIGN KEY([libreriaID])
REFERENCES [dbo].[Libreria] ([libreriaID])
GO
ALTER TABLE [dbo].[ClienteLibreria] CHECK CONSTRAINT [FK_ClienteLibreria_Libreria]
GO
ALTER TABLE [dbo].[Libro]  WITH CHECK ADD  CONSTRAINT [FK_Libro_Materia] FOREIGN KEY([libroMateria])
REFERENCES [dbo].[Materia] ([materiaID])
GO
ALTER TABLE [dbo].[Libro] CHECK CONSTRAINT [FK_Libro_Materia]
GO
ALTER TABLE [dbo].[LibroSala]  WITH CHECK ADD  CONSTRAINT [FK_LibroSala_Libro] FOREIGN KEY([libroID])
REFERENCES [dbo].[Libro] ([libroID])
GO
ALTER TABLE [dbo].[LibroSala] CHECK CONSTRAINT [FK_LibroSala_Libro]
GO
ALTER TABLE [dbo].[LibroSala]  WITH CHECK ADD  CONSTRAINT [FK_LibroSala_Sala] FOREIGN KEY([salaID])
REFERENCES [dbo].[Sala] ([salaID])
GO
ALTER TABLE [dbo].[LibroSala] CHECK CONSTRAINT [FK_LibroSala_Sala]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_Cliente] FOREIGN KEY([clienteID])
REFERENCES [dbo].[Cliente] ([clienteID])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_Pedido_Cliente]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_EstadoPedido] FOREIGN KEY([estadopedidoID])
REFERENCES [dbo].[EstadoPedido] ([estadopedidoID])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_Pedido_EstadoPedido]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_LibroSala] FOREIGN KEY([librosalaID])
REFERENCES [dbo].[LibroSala] ([librosalaID])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_Pedido_LibroSala]
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD  CONSTRAINT [FK_Pedido_Vendedor] FOREIGN KEY([vendedorID])
REFERENCES [dbo].[Vendedor] ([aspUserID])
GO
ALTER TABLE [dbo].[Pedido] CHECK CONSTRAINT [FK_Pedido_Vendedor]
GO
ALTER TABLE [dbo].[Sala]  WITH CHECK ADD  CONSTRAINT [FK_Sala_Libreria] FOREIGN KEY([salaLibreria])
REFERENCES [dbo].[Libreria] ([libreriaID])
GO
ALTER TABLE [dbo].[Sala] CHECK CONSTRAINT [FK_Sala_Libreria]
GO
ALTER TABLE [dbo].[Vendedor]  WITH CHECK ADD  CONSTRAINT [FK_Vendedor_AspNetUsers] FOREIGN KEY([aspUserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Vendedor] CHECK CONSTRAINT [FK_Vendedor_AspNetUsers]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "View_Listar_Pedidos_PorLiquidar"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 236
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 6
               Left = 274
               Bottom = 136
               Right = 472
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Listar_Pedidos_Por_Lq_Clientes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Listar_Pedidos_Por_Lq_Clientes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "View_Listar_Pedidos_Resumen"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 236
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 6
               Left = 274
               Bottom = 136
               Right = 472
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Listar_Pedidos_ResumenCliente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Listar_Pedidos_ResumenCliente'
GO
USE [master]
GO
ALTER DATABASE [dbFeriaLibro] SET  READ_WRITE 
GO
