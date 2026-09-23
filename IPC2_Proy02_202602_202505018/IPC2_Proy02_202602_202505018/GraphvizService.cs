using IPC2_Proy02_202602_202505018.Modelo;
using IPC2_Proy02_202602_202505018.TDA;
using System.Text;

namespace IPC2_Proy02_202602_202505018.Servicios
{
    public class GraphvizService
    {
        // Generar árbol de categorías
        public string GenerarCategorias(
            NodoArbol? raiz)
        {
            StringBuilder contenido =
                new StringBuilder();

            contenido.AppendLine(
                "digraph Categorias {");

            contenido.AppendLine(
                "node [shape=box];");

            if (raiz != null)
            {
                GenerarCategoriaRecursiva(
                    raiz,
                    contenido);
            }

            contenido.AppendLine("}");

            return contenido.ToString();
        }

        private void GenerarCategoriaRecursiva(
            NodoArbol actual,
            StringBuilder contenido)
        {
            string idActual =
                ObtenerId(actual);

            contenido.AppendLine(
                idActual +
                " [label=\"" +
                EscaparTexto(
                    actual.Dato.Nombre) +
                "\"];");

            Nodo? hijo =
                actual.Hijos.ObtenerPrimero();

            while (hijo != null)
            {
                NodoArbol nodoHijo =
                    (NodoArbol)hijo.Dato;

                string idHijo =
                    ObtenerId(nodoHijo);

                contenido.AppendLine(
                    idActual +
                    " -> " +
                    idHijo +
                    ";");

                GenerarCategoriaRecursiva(
                    nodoHijo,
                    contenido);

                hijo = hijo.Siguiente;
            }
        }

        // Generar libros de una categoría
        public string GenerarLibrosCategoria(
            NodoArbol? categoria)
        {
            StringBuilder contenido =
                new StringBuilder();

            contenido.AppendLine(
                "digraph Libros {");

            contenido.AppendLine(
                "node [shape=box];");

            if (categoria != null)
            {
                contenido.AppendLine(
                    "categoria [label=\"" +
                    EscaparTexto(
                        categoria.Dato.Nombre) +
                    "\"];");

                NodoISBN?[] libros =
                    ObtenerLibrosOrdenados(
                        categoria);

                for (int i = 0;
                     i < libros.Length;
                     i++)
                {
                    if (libros[i] == null)
                    {
                        continue;
                    }

                    string idLibro =
                        "libro" + i;

                    contenido.AppendLine(
                        idLibro +
                        " [label=\"" +
                        EscaparTexto(
                            libros[i]!.Dato.ISBN +
                            " - " +
                            libros[i]!.Dato.Titulo) +
                        "\"];");

                    contenido.AppendLine(
                        "categoria -> " +
                        idLibro +
                        ";");
                }
            }

            contenido.AppendLine("}");

            return contenido.ToString();
        }

        private NodoISBN?[] ObtenerLibrosOrdenados(
            NodoArbol categoria)
        {
            int cantidad =
                categoria.Libros.ObtenerCantidad();

            NodoISBN?[] resultado =
                new NodoISBN?[cantidad];

            int posicion = 0;

            Nodo? actual =
                categoria.Libros.ObtenerPrimero();

            while (actual != null)
            {
                Libro libro =
                    (Libro)actual.Dato;

                resultado[posicion] =
                    new NodoISBN(libro);

                posicion++;

                actual = actual.Siguiente;
            }

            OrdenarLibros(resultado);

            return resultado;
        }

        // Ordenar libros por ISBN
        private void OrdenarLibros(
            NodoISBN?[] libros)
        {
            for (int i = 0;
                 i < libros.Length - 1;
                 i++)
            {
                for (int j = 0;
                     j < libros.Length - 1 - i;
                     j++)
                {
                    if (libros[j] == null ||
                        libros[j + 1] == null)
                    {
                        continue;
                    }

                    if (libros[j]!.Dato.ISBN >
                        libros[j + 1]!.Dato.ISBN)
                    {
                        NodoISBN? temporal =
                            libros[j];

                        libros[j] =
                            libros[j + 1];

                        libros[j + 1] =
                            temporal;
                    }
                }
            }
        }

        // Obtener identificador de categoría
        private string ObtenerId(
            NodoArbol nodo)
        {
            return "categoria_" +
                   nodo.GetHashCode();
        }

        // Escapar texto para Graphviz
        private string EscaparTexto(
            string texto)
        {
            return texto
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
        }
    }
}