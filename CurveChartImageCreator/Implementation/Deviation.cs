

using System;
using System.Collections.Generic;

namespace CurveChartImageCreator
{
    internal class Deviation
    {
        internal static bool IsMaxDeviationOk(IList<FreqCrv> curves1, IList<FreqCrv> curves2, double desiredAccuracy, double lowFrequencyLimit, double highFrequencyLimit, ref double maxDeviation, ref double freqOfmaxDeviation)
        {
            var bOk = (curves1.Count == curves2.Count);
            maxDeviation = 0.0;
            freqOfmaxDeviation = 0.0;

            for (var i = 0; i < curves1.Count && bOk; i++)
            {
                if (curves1[i].Count != curves2[i].Count) bOk = false;

                for (var j = 0; j < curves1[i].Count && bOk; j++)
                {
                    if (curves1[i][j].X >= lowFrequencyLimit && curves1[i][j].X <= highFrequencyLimit)
                    {
                        var absDifference = Math.Abs((curves1[i][j].Y - curves2[i][j].Y));
                        if (absDifference > maxDeviation)
                        {
                            maxDeviation = absDifference;
                            freqOfmaxDeviation = curves1[i][j].X;
                        }
                    }
                }
            }
            if (maxDeviation > desiredAccuracy) bOk = false;
            return bOk;
        }

        internal static bool IsRmsDeviationOk(IList<FreqCrv> curves1, IList<FreqCrv> curves2, double desiredAccuracy, double lowFrequencyLimit, double highFrequencyLimit, ref double maxDeviation)
        {
            var bOk = true;
            maxDeviation = 0.0;

            if (curves1.Count != curves2.Count) bOk = false;

            for (var i = 0; i < curves1.Count && bOk; i++)
            {
                if (curves1[i].Count != curves2[i].Count) bOk = false;
                if (curves1[i].Count == 1) continue;

                var dSum = 0.0;
                double dBandwidth;

                for (var j = 0; j < curves1[i].Count - 1 && bOk; j++)
                {
                    if (curves1[i][j].X >= lowFrequencyLimit && curves1[i][j].X <= highFrequencyLimit)
                    {
                        dBandwidth = curves1[i][j + 1].X - curves1[i][j].X;
                        dSum += dBandwidth *
                                         (curves1[i][j].Y - curves2[i][j].Y) *
                                         (curves1[i][j].Y - curves2[i][j].Y);
                    }
                }

                dBandwidth = curves1[i][curves1[i].Count - 1].X - curves1[i][0].X;
                dSum /= dBandwidth;
                maxDeviation += dSum;
            }

            maxDeviation /= curves1.Count;
            maxDeviation = Math.Sqrt(maxDeviation);

            bOk = bOk && (Math.Abs(maxDeviation) <= desiredAccuracy);
            return bOk;
        }

        internal static bool IsMaxPositiveDeviationOk(FreqCrv curve, IList<FreqCrv> curves2, double desiredAccuracy, double lowFrequencyLimit, double highFrequencyLimit, ref double maxDeviation, ref double freqOfmaxDeviation)
        {
            var bOk = true;
            maxDeviation = 0.0;
            freqOfmaxDeviation = 0.0;

            for (var i = 0; i < curves2.Count && bOk; i++)
            {
                if (curves2[i].Count != curve.Count) bOk = false;

                for (var j = 0; j < curves2[i].Count && bOk; j++)
                {
                    if (curves2[i][j].X >= lowFrequencyLimit && curves2[i][j].X <= highFrequencyLimit)
                    {
                        var difference = curves2[i][j].Y - curve[j].Y;
                        if (difference > maxDeviation)
                        {
                            maxDeviation = difference;
                            freqOfmaxDeviation = curves2[i][j].X;
                        }
                    }
                }
            }
            if (maxDeviation > desiredAccuracy) bOk = false;
            return bOk;
        }

        internal static bool IsRmsPositiveDeviationOk(FreqCrv curve, IList<FreqCrv> curves2, double desiredAccuracy, double lowFrequencyLimit, double highFrequencyLimit, ref double maxDeviation)
        {
            var bOk = true;
            maxDeviation = 0.0;

            for (var i = 0; i < curves2.Count && bOk; i++)
            {
                if (curve.Count != curves2[i].Count) bOk = false;

                var dSum = 0.0;
                double dBandwidth;

                for (var j = 0; j < curves2[i].Count - 1 && bOk; j++)
                {
                    if (curves2[i][j].X >= lowFrequencyLimit && curves2[i][j].X <= highFrequencyLimit && curves2[i][j].Y > curve[j].Y)
                    {
                        dBandwidth = curves2[i][j + 1].X - curves2[i][j].X;
                        dSum += dBandwidth *
                                         (curves2[i][j].Y - curve[j].Y) *
                                         (curves2[i][j].Y - curve[j].Y);
                    }
                }
                dBandwidth = curves2[i][curves2[i].Count - 1].X - curves2[i][0].X;
                dSum /= dBandwidth;
                maxDeviation += dSum;
            }
            maxDeviation /= curves2.Count;
            maxDeviation = Math.Sqrt(maxDeviation);

            bOk = bOk && (Math.Abs(maxDeviation) <= desiredAccuracy);
            return bOk;
        }
    }
}
