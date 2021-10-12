using System;

namespace OOP_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding =
                Console.OutputEncoding =
                    System.Text.Encoding.Unicode;

            PrintGuide();

            ParsedExpression expression = ParseInput(new Parser());
            Calculator calc = new Calculator();
            decimal result = calc.EvaluateExpression(expression);

            Console.WriteLine($"Ответ: {result}");
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Нажмите ENTER чтобы продолжить...");
            Console.WriteLine();
            Console.ReadLine();
        }

        static void PrintGuide()
        {
            Console.WriteLine("Инструкция:");

            Console.WriteLine(
                "Введите математическое выражение, " +
                "содержащее в себе левый операнд, " +
                "правый операнд и операцию. " +
                @"Пробелы и '=' (знак равенства) в выражении игнорируются. " +
                "Разделителем между целой и дробной " +
                @"частями числа может быть и '.' (точка) " +
                @"и ',' (запятая). " +
                "Выражение можно вводить по частям, " +
                "по одной части на строку " +
                "(частями здесь являются операнды и знак операции)."
            );

            Console.WriteLine();

            Console.WriteLine("Доступные операции:");
            Console.WriteLine(@"Сложение  - '+'");
            Console.WriteLine(@"Вычитание - '-'");
            Console.WriteLine(@"Умножение - '*'");

            Console.WriteLine();

            Console.WriteLine("Примеры выражений:");
            Console.WriteLine("5+10");
            Console.WriteLine("123 -31");
            Console.WriteLine("52 * 63");
            Console.WriteLine("5345+ 42=");
            Console.WriteLine("74");
            Console.WriteLine("+");
            Console.WriteLine("84");

            Console.WriteLine();
        }

        static ParsedExpression ParseInput(Parser parser)
        {
            try
            {
                if (parser.ProcessInput(Console.ReadLine()))
                    return parser.GetResult();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return ParseInput(parser);
        }
    }
}
