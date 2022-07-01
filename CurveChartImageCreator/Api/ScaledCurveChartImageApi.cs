using System.Collections.Generic;
using System.Linq;

namespace CurveChartImageCreator
{
    public class ScaledCurveChartImageApi
    {
        public static void Create(
            string fileNameWithoutExtention,
            string headerCaption,
            IList<double> grid1,
            IList<double> grid2,
            IList<double> curve1,
            IList<double> curve2,
            string outputDirCurveChart,
            bool linearFreqAxis,
            uint imageWidth = 900,
            uint imageHeight = 600)
        {
            IList<double> GetUsedGrid(bool linFreqAxis, IList<double> grid) =>
                (!linFreqAxis && (grid?.Count > 0) && grid.Any(x => x <= 0)) ? null : grid;

            var usedGrid1 = GetUsedGrid(linearFreqAxis, grid1);
            var usedCurve1 = (usedGrid1 == null) ? null : curve1;
            var usedGrid2 = GetUsedGrid(linearFreqAxis, grid2);
            var usedCurve2 = (usedGrid2 == null) ? null : curve2;

            if ( ((usedGrid1 == null || usedGrid1.Count == 0)) && ((usedGrid2 == null) || (usedGrid2.Count == 0)) )
                headerCaption = "Negative x-values are not allowed in logarithmic scale";

            IList<double> scaledCurve1 = new List<double>();
            IList<double> scaledCurve2 = new List<double>();
            var scalingExponent = 0;
            Scaling.Apply(
                curve1: usedCurve1,
                curve2: usedCurve2,
                minExponent: 1,
                maxExponent: 1,
                curveOutput1: ref scaledCurve1,
                curveOutput2: ref scaledCurve2,
                scalingExponent: ref scalingExponent);

            CurveChartImageApi.Create(
                fileNameWithoutExtention: fileNameWithoutExtention,
                headerCaption: $"{headerCaption}, ScalingFactor=10^({scalingExponent})",
                xGrid1: usedGrid1,
                xGrid2: usedGrid2,
                curveList1: new List<IList<double>> { scaledCurve1 },
                curveList2: new List<IList<double>> { scaledCurve2 },
                outputDir: outputDirCurveChart,
                linearFreqAxis: linearFreqAxis,
                imageWidth: imageWidth,
                imageHeight: imageHeight);
        }
    }
}
