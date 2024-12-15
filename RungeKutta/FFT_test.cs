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
            var f0 = 100; // sample frequency in Hz
            var T0 = 1.0 / f0; // sample time in seconds

            // discret time samples in seconds
            var t_discrete = Np.Arange(0, T, T0);

            var f = 2;

            var xd = t_discrete.Select(t => Math.Sin(2 * Math.PI * f * t)).ToArray();

            var inputComplex = xd.Select(x => new Complex(x, 0)).ToArray();

            var Xd = inputComplex.Select(x => x).ToArray();
            Fourier.Forward(Xd, FourierOptions.NoScaling);
            var Xd_Abs = Xd.Select(x => x.Magnitude * T0).ToArray();

            var Xd2 = DiscreteCalculus.DFT(xd);
            var Xd_Abs2 = Xd2.Select(x => x.Magnitude * T0).ToArray();

            var deltaMax = Xd_Abs.Select((value, index) => Math.Abs(Xd_Abs2[index] - value)).Max();
        }
    }
}
