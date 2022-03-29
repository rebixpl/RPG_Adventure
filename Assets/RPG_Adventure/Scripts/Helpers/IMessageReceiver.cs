using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Adventure
{
    public enum MessageType
    {
        DAMAGED,
        DEAD,
    }

    public interface IMessageReceiver
    {
        void OnReceiveMessage(MessageType type);
    }
}
