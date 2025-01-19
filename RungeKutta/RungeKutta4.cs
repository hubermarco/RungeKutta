using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    public class RungeKutta4
    {
        /// <summary>
        /// Applies the Runge-Kutta 4th order method to solve ODEs.
        /// </summary>
        /// <param name="model">The model function representing the ODE system.</param>
        /// <param name="y0">The initial state vector.</param>
        /// <param name="t">The time steps.</param>
        /// <param name="paramList">Additional parameters for the model function.</param>
        /// <returns>The state vectors at each time step.</returns>
        public static IList<IList<double>> Apply(
            Func<IList<double>, double, object[], IList<double>> model, // INPUT: State, t, paramList, OUTPUT: dState/dt
            IList<double> y0, 
            IList<double> t, 
            params object[] paramList)
        {
            var n = t.Count;
            var y = Enumerable.Repeat(0, n).Select(
                x => new DoubleVector(Enumerable.Repeat(0.0, y0.Count).ToList())).ToList();

            y[0] = new DoubleVector(y0);

            for (var i = 0; i < n - 1; i++)
            {
                var h = t[i + 1] - t[i];
                var k1 = new DoubleVector(model(y[i], t[i], paramList));
                var k2 = new DoubleVector(model(y[i] + k1 * h / 2.0, t[i] + h / 2.0, paramList));
                var k3 = new DoubleVector(model(y[i] + k2 * h / 2.0, t[i] + h / 2.0, paramList));
                var k4 = new DoubleVector(model(y[i] + k3 * h, t[i] + h, paramList));
                y[i + 1] = y[i] + (h / 6.0) * (k1 + 2*k2 + 2*k3 + k4);
            }

            return y.Select(x => (IList<double>)x).ToList();
        }
    }
}
