using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    class Program
    {
        static void Main(string[] args)
        {
            var T0 = 0.01; // in seconds
            var Tend = 50; // in seconds
            var t = Np.Arange(0, Tend, T0);

            var D = 0.25;
            var w0 = 1;
            var a = 2 * D;
            var b = Math.Pow(w0, 2.0);

            Func<double, double> inputFunction = InputFunctions.Step;

            var u = t.Select(time => inputFunction(time)).ToList();

            var y_rungekutta = RungeKutta4.Apply(
                Models.HarmonicOscillator,
                new List<double> { 0, 0 }, t, a, b, inputFunction);

            var y_rk = (IList<double>)y_rungekutta.Select(x => x[0]).ToList();

            double y_step_analytical(double time) => 1 - Math.Pow(0.77880078307141, time) * Math.Cos(0.96824583655185 * time) -
                0.258198888974715 * Math.Pow(0.77880078307141, time) * Math.Sin(0.96824583655185 * time);

            var y_analytical = t.Select(y_step_analytical).ToList();

            var delta = y_rk.Select((value_rk, index) => (value_rk - y_analytical[index])).ToList();

            DisplayResults.Apply(t, u, y_rk, delta);
        }
    }
}
