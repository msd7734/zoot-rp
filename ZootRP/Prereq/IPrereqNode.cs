using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core.Prereq
{
    /// <summary>
    /// A node in a PrereqTree, it represents an evaluatable comparison expression or a
    ///     composition thereof in the form of lower order branches.
    /// </summary>
    public interface IPrereqNode
    {
        /// <summary>
        /// Evaluate this node and determine if the underlying structure meets the
        ///     represented prerequesite or not.
        /// </summary>
        /// <returns>True if the prerequesite is met, false if not.</returns>
        bool Compare();
    }
}
