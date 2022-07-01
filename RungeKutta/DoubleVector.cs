using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    internal class DoubleVector : List<double>
    {
        private static double _tolerance = Math.Pow(10, -6);

        public DoubleVector(IEnumerable<double> list)
        {
            AddRange(list);
        }

        public DoubleVector(params double[] paramList)
        {
            AddRange(paramList);
        }

        public override bool Equals(object obj)
        {
            return obj is DoubleVector vector &&
                   Capacity == vector.Capacity &&
                   Count == vector.Count;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static DoubleVector operator +(DoubleVector a, DoubleVector b)
            => new DoubleVector(a.Select((value, index) => value + b[index]).ToList());

        public static DoubleVector operator *(DoubleVector a, double b)
            => new DoubleVector(a.Select(value => value * b).ToList());

        public static DoubleVector operator *(double a, DoubleVector b)
            => new DoubleVector(b.Select(value => value * a).ToList());

        public static DoubleVector operator /(DoubleVector a, double b)
            => new DoubleVector(a.Select(value => value / b).ToList());

        public static bool operator ==(DoubleVector a, DoubleVector b)
            => (a.Count == b.Count) && a.Select((aValue, index) => Math.Abs(aValue - b[index])).All(x => x < _tolerance);

        public static bool operator !=(DoubleVector a, DoubleVector b)
            => (a.Count != b.Count) || a.Select((aValue, index) => Math.Abs(aValue - b[index])).Any(x => x > _tolerance);
    }
}
