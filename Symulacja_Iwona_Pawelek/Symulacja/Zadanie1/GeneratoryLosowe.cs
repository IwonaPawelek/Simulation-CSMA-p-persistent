using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Ten plik zawiera ostateczny generator*/

namespace Zadanie1
{
    public class UniwersalnyGeneratorLosowy
    {
        public UniwersalnyGeneratorLosowy(int kernel)
        {
            uniform_generator_ = new GeneratorRownomierny(kernel);
        }
        public double Rand()
        {
            //assert(uniform_generator_ != nullptr);
            return uniform_generator_.Rand();
        }
        public double Rand(int min, int max)
        {
            //assert(uniform_generator_ != nullptr);
            return uniform_generator_.Rand(min, max);
        }

        public int GetKernel()
        {
            //assert(uniform_generator_ != nullptr);
            return uniform_generator_.get_kernel();
        }
        private	GeneratorRownomierny uniform_generator_;
    }

    public class WykladniczyGeneratorLosowy
    {
        public WykladniczyGeneratorLosowy(double lambda, int kernel)
        {
            exp_generator_ = new GeneratorWykladniczy(lambda, new GeneratorRownomierny(kernel));
        }
        public double Rand()
        {
            //assert(exp_generator_ != nullptr);
            return exp_generator_.Rand();
        }
        private GeneratorWykladniczy exp_generator_;
    }

}