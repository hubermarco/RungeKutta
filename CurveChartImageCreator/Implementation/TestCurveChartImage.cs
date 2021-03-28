
using System.Collections.Generic;
using System.IO;

namespace CurveChartImageCreator
{
    internal class TestCurveChartImage
    {
        /// <summary>
        ///  Creates curve chart image in form of a png-file in certain directory (outputDir)
        /// The x-achsis of the curve chart has a logarithmic scale for the frequencies
        /// The y-achsis shows the values from the FreqCrv points in a linear way
        /// The curve chart can be configured in width and hight and a headerCaption can be 
        /// determined
        /// </summary>
        /// <param name="fileNameWithoutExtention">File name without extention. Files are stored as png-images</param>
        /// <param name="headerCaption">camption of the header in image file</param>
        /// <param name="targetCurves">target curve or expected curve for a visual comparison</param>
        /// <param name="simCurves">simulation curves or expected curve to be shown in chart</param>
        /// <param name="outputDir">Directory where curve chart file is created. When path is
        /// not available it is created. It is also possible to set a relativ path in form of a 
        /// folder name. For recommended relative pathes the files are created under
        /// {WorkspacePath}\{Branchname}\output\Fitting\ManagedTests\IntegrationTests\Debug\{outputDir} 
        /// 
        /// e.g. Workspacepath = "W:\Workspace"
        ///      Branchname = "Lemongrass_Dev"
        ///      outputDir = "Fco"
        /// 
        /// -> W:\Workspace\Lemongrass_Dev\output\Fitting\ManagedTests\IntegrationTests\Debug\Fco</param>
        /// <param name="linearFreqAxis">linear frequency axis</param>
        /// <param name="imageWidth">upper frequency limit</param>
        /// <param name="imageHeight">upper frequency limit</param>
        internal static void Create(
             string fileNameWithoutExtention,
             string headerCaption,
             IList<FreqCrv> targetCurves,
             IList<FreqCrv> simCurves,
             string outputDir,
             bool linearFreqAxis = false,
             uint imageWidth = 600,
             uint imageHeight = 400)
        {
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var filePath = Path.Combine(outputDir, fileNameWithoutExtention + ".png");

            GraphicCurve.HeaderCaption = headerCaption;
            GraphicCurve.LinearFreqAxis = linearFreqAxis;

            var fileStream = new FileStream(filePath, FileMode.Create);

            GraphicCurve.WriteFile(
                targetCurves,
                simCurves,
                fileStream,
                imageWidth, 
                imageHeight);

            fileStream.Close();
            fileStream.Dispose();
        }
    }
}