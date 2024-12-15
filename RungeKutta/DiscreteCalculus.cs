using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RungeKutta
{
    public class DiscreteCalculus
    {
        public static IList<double> Differentiate(IList<double> input, double T0) =>
            input.Select((value, index) => (index == 0) ? 0 : (value - input[index - 1]) / T0).ToList();

        public static IList<double> Integrate(IList<double> input, double T0)
        {
            var y_1 = 0.0;
            return input.Select((value, i) =>
            {
                var y_0 = (i == 0) ? 0 : (y_1 + input[i - 1] * T0);
                y_1 = y_0;
                return y_0;
            }).ToList();
        }

        public static Complex[] DFT(double[] x)
        {
            var j = new Complex(0, 1);
            var N = x.Length;
            var X = Enumerable.Repeat(new Complex(0, 0), N).ToList();

            for (var n = 0; n < N; n++)
                for (var k = 0; k < N; k++)
                    X[n] += x[k] * Complex.Exp(-j * 2 * Math.PI * (n * 1.0 / N) * k);

            return X.ToArray();
        }
    }
}
