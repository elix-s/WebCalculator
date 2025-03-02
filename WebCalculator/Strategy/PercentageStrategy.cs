namespace CalculatorApp.Operations
{
    public class PercentageStrategy : IOperationStrategy
    {
        public double Execute(double operand1, double operand2 = 0)
        {
            // example: 100 % 50  =>  100 * 50 / 100 = 50.
            return operand1 * operand2 / 100;
        }
    }
}