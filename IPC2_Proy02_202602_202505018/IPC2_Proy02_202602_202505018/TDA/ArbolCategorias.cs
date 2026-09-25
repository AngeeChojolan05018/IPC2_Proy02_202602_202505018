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

        public void EstablecerRaiz(
            NodoArbol nuevaRaiz)
        {
            raiz = nuevaRaiz;
            raiz.Padre = null;
        }

        public bool EstaVacio()
        {
            return raiz == null;
        }

        public bool AgregarCategoria(
            NodoArbol nuevaCategoria,
            string nombrePadre)
        {
            if (nuevaCategoria == null)
            {
                return false;
            }

            if (raiz == null)
            {
                if (!string.IsNullOrWhiteSpace(
                        nombrePadre))
                {
                    return false;
                }

                raiz = nuevaCategoria;
                raiz.Padre = null;

                return true;
            }

            if (BuscarCategoria(
                    nuevaCategoria.Dato.Nombre) != null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    nombrePadre))
            {
                return false;
            }

            NodoArbol? padre =
                BuscarCategoria(nombrePadre);

            if (padre == null)
            {
                return false;
            }

            padre.AgregarHijoOrdenado(
                nuevaCategoria);

            return true;
        }

        public NodoArbol? BuscarCategoria(
            string nombre)
        {
            if (raiz == null ||
                string.IsNullOrWhiteSpace(nombre))
            {
                return null;
            }

            return BuscarCategoriaRecursivo(
                raiz,
                nombre.Trim());
        }

        private NodoArbol? BuscarCategoriaRecursivo(
            NodoArbol actual,
            string nombre)
        {
            if (string.Equals(
                    actual.Dato.Nombre,
                    nombre,
                    StringComparison.OrdinalIgnoreCase))
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
    }
}