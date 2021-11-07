using System;

namespace RedNeuronal___Entrrenar_Puntos
{
    class Program
    {
        static void Main(string[] args)
        {
            //testing
            Punto p = new Punto(1.3, 1.14, 0);
            Console.WriteLine($"x = {p.X}, y = {p.Y}, Tipo: {p.Tipo}");
        }
    }

    class Punto
    {       
        public Punto(double x, double y, int tipo)
        {
            X = x;
            Y = y;
            Tipo = tipo;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public int Tipo { get; set; }
    }

    class Neurona
    {
        public Neurona()
        {
            //Definir denuronas por capas
            //Capa  de entrada, oculta y de salida
            // variables para cantidad de neuronas según capas
            int ce, co, cs; // ce = capa entrada
              // x, h, y -> x = input, h = capa oculta,  y = capa de salida
            // Establecer pesos para cada conexión -> w = weight o peso
            // wh peso en capa h ->  wh [1,1] , [1,3]

            // calcular sesgos en h y en y -> bh, bw

        }

        //Función euler
        public float f(float suma)
        {
            return (float)(1 / ( 1 + Math.Exp(-suma) ));
        }

        //Función diferencial
        public float df(float x)
        {
            return f(x) * ( 1 - f(x) );
        }
    }
}
