using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ZootRP.Strings
{
    /// <summary>
    /// Stores a list of regex-containing token patterns, and also allows for the creation
    ///     of expressions - regex patterns that conntain token expansions.
    /// Together, tokens and expressions define a langauge. The Lexicon can check a string
    ///     to see if it is in the Lexicon's langauge.
    /// </summary>
    public class Lexicon
    {
        private static readonly Regex replToken = new Regex(@"(\[\[.+?\]\])");

        private Dictionary<string, IMatcher> tokens;
        private Dictionary<string, IMatcher> expressions;

        /// <summary>
        /// Create a Lexicon.
        /// </summary>
        /// <param name="defs">The valid tokens in this language.</param>
        /// <param name="tokenIsExpression">Whether to treat each token as valid string in this lexicon's langauge.</param>
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

        /// <summary>
        /// Create a Lexicon.
        /// </summary>
        /// <param name="defs">The valid tokens in this language.</param>
        /// <param name="expressions">The expressions which define vallid strings in the lexicon's language. <see cref="Lexicon.DefineExpression"/></param>
        /// <param name="tokenIsExpression">Whether to treat each token as valid string in this lexicon's langauge.</param>
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

        /// <summary>
        /// Define an expression in this lexicon.
        /// </summary>
        /// <param name="exp">
        ///     The unexpanded expression string. This string is a regular expression, where values in [[double-backets]]
        ///     are expanded into matching token regular expressions.
        /// </param>
        /// <param name="identifier">A unique string that identifies the expression.</param>
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

        /// <summary>
        /// Check if a string is in this lexicon's language.
        /// </summary>
        /// <param name="exp">The expression to check.</param>
        /// <returns>True if <c>exp</c> is in the lexicon, false otherwise.</returns>
        public bool InLexicon(string exp)
        {
            return !(String.IsNullOrEmpty(MatchExpression(exp)));
        }

        /// <summary>
        /// Get the identifier of an expression in this lexicon which matches a given string.
        /// </summary>
        /// <param name="exp">The string to attempt to match.</param>
        /// <returns>A string identifier representing an expression in the lexicon.</returns>
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
