﻿using CurveChartImageCreator;
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

            double y_impulse_analytical(double time) => 1.0327955589886 * Math.Exp(-time / 4) * Math.Sin(0.96824583655185 * time);

            double y_step_analytical(double time) => 1 - Math.Pow(0.77880078307141, time) * Math.Cos(0.96824583655185 * time) -
                0.258198888974715 * Math.Pow(0.77880078307141, time) * Math.Sin(0.96824583655185 * time);

            var y_impulse_analytical_curve = t.Select(y_impulse_analytical).ToList();
            var y_step_analytical_curve = t.Select(y_step_analytical).ToList();

            // ########################### Time Domain ###############################################################

            var y_rungekutta = RungeKutta4.Apply(
                Models.HarmonicOscillator,
                y0: new List<double> { 0, 0 }, t, a, b, inputFunction);

            var y_rk = y_rungekutta.Select(x => x[0]).ToList();

           
            var delta = y_rk.Select((value_rk, index) => (value_rk - y_step_analytical_curve[index])).ToList();

            // ########################### Frequency Domain #############################################################

            var freq = Np.Linspace(0, f0, t.Count).Where((x, index) => ((0 < index) && (index <= t.Count / 2))).ToList();
            var input = y_impulse_analytical_curve.Select(x => new Complex(x, 0)).ToArray();
            Fourier.Forward(input, FourierOptions.NoScaling);
            var output = input.Select(x => x.Magnitude * T0).Where((x, index) => ((0 < index) && (index <= input.Length / 2))).ToList();

            FFT();

            // ########################### Display Results ##############################################################

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
                outputDirCurveChart: "RungeKutta",
                fileNameWithoutExtention: "RungeKutta",
                headerCaption: "HarmonicOsziallor Input and Output",
                grid1: t,
                grid2: t,
                curve1: y_rk,
                curve2: u,
                linearFreqAxis: true);

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
               outputDirCurveChart: "RungeKutta",
               fileNameWithoutExtention: "RungeKutta_Delta",
               headerCaption: "HarmonicOsziallor delta to analytical solution",
               grid1: null,
               grid2: t,
               curve1: null,
               curve2: delta,
               linearFreqAxis: true);

            DisplayResults.CreateCurveChartAndShowItWithInternetExplorer(
               outputDirCurveChart: "RungeKutta",
               fileNameWithoutExtention: "FFT",
               headerCaption: "HarmonicOsziallor FFT of output",
               grid1: null,
               grid2: freq,
               curve1: null,
               curve2: output,
               linearFreqAxis: false);
        }

        private static void FFT()
        {
            var T = 1; // period duration in seconds
            var fs = 100; // sample frequency in Hz
            var ts = 1.0 / fs; // sample time in seconds

            // discret time samples in seconds
            var t_discrete = Np.Arange(0, T, ts);

            var f = 2;

            var xd = t_discrete.Select(t => Math.Sin(2 * Math.PI * f * t)).ToList();

            var inputComplex = xd.Select(x => new Complex(x, 0)).ToArray();

            var Xd = inputComplex.Select(x => x).ToArray();

            Fourier.Forward(Xd, FourierOptions.NoScaling);

            var Xd_Abs = Xd.Select(x => x.Magnitude * ts).ToArray();
        }
    }
}
