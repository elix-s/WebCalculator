namespace CalculatorApp.Operations
{
    public class DivideStrategy : IOperationStrategy
    {
        public double Execute(double operand1, double operand2 = 0)
        {
            if (operand2 == 0)
                throw new DivideByZeroException("Division by zero is not allowed.");
            return operand1 / operand2;
        }
    }
}