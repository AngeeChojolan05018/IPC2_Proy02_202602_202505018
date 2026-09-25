using IPC2_Proy02_202602_202505018.Modelo;

namespace IPC2_Proy02_202602_202505018.TDA
{
    public class NodoArbol
    {
        public Categoria Dato { get; set; }

        public NodoArbol? Padre { get; set; }

        public ListaSimple Hijos { get; set; }

        public ListaSimple Libros { get; set; }

        public NodoArbol(Categoria dato)
        {
            Dato = dato;
            Padre = null;

            Hijos = new ListaSimple();
            Libros = new ListaSimple();
        }

        public void AgregarHijo(NodoArbol hijo)
        {
            hijo.Padre = this;

            Hijos.Agregar(hijo);
        }

        public void AgregarHijoOrdenado(
            NodoArbol hijo)
        {
            hijo.Padre = this;

            if (Hijos.EstaVacia())
            {
                Hijos.Agregar(hijo);
                return;
            }

            Nodo? anterior = null;

            Nodo? actual =
                Hijos.ObtenerPrimero();

            while (actual != null)
            {
                NodoArbol categoriaActual =
                    (NodoArbol)actual.Dato;

                int comparacion =
                    string.Compare(
                        hijo.Dato.Nombre,
                        categoriaActual.Dato.Nombre,
                        StringComparison.OrdinalIgnoreCase);

                if (comparacion < 0)
                {
                    break;
                }

                anterior = actual;
                actual = actual.Siguiente;
            }

            if (anterior == null)
            {
                Hijos.AgregarAlInicio(hijo);
            }
            else
            {
                Hijos.AgregarAntesDe(
                    anterior,
                    hijo);
            }
        }

        public void AgregarLibro(
            Libro libro)
        {
            Libros.AgregarOrdenadoISBN(
                libro);
        }
    }
}