using MathNet.Numerics.IntegralTransforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RungeKutta
{
    class Program
    {
        static void Main(string[] args)
        {
            // Simulation of a damped harmonic oscillator:
            // y'' + 2*D*y' + w0^2*y = u(t)
            // y'' + a*y' + b*y = u(t)

            // ###########################  Preparation of Inputs ##############################################

            var T0 = 0.01; // in seconds
            var f0 = 1 / T0;
            var Tend = 50; // in seconds
            var t = Np.Arange(0, Tend, T0);

            var D = 0.25;
            var w0 = 1;
            var a = 2 * D;
            var b = Math.Pow(w0, 2.0);

            Func<double, double> inputFunction = InputFunctions.Step;
            var u = t.Select(time => inputFunction(time)).ToList();

            // ########################### Analytical Results #########################################################

            var y_impulse_analytical_curve = t.Select(AnalyticalResults.Y_impulse).ToList();
            var y_step_analytical_curve = t.Select(AnalyticalResults.Y_step).ToList();

            // ########################### Time Domain ###############################################################

            var y_rungekutta = RungeKutta4.Apply(
                Models.HarmonicOscillator,
                y0: new List<double> { 0, 0 }, t, a, b, inputFunction);

            // Step response
            var y_rk_step = y_rungekutta.Select(x => x[0]).ToList();
            var delta_step = y_rk_step.Select((value_rk, index) => (value_rk - y_step_analytical_curve[index])).ToList();

            // Impulse response (differentiate y_rk to calculate impulse response)
            var y_rk_impulse = DiscreteCalculus.Differentiate(y_rk_step, T0);
            var delta_impulse = y_rk_impulse.Select((value_rk, index) => (value_rk - y_impulse_analytical_curve[index])).ToList();

            var y_rk_step_integrated = DiscreteCalculus.Integrate(y_rk_impulse, T0);
       
            // ########################### Frequency Domain #############################################################

            var freq = Np.Linspace(0, f0, t.Count).Where((x, index) => ((0 < index) && (index <= t.Count / 2))).ToList();
            var input = y_impulse_analytical_curve.Select(x => new Complex(x, 0)).ToArray();
            Fourier.Forward(input, FourierOptions.NoScaling);
            var frequencyResponse = input.Select(x => x.Magnitude * T0).Where((x, index) => ((0 < index) && (index <= input.Length / 2))).ToList();

            FFT_test.FFT();

            // ########################### Display Results ##############################################################

            ShowResults.Apply(
                t, 
                y_impulse_analytical_curve, 
                y_rk_impulse, 
                delta_impulse, 
                y_rk_step, 
                u, 
                delta_step, 
                freq, 
                frequencyResponse);
        }
    }
}
