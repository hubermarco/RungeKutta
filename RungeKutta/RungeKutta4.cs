using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    public class RungeKutta4
    {
        public static IList<IList<double>> Apply(Func<IList<double>, double, object[], IList<double>> func, IList<double> y0, IList<double> time, params object[] paramList)
        {
            var n = time.Count;
            var y = Enumerable.Repeat(0, n).Select(
                x => new DoubleVector(Enumerable.Repeat(0.0, y0.Count).ToList())).ToList();

            y[0] = new DoubleVector(y0);

            for (var i = 0; i < n - 1; i++)
            {
                var h = time[i + 1] - time[i];
                var k1 = new DoubleVector(func(y[i], time[i], paramList));
                var k2 = new DoubleVector(func(y[i] + k1 * h / 2.0, time[i] + h / 2.0, paramList));
                var k3 = new DoubleVector(func(y[i] + k2 * h / 2.0, time[i] + h / 2.0, paramList));
                var k4 = new DoubleVector(func(y[i] + k3 * h, time[i] + h, paramList));
                y[i + 1] = y[i] + (k1 + k2 * 2 + k3 * 2 + k4) * (h / 6.0);
            }

            return y.Select(x => (IList<double>)x).ToList();
        }

        private class DoubleVector : List<double>
        {
            public DoubleVector(IEnumerable<double> list)
            {
                AddRange(list);
            }

            public DoubleVector(params double[] paramList)
            {
                AddRange(paramList);
            }

            public static DoubleVector operator +(DoubleVector a, DoubleVector b)
                => new DoubleVector(a.Select((value, index) => value + b[index]).ToList());

            public static DoubleVector operator *(DoubleVector a, double b)
                => new DoubleVector(a.Select((value, index) => value * b).ToList());

            public static DoubleVector operator /(DoubleVector a, double b)
                => new DoubleVector(a.Select((value, index) => value / b).ToList());
        }
    }
}
