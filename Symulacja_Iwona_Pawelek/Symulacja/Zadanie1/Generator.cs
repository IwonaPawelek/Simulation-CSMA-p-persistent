using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Zadanie1
{
    //Klasa odpowiedzialna za generowanie ziaren
    static class Ziarno
    {
        public static StreamReader sr;

        public static void Generuj()
        {
            int liczbaZiaren = 200;
            int odlegloscZiaren = 100000;
            UniwersalnyGeneratorLosowy generatorLosowy = new UniwersalnyGeneratorLosowy(1);
            StreamWriter ziarna = new StreamWriter("ziarna.txt");

            for (var i = 0; i < liczbaZiaren; ++i)
            {
                string zapisz = "";
                for (int k = 0; k < odlegloscZiaren; ++k) generatorLosowy.Rand();
                zapisz = generatorLosowy.GetKernel().ToString();
                ziarna.WriteLine(zapisz);
            }
            ziarna.Close();
            Ziarno.sr = new StreamReader("ziarna.txt");
        }

        public static int PobierzZiarno()
        {
            int ziarno = int.Parse(Ziarno.sr.ReadLine());
            return ziarno;
        }
    }
    public class GeneratorRownomierny
    {
        public GeneratorRownomierny(int kernel)
        {
            kernel_ = kernel;
            M = 2147483647.0;
            A = 16807;
            Q = 127773;
            R = 2836;
        }

        // Draws number between <0,1>
        public double Rand()
        {
            int h = kernel_ / Q;
            kernel_ = A * (kernel_ - Q * h) - R * h;
            if (kernel_ < 0)
                kernel_ = kernel_ + (int)M;
            return kernel_ / M;
        }
        // Draws number between <start, end>
        public double Rand(int start, int end)
        {
            return Rand() * (end - start) + start;
        }

        public int get_kernel() { return kernel_; }

	    private int kernel_;
        private double M;
        private int A;
        private int Q;
        private int R;
    }

    public class GeneratorWykladniczy
    {
        public GeneratorWykladniczy(double lambda, GeneratorRownomierny gr)
        {
            lambda_ = lambda;
            rownomierny_ = gr;
        }

        public double Rand()
        {
            double k = rownomierny_.Rand();
            return -(1.0 / lambda_) * Math.Log(k);
        }

	    private double lambda_;
        GeneratorRownomierny rownomierny_;
    }
}