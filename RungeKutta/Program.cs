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

            var y_rungekutta = RungeKutta4.Apply(Models.HarmonicOsziallator, new List<double> { 0, 0 }, t, a, b, inputFunction);
        }
    }
}
