using CurveChartImageCreater;
using CurveChartImageCreator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RungeKutta
{
    public class DisplayResults
    {
        public static void Apply(IList<double> t, List<double> u, IList<double> y_rk, List<double> delta)
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
