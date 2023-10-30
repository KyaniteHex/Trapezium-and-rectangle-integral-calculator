using Lab3;

namespace Functions
{
    public class Function6 : iFunction
    {
        public double Evaluate(double x)
        {
            return 3 * Math.Pow(x, 3) - 5 * Math.Pow(x, 2) + 2 * x + 8;
        }
    }
}
