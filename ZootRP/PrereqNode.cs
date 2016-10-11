using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public enum PrereqNodeType
    {
        Comparison,
        LogicalBranch
    }
    public class PrereqNode
    {
        public PrereqNodeType NodeType
        {
            get;
            private set;
        }

        public PrereqNode Left
        {
            get;
            private set;
        }

        public PrereqNode Right
        {
            get;
            private set;
        }

        public Func<ulong, ulong, bool> CompareFunc;

        public PrereqNode(PlayerValue playerValue, Comparator comparator, ulong val)
        {
            NodeType = PrereqNodeType.Comparison;
            Left = null;
            Right = null;
        }

        public PrereqNode(PrereqNode leftNode, LogicalOperator logicalOp, PrereqNode rightNode)
        {
            NodeType = PrereqNodeType.LogicalBranch;
            Left = leftNode;
            Right = rightNode;
            CompareFunc = null;
        }
    }
}
