namespace CalculatorApp.Operations
{
    public interface IOperationStrategy
    {
        double Execute(double currentValue, double newValue);
    }
}