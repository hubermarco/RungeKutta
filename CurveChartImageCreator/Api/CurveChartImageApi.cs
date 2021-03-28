
using System.Collections.Generic;
using System.Linq;

namespace CurveChartImageCreator
{
    public class CurveChartImageApi
    {
        public static void Create(
            string fileNameWithoutExtention,
            string headerCaption,
            IList<double> xGrid1,
            IList<double> xGrid2,
            IEnumerable<IList<double>> curveList1,
            IEnumerable<IList<double>> curveList2,
            string outputDir,
            bool linearFreqAxis = false,
            uint imageWidth = 600,
            uint imageHeight = 400)
        {
            var freqCurveList1 = ConvertCurveListToFreqCrv(curveList1, xGrid1);
            var freqCurveList2 = ConvertCurveListToFreqCrv(curveList2, xGrid2);

            TestCurveChartImage.Create(
                fileNameWithoutExtention: fileNameWithoutExtention,
                headerCaption: headerCaption,
                targetCurves: freqCurveList1,
                simCurves: freqCurveList2,
                outputDir,
                linearFreqAxis,
                imageWidth,
                imageHeight);
        }

        private static IList<FreqCrv> ConvertCurveListToFreqCrv(IEnumerable<IList<double>> curveList, IList<double> xGrid)
        {
            var freqCurveList = (curveList != null) ? new List<FreqCrv>() : null;

            if (curveList != null)
            {
                foreach (var curve in curveList)
                {
                    if( (curve != null) && (curve.Count != 0) )
                    {
                        var freqCrv = new FreqCrv(TCurveType.None);
                        xGrid.Select((x, index) => new FreqPt(x, curve[index])).ToList().ForEach(freqPt => freqCrv.Add(freqPt));
                        freqCurveList.Add(freqCrv);
                    }
                }
            }

            return freqCurveList;
        }
    }
}
