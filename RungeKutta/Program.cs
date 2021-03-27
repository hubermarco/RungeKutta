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

            IList<IList<double>> rungekutta4(Func<IList<double>, double, object[], IList<double>> func, IList<double> y0, IList<double> time, params object[] paramList)
            {
                var n = time.Count;
                var y = Enumerable.Repeat(0, n).Select(
                    x => new DoubleVector(Enumerable.Repeat(0.0, y0.Count).ToList())).ToList();

                y[0] = new DoubleVector(y0);

                for(var i = 0; i < n -1; i++)
                {
                    var h = time[i + 1] - time[i];
                    var k1 = new DoubleVector(func(y[i], time[i], paramList));
                    var k2 = new DoubleVector(func(y[i] + ((k1 * h) / 2.0), time[i] + (h / 2.0), paramList));
                    var k3 = new DoubleVector(func(y[i] + ((k2 * h) / 2.0), time[i] + (h / 2.0), paramList));
                    var k4 = new DoubleVector(func(y[i] + (k3 * h), time[i] + h, paramList));
                    y[i + 1] = y[i] + ((k1 + (k2 * 2)) + ((k3 * 2) + k4)) * (h / 6.0);
                }

                return y.Select(x => (IList<double>)x).ToList();
            }

            var T0 = 0.01; // in seconds
            var Tend = 50; // in seconds
            var t = arange(0, Tend, T0);

            var D = 0.25;
            var w0 = 1;
            var a = 2 * D;
            var b = Math.Pow(w0, 2);

            Func<double, double> f = time => time < 0 ? 0 : 1;

            var y_rungekutta = rungekutta4(model, new List<double> { 0, 0 }, t, a, b, f);
        }
    }
}
