namespace CalculatorApp.Commands
{
    //command that performs the display clearing operation
    public class ClearCommand : ICalculatorCommand
    {
        private string _previousState;

        public void Execute(Models.CalculatorModel model)
        {
            _previousState = model.Display;
            model.Display = "";
        }

        public void Undo(Models.CalculatorModel model)
        {
            model.Display = _previousState;
        }
    }
}