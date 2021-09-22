using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OOP_Lab3
{
    public class Calculator
    {
        // Распарсить выражение и вычислить результат
        public double ParseAndEvaluate(string expression, out bool needMoreInput)
        {
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            expression = expression.Replace(".", ",");

            char[] operations = new char[] { '+', '-', '*' };

            string lexem = "";

            foreach (char ch in expression)
            {
                if (char.IsDigit(ch) || ch == ',')
                    lexem += ch;
                else if (operations.Contains(ch))
                {
                    if (state == State.Initial)
                        leftOperand = double.Parse(lexem);

                    operation = ch;

                    lexem = "";
                    state = State.OperationParsed;
                }
            }

            if (state == State.Initial)
            {
                leftOperand = double.Parse(lexem);
                state = State.LeftOperandParsed;
            }
            else if (state == State.OperationParsed &&
                     lexem != "")
            {
                rightOperand = double.Parse(lexem);
                state = State.RightOperandParsed;
            }

            needMoreInput = state != State.RightOperandParsed;

            if (needMoreInput)
                return 0;

            // Стоило бы это делать внутри
            // switch'а, но я не хотел
            // копировать эту строчку
            state = State.Initial;

            switch (operation)
            {
                case '+': return Add(leftOperand, rightOperand);
                case '-': return Substract(leftOperand, rightOperand);
                case '*': return Multiply(leftOperand, rightOperand);
            }

            // Эта строчка вообще никогда
            // не должна исполняться
            throw new Exception();
        }


        private enum State
        {
            Initial,
            LeftOperandParsed,
            OperationParsed,
            RightOperandParsed
        };

        private State state = State.Initial;

        private double leftOperand = 0;
        private char operation = '\0';
        private double rightOperand = 0;

        // Сложение
        private double Add(double a, double b)
        {
            return a + b;
        }

        // Вычитание
        private double Substract(double a, double b)
        {
            return a - b;
        }

        // Умножение
        private double Multiply(double a, double b)
        {
            return a * b;
        }
    }
}
