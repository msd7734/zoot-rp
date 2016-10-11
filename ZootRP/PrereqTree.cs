using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Strings;

namespace ZootRP.Core
{
    public class PrereqTree
    {
        private static Lexicon nodeLexicon =
            new Lexicon(
                    new TokenDefinition[]
                    {
                        new TokenDefinition(@"(?i)(health|endurance|dexterity|ingenuity|charisma)(?-i)","PLAYER-VALUE"),
                        new TokenDefinition(@"(<|>|<=|>=|=)", "COMPARATOR"),
                        new TokenDefinition(@"[0-9]+", "INTEGER"),
                        new TokenDefinition(@"\s*", "SPACE"),
                        new TokenDefinition(@"(&&|\|\|)", "LOGIC-BRANCH")
                    },
                    new Dictionary<string, string>
                    { 
                        { "COMPARISON", "[[PLAYER-VALUE]][[SPACE]][[COMPARATOR]][[SPACE]][[INTEGER]]" },
                        { "BRANCH-EXPR", "[[COMPARISON]][[SPACE]][[LOGIC-BRANCH]][[SPACE]][[COMPARISON]]" }
                    },
                    false
            );

        public PrereqTree(string expression)
        {
            IsValid = nodeLexicon.InLexicon(expression);
        }

        public bool IsValid
        {
            get;
            private set;
        }
    }
}
