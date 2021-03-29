using CurveChartImageCreator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

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

            var y_rungekutta = RungeKutta4.Apply(
                Models.HarmonicOsziallator,
                new List<double> { 0, 0 }, t, a, b, inputFunction);

            var u = t.Select(time => inputFunction(time)).ToList();
            var y_rk = (IList<double>)y_rungekutta.Select(x => x[0]).ToList();

            var y_analytical = t.Select(
                time => 1 - Math.Pow(0.77880078307141, time) * Math.Cos(0.96824583655185 * time) -
                0.258198888974715 * Math.Pow(0.77880078307141, time) * Math.Sin(0.96824583655185 * time)).ToList();

            var delta_in_10_power_minus_10 = y_rk.Select((value_rk, index) => (value_rk - y_analytical[index]) * Math.Pow(10, 10)).ToList();
            var y_rk_display = y_rk.Select(value => value * 10).ToList();
            var u_display = u.Select(value => value * 10).ToList();

            DisplayResults(t, u_display, y_rk_display, delta_in_10_power_minus_10);
        }

        private static void DisplayResults(IList<double> t, List<double> u, IList<double> y_rk, List<double> delta_in_10_power_minus_10)
        {
            CurveChartImageApi.Create(
                            fileNameWithoutExtention: "RungeKutta",
                            headerCaption: "HarmonicOsziallor Input and Output in 10^(-1)",
                            xGrid1: t,
                            xGrid2: t,
                            curveList1: new List<IList<double>> { u },
                            curveList2: new List<IList<double>> { y_rk },
                            outputDir: "RungeKutta",
                            linearFreqAxis: true
                            );

            CurveChartImageApi.Create(
                fileNameWithoutExtention: "RungeKutta_Delta",
                headerCaption: "HarmonicOsziallor delta to analytical solution in 10^(-10)",
                xGrid1: null,
                xGrid2: t,
                curveList1: null,
                curveList2: new List<IList<double>> { delta_in_10_power_minus_10 },
                outputDir: "RungeKutta",
                linearFreqAxis: true
                );

            CloseInternetExplorer();
            StartInternetExplorerwithPngFile("RungeKutta", "RungeKutta");

            Console.Write("Press any key");
            Console.ReadKey(true);

            StartInternetExplorerwithPngFile("RungeKutta", "RungeKutta_Delta");
        }

        private static void StartInternetExplorerwithPngFile(string outputFolderName, string fileNameWithoutExtention)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var outputDir = Path.Combine(currentDirectory, outputFolderName);
            var filePath = Path.Combine(outputDir, fileNameWithoutExtention + ".png");
            Process.Start("IExplore.exe", filePath);
        }

        private static void CloseInternetExplorer()
        {
            Process[] ps = Process.GetProcessesByName("IEXPLORE");

            foreach (Process p in ps)
            {
                p.Kill();
            }
        }
    }
}
