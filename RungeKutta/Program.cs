using CurveChartImageCreater;
using CurveChartImageCreator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            var y_analytical = t.Select(
                time => 1 - Math.Pow(0.77880078307141, time) * Math.Cos(0.96824583655185 * time) -
                0.258198888974715 * Math.Pow(0.77880078307141, time) * Math.Sin(0.96824583655185 * time)).ToList();

            var delta = y_rk.Select((value_rk, index) => (value_rk - y_analytical[index])).ToList();

            DisplayResults(t, u, y_rk, delta);
        }

        private static void DisplayResults(IList<double> t, List<double> u, IList<double> y_rk, List<double> delta)
        {
            Scaling.ApplyWithOutput(
                curve1: y_rk,
                curve2: u,
                maxExponent: 1,
                minExponent: 1,
                curveOutput1: out var y_rk_display,
                curveOutput2: out var u_display,
                scalingExponent: out var scalingExponent);

            Scaling.ApplyWithOutput(
                curve: delta,
                minExponent: 1,
                maxExponent: 1,
                curveOutput: out var delta_in_10_power_minus_10,
                scalingExponent: out var scalingExponentDelta);

            CurveChartImageApi.Create(
                            fileNameWithoutExtention: "RungeKutta",
                            headerCaption: $"HarmonicOsziallor Input and Output in 10^({scalingExponent})",
                            xGrid1: t,
                            xGrid2: t,
                            curveList1: new List<IList<double>> { u_display },
                            curveList2: new List<IList<double>> { y_rk_display },
                            outputDir: "RungeKutta",
                            linearFreqAxis: true
                            );

            CurveChartImageApi.Create(
                fileNameWithoutExtention: "RungeKutta_Delta",
                headerCaption: $"HarmonicOsziallor delta to analytical solution in 10^({scalingExponentDelta})",
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
