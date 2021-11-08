using System;
using System.Threading;

namespace RedNeuronal___Entrrenar_Puntos
{
    class Program
    {
        static void Main(string[] args)
        {

            int n = 2;
            int h = 4;
            int k = 2;

            int MAX_PUNTOS = 15;
            double alfa = 0.1;

            Punto[] puntos = new Punto[MAX_PUNTOS]; //redNeuronal.entrenar(x, claseCorrecta);
            Punto clase1 = new Punto(100,300,1);
            Punto clase2 = new Punto(300,100,3);
            int[] claseCorrecta = new int[2];

            Neurona redNeuronal = new Neurona(n, h, k, (float)alfa);
            float[] x = new float[n];
            float[] y = new float[k];


            while (true)
            {

                for (int i = 0; i < puntos.Length; i++)
                {
                    puntos[i] = new Punto(0, 0);
                }

                redNeuronal.printPesos();
                redNeuronal.entrenar(x, claseCorrecta);
                Console.WriteLine("----");
                redNeuronal.printPesos();

                //Thread.Sleep(80);
                //Console.Clear();
            }
            

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

        public Punto(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public int Tipo { get; set; }
    }

    class Neurona
    {
        //Definir denuronas por capas
        //Capa  de entrada, oculta y de salida
        // variables para cantidad de neuronas según capas
        int ce, co, cs; // ce = capa entrada
            // x, h, y -> x = input, h = capa oculta,  y = capa de salida
        // Establecer pesos para cada conexión -> w = weight o peso
        // wh peso en capa h ->  wh [1,1] , [1,3]
        float[,] wh; float[,] wy;
        // calcular sesgos en h y en y -> bh, bw
        float[] bh; float[] by;
        float[] fh; float[] fy;

        float alfa; //Tasa de aprendizaje
        //double alfa = 0.1;

        int n = 2; // entradas x[i]
        int h = 4; // capa oculta
        int k = 2; // salidas y[i]

        int MAX_PUNTOS = 15;

        float[] x;
        float[] y;

        public Neurona(int n, int h, int k, float tasa)
        {
            ce = n;
            co = h;
            cs = k;

            alfa = tasa;

            wh = new float[n,h];
            wy = new float[h,k];
            bh = new float[h];
            by = new float[k];
            fh = new float[h];
            fy = new float[k];

            // ce co cs
            // ni nh nk

            for (int i = 0; i < ce; i++)
            {
                for (int j = 0; j < co; j++)
                {
                    wh[i,j] = pesoInicial();
                }
            }
            for (int i = 0; i < co; i++)
            {
                for (int j = 0; j < cs; j++)
                {
                    wy[i,j] = pesoInicial();
                }
            }

            for (int i = 0; i < co; i++)
            {
                bh[i] = pesoInicial();
            }
            for (int i = 0; i < cs; i++)
            {
                by[i] = pesoInicial();
            }
        }

        public void printPesos()
        {
            // ce co cs
            // ni nh nk
            for (int i = 0; i < cs; i++)
            {
                for (int j = 0; j < cs; j++)
                {
                    Console.WriteLine($"Pesos CO: {wy[i,j]}");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < co; i++)
            {
                Console.Write($"Sesco CO: {bh[i]} ");
            }
            Console.WriteLine();

            for (int i = 0; i < cs; i++)
            {
                Console.WriteLine($"Sesgo CS: {by[i]}");
            }
            Console.WriteLine();
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

        //Genera un número aleatorio entre -1 y 1
        static float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            // choose -149 instead of -126 to also generate subnormal floats (*)
            double exponent = Math.Pow(2.0, random.Next(0, 1));
            return (float)(mantissa * exponent);
        }

        //Convierte en valor absoluto a un número entre 0 y 1
        public float pesoInicial()
        { 
            Random rnd = new Random();
            float value = Math.Abs( NextFloat(rnd) );
            return value;
        }

        public void clasificar(float[] x)
        {
            float sumatoria;
            for (int i = 0; i < co; i++)
            {
                sumatoria = bh[i];
                for (int j = 0; j < ce; j++)
                {
                    sumatoria += x[j] * wh[j,i];
                }
                fh[i] = f(sumatoria);
            }

            for (int i = 0; i < cs; i++)
            {
                sumatoria = by[i];
                for (int j = 0; j < co; j++)
                {
                    sumatoria += fh[j] * wy[j,i];
                }
                this.fy[i] = f(sumatoria);
            }

            for (int j = 0; j < cs; j++)
            {
                if (this.fy != null && this.y != null)
                {
                    this.y[j] = (float)Math.Round(this.fy[j]);
                }
            }
        }

        public float entrenar(float[] x, int [] aciertos)
        {
            float errorSesgo = 0;
            float[] dh; float[] dy;

            clasificar(x);

            // ce co cs
            // ni nh nk

            dy = new float[k];
            for (int i = 0; i < cs; i++) // cs
            {
                if (/*dy != null && */this.y != null && this.fy != null)
                {
                    float error = aciertos[i] - this.y[i];
                    dy[i] = error * df(this.fy[i]);
                    errorSesgo += error * error;
                }
            }

            dh = new float[h];
            if (dh != null)
            {
                for (int i = 0; i < co; i++) // co
                {
                    //Console.WriteLine(co);
                    float error = 0;
                    for (int j = 0; j < cs; j++)
                    {
                        if (this.wy != null && dy != null)
                        {
                            //Console.WriteLine(cs);
                            error += dy[j] * wy[i, j];
                        }
                        dh[i] = error * df(fh[i]);
                    }
                }
            }

            for (int i = 0; i < cs; i++)// pesos wy
            {
                by[i] += dy[i] * alfa;
                for (int j = 0; j < co; j++)
                {
                    wy[j,i] += fh[j] * dy[i] * alfa;
                }
            }

            for (int i = 0; i < cs; i++)// pesos wh
            {
                Console.WriteLine($"Antes i={i}");
                bh[i] += dh[i] * alfa;
                for (int j = 0; j < ce; k++)
                {
                    //wh[j,i] += x[j] * dh[i] * alfa;
                    Console.WriteLine($"Durante i={i}, j={j}");
                }
                Console.WriteLine($"Después i={i}");
            }

            return (float)Math.Sqrt(errorSesgo);
        }
    }
}