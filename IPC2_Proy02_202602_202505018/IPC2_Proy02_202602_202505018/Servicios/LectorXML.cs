using System.Xml;
using IPC2_Proy02_202602_202505018.Modelo;
using IPC2_Proy02_202602_202505018.TDA;

namespace IPC2_Proy02_202602_202505018.Servicios
{
    public class LectorXML
    {
        public bool CargarArchivo(
            string ruta,
            Catalogo catalogo)
        {
            XmlDocument documento = new XmlDocument();

            documento.Load(ruta);

            XmlNode? listaCategorias =
                documento.SelectSingleNode(
                    "/config/listaCategorias");

            if (listaCategorias != null)
            {
                CargarCategorias(
                    listaCategorias,
                    catalogo);
            }

            XmlNode? listaLibros =
                documento.SelectSingleNode(
                    "/config/listaLibros");

            if (listaLibros != null)
            {
                CargarLibros(
                    listaLibros,
                    catalogo);
            }

            return true;
        }

        private void CargarCategorias(
            XmlNode listaCategorias,
            Catalogo catalogo)
        {
            foreach (XmlNode nodoCategoria
                in listaCategorias.SelectNodes("categoria")!)
            {
                string nombre =
                    nodoCategoria.InnerText.Trim();

                string? nombrePadre =
                    nodoCategoria.Attributes?["padre"]?.Value;

                if (nombre == "")
                {
                    continue;
                }

                Categoria categoria =
                    new Categoria(nombre);

                NodoArbol nuevaCategoria =
                    new NodoArbol(categoria);

                if (catalogo.Categorias.EstaVacio())
                {
                    catalogo.Categorias.EstablecerRaiz(
                        nuevaCategoria);
                }
                else if (nombrePadre != null &&
                         nombrePadre.Trim() != "")
                {
                    catalogo.Categorias.AgregarCategoria(
                        nuevaCategoria,
                        nombrePadre.Trim());
                }
            }
        }

        private void CargarLibros(
            XmlNode listaLibros,
            Catalogo catalogo)
        {
            foreach (XmlNode nodoLibro
                in listaLibros.SelectNodes("libro")!)
            {
                XmlNode? nodoISBN =
                    nodoLibro.SelectSingleNode("ISBN");

                XmlNode? nodoTitulo =
                    nodoLibro.SelectSingleNode("titulo");

                XmlNode? nodoAutor =
                    nodoLibro.SelectSingleNode("autor");

                XmlNode? nodoCategoria =
                    nodoLibro.SelectSingleNode("categoria");

                if (nodoISBN == null ||
                    nodoTitulo == null ||
                    nodoAutor == null ||
                    nodoCategoria == null)
                {
                    continue;
                }

                int isbn =
                    int.Parse(nodoISBN.InnerText.Trim());

                string titulo =
                    nodoTitulo.InnerText.Trim();

                string autor =
                    nodoAutor.InnerText.Trim();

                string categoria =
                    nodoCategoria.InnerText.Trim();

                Libro libro =
                    new Libro(
                        isbn,
                        titulo,
                        autor,
                        categoria);

                NodoISBN nuevo =
                    new NodoISBN(libro);

                catalogo.Libros.Insertar(nuevo);
            }
        }
    }
}