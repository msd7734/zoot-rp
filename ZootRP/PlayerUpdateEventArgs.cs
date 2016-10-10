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

        public string Message { get; private set; }

        public PlayerUpdateEventArgs(PlayerMutableState state)
        {
            this._state = state;
            Message = String.Empty;
        }

        public PlayerUpdateEventArgs(PlayerMutableState state, string message)
        {
            this._state = state;
            Message = message;
        }

        public IPlayer GetPlayerFromMutableState()
        {
            return this._state;
        }

        public bool HasMessage()
        {
            return String.IsNullOrEmpty(Message);
        }
    }
}
