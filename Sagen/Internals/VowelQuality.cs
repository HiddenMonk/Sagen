using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagen.Internals
{
    internal struct VowelQuality
    {
        public double Height;
        public double Backness;
        public double Roundedness;
        public double F1;
        public double F2;
        public double F3;

        public VowelQuality(double h, double b, double r, double f1, double f2, double f3)
        {
            Height = h;
            Backness = b;
            Roundedness = r;
            F1 = f1;
            F2 = f2;
            F3 = f3;
        }
    }
}
