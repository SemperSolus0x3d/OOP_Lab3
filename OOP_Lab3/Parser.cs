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

        private char[] operationSigns = new char[] { '*' };

        private decimal operandSign = 1m;

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
            decimal initialOperandSign = operandSign;

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
                operandSign = initialOperandSign;

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
                else if (ch == '+')
                {
                    if (lexem == "")
                    {
                        position++;
                        return new Token("+", TokenType.Plus);
                    }
                    else
                        return new Token(lexem, TokenType.Number);
                }
                else if (ch == '-')
                {
                    if (lexem == "")
                    {
                        position++;
                        return new Token("-", TokenType.Minus);
                    }
                    else
                        return new Token(lexem, TokenType.Number);
                }
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
                        parsedExpression.LeftOperand =
                            ParseDecimal(token.Content) * operandSign;
                        state = State.LeftOperandParsed;

                        operandSign = 1m;
                    }
                    else if (state == State.OperationParsed)
                    {
                        parsedExpression.RightOperand =
                            ParseDecimal(token.Content) * operandSign;
                        state = State.RightOperandParsed;

                        operandSign = 1m;
                    }
                    break;

                case TokenType.Plus:
                    if (state == State.Initial ||
                        state == State.OperationParsed)
                        operandSign = 1m;
                    else if (state == State.LeftOperandParsed)
                    {
                        parsedExpression.Operation = Operation.Addition;
                        state = State.OperationParsed;
                    }
                    else
                        throw new Exception(@"Встречено '+' в неожиданном месте");

                    break;

                case TokenType.Minus:
                    if (state == State.Initial ||
                        state == State.OperationParsed)
                        operandSign = -1m;
                    else if (state == State.LeftOperandParsed)
                    {
                        parsedExpression.Operation = Operation.Substraction;
                        state = State.OperationParsed;
                    }
                    else
                        throw new Exception(@"Встречено - в неожиданном месте");

                    break;

                case TokenType.OperationSign:
                    if (state < State.LeftOperandParsed)
                        throw new Exception(
                            "Пропущен левый операнд"
                        );

                    if (state >= State.OperationParsed)
                        throw new Exception(
                            "Встречено несколько знаков операции"
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
                case "*": return Operation.Multiplication;
                default: throw new Exception("Неизвестная операция");
            }
        }
    }
}
