using System;
using System.Linq;

namespace OOP_Lab3
{
    public class Parser
    {
        // Возвращает true, если парсер готов
        // выдать распарсенное выражение
        public bool ProcessInput(string input)
        {
            if (state == State.RightOperandParsed)
                throw new Exception("Парсер больше не может принимать данные");

            // Сохранить состояние парсера
            // на случай неверных входных данных
            State initialState = state;
            decimal initialLeftOperand = leftOperand;
            char initialOperation = operationSign;
            decimal initialRightOperand = rightOperand;

            try
            {
                input = input
                    .Replace(" ", "")
                    .Replace("=", "")
                    .Replace(',', '.');

                char[] operations = new char[] { '+', '-', '*' };

                string lexem = "";

                foreach (char ch in input)
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

                        operationSign = ch;

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

                return state == State.RightOperandParsed;
            }
            catch
            {
                // Восстановить состояние парсера,
                // которое было до вызова этого метода
                state = initialState;
                leftOperand = initialLeftOperand;
                operationSign = initialOperation;
                rightOperand = initialRightOperand;

                // Пробросить исключение наверх
                throw;
            }
        }

        public ParsedExpression GetResult()
        {
            if (state != State.RightOperandParsed)
                throw new Exception("Парсер еще не готов выдать результат");

            Operation operation;

            switch (operationSign)
            {
                case '+':
                    operation = Operation.Addition;
                    break;

                case '-':
                    operation = Operation.Substraction;
                    break;

                case '*':
                    operation = Operation.Multiplication;
                    break;

                default:
                    throw new Exception("Неизвестная операция");
            }

            state = State.Initial;

            return new ParsedExpression(leftOperand, operation, rightOperand);
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
        private char operationSign = '\0';
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
    }
}
