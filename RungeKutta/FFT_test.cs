using MathNet.Numerics.IntegralTransforms;
using System;
using System.Linq;
using System.Numerics;

namespace RungeKutta
{
    class FFT_test
    {
        public static void FFT()
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
