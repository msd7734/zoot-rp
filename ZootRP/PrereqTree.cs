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
            new TokenDefinition(
                String.Format(@"(?i)({0})(?-i)", String.Join("|", PlayerUtil.ComparableIntValues)),
                "PLAYER-INT"),
            new TokenDefinition(
                String.Format(@"(?i)({0})(?-i)", String.Join("|", PlayerUtil.ComparableStrValues)),
                "PLAYER-STR"),
            new TokenDefinition(@"=", "EQUALS"),
            new TokenDefinition(@"(<|>|<=|>=|=)", "COMPARATOR"),
            new TokenDefinition(@"[0-9]+", "INTEGER"),
            new TokenDefinition(@"\"".+?""", "QUOTED-STRING"),
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
            STR_COMPARISON,
            INT_COMPARISON,
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
            
            if (IsValid)
                Build(expression.Trim(), new Lexer(new StringReader(trim), tokenDefs));
            
        }

        public bool IsValid
        {
            get;
            private set;
        }

        
        private void Build(string exp, Lexer lexer)
        {
            // consider pulling out lexer or exp to be members...

            if (_exprType == ExpressionType.INT_COMPARISON)
            {
                _rootNode = BuildIntcomparison(lexer);
                Console.WriteLine(_rootNode.Compare());
            }
            else if (_exprType == ExpressionType.STR_COMPARISON)
            {
                _rootNode = BuildStrComparison(lexer);
                Console.WriteLine(_rootNode.Compare());
            }
        }

        private IntCompareNode BuildIntcomparison(Lexer lexer)
        {
            var funcs = PlayerUtil.GetIntegerPropertyFuncs(_player);
            // player-int
            lexer.Next();
            PlayerIntegerProperty property = PlayerPropFromString<PlayerIntegerProperty>(lexer.TokenContents);

            // check space
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();

            // comparison operator
            Comparator comparison = ComparatorFromString(lexer.TokenContents);

            //check space
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();

            // integer value to compare to
            uint compareToVal = UInt32.Parse(lexer.TokenContents);

            return new IntCompareNode(funcs[property](), compareToVal, comparison);
        }

        private StrCompareNode BuildStrComparison(Lexer lexer)
        {
            var funcs = PlayerUtil.GetStringPropertyFuncs(_player);

            // player-string
            lexer.Next();
            PlayerStringProperty property = PlayerPropFromString<PlayerStringProperty>(lexer.TokenContents);

            // check space
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();

            // consume and ignore comparison operator since it's always =
            Comparator comparison = Comparator.EqualTo;

            //check space
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();

            // integer value to compare to
            // extract from between quotes
            string compareToVal = lexer.TokenContents.Split('\"')[1];

            return new StrCompareNode(funcs[property](), compareToVal);
        }

        private static T PlayerPropFromString<T>(string str)
        {
            return (T) Enum.Parse(typeof(T), str, ignoreCase:true);
        }
        
        /*
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
