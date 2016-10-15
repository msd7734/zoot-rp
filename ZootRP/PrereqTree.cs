using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

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

        // Static due to potential high frequency of calling on logic branches
        // make sure to clip off added ^
        private static Regex LogicBranchPattern = new Regex(
            tokenDefs.First(x => x.Token.ToString() == "LOGIC-BRANCH").Matcher.ToString().Substring(1)
        );

        private static Lexicon nodeLexicon = new Lexicon(
            tokenDefs,
            new Dictionary<string, string>
            { 
                { "STR-COMPARISON", "[[PLAYER-STR]][[SPACE]]=[[SPACE]][[QUOTED-STRING]]"},
                { "INT-COMPARISON", "[[PLAYER-INT]][[SPACE]][[COMPARATOR]][[SPACE]][[INTEGER]]"},
                { "COMPARISON", "(([[STR-COMPARISON]])|([[INT-COMPARISON]]))" },
                { "BRANCH-EXPR", "[[COMPARISON]][[SPACE]][[LOGIC-BRANCH]][[SPACE]][[COMPARISON]]" },
                { "MULTI-BRANCH", "[[BRANCH-EXPR]][[SPACE]]([[LOGIC-BRANCH]][[SPACE]][[COMPARISON]][[SPACE]])+"}
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
            _exprType = ExpressionTypeFromString(exprMatch);
            IsValid = (_exprType != ExpressionType.Unrecognized);
            _rootNode = null;
            _player = player;

            if (IsValid)
            {
                // Build underlying node structure
                _rootNode = Build(trim, new Lexer(new StringReader(trim), tokenDefs), _exprType);
            }
            
        }

        public bool IsValid
        {
            get;
            private set;
        }

        public IPrereqNode GetRoot()
        {
            return _rootNode;
        }

        public bool IsMet()
        {
            // probably don't just return false here since that would imply the tree is good
            // caller should check IsValid before calling or handle NullRefException
            return _rootNode.Compare();
        }
        
        private IPrereqNode Build(string exp, Lexer lexer, ExpressionType exprType)
        {
            switch (exprType)
            {
                case ExpressionType.INT_COMPARISON:
                    return BuildIntComparison(lexer);
                case ExpressionType.STR_COMPARISON:
                    return BuildStrComparison(lexer);
                case ExpressionType.BRANCH_EXPR:
                    return BuildBranch(exp, lexer);
                case ExpressionType.MULTI_BRANCH:
                    return BuildMultiBranch(exp, lexer);
                default:
                    return null;
            }
        }

        private LogicBranchNode BuildMultiBranch(string exp, Lexer lexer)
        {
            // parse entire expression from left->right
            // so in a multi-branch of 2 branches (degree 3):
            //             [Branch]
            //            /        \
            //   [Comparison]    [Branch]
            //                  /       \
            //           [Comparison]  [Comparison]                 
            // 
            //

            string subExpr = String.Empty;
            ExpressionType subExprType = ExpressionType.Unrecognized;

            while (lexer.Next())
            {
                subExpr += lexer.TokenContents;
                subExprType = ExpressionTypeFromString(nodeLexicon.MatchExpression(subExpr));
                if (subExprType == ExpressionType.INT_COMPARISON || subExprType == ExpressionType.STR_COMPARISON)
                {
                    IPrereqNode left = Build(subExpr, new Lexer(new StringReader(subExpr), tokenDefs), subExprType);

                    // consume the branch symbol and any space before passing the rest on to a subtree
                    lexer.Next();
                    if (lexer.Token.ToString() == "SPACE")
                        lexer.Next();

                    string theRest = exp.Substring(lexer.Position);
                    IPrereqNode right = new PrereqTree(_player, theRest).GetRoot();
                    string branchSymbol = LogicBranchPattern.Match(exp).Value;
                    return new LogicBranchNode(left, right, LogicalOperatorFromString(branchSymbol));
                }
            }

            return null;
        }

        private LogicBranchNode BuildBranch(string exp, Lexer lexer)
        {
            string branchSymbol = LogicBranchPattern.Match(exp).Value;
            string[] comparisons = exp.Split(new string[]{branchSymbol}, StringSplitOptions.RemoveEmptyEntries);
            string l = comparisons[0].Trim();
            ExpressionType leftExprType = ExpressionTypeFromString(nodeLexicon.MatchExpression(l));
            string r = comparisons[1].Trim();
            ExpressionType rightExprType = ExpressionTypeFromString(nodeLexicon.MatchExpression(r));
            IPrereqNode leftCompare = Build(l, new Lexer(new StringReader(l), tokenDefs), leftExprType);
            IPrereqNode rightCompare = Build(r, new Lexer(new StringReader(r), tokenDefs), rightExprType);
            return new LogicBranchNode(leftCompare, rightCompare, LogicalOperatorFromString(branchSymbol));
        }

        private IntCompareNode BuildIntComparison(Lexer lexer)
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
            // StrCompareNode doesn't need it anyway

            //check space
            lexer.Next();
            if (lexer.Token.ToString() == "SPACE")
                lexer.Next();

            // integer value to compare to
            // extract from between quotes
            string compareToVal = lexer.TokenContents.Split('\"')[1];

            return new StrCompareNode(funcs[property](), compareToVal);
        }

        private static ExpressionType ExpressionTypeFromString(string str)
        {
            ExpressionType res = ExpressionType.Unrecognized;
            Enum.TryParse<ExpressionType>(str.Replace('-', '_'), out res);
            return res;
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

        private static LogicalOperator LogicalOperatorFromString(string s)
        {
            switch (s)
            {
                case "||":
                    return LogicalOperator.Or;
                case "&&":
                    return LogicalOperator.And;
                default:
                    return LogicalOperator.Unknown;
            }
        }
    }
}
