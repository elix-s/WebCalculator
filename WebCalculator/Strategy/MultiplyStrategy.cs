namespace CalculatorApp.Operations
{
    public class MultiplyStrategy : IOperationStrategy
    {
        public double Execute(double operand1, double operand2 = 0)
        {
            return operand1 * operand2;
        }
    }
}