using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    public class RungeKutta4
    {
        public static IList<IList<double>> Apply(Func<IList<double>, double, object[], IList<double>> func, IList<double> y0, IList<double> t, params object[] paramList)
        {
            var n = t.Count;
            var y = Enumerable.Repeat(0, n).Select(
                x => new DoubleVector(Enumerable.Repeat(0.0, y0.Count).ToList())).ToList();

            y[0] = new DoubleVector(y0);

            for (var i = 0; i < n - 1; i++)
            {
                var h = t[i + 1] - t[i];
                var k1 = new DoubleVector(func(y[i], t[i], paramList));
                var k2 = new DoubleVector(func(y[i] + k1 * h / 2.0, t[i] + h / 2.0, paramList));
                var k3 = new DoubleVector(func(y[i] + k2 * h / 2.0, t[i] + h / 2.0, paramList));
                var k4 = new DoubleVector(func(y[i] + k3 * h, t[i] + h, paramList));
                y[i + 1] = y[i] + (k1 + k2 * 2 + k3 * 2 + k4) * (h / 6.0);
            }

            return y.Select(x => (IList<double>)x).ToList();
        }
    }
}
