--Reporte 1--
create procedure importe_total
as
select libroNombre, precioLibroSala
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID
where Pedido.estadopedidoID = 3 and Pedido.estadopedidoID=2;

go
create procedure importe_total_suma
as
select sum(precioLibroSala) as Total
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID
where Pedido.estadopedidoID = 3 and Pedido.estadopedidoID=2;


--Reporte 2--
go
create procedure vendedores
as
select TOP(3)  vendedorNombre, sum(totalPedido) as total  
from Vendedor inner join Pedido
on Vendedor.vendedorID = Pedido.vendedorID
group by Vendedor.vendedorNombre
order by total ASC 
 

 --Reporte 3--
 go
 create procedure titulos
as
select libroNombre, count(*) as totalpedido
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID
group by libroNombre;

--Reporte 4--
 go
 create procedure Autor
as
select libroAutor, count(*)
from libro inner join Materia
on libro.libroMateria=materiaID
group by libroAutor;

 go
 create procedure Materias
as
select materiaNombre, count(*)
from libro inner join Materia
on libro.libroMateria=materiaID
group by materiaNombre;

--Reporte 5--
go
 create procedure Dia
as
select fechaInicioPedido, count(*) as NumeroPedido
from Pedido
group by fechaInicioPedido;

--Reporte 6--
go
create procedure recaudado_titulo
as
select libroNombre, count(*) as libros, sum(precioLibroSala) as total
from Libro inner join LibroSala 
on Libro.libroID = LibroSala.libroID 
inner join pedido on
LibroSala.librosalaID = Pedido.librosalaID 
group by libroNombre;

--Reporte 7--
