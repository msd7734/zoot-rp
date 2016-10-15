using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core.Prereq
{
    /// <summary>
    /// A branch node, created due to a logical branch AND or OR.
    /// </summary>
    public class LogicBranchNode : IPrereqNode
    {
        private IPrereqNode left, right;
        private LogicalOperator op;

        public LogicBranchNode(IPrereqNode leftNode, IPrereqNode rightNode, LogicalOperator operation)
            : base()
        {
            left = leftNode;
            right = rightNode;
            op = operation;
        }

        public bool Compare()
        {
            switch (op)
            {
                case LogicalOperator.And:
                    return left.Compare() && right.Compare();
                case LogicalOperator.Or:
                    return left.Compare() || right.Compare();
                default:
                    return false;
            }
        }
    }
}
