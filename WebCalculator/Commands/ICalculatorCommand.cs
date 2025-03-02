namespace CalculatorApp.Commands
{
    public interface ICalculatorCommand
    {
        void Execute(Models.CalculatorModel model);
        void Undo(Models.CalculatorModel model);
    }
}