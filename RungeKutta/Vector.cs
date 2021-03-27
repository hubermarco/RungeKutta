using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    public class DoubleVector : List<double>
    {
        public DoubleVector(IEnumerable<double> list)
        {
            AddRange(list);
        }

        public static DoubleVector operator +(DoubleVector a, DoubleVector b)
            => new DoubleVector(a.Select((value, index) => value + b[index]).ToList());

        public static DoubleVector operator *(DoubleVector a, double b)
            => new DoubleVector(a.Select((value, index) => value * b).ToList());

        public static DoubleVector operator /(DoubleVector a, double b)
            => new DoubleVector(a.Select((value, index) => value / b).ToList());
    }
}
