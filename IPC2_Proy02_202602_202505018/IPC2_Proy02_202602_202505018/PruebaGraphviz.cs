using System.IO;
using IPC2_Proy02_202602_202505018.Modelo;

namespace IPC2_Proy02_202602_202505018.Servicios
{
    public class PruebaGraphviz
    {
        public void Ejecutar()
        {
            Catalogo catalogo =
                new Catalogo();

            catalogo.AgregarCategoria(
                "Programacion",
                "");

            catalogo.AgregarCategoria(
                "Algoritmos",
                "Programacion");

            catalogo.AgregarCategoria(
                "Bases de Datos",
                "Programacion");

            catalogo.RegistrarLibro(
                new Libro(
                    1005,
                    "Estructuras de Datos",
                    "Autor 1",
                    "Programacion"));

            catalogo.RegistrarLibro(
                new Libro(
                    1001,
                    "Introduccion a C#",
                    "Autor 2",
                    "Programacion"));

            catalogo.RegistrarLibro(
                new Libro(
                    1003,
                    "Programacion en C#",
                    "Autor 3",
                    "Programacion"));

            GraphvizService graphviz =
                new GraphvizService();

            string contenidoCategorias =
                graphviz.GenerarCategorias(
                    catalogo.ObtenerRaizCategorias());

            string rutaCategorias =
                Path.Combine(
                    AppContext.BaseDirectory,
                    "categorias.dot");

            File.WriteAllText(
                rutaCategorias,
                contenidoCategorias);

            var categoria =
                catalogo.Categorias
                    .BuscarCategoria(
                        "Programacion");

            string contenidoLibros =
                graphviz.GenerarLibrosCategoria(
                    categoria);

            string rutaLibros =
                Path.Combine(
                    AppContext.BaseDirectory,
                    "libros.dot");

            File.WriteAllText(
                rutaLibros,
                contenidoLibros);

            Console.WriteLine(
                "Archivo de categorías: " +
                rutaCategorias);

            Console.WriteLine(
                "Archivo de libros: " +
                rutaLibros);
        }
    }
}