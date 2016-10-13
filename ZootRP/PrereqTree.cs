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
            new TokenDefinition(@"(?i)(health|endurance|dexterity|ingenuity|charisma|level)(?-i)","PLAYER-INT"),
            new TokenDefinition(@"(?i)(job|species|residence)(?-i)", "PLAYER-STR"),
            new TokenDefinition(@"=", "EQUALS"),
            new TokenDefinition(@"(<|>|<=|>=|=)", "COMPARATOR"),
            new TokenDefinition(@"[0-9]+", "INTEGER"),
            new TokenDefinition(@"\"".+\""", "QUOTED-STRING"),
            new TokenDefinition(@"\s*", "SPACE"),
            new TokenDefinition(@"(&&|\|\|)", "LOGIC-BRANCH")
        };

        private static Lexicon nodeLexicon = new Lexicon(
            tokenDefs,
            new Dictionary<string, string>
            { 
                { "STR-COMPARISON", "[[PLAYER-STR]][[SPACE]]=[[SPACE]][[QUOTED-STRING]]"},
                { "INT-COMPARISON", "[[PLAYER-INT]][[SPACE]][[COMPARATOR]][[SPACE]][[INTEGER]]"},
                { "COMPARISON", "([[STR-COMPARISON]] | [[INT-COMPARISON]])" },
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

        private IPrereqNode _rootNode;
        private ExpressionType _exprType;
        private IPlayer _player;

        public PrereqTree(IPlayer player, string expression)
        {
            string trim = expression.Trim();
            string exprMatch = nodeLexicon.MatchExpression(trim);
            IsValid = Enum.TryParse<ExpressionType>(exprMatch.Replace('-', '_'), out _exprType);
            _rootNode = null;
            _player = player;
            /*
            if (IsValid)
                Build(expression.Trim(), new Lexer(new StringReader(trim), tokenDefs));
            */
        }

        public bool IsValid
        {
            get;
            private set;
        }

        /*
        private void Build(string exp, Lexer lexer)
        {
            if (_exprType == ExpressionType.COMPARISON)
                _rootNode = BuildComparison(lexer);
        }

        private IPrereqNode BuildComparison(Lexer lexer)
        {
            // player value
            lexer.Next();
            ComparablePlayerVal pval = (ComparablePlayerVal) Enum.Parse(typeof(ComparablePlayerVal), lexer.TokenContents, true);

            // comparison operator
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();
            Comparator compare = ComparatorFromString(lexer.TokenContents);

            // numerical value
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();
            ulong val = ulong.Parse(lexer.TokenContents);

            return new IPrereqNode(pval, compare, val);
        }
        */

        private static Comparator ComparatorFromString(string s)
        {
            switch (s)
            {
                case "<":
                    return Comparator.LessThan;
                case "<=":
                    return Comparator.LessThanEqualTo;
                case ">":
                    return Comparator.GreaterThan;
                case ">=":
                    return Comparator.GreaterThanEqualTo;
                case "=":
                    return Comparator.EqualTo;
                default:
                    return Comparator.Unknown;
            }
        }
    }
}
