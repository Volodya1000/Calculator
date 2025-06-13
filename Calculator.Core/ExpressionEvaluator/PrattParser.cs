using Calculator.Core.Exceptions.ExpressionExceptions;
using Calculator.Core.ExpressionEvaluator.Tokinezation;
using Calculator.Core.Interfaces;
using Calculator.Core.Operations;

namespace Calculator.Core.ExpressionEvaluator;

public class PrattParser
{
    private readonly Dictionary<string, IOperation> _operations;
    //private readonly Dictionary<string, int> _operators;

    private List<Token> _tokens;
    private int _index;
    private Token _currentToken;
    private List<Token> _rpn;


    public PrattParser(Dictionary<string, IOperation> operations)
    {
        _operations = operations;
        //_operators = new Dictionary<string, int>
        //{
        //    { "+", 1 }, { "-", 1 }, { "*", 2 },
        //    { "/", 2 }, { "~", 3 }, { "^", 4 },
        //    { "!", 5 }, { "%", 5 }
        //};
    }

    //null используется в качестве маркера конца ввода 
    public IEnumerable<Token> Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _index = 0;
        _currentToken = _tokens.Count > 0 ? _tokens[0] : null;
        _rpn = new List<Token>();

        ParseExpression();

        if (_index < _tokens.Count)
            throw new ExtraTokensException(_currentToken.Start);

         return _rpn;
    }

    //Проверяет, соответствует ли текущий токен ожидаемому типу.
    //Если нет токенов или тип не совпадает – бросает исключение.
    //Иначе возвращает текущий токен и переходит на следующий.
    private Token Consume(TokenType expectedType)
    {
        var token = _currentToken;

        if (token == null)
            throw new UnexpectedEndException(expectedType);

        if (token.Type != expectedType)
            throw new UnexpectedTokenException(token.Value,$", expected {expectedType}",token.Start,token.End);

        _index++;
        _currentToken = _index < _tokens.Count ? _tokens[_index] : null;
        return token;
    }

    private int GetPrecedence(Token token)
    {
        if (token != null && token.Type == TokenType.Operator)
        {
            if (_operations.TryGetValue(token.Value, out IOperation operation)
                && operation is Operator @operator)
            return @operator.Precedence;
        }

        return 0;
    }

    /*
    * Expression
    *   = Postfix (Infix)*
    */
    private void ParseExpression(int precedence = 0)
    {
        ParsePostfix();

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
        ParseExpression(GetPrecedence(token));
        _rpn.Add(token);
    }

    /*
    * Prefix
    *   = ParenthesizedExpression
    *   | UnaryExpression
    *   | FunctionExpression
    *   | CONSTANT
    *   | NUMBER
    */
    private void ParsePrefix()
    {
        if (_currentToken == null)
            throw new UnexpectedEndException(TokenType.Number);

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
                _rpn.Add(Consume(TokenType.Constant));
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
        ParseExpression(4);
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

        int argCount = _operations[token.Value].ArgsCount;

        if (argCount == -1)
        {
            // Add marker for functions with unlimited arguments
            _rpn.Add(new Token(TokenType.Delimiter, "#", token.Start, token.End));
            while (_currentToken != null && _currentToken.Type != TokenType.RightParenthesis)
            {
                ParseExpression();
                if (_currentToken != null && _currentToken.Type == TokenType.Delimiter)
                    Consume(TokenType.Delimiter);
            }
        }
        else
        {
            // Parse fixed number of arguments
            for (int i = 0; i < argCount; i++)
            {
                ParseExpression();
                if (i < argCount - 1)
                    Consume(TokenType.Delimiter);
            }
        }

        Consume(TokenType.RightParenthesis);
        _rpn.Add(token);
    }


    /*
     * Postfix
     *  = Prefix ("!" / "%")*
     */
    private void ParsePostfix()
    {
        ParsePrefix();
        while (_currentToken != null && _currentToken.Type == TokenType.Operator &&
               (_currentToken.Value == "!" || _currentToken.Value == "%"))
        {
            var token = Consume(TokenType.Operator);
            _rpn.Add(token);
        }
    }
}
