using System;
using System.Collections.Generic;

namespace RungeKutta
{
    public class Models
    {
        // ***************************************
        // Model of a harmonic oszillator
        // ***************************************
        // y'' + a*y' + b*y = u(t)
        // y' = v
        // v' =  - b*y - a*v + u(t)
        // State space representation
        //
        //      -------- D -------
        //      |                |
        //      |    X'   X      |
        // u(t)--->B--->S--->C------>y(t)
        //            ^   |
        //            |_A_| X
        //
        // X' = A*X + B*u(t)
        // y(t) = C*X + D*u(t)
        //
        // X' = [0,1;-b,-a]*[y;v]+[0;1]*u(t)
        // y(t)= X*[1;0]
        // A = [0,1;-b,-a]   B=[0;1]   C=[1;0]   D=[0;0]
        public static IList<double> HarmonicOscillator(IList<double> state, double time, object[] paramList)
        {
            var y = state[0];
            var v = state[1];

            var a = (double)paramList[0];
            var b = (double)paramList[1];
            var func = (Func<double, double>)paramList[2];

            var dydt = v;
            var dvdt = -b * y - a * v + func(time);

            return new List<double> { dydt, dvdt };
        }
    }
}
