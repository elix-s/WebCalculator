﻿namespace CalculatorApp.Operations
{
    public class SubtractStrategy : IOperationStrategy
    {
        public double Execute(double operand1, double operand2 = 0)
        {
            return operand1 - operand2;
        }
    }
}