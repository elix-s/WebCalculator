namespace CalculatorApp.Operations
{
    public class SquareRootStrategy : IOperationStrategy
    {
        public double Execute(double operand1, double operand2 = 0)
        {
            if (operand1 < 0)
                throw new ArgumentException("It is impossible to calculate the square root of a negative number.");
            return Math.Sqrt(operand1);
        }
    }
}