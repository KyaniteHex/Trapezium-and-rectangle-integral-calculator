using Lab3;

namespace Functions
{
    public class Function4 : iFunction
    {
        public double Evaluate(double x)
        {
            return 2 * Math.Pow(x, 3) - 3 * Math.Pow(x, 2) + 4 * x - 1;
        }
    }
}
