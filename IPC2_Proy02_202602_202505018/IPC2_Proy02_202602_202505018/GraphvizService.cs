using System.Diagnostics;
using System.Text;
using IPC2_Proy02_202602_202505018.Modelo;
using IPC2_Proy02_202602_202505018.TDA;

namespace IPC2_Proy02_202602_202505018.Servicios
{
    public class GraphvizService
    {
        public string GenerarCategorias(
            NodoArbol? raiz)
        {
            StringBuilder contenido =
                new StringBuilder();

            contenido.AppendLine(
                "digraph Categorias {");

            contenido.AppendLine(
                "rankdir=TB;");

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

                hijo =
                    hijo.Siguiente;
            }
        }

        public string GenerarLibrosCategoria(
            NodoArbol? categoria)
        {
            StringBuilder contenido =
                new StringBuilder();

            contenido.AppendLine(
                "digraph Libros {");

            contenido.AppendLine(
                "rankdir=TB;");

            contenido.AppendLine(
                "node [shape=box];");

            if (categoria == null)
            {
                contenido.AppendLine("}");

                return contenido.ToString();
            }

            contenido.AppendLine(
                "categoria [label=\"" +
                EscaparTexto(
                    categoria.Dato.Nombre) +
                "\"];");

            Nodo? actual =
                categoria.Libros.ObtenerPrimero();

            int posicion = 0;

            while (actual != null)
            {
                Libro libro =
                    (Libro)actual.Dato;

                string idLibro =
                    "libro" + posicion;

                contenido.AppendLine(
                    idLibro +
                    " [label=\"" +
                    "ISBN: " +
                    libro.ISBN +
                    "\\nTítulo: " +
                    EscaparTexto(
                        libro.Titulo) +
                    "\\nAutor: " +
                    EscaparTexto(
                        libro.Autor) +
                    "\"];");

                contenido.AppendLine(
                    "categoria -> " +
                    idLibro +
                    ";");

                posicion++;

                actual =
                    actual.Siguiente;
            }

            contenido.AppendLine("}");

            return contenido.ToString();
        }

        public string GenerarImagenCategorias(
            NodoArbol raiz,
            string carpetaSalida)
        {
            return GenerarImagen(
                GenerarCategorias(raiz),
                carpetaSalida,
                "categorias");
        }

        public string GenerarImagenLibrosCategoria(
            NodoArbol categoria,
            string carpetaSalida)
        {
            return GenerarImagen(
                GenerarLibrosCategoria(categoria),
                carpetaSalida,
                "libros_categoria");
        }

        private string GenerarImagen(
            string contenidoDot,
            string carpetaSalida,
            string nombreBase)
        {
            Directory.CreateDirectory(
                carpetaSalida);

            string rutaDot =
                Path.Combine(
                    carpetaSalida,
                    nombreBase + ".dot");

            string rutaPng =
                Path.Combine(
                    carpetaSalida,
                    nombreBase + ".png");

            File.WriteAllText(
     rutaDot,
     contenidoDot,
     new UTF8Encoding(false)
 );

            ProcessStartInfo proceso =
                new ProcessStartInfo();

            proceso.FileName = "dot";

            proceso.Arguments =
                "-Tpng \"" +
                rutaDot +
                "\" -o \"" +
                rutaPng +
                "\"";

            proceso.UseShellExecute = false;

            proceso.CreateNoWindow = true;

            proceso.RedirectStandardOutput = true;

            proceso.RedirectStandardError = true;

            using Process? ejecutado =
                Process.Start(proceso);

            if (ejecutado == null)
            {
                throw new Exception(
                    "No se pudo iniciar Graphviz. " +
                    "Verifique que Graphviz esté instalado " +
                    "y que el comando dot esté disponible.");
            }

            string error =
                ejecutado
                    .StandardError
                    .ReadToEnd();

            ejecutado.WaitForExit();

            if (ejecutado.ExitCode != 0)
            {
                throw new Exception(
                    "Graphviz produjo un error: " +
                    error);
            }

            if (!File.Exists(rutaPng))
            {
                throw new Exception(
                    "Graphviz no generó la imagen PNG.");
            }

            return rutaPng;
        }

        private string ObtenerId(
            NodoArbol nodo)
        {
            return "categoria_" +
                   nodo.GetHashCode();
        }

        private string EscaparTexto(
            string texto)
        {
            return texto
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
        }
    }
}