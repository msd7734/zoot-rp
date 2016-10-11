using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Strings
{
    public sealed class TokenDefinition
    {
        public readonly IMatcher Matcher;
        public readonly object Token;

        public TokenDefinition(string regex, object token)
        {
            this.Matcher = new RegexMatcher(regex);
            this.Token = token;
        }
    }
}
