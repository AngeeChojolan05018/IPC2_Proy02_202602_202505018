
namespace IPC2_Proy02_202602_202505018.TDA
{
    public class Nodo
    {
        public object Dato { get; set; }

        public Nodo? Siguiente { get; set; }

        public Nodo(object dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}