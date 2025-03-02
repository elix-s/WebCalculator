namespace CalculatorApp.Commands
{
    using Services;
    
    //command to evaluate an expression using the service ExpressionEvaluator
    public class EvaluateCommand : ICalculatorCommand
    {
        private string _previousState;
        
        public void Execute(Models.CalculatorModel model)
        {
            _previousState = model.Display;
            try
            {
                double result = ExpressionEvaluator.Evaluate(model.Display);
                model.Display = result.ToString();
            }
            catch(Exception ex)
            {
                throw new Exception("Error execution: " + ex.Message);
            }
        }
        
        public void Undo(Models.CalculatorModel model)
        {
            model.Display = _previousState;
        }
    }
}