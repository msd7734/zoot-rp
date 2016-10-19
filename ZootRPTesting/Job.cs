using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;
using ZootRP.Core.Prereq;

namespace ZootRPTesting
{
    public class Job : IJob
    {
        public string Name
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public IReward Work(IPlayer player)
        {
            throw new NotImplementedException();
        }


        public IPrerequisite Prerequesite
        {
            get { throw new NotImplementedException(); }
        }
    }
}
