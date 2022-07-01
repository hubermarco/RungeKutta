
using CurveChartImageCreator;
using System.Collections.Generic;

namespace RungeKutta
{
    public class ShowResults
    {
        public static void Apply(
           IList<double> t,
           IList<double> y_impulse_analytical_curve,
           IList<double> y_rk_impulse,
           IList<double> delta_impulse,
           IList<double> y_rk,
           IList<double> u,
           IList<double> delta_step,
           IList<double> freq,
           IList<double> frequencyResponse
           )
        {
            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
               outputDirCurveChart: "RungeKutta",
               fileNameWithoutExtention: "RungeKuttaImpulse",
               headerCaption: "HarmonicOsziallor Impulse response (red=analytical, black=RungeKutta4)",
               grid1: t,
               grid2: t,
               curve1: y_impulse_analytical_curve,
               curve2: y_rk_impulse,
               linearFreqAxis: true);

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
               outputDirCurveChart: "RungeKutta",
               fileNameWithoutExtention: "RungeKutta_Delta_Impulse",
               headerCaption: "HarmonicOsziallor delta to analytical solution (Impulse)",
               grid: t,
               curve: delta_impulse,
               linearFreqAxis: true);

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
                outputDirCurveChart: "RungeKutta",
                fileNameWithoutExtention: "RungeKuttaStep",
                headerCaption: "HarmonicOsziallor Input(Step) and Output",
                grid1: t,
                grid2: t,
                curve1: y_rk,
                curve2: u,
                linearFreqAxis: true);

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
               outputDirCurveChart: "RungeKutta",
               fileNameWithoutExtention: "RungeKutta_Delta_Step",
               headerCaption: "HarmonicOsziallor delta to analytical solution (Step)",
               grid: t,
               curve: delta_step,
               linearFreqAxis: true);

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
               outputDirCurveChart: "RungeKutta",
               fileNameWithoutExtention: "FFT",
               headerCaption: "HarmonicOsziallor FFT of output",
               grid: freq,
               curve: frequencyResponse,
               linearFreqAxis: false);

            DisplayResults.CloseInternetExplorer();
        }
    }
}
