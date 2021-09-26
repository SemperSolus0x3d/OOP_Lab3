using System;
using System.Linq;

namespace OOP_Lab3
{
    public class Parser
    {
        private enum State
        {
            Initial = 0,
            LeftOperandParsed,
            OperationParsed,
            RightOperandParsed
        };

        private State state = State.Initial;
        private ParsedExpression parsedExpression = new ParsedExpression();

        private char[] operationSigns = new char[] { '+', '-', '*' };

        // Возвращает true, если парсер готов
        // выдать распарсенное выражение
        public bool ProcessInput(string input)
        {
            if (state == State.RightOperandParsed)
                throw new Exception("Парсер больше не может принимать данные");

            // Сохранить состояние парсера
            // на случай неверных входных данных
            State initialState = state;
            decimal initialLeftOperand = parsedExpression.LeftOperand;
            Operation initialOperation = parsedExpression.Operation;
            decimal initialRightOperand = parsedExpression.RightOperand;

            try
            {
                input = NormalizeInput(input);

                int position = 0;
                Token token = GetNextToken(input, ref position);
                while (token != null)
                {
                    ParseToken(token);
                    token = GetNextToken(input, ref position);
                }

                return state == State.RightOperandParsed;
            }
            catch
            {
                // Восстановить состояние парсера,
                // которое было до вызова этого метода
                state = initialState;
                parsedExpression.LeftOperand = initialLeftOperand;
                parsedExpression.Operation = initialOperation;
                parsedExpression.RightOperand = initialRightOperand;

                // Пробросить исключение наверх
                throw;
            }
        }

        public ParsedExpression GetResult()
        {
            if (state != State.RightOperandParsed)
                throw new Exception("Парсер еще не готов выдать результат");

            ParsedExpression result = parsedExpression;

            state = State.Initial;
            parsedExpression = new ParsedExpression();

            return result;
        }

        private string NormalizeInput(string input)
        {
            return input
                .Replace(" ", "")
                .Replace("=", "")
                .Replace(',', '.');
        }

        private Token GetNextToken(string input, ref int position)
        {
            string lexem = "";
            for (; position < input.Length; position++)
            {
                char ch = input[position];

                if (char.IsDigit(ch) || ch == '.')
                    lexem += ch;
                else if (operationSigns.Contains(ch))
                {
                    if (lexem != "")
                        return new Token(lexem, TokenType.Number);
                    else
                    {
                        position++;
                        return new Token(ch.ToString(), TokenType.OperationSign);
                    }
                }
            }

            return lexem == "" ? null : new Token(lexem, TokenType.Number);
        }

        private void ParseToken(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    if (state == State.LeftOperandParsed)
                        throw new Exception("Пропущен знак операции");

                    if (state == State.Initial)
                    {
                        parsedExpression.LeftOperand = ParseDecimal(token.Content);
                        state = State.LeftOperandParsed;
                    }
                    else if (state == State.OperationParsed)
                    {
                        parsedExpression.RightOperand = ParseDecimal(token.Content);
                        state = State.RightOperandParsed;
                    }
                    break;

                case TokenType.OperationSign:
                    if (state < State.LeftOperandParsed)
                        throw new Exception(
                            "Пропущен левый операнд"
                        );

                    if (state >= State.OperationParsed)
                        throw new Exception(
                            "В строке встречено несколько знаков операции"
                        );

                    parsedExpression.Operation = ParseOperationSign(token.Content);
                    state = State.OperationParsed;
                    break;

                default: throw new Exception("Неизвестный токен");
            }
        }

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

        private static Operation ParseOperationSign(string s)
        {
            switch (s)
            {
                case "+": return Operation.Addition;
                case "-": return Operation.Substraction;
                case "*": return Operation.Multiplication;
                default: throw new Exception("Неизвестная операция");
            }
        }
    }
}
