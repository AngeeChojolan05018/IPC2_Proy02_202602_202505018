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

        // Cargar categorías
        private void CargarCategorias(
            XmlNode listaCategorias,
            Catalogo catalogo)
        {
            ListaSimple pendientes =
                new ListaSimple();

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

                string padre = "";

                if (nombrePadre != null)
                {
                    padre = nombrePadre.Trim();
                }

                DatosCategoriaXML datos =
                    new DatosCategoriaXML(
                        nombre,
                        padre);

                pendientes.Agregar(datos);
            }

            bool huboCambios = true;

            while (!pendientes.EstaVacia() &&
                   huboCambios)
            {
                huboCambios = false;

                Nodo? actual =
                    pendientes.ObtenerPrimero();

                while (actual != null)
                {
                    Nodo? siguiente =
                        actual.Siguiente;

                    DatosCategoriaXML datos =
                        (DatosCategoriaXML)actual.Dato;

                    bool agregada = false;

                    // Agregar la categoría raíz
                    if (catalogo.Categorias.EstaVacio())
                    {
                        if (datos.Padre == "")
                        {
                            Categoria categoria =
                                new Categoria(
                                    datos.Nombre);

                            NodoArbol nuevaCategoria =
                                new NodoArbol(
                                    categoria);

                            catalogo.Categorias
                                .EstablecerRaiz(
                                    nuevaCategoria);

                            agregada = true;
                        }
                    }
                    // Agregar una categoría hija
                    else if (datos.Padre != "" &&
                             catalogo.ExisteCategoria(
                                 datos.Padre))
                    {
                        agregada =
                            catalogo.AgregarCategoria(
                                datos.Nombre,
                                datos.Padre);
                    }

                    if (agregada)
                    {
                        pendientes.Eliminar(
                            datos);

                        huboCambios = true;
                    }

                    actual = siguiente;
                }
            }
        }

        // Cargar libros
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
                    int.Parse(
                        nodoISBN.InnerText.Trim());

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

                catalogo.RegistrarLibro(libro);
            }
        }

        private class DatosCategoriaXML
        {
            public string Nombre { get; set; }

            public string Padre { get; set; }

            public DatosCategoriaXML(
                string nombre,
                string padre)
            {
                Nombre = nombre;
                Padre = padre;
            }
        }
    }
}