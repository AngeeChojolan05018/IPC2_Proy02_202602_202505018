using System.Text;
using IPC2_Proy02_202602_202505018.Modelo;
using IPC2_Proy02_202602_202505018.TDA;

namespace IPC2_Proy02_202602_202505018.Servicios
{
    public class Catalogo
    {
        public ArbolCategorias Categorias { get; set; }

        public ArbolISBN Libros { get; set; }

        public Catalogo()
        {
            Categorias = new ArbolCategorias();
            Libros = new ArbolISBN();
        }

        public bool RegistrarLibro(Libro libro)
        {
            if (libro == null)
            {
                return false;
            }

            if (libro.ISBN <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(libro.Titulo) ||
                string.IsNullOrWhiteSpace(libro.Autor) ||
                string.IsNullOrWhiteSpace(libro.Categoria))
            {
                return false;
            }

            if (Libros.Buscar(libro.ISBN) != null)
            {
                return false;
            }

            NodoArbol? categoria =
                Categorias.BuscarCategoria(
                    libro.Categoria);

            if (categoria == null)
            {
                return false;
            }

            NodoISBN nuevo =
                new NodoISBN(libro);

            Libros.Insertar(nuevo);

            categoria.AgregarLibro(libro);

            return true;
        }

        public Libro? BuscarLibro(int isbn)
        {
            NodoISBN? encontrado =
                Libros.Buscar(isbn);

            if (encontrado == null)
            {
                return null;
            }

            return encontrado.Dato;
        }

        public bool EliminarLibro(int isbn)
        {
            NodoISBN? encontrado =
                Libros.Buscar(isbn);

            if (encontrado == null)
            {
                return false;
            }

            Libro libro =
                encontrado.Dato;

            NodoArbol? categoria =
                Categorias.BuscarCategoria(
                    libro.Categoria);

            Libros.Eliminar(isbn);

            if (categoria != null)
            {
                categoria.Libros.Eliminar(libro);
            }

            return true;
        }

        public Libro? ObtenerLibroMenor()
        {
            NodoISBN? minimo =
                Libros.ObtenerMinimo();

            if (minimo == null)
            {
                return null;
            }

            return minimo.Dato;
        }

        public Libro? ObtenerLibroMayor()
        {
            NodoISBN? maximo =
                Libros.ObtenerMaximo();

            if (maximo == null)
            {
                return null;
            }

            return maximo.Dato;
        }

        public int ObtenerCantidadLibros()
        {
            return ContarLibros(
                Libros.ObtenerRaiz());
        }

        private int ContarLibros(
            NodoISBN? actual)
        {
            if (actual == null)
            {
                return 0;
            }

            return 1
                + ContarLibros(actual.Izquierdo)
                + ContarLibros(actual.Derecho);
        }

        public ListaSimple? ObtenerLibrosDeCategoria(
            string nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(
                    nombreCategoria))
            {
                return null;
            }

            NodoArbol? categoria =
                Categorias.BuscarCategoria(
                    nombreCategoria.Trim());

            if (categoria == null)
            {
                return null;
            }

            return categoria.Libros;
        }

        public bool ExisteCategoria(
            string nombreCategoria)
        {
            if (string.IsNullOrWhiteSpace(
                    nombreCategoria))
            {
                return false;
            }

            return Categorias.BuscarCategoria(
                nombreCategoria.Trim()) != null;
        }

        public bool AgregarCategoria(
            string nombreCategoria,
            string nombrePadre)
        {
            if (string.IsNullOrWhiteSpace(
                    nombreCategoria))
            {
                return false;
            }

            nombreCategoria =
                nombreCategoria.Trim();

            nombrePadre =
                nombrePadre?.Trim() ?? "";

            if (ExisteCategoria(nombreCategoria))
            {
                return false;
            }

            Categoria categoria =
                new Categoria(nombreCategoria);

            NodoArbol nuevaCategoria =
                new NodoArbol(categoria);

            if (Categorias.EstaVacio())
            {
                if (nombrePadre != "")
                {
                    return false;
                }

                Categorias.EstablecerRaiz(
                    nuevaCategoria);

                return true;
            }

            if (nombrePadre == "")
            {
                return false;
            }

            return Categorias.AgregarCategoria(
                nuevaCategoria,
                nombrePadre);
        }

        public NodoArbol? ObtenerRaizCategorias()
        {
            return Categorias.ObtenerRaiz();
        }

        public NodoArbol? BuscarCategoria(
            string nombre)
        {
            return Categorias.BuscarCategoria(
                nombre);
        }

        public string ObtenerLibrosAscendenteTexto()
        {
            StringBuilder resultado =
                new StringBuilder();

            if (Libros.EstaVacio())
            {
                resultado.Append(
                    "No hay libros registrados.");

                return resultado.ToString();
            }

            resultado.AppendLine(
                "LIBROS ORDENADOS POR ISBN");

            resultado.AppendLine(
                "==========================");

            AgregarLibrosAscendenteTexto(
                Libros.ObtenerRaiz(),
                resultado);

            return resultado.ToString();
        }

        private void AgregarLibrosAscendenteTexto(
            NodoISBN? actual,
            StringBuilder resultado)
        {
            if (actual == null)
            {
                return;
            }

            AgregarLibrosAscendenteTexto(
                actual.Izquierdo,
                resultado);

            resultado.AppendLine(
                "ISBN: " +
                actual.Dato.ISBN +
                " | Título: " +
                actual.Dato.Titulo +
                " | Autor: " +
                actual.Dato.Autor +
                " | Categoría: " +
                actual.Dato.Categoria);

            AgregarLibrosAscendenteTexto(
                actual.Derecho,
                resultado);
        }

        public string ObtenerLibrosCategoriaTexto(
            string nombreCategoria)
        {
            NodoArbol? categoria =
                BuscarCategoria(nombreCategoria);

            if (categoria == null)
            {
                return "No se encontró la categoría.";
            }

            StringBuilder resultado =
                new StringBuilder();

            resultado.AppendLine(
                "CATEGORÍA: " +
                categoria.Dato.Nombre);

            resultado.AppendLine(
                "==========================");

            if (categoria.Libros.EstaVacia())
            {
                resultado.Append(
                    "No hay libros asociados.");

                return resultado.ToString();
            }

            AgregarLibrosCategoriaTexto(
                categoria.Libros.ObtenerPrimero(),
                resultado);

            return resultado.ToString();
        }

        private void AgregarLibrosCategoriaTexto(
            Nodo? actual,
            StringBuilder resultado)
        {
            if (actual == null)
            {
                return;
            }

            Libro libro =
                (Libro)actual.Dato;

            resultado.AppendLine(
                "ISBN: " +
                libro.ISBN +
                " | Título: " +
                libro.Titulo +
                " | Autor: " +
                libro.Autor);

            AgregarLibrosCategoriaTexto(
                actual.Siguiente,
                resultado);
        }

        public void Reiniciar()
        {
            Categorias =
                new ArbolCategorias();

            Libros =
                new ArbolISBN();
        }
    }
}