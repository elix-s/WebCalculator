namespace CalculatorApp.Commands
{
    //command to add a symbol to the entered expression
    public class AppendCommand : ICalculatorCommand
    {
        private string _appendValue;
        private string _previousState;

        public AppendCommand(string value)
        {
            _appendValue = value;
        }

        public void Execute(Models.CalculatorModel model)
        {
            _previousState = model.Display;
            model.Display += _appendValue;
        }

        public void Undo(Models.CalculatorModel model)
        {
            model.Display = _previousState;
        }
    }
}