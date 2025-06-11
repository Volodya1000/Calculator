using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;
using System.Reflection.Metadata;

namespace Calculator.Core.ExpressionEvaluator;

public class PrattParser
{
    private readonly Dictionary<string, IOperation> _operations;
    private readonly Dictionary<string, int> _operators;

    private List<Token> _tokens;
    private int _index;
    private Token _currentToken;
    private List<Token> _rpn;


    public PrattParser(Dictionary<string, IOperation> operations)
    {
        _operations = operations;
        _operators = new Dictionary<string, int>
        {
            { "+", 1 }, { "-", 1 }, { "*", 2 },
            { "/", 2 }, { "~", 3 }, { "^", 4 }
        };
    }

    //null используется в качестве маркера конца ввода 
    public IEnumerable<Token> Parse(IEnumerable<Token> tokens)
    {
        _tokens = tokens;
        _index = 0;
        _currentToken = _tokens.Count > 0 ? _tokens[0] : null;
        _rpn = new List<Token>();

        if (_index < _tokens.Count)
            throw new Exception($"extra tokens after end of expression ({_currentToken.Start})");

         return _rpn;
    }

    //Проверяет, соответствует ли текущий токен ожидаемому типу.
    //Если нет токенов или тип не совпадает – бросает исключение.
    //Иначе возвращает текущий токен и переходит на следующий.
    private Token Consume(TokenType expectedType)
    {
        var token = _currentToken;

        if (token == null)
            throw new Exception($"unexpected end of input, expected {expectedType}");

        if (token.Type != expectedType)
            throw new Exception($"unexpected token: \"{token.Value}\", expected {expectedType} ({token.Start}:{token.End})");

        _index++;
        _currentToken = _index < _tokens.Count ? _tokens[_index] : null;
        return token;
    }

    private int GetPrecedence(Token token)
    {
        if (token != null && token.Type == TokenType.Operator)
            return _operators.TryGetValue(token.Value, out int precedence) ? precedence : 0;

        return 0;
    }

    /*
    * Expression
    *   = Prefix (Infix)*
    */
    private void ParseExpression(int precedence = 0)
    {
        ParsePrefix();

        while (_currentToken != null && precedence < GetPrecedence(_currentToken))
            ParseInfix();
    }

    /*
     * Infix
     *   = ("+" | "-" | "*" | "/" | "^") (Expression)*
     */
    private void ParseInfix()
    {
        var token = Consume(TokenType.Operator);
        int newPrecedence = _operators[token.Value] - (token.Value == "^" ? 1 : 0);
        ParseExpression(newPrecedence);
        _rpn.Add(token);
    }

    /*
    * Prefix
    *   = ParenthesizedExpression
    *   | UnaryExpression
    *   | FunctionExpression
    *   | CONSTANT
    *   | VARIABLE
    *   | NUMBER
    */
    private void ParsePrefix()
    {
        if (_currentToken == null)
            throw new Exception("Unexpected end of input");

        switch (_currentToken.Type)
        {
            case TokenType.LeftParenthesis:
                ParseParenthesizedExpression();
                return;
            case TokenType.Operator when _currentToken.Value == "-":
                ParseUnaryExpression();
                return;
            case TokenType.Function:
                ParseFunctionExpression();
                return;
            case TokenType.Constant:
                _rpn.Add(Consume(TokenType.Constan));
                return;
            default:
                _rpn.Add(Consume(TokenType.Number));
                return;
        }
    }

    /*
    * ParenthesizedExpression
    *   = "(" Expression ")"
    */
    private void ParseParenthesizedExpression()
    {
        Consume(TokenType.LeftParenthesis);
        ParseExpression();
        Consume(TokenType.RightParenthesis);
    }

    /*
    * UnaryExpression
    *   = "-" Expression
    */
    private void ParseUnaryExpression()
    {
        var token = Consume(TokenType.Operator);
        var unaryToken = new Token(TokenType.Operator, "~", token.Start, token.End);
        ParseExpression(_operators["~"]);
        _rpn.Add(unaryToken);
    }

    /*
    * FunctionExpression
    *   = FUNCTION "(" Expression ("," Expression)* ")"
    */
    private void ParseFunctionExpression()
    {
        var token = Consume(TokenType.Function);
        Consume(TokenType.LeftParenthesis);
        ParseExpression();

        int argCount = _functions[token.Value].Args;
        for (int i = 1; i < argCount; i++)
        {
            Consume(TokenType.Delimiter);
            ParseExpression();
        }

        Consume(TokenType.RightParenthesis);
        _rpn.Add(token);
    }
}
