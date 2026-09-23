namespace IPC2_Proy02_202602_202505018.TDA
{
    public class ListaSimple
    {
        private Nodo? primero;

        public ListaSimple()
        {
            primero = null;
        }

        public void Agregar(object dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (primero == null)
            {
                primero = nuevo;
                return;
            }

            Nodo actual = primero;

            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }

            actual.Siguiente = nuevo;
        }

        public void AgregarAlInicio(object dato)
        {
            Nodo nuevo = new Nodo(dato);

            nuevo.Siguiente = primero;
            primero = nuevo;
        }

        public void AgregarAntesDe(
            Nodo? anterior,
            object dato)
        {
            Nodo nuevo = new Nodo(dato);

            if (anterior == null)
            {
                nuevo.Siguiente = primero;
                primero = nuevo;
                return;
            }

            nuevo.Siguiente = anterior.Siguiente;
            anterior.Siguiente = nuevo;
        }

        public bool EstaVacia()
        {
            return primero == null;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            Nodo? actual = primero;

            while (actual != null)
            {
                cantidad++;
                actual = actual.Siguiente;
            }

            return cantidad;
        }

        public Nodo? ObtenerPrimero()
        {
            return primero;
        }
    }
}