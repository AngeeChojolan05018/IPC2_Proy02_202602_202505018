using IPC2_Proy02_202602_202505018.Modelo;

namespace IPC2_Proy02_202602_202505018.TDA
{
    public class NodoISBN
    {
        public Libro Dato { get; set; }

        public NodoISBN? Izquierdo { get; set; }

        public NodoISBN? Derecho { get; set; }

        public NodoISBN(Libro dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
        }
    }
}