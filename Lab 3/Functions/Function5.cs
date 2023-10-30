using Lab3;

namespace Functions
{
    public class Function5 : iFunction
    {
        public double Evaluate(double x)
        {
            return Math.Pow(x, 3) - 6 * Math.Pow(x, 2) + 11 * x - 6;
        }
    }
}
