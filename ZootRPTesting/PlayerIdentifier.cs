using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class PlayerIdentifier : IPlayerIdentifier
    {
        public Guid Id
        {
            get;
            private set;
        }

        public string CanonicalName
        {
            get;
            private set;
        }

        public PlayerIdentifier(Guid guid, string name)
        {
            Id = guid;
            CanonicalName = name;
        }
    }
}
