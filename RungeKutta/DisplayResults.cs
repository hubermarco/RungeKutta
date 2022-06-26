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
            CloseInternetExplorer();

            ScaledCurveChartImageApi.Create(
                fileNameWithoutExtention: "RungeKutta",
                headerCaption: "HarmonicOsziallor Input and Output",
                grid1: t,
                grid2: t,
                curve1: y_rk,
                curve2: u,
                outputDirCurveChart: "RungeKutta",
                linearFreqAxis: true);

            StartInternetExplorerwithPngFile("RungeKutta", "RungeKutta");

            Console.Write("Press any key");
            Console.ReadKey(true);

            ScaledCurveChartImageApi.Create(
                fileNameWithoutExtention: "RungeKutta_Delta",
                headerCaption: "HarmonicOsziallor delta to analytical solution",
                grid1: null,
                grid2: t,
                curve1: null,
                curve2: delta,
                outputDirCurveChart: "RungeKutta",
                linearFreqAxis: true);

            StartInternetExplorerwithPngFile("RungeKutta", "RungeKutta_Delta");

            Console.Write("Press any key");
            Console.ReadKey(true);
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
