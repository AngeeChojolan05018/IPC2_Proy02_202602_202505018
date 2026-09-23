namespace IPC2_Proy02_202602_202505018.TDA
{
    public class ArbolCategorias
    {
        private NodoArbol? raiz;

        public ArbolCategorias()
        {
            raiz = null;
        }

        public NodoArbol? ObtenerRaiz()
        {
            return raiz;
        }

        public void EstablecerRaiz(NodoArbol nuevaRaiz)
        {
            raiz = nuevaRaiz;
        }

        public bool EstaVacio()
        {
            return raiz == null;
        }

        public bool AgregarCategoria(
            NodoArbol nuevaCategoria,
            string nombrePadre)
        {
            if (raiz == null)
            {
                raiz = nuevaCategoria;
                return true;
            }

            if (BuscarCategoria(
                nuevaCategoria.Dato.Nombre) != null)
            {
                return false;
            }

            NodoArbol? padre =
                BuscarCategoria(nombrePadre);

            if (padre == null)
            {
                return false;
            }

            AgregarHijoOrdenado(
                padre,
                nuevaCategoria);

            return true;
        }

        public NodoArbol? BuscarCategoria(
            string nombre)
        {
            if (raiz == null)
            {
                return null;
            }

            return BuscarCategoriaRecursivo(
                raiz,
                nombre);
        }

        private NodoArbol? BuscarCategoriaRecursivo(
            NodoArbol actual,
            string nombre)
        {
            if (actual.Dato.Nombre == nombre)
            {
                return actual;
            }

            Nodo? hijo =
                actual.Hijos.ObtenerPrimero();

            while (hijo != null)
            {
                NodoArbol nodoHijo =
                    (NodoArbol)hijo.Dato;

                NodoArbol? resultado =
                    BuscarCategoriaRecursivo(
                        nodoHijo,
                        nombre);

                if (resultado != null)
                {
                    return resultado;
                }

                hijo = hijo.Siguiente;
            }

            return null;
        }

        private void AgregarHijoOrdenado(
            NodoArbol padre,
            NodoArbol nuevo)
        {
            nuevo.Padre = padre;

            Nodo? anterior = null;

            Nodo? actual =
                padre.Hijos.ObtenerPrimero();

            while (actual != null)
            {
                NodoArbol hijoActual =
                    (NodoArbol)actual.Dato;

                int comparacion =
                    string.Compare(
                        nuevo.Dato.Nombre,
                        hijoActual.Dato.Nombre,
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
                padre.Hijos.AgregarAlInicio(nuevo);
            }
            else
            {
                padre.Hijos.AgregarAntesDe(
                    anterior,
                    nuevo);
            }
        }
    }
}