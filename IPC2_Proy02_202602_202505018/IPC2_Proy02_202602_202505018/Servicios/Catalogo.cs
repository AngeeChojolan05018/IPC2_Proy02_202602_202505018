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
            if (Libros.Buscar(libro.ISBN) != null)
            {
                return false;
            }

            NodoArbol? categoria =
                Categorias.BuscarCategoria(libro.Categoria);

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

            NodoArbol? categoria =
                Categorias.BuscarCategoria(
                    encontrado.Dato.Categoria);

            Libros.Eliminar(isbn);

            if (categoria != null)
            {
                EliminarLibroDeCategoria(
                    categoria,
                    isbn);
            }

            return true;
        }

        private void EliminarLibroDeCategoria(
            NodoArbol categoria,
            int isbn)
        {
            Nodo? actual =
                categoria.Libros.ObtenerPrimero();

            Nodo? anterior = null;

            while (actual != null)
            {
                Libro libro =
                    (Libro)actual.Dato;

                if (libro.ISBN == isbn)
                {
                    if (anterior == null)
                    {
                        EliminarPrimerLibro(
                            categoria);
                    }
                    else
                    {
                        anterior.Siguiente =
                            actual.Siguiente;
                    }

                    return;
                }

                anterior = actual;
                actual = actual.Siguiente;
            }
        }

        private void EliminarPrimerLibro(
            NodoArbol categoria)
        {
            Nodo? primero =
                categoria.Libros.ObtenerPrimero();

            if (primero == null)
            {
                return;
            }

            Nodo? siguiente =
                primero.Siguiente;

            categoria.Libros =
                CrearListaDesdeNodo(siguiente);
        }

        private ListaSimple CrearListaDesdeNodo(
            Nodo? inicio)
        {
            ListaSimple nuevaLista =
                new ListaSimple();

            Nodo? actual = inicio;

            while (actual != null)
            {
                nuevaLista.Agregar(actual.Dato);
                actual = actual.Siguiente;
            }

            return nuevaLista;
        }

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
    }
}