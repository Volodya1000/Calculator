using System.Text.RegularExpressions;

namespace Calculator.Core.ExpressionEvaluator.Tokinezation;

public class ExpressionTokenizer
{
    private readonly Regex _regex;

    public ExpressionTokenizer(List<string> functions, List<string> constants)
    {
        //Regex.Escape экранирует заданные строки , чтобы их можно было безопасно использовать в регулярном выражении
        string functionPattern = string.Join("|", functions.Select(Regex.Escape));
        //  \b нужно для точного распознавания констант без ложных срабатываний при частичном совпадении 
        // например чтоб e не было распознано в exp
        string constantPattern = $@"\b({string.Join("|", constants.Select(Regex.Escape))})\b";

        string pattern = string.Join("|", new[]
        {
            @"(?<LeftParenthesis>\()",
            @"(?<RightParenthesis>\))",
            @"(?<Delimiter>,)",
            @"(?<Operator>[-+*/^])",
            $@"(?<Function>{functionPattern})",
            $@"(?<Constant>{constantPattern})",
            @"(?<Number>\d+(\.\d+)?)",
            @"(?<Variable>[a-z]\w*)",
            @"(?<Unknown>\S)"
        });

        // Используем ExplicitCapture чтобы и гнорировать автоматически нумеруемые группы (только именованные через ?<name>)
        _regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
    }

    public IEnumerable<Token> Tokenize(string expression)
    {
        var matches = _regex.Matches(expression);
        var tokens = new List<Token>();

        foreach (Match match in matches.Cast<Match>())
            tokens.Add(MatchToToken(match));

        var unknownTokens = tokens.Where(t => t.Type == TokenType.Unknown).ToList();
        if (unknownTokens.Count > 0)
        {
            string invalidTokens = string.Join(", ", unknownTokens.Select(t => t.Value));
            throw new ArgumentException($"Invalid tokens found: {invalidTokens}");
        }

        return tokens;
    }

    private Token MatchToToken(Match match)
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
