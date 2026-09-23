namespace IPC2_Proy02_202602_202505018.TDA
{
    public class ArbolISBN
    {
        private NodoISBN? raiz;

        public ArbolISBN()
        {
            raiz = null;
        }

        public NodoISBN? ObtenerRaiz()
        {
            return raiz;
        }

        public bool EstaVacio()
        {
            return raiz == null;
        }

        public void Insertar(NodoISBN nuevo)
        {
            if (raiz == null)
            {
                raiz = nuevo;
                return;
            }

            NodoISBN actual = raiz;

            while (true)
            {
                if (nuevo.Dato.ISBN < actual.Dato.ISBN)
                {
                    if (actual.Izquierdo == null)
                    {
                        actual.Izquierdo = nuevo;
                        return;
                    }

                    actual = actual.Izquierdo;
                }
                else if (nuevo.Dato.ISBN > actual.Dato.ISBN)
                {
                    if (actual.Derecho == null)
                    {
                        actual.Derecho = nuevo;
                        return;
                    }

                    actual = actual.Derecho;
                }
                else
                {
                    return;
                }
            }
        }

        public NodoISBN? Buscar(int isbn)
        {
            NodoISBN? actual = raiz;

            while (actual != null)
            {
                if (isbn == actual.Dato.ISBN)
                {
                    return actual;
                }

                if (isbn < actual.Dato.ISBN)
                {
                    actual = actual.Izquierdo;
                }
                else
                {
                    actual = actual.Derecho;
                }
            }

            return null;
        }

        public NodoISBN? ObtenerMinimo()
        {
            if (raiz == null)
            {
                return null;
            }

            NodoISBN actual = raiz;

            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }

            return actual;
        }

        public NodoISBN? ObtenerMaximo()
        {
            if (raiz == null)
            {
                return null;
            }

            NodoISBN actual = raiz;

            while (actual.Derecho != null)
            {
                actual = actual.Derecho;
            }

            return actual;
        }

        public void MostrarAscendente()
        {
            MostrarAscendenteRecursivo(raiz);
        }

        private void MostrarAscendenteRecursivo(NodoISBN? actual)
        {
            if (actual == null)
            {
                return;
            }

            MostrarAscendenteRecursivo(actual.Izquierdo);

            Console.WriteLine(
                "ISBN: " + actual.Dato.ISBN +
                " - Titulo: " + actual.Dato.Titulo);

            MostrarAscendenteRecursivo(actual.Derecho);
        }

        public void Eliminar(int isbn)
        {
            raiz = EliminarRecursivo(raiz, isbn);
        }

        private NodoISBN? EliminarRecursivo(
            NodoISBN? actual,
            int isbn)
        {
            if (actual == null)
            {
                return null;
            }

            if (isbn < actual.Dato.ISBN)
            {
                actual.Izquierdo =
                    EliminarRecursivo(actual.Izquierdo, isbn);

                return actual;
            }

            if (isbn > actual.Dato.ISBN)
            {
                actual.Derecho =
                    EliminarRecursivo(actual.Derecho, isbn);

                return actual;
            }

            if (actual.Izquierdo == null &&
                actual.Derecho == null)
            {
                return null;
            }

            if (actual.Izquierdo == null)
            {
                return actual.Derecho;
            }

            if (actual.Derecho == null)
            {
                return actual.Izquierdo;
            }

            NodoISBN sucesor = ObtenerMinimoDesdeNodo(
                actual.Derecho);

            actual.Dato = sucesor.Dato;

            actual.Derecho =
                EliminarRecursivo(
                    actual.Derecho,
                    sucesor.Dato.ISBN);

            return actual;
        }

        private NodoISBN ObtenerMinimoDesdeNodo(
            NodoISBN actual)
        {
            while (actual.Izquierdo != null)
            {
                actual = actual.Izquierdo;
            }

            return actual;
        }
    }
}