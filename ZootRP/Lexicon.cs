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
        private Dictionary<string, IMatcher> expressions;

        public Lexicon(TokenDefinition[] defs, bool tokenIsExpression = true)
        {
            tokens = new Dictionary<string, IMatcher>(defs.Length);
            expressions = new Dictionary<string, IMatcher>();
            foreach (var def in defs)
            {
                tokens.Add(def.Token.ToString(), def.Matcher);

                if (tokenIsExpression)
                    expressions.Add(def.Token.ToString(), def.Matcher);
            }
        }

        public Lexicon(TokenDefinition[] defs, Dictionary<string, string> expressions,
            bool tokenIsExpression = true)
        {
            tokens = new Dictionary<string, IMatcher>(defs.Length);
            this.expressions = new Dictionary<string, IMatcher>();

            foreach (var def in defs)
            {
                tokens.Add(def.Token.ToString(), def.Matcher);

                if (tokenIsExpression)
                    this.expressions.Add(def.Token.ToString(), def.Matcher);
            }
            foreach (var kv in expressions)
            {
                // define expressions
                DefineExpression(kv.Value, kv.Key);
            }
        }

        public void DefineExpression(string exp, string identifier)
        {
            var foundTokens = replToken.Matches(exp);
            string tokenIdent = String.Empty;
            string parsedExp = exp;
            foreach (Match m in foundTokens)
            {
                tokenIdent = m.Value.Substring(2, m.Value.Length - 4);
                
                // Sometimes tokens === expresssions, so check expressions first
                // to avoid treating a token-expression as simply a token
                if (this.expressions.Keys.Contains(tokenIdent))
                {
                    //.Substring to remove leading ^ and trailing $? from expression
                    string snip = expressions[tokenIdent].ToString();
                    snip = snip.Substring(1, snip.Length - 3);
                    parsedExp = parsedExp.Replace(m.Value, snip);
                }
                else if (this.tokens.Keys.Contains(tokenIdent))
                {
                    //.Substring to remove leading ^ from token
                    string snip = tokens[tokenIdent].ToString();
                    snip = snip.Substring(1);
                    //wrap individual tokens in parens
                    //parsedExp = parsedExp.Replace(m.Value, "(" + snip + ")");
                    parsedExp = parsedExp.Replace(m.Value, snip);
                }
                else
                {
                    // unknown token
                }
                

                //TODO: handle error for unknown token
                //if handled by the caller, this should at least rethrow as a properly labeled exception
            }

            // make final expansion match a whole line only, LAZILY (^ is prepended by RegexMatcher)
            parsedExp = parsedExp + "$?";
            expressions.Add(identifier, new RegexMatcher(parsedExp));
        }

        public bool InLexicon(string exp)
        {
            return !(String.IsNullOrEmpty(MatchExpression(exp)));
        }

        public string MatchExpression(string exp)
        {
            string trim = exp.Trim();
            foreach (var kv in expressions)
            {
                if (kv.Value.Match(trim) == exp.Length)
                {
                    return kv.Key;
                }
            }
            return String.Empty;
        }
    }
}
