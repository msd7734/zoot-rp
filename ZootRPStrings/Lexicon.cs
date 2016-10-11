using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ZootRP.Strings
{
    public class Lexicon
    {
        private static readonly Regex replToken = new Regex(@"(\[\[.+?\]\])");

        private Dictionary<string, IMatcher> tokens;
        private List<IMatcher> expressions;

        public Lexicon(TokenDefinition[] defs, bool tokenIsExpression = true)
        {
            tokens = new Dictionary<string, IMatcher>(defs.Length);
            expressions = new List<IMatcher>();
            foreach (var def in defs)
            {
                tokens.Add(def.Token.ToString(), def.Matcher);

                if (tokenIsExpression)
                    expressions.Add(def.Matcher);
            }
        }

        public Lexicon(TokenDefinition[] defs, IMatcher[] expressions, bool tokenIsExpression = true)
        {
            tokens = new Dictionary<string, IMatcher>(defs.Length);
            this.expressions = new List<IMatcher>(expressions);
            foreach (var def in defs)
            {
                tokens.Add(def.Token.ToString(), def.Matcher);

                if (tokenIsExpression)
                    this.expressions.Add(def.Matcher);
            }
        }

        public void DefineExpression(string exp)
        {
            var foundTokens = replToken.Matches(exp);
            string tokenIdent = String.Empty;
            string parsedExp = exp;
            foreach (Match m in foundTokens)
            {
                tokenIdent = m.Value.Substring(2, m.Value.Length - 4);
                //.Substring(1) to remove leading ^
                parsedExp = parsedExp.Replace(m.Value, tokens[tokenIdent].ToString().Substring(1));
            }
            expressions.Add(new RegexMatcher(parsedExp));
        }

        public bool InLexicon(string exp)
        {
            foreach (var matcher in expressions)
            {
                if (matcher.Match(exp) > 0)
                    return true;
            }
            return false;
        }
    }
}
