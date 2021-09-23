using System;
using System.Linq;

namespace OOP_Lab3
{
    public class Calculator
    {
        // Распарсить выражение и вычислить результат
        public decimal ParseAndEvaluate(string expression, out bool needMoreInput)
        {
            // Сохранить состояние калькулятора
            // на случай неверных входных данных
            State initialState = state;
            decimal initialLeftOperand = leftOperand;
            char initialOperation = operation;
            decimal initialRightOperand = rightOperand;

            try
            {
                expression = expression
                    .Replace(" ", "")
                    .Replace("=", "")
                    .Replace(',', '.');

                char[] operations = new char[] { '+', '-', '*' };

                string lexem = "";

                foreach (char ch in expression)
                {
                    if (char.IsDigit(ch) || ch == '.')
                        lexem += ch;
                    else if (operations.Contains(ch))
                    {
                        if (state >= State.OperationParsed)
                            throw new Exception(
                                "В строке встречено несколько знаков операции"
                            );

                        if (state == State.Initial)
                            leftOperand = ParseDecimal(lexem);

                        operation = ch;

                        lexem = "";
                        state = State.OperationParsed;
                    }
                }

                if (state == State.Initial && lexem != "")
                {
                    leftOperand = ParseDecimal(lexem);
                    state = State.LeftOperandParsed;
                }
                else if (state == State.OperationParsed &&
                         lexem != "")
                {
                    rightOperand = ParseDecimal(lexem);
                    state = State.RightOperandParsed;
                }

                needMoreInput = state != State.RightOperandParsed;

                if (needMoreInput)
                    return 0;

                state = State.Initial;

                switch (operation)
                {
                    case '+': return Add(leftOperand, rightOperand);
                    case '-': return Substract(leftOperand, rightOperand);
                    case '*': return Multiply(leftOperand, rightOperand);
                    default: throw new Exception("Something went wrong...");
                }
            }
            catch
            {
                // Восстановить состояние калькулятора,
                // которое было до вызова этого метода
                state = initialState;
                leftOperand = initialLeftOperand;
                operation = initialOperation;
                rightOperand = initialRightOperand;

                needMoreInput = true;

                // Пробросить исключение наверх
                throw;
            }
        }


        private enum State
        {
            Initial = 0,
            LeftOperandParsed,
            OperationParsed,
            RightOperandParsed
        };

        private State state = State.Initial;

        private decimal leftOperand = 0;
        private char operation = '\0';
        private decimal rightOperand = 0;

        // Этот метод нужен для того, чтобы всегда парсить
        // decimal'ы с десятичной точкой, а не запятой,
        // независимо от языка системы и других параметров
        private static decimal ParseDecimal(string s)
        {
            return decimal.Parse(
                s,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture
            );
        }

        // Сложение
        private decimal Add(decimal a, decimal b)
        {
            return a + b;
        }

        // Вычитание
        private decimal Substract(decimal a, decimal b)
        {
            return a - b;
        }

        // Умножение
        private decimal Multiply(decimal a, decimal b)
        {
            return a * b;
        }
    }
}
