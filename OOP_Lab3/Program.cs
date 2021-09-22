using System;

namespace OOP_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calc = new Calculator();
            bool needMoreInput = true;
            decimal result = 0;

            while (needMoreInput)
            {
                string expression = Console.ReadLine();

                result = calc.ParseAndEvaluate(
                    expression, out needMoreInput
                );
            }

            Console.WriteLine($"Ответ: {result}");
            Console.ReadLine();
        }
    }
}
