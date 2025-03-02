using System.Globalization;

namespace CalculatorApp.Services
{
    //helper class for solving expressions
    public static class ExpressionEvaluator
    {
        private class OperatorInfo
        {
            public int Precedence { get; set; }
            public bool RightAssociative { get; set; }
            public int Arity { get; set; }
        }
        
        private static Dictionary<string, OperatorInfo> operators = new Dictionary<string, OperatorInfo>
        {
            { "+", new OperatorInfo { Precedence = 1, RightAssociative = false, Arity = 2 } },
            { "-", new OperatorInfo { Precedence = 1, RightAssociative = false, Arity = 2 } },
            { "*", new OperatorInfo { Precedence = 2, RightAssociative = false, Arity = 2 } },
            { "/", new OperatorInfo { Precedence = 2, RightAssociative = false, Arity = 2 } },
            { "^", new OperatorInfo { Precedence = 3, RightAssociative = true, Arity = 2 } },
            { "%", new OperatorInfo { Precedence = 4, RightAssociative = false, Arity = 2 } }, 
            { "√", new OperatorInfo { Precedence = 4, RightAssociative = false, Arity = 1 } }, 
        };
        
        private static double ApplyOperator(string op, double operand1, double operand2 = 0)
        {
            switch(op)
            {
                case "+": return new Operations.AddStrategy().Execute(operand1, operand2);
                case "-": return new Operations.SubtractStrategy().Execute(operand1, operand2);
                case "*": return new Operations.MultiplyStrategy().Execute(operand1, operand2);
                case "/": return new Operations.DivideStrategy().Execute(operand1, operand2);
                case "^": return new Operations.ExponentStrategy().Execute(operand1, operand2);
                case "%": 
                    return new Operations.PercentageStrategy().Execute(operand1, operand2);
                case "√": return new Operations.SquareRootStrategy().Execute(operand1);
                default:
                    throw new Exception("Unknown operator-" + op);
            }
        }
        
        private static List<string> Tokenize(string expression)
        {
            List<string> tokens = new List<string>();
            int i = 0;
            
            while (i < expression.Length)
            {
                char c = expression[i];
                
                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }
                
                if (char.IsDigit(c) || c == '.')
                {
                    string number = "";
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i];
                        i++;
                    }
                    tokens.Add(number);
                    continue;
                }
               
                if (operators.ContainsKey(c.ToString()) || c == '(' || c == ')')
                {
                    tokens.Add(c.ToString());
                    i++;
                    continue;
                }
                
                throw new Exception($"Invalid character: {c}");
            }
            
            return tokens;
        }

        public static double Evaluate(string expression)
        {
            var tokens = Tokenize(expression);
            List<string> outputQueue = new List<string>();
            Stack<string> operatorStack = new Stack<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    outputQueue.Add(token);
                    continue;
                }
                
                if (token == "(")
                {
                    operatorStack.Push(token);
                    continue;
                }
                
                if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Add(operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0)
                        throw new Exception("Mismatched parentheses");
                    operatorStack.Pop(); 
                    continue;
                }
               
                if (operators.ContainsKey(token))
                {
                    if (token == "-" && (i == 0 || (tokens[i-1] != ")" && !double.TryParse(tokens[i-1], NumberStyles.Any, CultureInfo.InvariantCulture, out double _))))
                    {
                        outputQueue.Add("0");
                    }
                    
                    while (operatorStack.Count > 0 && operators.ContainsKey(operatorStack.Peek()))
                    {
                        var topOp = operatorStack.Peek();
                        var currOpInfo = operators[token];
                        var topOpInfo = operators[topOp];

                        if ((!currOpInfo.RightAssociative && currOpInfo.Precedence <= topOpInfo.Precedence) ||
                            (currOpInfo.RightAssociative && currOpInfo.Precedence < topOpInfo.Precedence))
                        {
                            outputQueue.Add(operatorStack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    operatorStack.Push(token);
                    continue;
                }
                throw new Exception("Unknown token: " + token);
            }
            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                if (op == "(" || op == ")")
                    throw new Exception("Mismatched parentheses");
                outputQueue.Add(op);
            }
            
            Stack<double> evaluationStack = new Stack<double>();
            
            foreach (var token in outputQueue)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                {
                    evaluationStack.Push(val);
                    continue;
                }
                
                if (operators.ContainsKey(token))
                {
                    var opInfo = operators[token];
                    if (opInfo.Arity == 1)
                    {
                        if (evaluationStack.Count < 1)
                            throw new Exception("Not enough operators" + token);
                        double operand = evaluationStack.Pop();
                        double result = ApplyOperator(token, operand);
                        evaluationStack.Push(result);
                    }
                    else if (opInfo.Arity == 2)
                    {
                        if (evaluationStack.Count < 2)
                            throw new Exception("Not enough operators" + token);
                        double right = evaluationStack.Pop();
                        double left = evaluationStack.Pop();
                        double result = ApplyOperator(token, left, right);
                        evaluationStack.Push(result);
                    }
                    else
                    {
                        throw new Exception(token);
                    }
                }
                else
                {
                    throw new Exception(token);
                }
            }
            
            if (evaluationStack.Count != 1)
                throw new Exception("Error in expression evaluation.");
            return evaluationStack.Pop();
        }
    }
}