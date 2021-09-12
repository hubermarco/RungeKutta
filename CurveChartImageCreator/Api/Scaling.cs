using System;
using System.Collections.Generic;
using System.Linq;

namespace CurveChartImageCreater
{
    public class Scaling
    {
        public static void Apply(
           IList<double> curve,
           int minExponent,
           int maxExponent,
           ref IList<double> curveOutput,
           ref int scalingExponent)
        {
            int rangeExponent = CalculateRangeExponent(curve);
            int skalingExponentResulting = CalculateResultingSkalingExponent(minExponent, maxExponent, rangeExponent);

            curveOutput = curve?.Select(x => x * Math.Pow(10, skalingExponentResulting)).ToList();
            scalingExponent = -skalingExponentResulting;
        }

        public static void ApplyWithOutput(
           IList<double> curve,
           int minExponent,
           int maxExponent,
           out IList<double> curveOutput,
           out int scalingExponent)
        {
            scalingExponent = 0;
            curveOutput = new List<double>();

            Apply(
                curve,
                minExponent,
                maxExponent,
                ref curveOutput,
                ref scalingExponent);
        }

        public static void Apply(
            IList<double> curve1, 
            IList<double> curve2,
            int minExponent,
            int maxExponent,
            ref IList<double> curveOutput1,
            ref IList<double> curveOutput2,
            ref int scalingExponent)
        {
            var rangeExponent1 = ((curve1 != null) && (curve1.Count > 0)) ? CalculateRangeExponent(curve1) : int.MinValue;
            var rangeExponent2 = ((curve2 != null) && (curve2.Count > 0)) ? CalculateRangeExponent(curve2) : int.MinValue;

            var resultingRangeExponent = (rangeExponent1 > rangeExponent2) ? rangeExponent1 : rangeExponent2;
            var correctedResultingRangeExponent = (resultingRangeExponent == int.MinValue) ? 0 : resultingRangeExponent;
            var skalingExponentResulting = CalculateResultingSkalingExponent(minExponent, maxExponent, correctedResultingRangeExponent);

            curveOutput1 = curve1?.Select(x => x * Math.Pow(10, skalingExponentResulting)).ToList();
            curveOutput2 = curve2?.Select(x => x * Math.Pow(10, skalingExponentResulting)).ToList();
            scalingExponent = -skalingExponentResulting;
        }

        public static void ApplyWithOutput(
            IList<double> curve1,
            IList<double> curve2,
            int minExponent,
            int maxExponent,
            out IList<double> curveOutput1,
            out IList<double> curveOutput2,
            out int scalingExponent)
        {
            scalingExponent = 0;
            curveOutput1 = new List<double>();
            curveOutput2 = new List<double>();

            Apply(
                curve1,
                curve2,
                minExponent,
                maxExponent,
                ref curveOutput1,
                ref curveOutput2,
                ref scalingExponent);
        }

        private static int CalculateRangeExponent(IList<double> curve)
        {
            var range = ( (curve == null) || (curve.Count == 0)) ? 0 : curve.Max() - curve.Min();
            var usedRange = (range != 0) ? range : 1;
            var exponent = Math.Log10(Math.Abs(usedRange));
            var roundedExponent = exponent < 0 ? (int)Math.Floor(exponent) : (int)Math.Floor(exponent);
            return roundedExponent;
        }

        private static int CalculateResultingSkalingExponent(int minExponent, int maxExponent, int rangeExponent)
        {
            return (rangeExponent > 0) ?
                ((minExponent <= rangeExponent) && (rangeExponent <= maxExponent)) ? 0 : maxExponent - rangeExponent:
                ((minExponent <= rangeExponent) && (rangeExponent <= maxExponent)) ? 0 : minExponent - rangeExponent;
        }
    }
}
