using Lab3;

namespace Functions
{
    public class Function1 : iFunction
    {
        public double Evaluate(double x)
        {
            return 2 * x + (2 * Math.Pow(x, 2));
        }
    } 
}

