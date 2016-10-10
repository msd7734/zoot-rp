using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class PlayerUpdateEventArgs : EventArgs
    {
        private readonly IPlayer _previousPlayerState;

        public PlayerUpdateEventArgs(IPlayer previousPlayerState)
        {
            this._previousPlayerState = previousPlayerState;
        }

        public IPlayer GetPrevPlayerState()
        {
            return this._previousPlayerState;
        }
    }
}
