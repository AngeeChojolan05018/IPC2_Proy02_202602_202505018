using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proy02_202602_202505018.Modelo;
using IPC2_Proy02_202602_202505018.Servicios;
using IPC2_Proy02_202602_202505018.TDA;

namespace IPC2_Proy02_202602_202505018.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Catalogo catalogo;
        private readonly LectorXML lectorXML;
        private readonly GraphvizService graphvizService;

        public IndexModel(
            Catalogo catalogo,
            GraphvizService graphvizService)
        {
            this.catalogo = catalogo;
            this.lectorXML = new LectorXML();
            this.graphvizService = graphvizService;
        }

        public NodoArbol? RaizCategorias =>
            catalogo.ObtenerRaizCategorias();

        public int CantidadLibros =>
            catalogo.ObtenerCantidadLibros();

        public Libro? LibroEncontrado
        {
            get;
            private set;
        }

        public Libro? LibroMenor =>
            catalogo.ObtenerLibroMenor();

        public Libro? LibroMayor =>
            catalogo.ObtenerLibroMayor();

        public string Mensaje
        {
            get;
            private set;
        } = "";

        public string TipoMensaje
        {
            get;
            private set;
        } = "";

        public string RutaGraficoCategorias
        {
            get;
            private set;
        } = "";

        public string RutaGraficoLibros
        {
            get;
            private set;
        } = "";

        public string RutaGraficoEstructura
        {
            get;
            private set;
        } = "";

        [BindProperty]
        public string CategoriaGrafico
        {
            get;
            set;
        } = "";

        [BindProperty]
        public string CategoriaInicioGrafico
        {
            get;
            set;
        } = "";

        [BindProperty]
        public string NombreCategoria
        {
            get;
            set;
        } = "";

        [BindProperty]
        public string CategoriaPadre
        {
            get;
            set;
        } = "";

        [BindProperty]
        public int ISBN
        {
            get;
            set;
        }

        [BindProperty]
        public string Titulo
        {
            get;
            set;
        } = "";

        [BindProperty]
        public string Autor
        {
            get;
            set;
        } = "";

        [BindProperty]
        public string CategoriaLibro
        {
            get;
            set;
        } = "";

        [BindProperty]
        public int ISBNBuscar
        {
            get;
            set;
        }

        [BindProperty]
        public int ISBNEliminar
        {
            get;
            set;
        }

        [BindProperty]
        public IFormFile? ArchivoXML
        {
            get;
            set;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostReiniciar()
        {
            catalogo.Reiniciar();

            LibroEncontrado = null;

            LimpiarGraficos();

            EstablecerMensaje(
                "El sistema fue reiniciado correctamente.",
                "exito");

            return Page();
        }

        public IActionResult OnPostAgregarCategoria()
        {
            string nombre =
                NombreCategoria?.Trim() ?? "";

            string padre =
                CategoriaPadre?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                EstablecerMensaje(
                    "Debe escribir el nombre de la categoría.",
                    "error");

                return Page();
            }

            bool agregada =
                catalogo.AgregarCategoria(
                    nombre,
                    padre);

            if (agregada)
            {
                EstablecerMensaje(
                    "Categoría agregada correctamente.",
                    "exito");
            }
            else
            {
                EstablecerMensaje(
                    "No se pudo agregar la categoría. " +
                    "Verifique que el nombre no exista " +
                    "y que la categoría padre exista.",
                    "error");
            }

            return Page();
        }

        public IActionResult OnPostRegistrarLibro()
        {
            string titulo =
                Titulo?.Trim() ?? "";

            string autor =
                Autor?.Trim() ?? "";

            string categoria =
                CategoriaLibro?.Trim() ?? "";

            if (ISBN <= 0)
            {
                EstablecerMensaje(
                    "El ISBN debe ser mayor que cero.",
                    "error");

                return Page();
            }

            if (string.IsNullOrWhiteSpace(titulo) ||
                string.IsNullOrWhiteSpace(autor) ||
                string.IsNullOrWhiteSpace(categoria))
            {
                EstablecerMensaje(
                    "Debe completar todos los datos del libro.",
                    "error");

                return Page();
            }

            Libro libro =
                new Libro(
                    ISBN,
                    titulo,
                    autor,
                    categoria);

            bool registrado =
                catalogo.RegistrarLibro(libro);

            if (registrado)
            {
                EstablecerMensaje(
                    "Libro registrado correctamente.",
                    "exito");
            }
            else
            {
                EstablecerMensaje(
                    "No se pudo registrar el libro. " +
                    "Verifique que la categoría exista " +
                    "y que el ISBN no esté repetido.",
                    "error");
            }

            return Page();
        }

        public IActionResult OnPostBuscarLibro()
        {
            if (ISBNBuscar <= 0)
            {
                EstablecerMensaje(
                    "Escriba un ISBN válido.",
                    "error");

                return Page();
            }

            LibroEncontrado =
                catalogo.BuscarLibro(
                    ISBNBuscar);

            if (LibroEncontrado == null)
            {
                EstablecerMensaje(
                    "No se encontró un libro con ese ISBN.",
                    "error");
            }
            else
            {
                EstablecerMensaje(
                    "Libro encontrado correctamente.",
                    "exito");
            }

            return Page();
        }

        public IActionResult OnPostEliminarLibro()
        {
            if (ISBNEliminar <= 0)
            {
                EstablecerMensaje(
                    "Escriba un ISBN válido.",
                    "error");

                return Page();
            }

            bool eliminado =
                catalogo.EliminarLibro(
                    ISBNEliminar);

            if (eliminado)
            {
                EstablecerMensaje(
                    "Libro eliminado correctamente.",
                    "exito");
            }
            else
            {
                EstablecerMensaje(
                    "No se encontró un libro con ese ISBN.",
                    "error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCargarXMLAsync()
        {
            if (ArchivoXML == null || ArchivoXML.Length == 0)
            {
                EstablecerMensaje("Seleccione un archivo XML.", "error");
                return Page();
            }

            string carpetaTemporal = Path.GetTempPath();
            string rutaTemporal = Path.Combine(
                carpetaTemporal,
                Guid.NewGuid().ToString() + ".xml"
            );

            try
            {
                // Primero guardamos completamente el archivo y cerramos el stream.
                using (FileStream stream = new FileStream(
                    rutaTemporal,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None))
                {
                    await ArchivoXML.CopyToAsync(stream);
                }

                // El archivo ya está cerrado, por lo que ahora sí podemos leerlo.
                LectorXML lectorXML = new LectorXML();

                catalogo.Reiniciar();

                bool cargado = lectorXML.CargarArchivo(rutaTemporal, catalogo);

                if (cargado)
                {
                    EstablecerMensaje(
                        "XML cargado correctamente. Se registraron "
                        + catalogo.ObtenerCantidadLibros()
                        + " libros.",
                        "exito"
                    );
                }
                else
                {
                    EstablecerMensaje(
                        "No fue posible cargar el XML.",
                        "error"
                    );
                }
            }
            catch (Exception ex)
            {
                EstablecerMensaje(
                    "Error al cargar el XML: " + ex.Message,
                    "error"
                );
            }
            finally
            {
                // Eliminamos el archivo temporal después de terminar la lectura.
                if (System.IO.File.Exists(rutaTemporal))
                {
                    try
                    {
                        System.IO.File.Delete(rutaTemporal);
                    }
                    catch
                    {
                        // Si Windows todavía lo tiene ocupado, no detenemos la aplicación.
                    }
                }
            }

            return Page();
        }

        public string ObtenerArbolCategoriasTexto()
        {
            NodoArbol? raiz =
                catalogo.ObtenerRaizCategorias();

            if (raiz == null)
            {
                return "No hay categorías cargadas.";
            }

            return ObtenerCategoriaTexto(
                raiz,
                0);
        }

        private string ObtenerCategoriaTexto(
            NodoArbol categoria,
            int nivel)
        {
            string texto = "";

            for (int i = 0; i < nivel; i++)
            {
                texto += "    ";
            }

            texto +=
                "- " +
                categoria.Dato.Nombre +
                Environment.NewLine;

            Nodo? actual =
                categoria.Hijos.ObtenerPrimero();

            while (actual != null)
            {
                NodoArbol hijo =
                    (NodoArbol)actual.Dato;

                texto +=
                    ObtenerCategoriaTexto(
                        hijo,
                        nivel + 1);

                actual =
                    actual.Siguiente;
            }

            return texto;
        }

        public string ObtenerLibrosAscendenteTexto()
        {
            return catalogo
                .ObtenerLibrosAscendenteTexto();
        }

        public string ObtenerLibrosCategoriaTexto()
        {
            if (string.IsNullOrWhiteSpace(
                    CategoriaGrafico))
            {
                return
                    "Escriba una categoría para consultar sus libros.";
            }

            return catalogo
                .ObtenerLibrosCategoriaTexto(
                    CategoriaGrafico.Trim());
        }

        public IActionResult
            OnPostGenerarGraficoCategorias()
        {
            NodoArbol? raiz =
                catalogo.ObtenerRaizCategorias();

            if (raiz == null)
            {
                EstablecerMensaje(
                    "No hay categorías para generar el gráfico.",
                    "error");

                return Page();
            }

            try
            {
                string carpetaSalida =
                    ObtenerCarpetaReportes();

                graphvizService
                    .GenerarImagenCategorias(
                        raiz,
                        carpetaSalida);

                RutaGraficoCategorias =
                    "/reportes/categorias.png?v=" +
                    DateTime.Now.Ticks;

                EstablecerMensaje(
                    "Árbol completo de categorías generado.",
                    "exito");
            }
            catch (Exception ex)
            {
                EstablecerMensaje(
                    "Error al generar el gráfico: " +
                    ex.Message,
                    "error");
            }

            return Page();
        }

        public IActionResult
            OnPostGenerarGraficoEstructura()
        {
            string nombre =
                CategoriaInicioGrafico?.Trim() ?? "";

            NodoArbol? inicio;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                inicio =
                    catalogo.ObtenerRaizCategorias();
            }
            else
            {
                inicio =
                    catalogo.BuscarCategoria(nombre);
            }

            if (inicio == null)
            {
                EstablecerMensaje(
                    "No se encontró la categoría desde la cual iniciar el gráfico.",
                    "error");

                return Page();
            }

            try
            {
                string carpetaSalida =
                    ObtenerCarpetaReportes();

                graphvizService
                    .GenerarImagenCategorias(
                        inicio,
                        carpetaSalida);

                RutaGraficoEstructura =
                    "/reportes/categorias.png?v=" +
                    DateTime.Now.Ticks;

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    EstablecerMensaje(
                        "Gráfico generado desde la raíz.",
                        "exito");
                }
                else
                {
                    EstablecerMensaje(
                        "Gráfico generado desde la categoría seleccionada.",
                        "exito");
                }
            }
            catch (Exception ex)
            {
                EstablecerMensaje(
                    "Error al generar el gráfico: " +
                    ex.Message,
                    "error");
            }

            return Page();
        }

        public IActionResult
            OnPostGenerarGraficoLibros()
        {
            string nombreCategoria =
                CategoriaGrafico?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(
                    nombreCategoria))
            {
                EstablecerMensaje(
                    "Escriba el nombre de una categoría.",
                    "error");

                return Page();
            }

            NodoArbol? categoria =
                catalogo.BuscarCategoria(
                    nombreCategoria);

            if (categoria == null)
            {
                EstablecerMensaje(
                    "No se encontró la categoría indicada.",
                    "error");

                return Page();
            }

            try
            {
                string carpetaSalida =
                    ObtenerCarpetaReportes();

                graphvizService
                    .GenerarImagenLibrosCategoria(
                        categoria,
                        carpetaSalida);

                RutaGraficoLibros =
                    "/reportes/libros_categoria.png?v=" +
                    DateTime.Now.Ticks;

                EstablecerMensaje(
                    "Gráfico generado para la categoría seleccionada.",
                    "exito");
            }
            catch (Exception ex)
            {
                EstablecerMensaje(
                    "Error al generar el gráfico: " +
                    ex.Message,
                    "error");
            }

            return Page();
        }

        private string ObtenerCarpetaReportes()
        {
            string carpeta =
                Path.Combine(
                    Environment.CurrentDirectory,
                    "wwwroot",
                    "reportes");

            Directory.CreateDirectory(
                carpeta);

            return carpeta;
        }

        private void LimpiarGraficos()
        {
            RutaGraficoCategorias = "";
            RutaGraficoEstructura = "";
            RutaGraficoLibros = "";
        }

        private void EstablecerMensaje(
            string mensaje,
            string tipo)
        {
            Mensaje = mensaje;
            TipoMensaje = tipo;
        }
    }
}