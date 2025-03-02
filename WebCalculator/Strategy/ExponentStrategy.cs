namespace CalculatorApp.Operations
{
    public class ExponentStrategy : IOperationStrategy
    {
        public double Execute(double operand1, double operand2 = 0)
        {
            return Math.Pow(operand1, operand2);
        }
    }
}