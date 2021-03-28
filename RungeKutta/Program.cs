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

            var y_rungekutta = RungeKutta4.Apply(
                Models.HarmonicOsziallator, 
                new List<double> { 0, 0 }, t, a, b, inputFunction);

            var u = t.Select(time => inputFunction(time)).ToList();
            var y = (IList<double>)y_rungekutta.Select(x => x[0]).ToList();

            CurveChartImageApi.Create(
                fileNameWithoutExtention:"RungeKutta",
                headerCaption: "HarmonicOsziallor",
                xGrid1: t,
                xGrid2: t,
                curveList1: new List<IList<double>> { u },
                curveList2: new List<IList<double>> { y },
                outputDir:"RungeKutta",
                linearFreqAxis:true
                );

            CloseInternetExplorer();
            StartInternetExplorerwithPngFile("RungeKutta", "RungeKutta");
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
