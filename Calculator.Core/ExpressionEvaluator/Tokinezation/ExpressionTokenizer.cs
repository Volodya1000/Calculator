using Calculator.Core.Exceptions.ExpressionExceptions;
using Calculator.Core.Operations;
using System.Text.RegularExpressions;

namespace Calculator.Core.ExpressionEvaluator.Tokinezation;

public class ExpressionTokenizer
{
    private readonly Regex _regex;


    public ExpressionTokenizer(List<string> functions, List<string> operators, List<string> constants)
    {
        var patternParts = new List<string>
        {
            @"(?<LeftParenthesis>\()",
            @"(?<RightParenthesis>\))",
            @"(?<Delimiter>,)",
            @"(?<Number>\d+(\.\d+)?)" 
        };

        //Regex.Escape экранирует заданные строки , чтобы их можно было безопасно использовать в регулярном выражении

        if(operators != null && operators.Count>0)
        {
            string operatorsPattern = string.Join("", operators.Select(f => f.ToLower()).Select(Regex.Escape));
            patternParts.Add($@"(?<Operator>[{operatorsPattern}])");
        }

        // Добавляем функции, если они есть
        if (functions != null && functions.Count > 0)
        {
            string functionPattern = string.Join("|", functions.Where(f=>!(f is Operator)).Select(f=>f.ToLower()).Select(Regex.Escape));
            patternParts.Add($@"(?<Function>{functionPattern})");
        }

        // Добавляем константы, если они есть
        if (constants != null && constants.Count > 0)
        {
            string constantPattern = string.Join("|", constants.Select(Regex.Escape));
            //  \b нужно для точного распознавания констант без ложных срабатываний при частичном совпадении 
            // например чтоб e не было распознано в exp
            patternParts.Add($@"(?<Constant>\b(?:{constantPattern})\b)");
        }

        // Unknown всегда в конце
        patternParts.Add(@"(?<Unknown>\S)");

        string pattern = string.Join("|", patternParts);
        _regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
    }

    public List<Token> Tokenize(string expression)
    {
        var matches = _regex.Matches(expression.ToLower());
        var tokens = new List<Token>();

        foreach (Match match in matches.Cast<Match>())
            tokens.Add(MatchToToken(match));

        var unknownTokens = tokens.Where(t => t.Type == TokenType.Unknown).ToList();
        if (unknownTokens.Count > 0)
        {
            var token = unknownTokens.First();
            throw new UnknownTokenException(token.Value, token.Start, token.End);
        }

        return tokens;
    }

    private Token ? MatchToToken(Match match)
    {
        foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
        {
            string groupName = type.ToString();
            Group group = match.Groups[groupName];

            if (group.Success)
            {
                return new Token(type, group.Value, group.Index, group.Index + group.Length);
            }
        }
        return null;
    }
}
