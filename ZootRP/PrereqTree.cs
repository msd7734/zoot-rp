using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ZootRP.Strings;

namespace ZootRP.Core
{
    public class PrereqTree
    {
        private static TokenDefinition[] tokenDefs = new TokenDefinition[]
        {
            new TokenDefinition(@"(?i)(health|endurance|dexterity|ingenuity|charisma|level)(?-i)","PLAYER-VALUE"),
            new TokenDefinition(@"(<|>|<=|>=|=)", "COMPARATOR"),
            new TokenDefinition(@"[0-9]+", "INTEGER"),
            new TokenDefinition(@"\s*", "SPACE"),
            new TokenDefinition(@"(&&|\|\|)", "LOGIC-BRANCH")
        };

        private static Lexicon nodeLexicon = new Lexicon(
                tokenDefs,
                new Dictionary<string, string>
                { 
                    { "COMPARISON", "[[PLAYER-VALUE]][[SPACE]][[COMPARATOR]][[SPACE]][[INTEGER]]" },
                    { "BRANCH-EXPR", "[[COMPARISON]][[SPACE]][[LOGIC-BRANCH]][[SPACE]][[COMPARISON]]" },
                    { "MULTI-BRANCH", "[[BRANCH-EXPR]][[SPACE]]([[LOGIC-BRANCH]][[SPACE]][[COMPARISON]])+"}
                },
                false
            );

        private enum ExpressionType
        {
            Unrecognized,
            COMPARISON,
            BRANCH_EXPR,
            MULTI_BRANCH
        }

        private PrereqNode _rootNode;
        private ExpressionType _exprType;
        private Lexer _lexer;

        public PrereqTree(string expression)
        {
            string trim = expression.Trim();
            string exprMatch = nodeLexicon.MatchExpression(trim);
            IsValid = Enum.TryParse<ExpressionType>(exprMatch.Replace('-', '_'), out _exprType);
            _lexer = new Lexer(new StringReader(trim), tokenDefs);
            if (IsValid)
                Build(expression.Trim());
        }

        public bool IsValid
        {
            get;
            private set;
        }

        private void Build(string exp)
        {

        }
    }
}
