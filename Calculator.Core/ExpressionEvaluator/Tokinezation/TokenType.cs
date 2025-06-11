namespace Calculator.Core.ExpressionEvaluator.Tokinezation;

public enum TokenType
{
    LeftParenthesis,    
    RightParenthesis,  
    Delimiter,          
    Operator,           
    Function,
    Constant,
    Number,
    Unknown
}
