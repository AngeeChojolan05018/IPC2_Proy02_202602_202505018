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

        public NodoArbol? RaizCategorias =>
            catalogo.ObtenerRaizCategorias();

        public int CantidadLibros =>
            catalogo.ObtenerCantidadLibros();

        public Libro? LibroEncontrado { get; private set; }

        public Libro? LibroMenor =>
            catalogo.ObtenerLibroMenor();

        public Libro? LibroMayor =>
            catalogo.ObtenerLibroMayor();

        public string Mensaje { get; private set; } = "";

        public string TipoMensaje { get; private set; } = "";

        [BindProperty]
        public string NombreCategoria { get; set; } = "";

        [BindProperty]
        public string CategoriaPadre { get; set; } = "";

        [BindProperty]
        public int ISBN { get; set; }

        [BindProperty]
        public string Titulo { get; set; } = "";

        [BindProperty]
        public string Autor { get; set; } = "";

        [BindProperty]
        public string CategoriaLibro { get; set; } = "";

        [BindProperty]
        public int ISBNBuscar { get; set; }

        [BindProperty]
        public int ISBNEliminar { get; set; }

        [BindProperty]
        public IFormFile? ArchivoXML { get; set; }

        public IndexModel(Catalogo catalogo)
        {
            this.catalogo = catalogo;
            lectorXML = new LectorXML();
        }

        public void OnGet()
        {
        }

        // Agregar categoría
        public IActionResult OnPostAgregarCategoria()
        {
            string nombre = NombreCategoria?.Trim() ?? "";
            string padre = CategoriaPadre?.Trim() ?? "";

            if (nombre == "")
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
                    "No se pudo agregar la categoría. Verifique el padre o si ya existe.",
                    "error");
            }

            return Page();
        }

        // Registrar libro
        public IActionResult OnPostRegistrarLibro()
        {
            string titulo = Titulo.Trim();
            string autor = Autor.Trim();
            string categoria = CategoriaLibro.Trim();

            if (ISBN <= 0 ||
                titulo == "" ||
                autor == "" ||
                categoria == "")
            {
                EstablecerMensaje(
                    "Complete todos los datos del libro y coloque un ISBN válido.",
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
                    "No se pudo registrar el libro. Revise que la categoría exista y que el ISBN no esté repetido.",
                    "error");
            }

            return Page();
        }

        // Buscar libro
        public IActionResult OnPostBuscarLibro()
        {
            if (ISBNBuscar <= 0)
            {
                EstablecerMensaje(
                    "Escriba un ISBN válido para buscar.",
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
                    "Libro encontrado.",
                    "exito");
            }

            return Page();
        }

        // Eliminar libro
        public IActionResult OnPostEliminarLibro()
        {
            if (ISBNEliminar <= 0)
            {
                EstablecerMensaje(
                    "Escriba un ISBN válido para eliminar.",
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

        // Cargar XML
        public async Task<IActionResult> OnPostCargarXMLAsync()
        {
            if (ArchivoXML == null ||
                ArchivoXML.Length == 0)
            {
                EstablecerMensaje(
                    "Seleccione un archivo XML.",
                    "error");

                return Page();
            }

            string extension =
                Path.GetExtension(
                    ArchivoXML.FileName);

            if (!extension.Equals(
                    ".xml",
                    StringComparison.OrdinalIgnoreCase))
            {
                EstablecerMensaje(
                    "El archivo seleccionado debe tener extensión .xml.",
                    "error");

                return Page();
            }

            string rutaTemporal =
                Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString() + ".xml");

            try
            {
                using (FileStream archivo =
                    new FileStream(
                        rutaTemporal,
                        FileMode.Create,
                        FileAccess.Write))
                {
                    await ArchivoXML.CopyToAsync(
                        archivo);
                }

                lectorXML.CargarArchivo(
                    rutaTemporal,
                    catalogo);

                EstablecerMensaje(
                    "Archivo XML cargado correctamente.",
                    "exito");
            }
            catch (Exception ex)
            {
                EstablecerMensaje(
                    "Error al cargar el XML: " +
                    ex.Message,
                    "error");
            }
            finally
            {
                if (System.IO.File.Exists(
                        rutaTemporal))
                {
                    System.IO.File.Delete(
                        rutaTemporal);
                }
            }

            return Page();
        }

        // Obtener categorías para mostrarlas
        // utilizando los nodos de ListaSimple.
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

            for (int i = 0;
                 i < nivel;
                 i++)
            {
                texto += "    ";
            }

            texto +=
                "- " +
                categoria.Dato.Nombre +
                "\n";

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

        private void EstablecerMensaje(
            string mensaje,
            string tipo)
        {
            Mensaje = mensaje;
            TipoMensaje = tipo;
        }
    }
}