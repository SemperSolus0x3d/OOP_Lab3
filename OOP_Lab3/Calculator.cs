using System;

namespace OOP_Lab3
{
    public class Calculator
    {
        // Сложение
        public decimal Add(decimal a, decimal b)
        {
            return a + b;
        }

        // Вычитание
        public decimal Substract(decimal a, decimal b)
        {
            return a - b;
        }

        // Умножение
        public decimal Multiply(decimal a, decimal b)
        {
            return a * b;
        }

        // Вычислить распарсенное выражение
        public decimal EvaluateExpression(ParsedExpression expression)
        {
            switch (expression.Operation)
            {
                case Operation.Addition:
                    return Add(
                        expression.LeftOperand,
                        expression.RightOperand
                    );

                case Operation.Substraction:
                    return Substract(
                        expression.LeftOperand,
                        expression.RightOperand
                    );

                case Operation.Multiplication:
                    return Multiply(
                        expression.LeftOperand,
                        expression.RightOperand
                    );

                default:
                    throw new Exception("Неизвестная операция");
            }
        }
    }
}
