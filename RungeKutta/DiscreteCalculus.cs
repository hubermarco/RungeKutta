using System.Collections.Generic;
using System.Linq;

namespace RungeKutta
{
    public class DiscreteCalculus
    {
        public static IList<double> Differentiate(IList<double> input, double T0 ) =>
            input.Select((value, index) => (index == 0) ? 0 : (value - input[index - 1]) / T0).ToList();

        public static IList<double> Integrate(IList<double> input, double T0)
        {
            var y_1 = 0.0;
            return input.Select((value, i) =>
            {
                var y_0 = (i == 0) ? 0 : (y_1 + input[i - 1] * T0);
                y_1 = y_0;
                return y_0;
            }).ToList();
        }  
    }
}
