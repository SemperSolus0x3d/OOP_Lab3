namespace OOP_Lab3
{
    public class ParsedExpression
    {
        public decimal LeftOperand { get; }
        public Operation Operation { get; }
        public decimal RightOperand { get; }

        public ParsedExpression(
            decimal leftOperand, 
            Operation operation, 
            decimal rightOperand
        )
        {
            LeftOperand = leftOperand;
            Operation = operation;
            RightOperand = rightOperand;
        }
    }
}
