using Microsoft.AspNetCore.Mvc;
using CalculatorApp.Models;
using CalculatorApp.Commands;

namespace CalculatorApp.Controllers
{
    public class CalculatorController : Controller
    {
        // history of entered commands
        private static Stack<ICalculatorCommand> _commandHistory = new Stack<ICalculatorCommand>();

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CalculatorModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string btn, CalculatorModel model)
        {
            try
            {
                model.Message = "";
                
                if (btn == "undo")
                {
                    if (_commandHistory.Count > 0)
                    {
                        var lastCommand = _commandHistory.Pop();
                        lastCommand.Undo(model);
                    }
                }
                else if (btn == "=")
                {
                    var evaluateCommand = new EvaluateCommand();
                    evaluateCommand.Execute(model);
                    _commandHistory.Push(evaluateCommand);
                }
                else if (btn == "Cancel")
                {
                    var clearCommand = new ClearCommand();
                    clearCommand.Execute(model);
                    _commandHistory.Push(clearCommand);
                }
                else
                {
                    var inputCommand = new AppendCommand(btn);
                    inputCommand.Execute(model);
                    _commandHistory.Push(inputCommand);
                }
            }
            catch (Exception ex)
            {
                model.Message = ex.Message;
            }
            return View(model);
        }
    }
}