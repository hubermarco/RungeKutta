using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CurveChartImageCreator
{
    public class DisplayResults
    {
        public static void CreateCurveChartAndShowItWithInternetExplorer(
            string outputDirCurveChart,
            string fileNameWithoutExtention,
            string headerCaption,
            IList<double> grid1, 
            IList<double> curve1,
            IList<double> grid2,
            IList<double> curve2,
            bool linearFreqAxis,
            uint imageWidth = 600,
            uint imageHeight = 450)
        {
            ScaledCurveChartImageApi.Create(
                fileNameWithoutExtention: fileNameWithoutExtention,
                headerCaption: headerCaption,
                grid1: grid1,
                grid2: grid2,
                curve1: curve1,
                curve2: curve2,
                outputDirCurveChart: outputDirCurveChart,
                linearFreqAxis: linearFreqAxis,
                imageWidth: imageWidth,
                imageHeight: imageHeight);

            StartInternetExplorerwithPngFile(outputDirCurveChart, fileNameWithoutExtention);

            Console.Write("Press any key");
            Console.ReadKey(true);
        }

        public static void CloseInternetExplorer()
        {
            Process[] ps = Process.GetProcessesByName("IEXPLORE");

            foreach (Process p in ps)
            {
                p.Kill();
            }
        }

        private static void StartInternetExplorerwithPngFile(string outputFolderName, string fileNameWithoutExtention)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var outputDir = Path.Combine(currentDirectory, outputFolderName);
            var filePath = Path.Combine(outputDir, fileNameWithoutExtention + ".png");
            Process.Start("IExplore.exe", filePath);
        }
    }
}
