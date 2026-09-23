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

        // Registrar libro
        public bool RegistrarLibro(Libro libro)
        {
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

        // Buscar libro por ISBN
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

        // Eliminar libro
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

        // Buscar libro dentro de una categoría
        public Libro? BuscarLibroEnCategoria(
            string nombreCategoria,
            int isbn)
        {
            NodoArbol? categoria =
                Categorias.BuscarCategoria(
                    nombreCategoria);

            if (categoria == null)
            {
                return null;
            }

            Nodo? actual =
                categoria.Libros.ObtenerPrimero();

            while (actual != null)
            {
                Libro libro =
                    (Libro)actual.Dato;

                if (libro.ISBN == isbn)
                {
                    return libro;
                }

                actual = actual.Siguiente;
            }

            return null;
        }

        // Obtener libro con menor ISBN
        public Libro? ObtenerLibroMenor()
        {
            NodoISBN? encontrado =
                Libros.ObtenerMinimo();

            if (encontrado == null)
            {
                return null;
            }

            return encontrado.Dato;
        }

        // Obtener libro con mayor ISBN
        public Libro? ObtenerLibroMayor()
        {
            NodoISBN? encontrado =
                Libros.ObtenerMaximo();

            if (encontrado == null)
            {
                return null;
            }

            return encontrado.Dato;
        }

        // Obtener categoría de un libro
        public NodoArbol? ObtenerCategoriaDeLibro(
            int isbn)
        {
            NodoISBN? encontrado =
                Libros.Buscar(isbn);

            if (encontrado == null)
            {
                return null;
            }

            return Categorias.BuscarCategoria(
                encontrado.Dato.Categoria);
        }

        // Obtener cantidad de libros
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

            int izquierda =
                ContarLibros(
                    actual.Izquierdo);

            int derecha =
                ContarLibros(
                    actual.Derecho);

            return 1 + izquierda + derecha;
        }

        // Obtener libros directos de una categoría
        public ListaSimple? ObtenerLibrosDeCategoria(
            string nombreCategoria)
        {
            NodoArbol? categoria =
                Categorias.BuscarCategoria(
                    nombreCategoria);

            if (categoria == null)
            {
                return null;
            }

            return categoria.Libros;
        }

        // Verificar si existe una categoría
        public bool ExisteCategoria(
            string nombreCategoria)
        {
            return Categorias.BuscarCategoria(
                nombreCategoria) != null;
        }

        // Agregar categoría
        public bool AgregarCategoria(
            string nombreCategoria,
            string nombrePadre)
        {
            if (nombreCategoria == null ||
                nombreCategoria.Trim() == "")
            {
                return false;
            }

            nombreCategoria =
                nombreCategoria.Trim();

            nombrePadre =
                nombrePadre.Trim();

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

        // Obtener raíz de categorías
        public NodoArbol? ObtenerRaizCategorias()
        {
            return Categorias.ObtenerRaiz();
        }
    }
}