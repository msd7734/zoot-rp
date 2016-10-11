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
                if (this.tokens.Keys.Contains(tokenIdent))
                {
                    //.Substring(1) to remove leading ^
                    parsedExp = parsedExp.Replace(m.Value, "("+tokens[tokenIdent].ToString().Substring(1)+")");
                }
                else if (this.expressions.Keys.Contains(tokenIdent))
                {
                    //.Substring(1) to remove leading ^
                    parsedExp = parsedExp.Replace(m.Value, expressions[tokenIdent].ToString().Substring(1));
                }
                else
                {
                    // unknown token
                }
                

                //TODO: handle error for unknown token
                //if handled by the caller, this should at least rethrow as a properly labeled exception
            }
            //The correct endgame:
            //^((?i)(health|endurance|dexterity|ingenuity|charisma)(?-i))(\s*)((<|>|<=|>=|=))(\s*)([0-9]+)(\s*)((&&|\|\|))(\s*)((?i)(health|endurance|dexterity|ingenuity|charisma)(?-i))(\s*)((<|>|<=|>=|=))(\s*)([0-9]+)(\s*)(((&&|\|\|))(\s*)((?i)(health|endurance|dexterity|ingenuity|charisma)(?-i))(\s*)((<|>|<=|>=|=))(\s*)([0-9]+))+$
            //What we're getting:
            //^((?i)(health|endurance|dexterity|ingenuity|charisma)(?-i))(\\s*)((<|>|<=|>=|=))(\\s*)([0-9]+)$(\\s*)((&&|\\|\\|))(\\s*)((?i)(health|endurance|dexterity|ingenuity|charisma)(?-i))(\\s*)((<|>|<=|>=|=))(\\s*)([0-9]+)$$(\\s*)(((&&|\\|\\|))(\\s*)((?i)(health|endurance|dexterity|ingenuity|charisma)(?-i))(\\s*)((<|>|<=|>=|=))(\\s*)([0-9]+)$)+
            expressions.Add(identifier, new RegexMatcher(parsedExp+"$"));
        }

        public bool InLexicon(string exp)
        {
            foreach (var kv in expressions)
            {
                if (kv.Value.Match(exp) > 0)
                {
                    Console.WriteLine("Matched with expr {0}", kv.Key);
                    Console.WriteLine("Matched with regex {0}", kv.Value.ToString());
                    return true;
                }
            }
            return false;
        }
    }
}
