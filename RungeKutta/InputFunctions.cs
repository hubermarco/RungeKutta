namespace RungeKutta
{
    public class InputFunctions
    {
        public static double Step(double t)  => t < 0 ? 0 : 1;

        public static double Impulse(double t) => t == 0 ? 1 : 0;
    }
}
