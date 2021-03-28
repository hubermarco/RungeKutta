using System;
using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    public class Np
    {
        public static IList<double> Linspace(double start, double end, int count) =>
                Enumerable.Range(0, count).Select(x => start + x * (end - start) / count).ToList();

        public static IList<double> Arange(double start, double end, double step) =>
            Enumerable.Range(0, (int)Math.Round((end - start) / step)).Select(x => start + x * step).ToList();
    }
}
