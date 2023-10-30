using Lab3;

namespace Functions
{
    public class Function2 : iFunction
    {
        public double Evaluate(double x)
        {
            return 2 * Math.Pow(x, 2);
        }
    }
}
