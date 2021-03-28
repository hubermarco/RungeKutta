using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<double> linspace(double start, double end, int count) =>
                Enumerable.Range(0, count).Select(x => start + x * (end - start) / count).ToList();

            IList<double> arange(double start, double end, double step) =>
                Enumerable.Range(0, (int)Math.Round((end - start) / step)).Select(x => start + x * step).ToList();

            IList<double> model(IList<double> state, double time, params object[] paramList)
            {
                var y = state[0];
                var v = state[1];

                var aPar = (double)paramList[0];
                var bPar = (double)paramList[1];
                Func<double, double> func = (Func<double, double>)paramList[2];

                var dydt = v;
                var dvdt = -bPar * y - aPar * v + func(time);

                return new List<double> { dydt, dvdt };
            }

            var T0 = 0.01; // in seconds
            var Tend = 50; // in seconds
            var t = arange(0, Tend, T0);

            var D = 0.25;
            var w0 = 1;
            var a = 2 * D;
            var b = Math.Pow(w0, 2);

            Func<double, double> f = time => time < 0 ? 0 : 1;

            var y_rungekutta = RungeKutta4.Apply(model, new List<double> { 0, 0 }, t, a, b, f);
        }
    }
}
