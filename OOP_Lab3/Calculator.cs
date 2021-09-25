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
            decimal result = 0m;

            switch (expression.Operation)
            {
                case Operation.Addition:
                    result = Add(
                        expression.LeftOperand,
                        expression.RightOperand
                    );
                    break;

                case Operation.Substraction:
                    result = Substract(
                        expression.LeftOperand,
                        expression.RightOperand
                    );
                    break;

                case Operation.Multiplication:
                    result = Multiply(
                        expression.LeftOperand,
                        expression.RightOperand
                    );
                    break;
            }

            return result;
        }
    }
}
