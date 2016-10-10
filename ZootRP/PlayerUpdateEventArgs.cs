using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public class PlayerUpdateEventArgs : EventArgs
    {
        private readonly PlayerMutableState _state;

        public PlayerUpdateEventArgs(PlayerMutableState state)
        {
            this._state = state;
        }

        public IPlayer GetPlayerFromMutableState()
        {
            return this._state;
        }
    }
}
