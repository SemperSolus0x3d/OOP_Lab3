namespace OOP_Lab3
{
    enum TokenType
    {
        Number,

        // Plus и Minus пришлось отделить
        // от OperationSign для
        // обработки отрицательных
        // операндов
        Plus,
        Minus,

        OperationSign
    }
}