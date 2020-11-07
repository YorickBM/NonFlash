using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSidedServer.Communication
{
    [Serializable]
    public class MessageClass
    {
        public MessageClass(object msg)
        {
            this.message = msg;
        }
        public object message;
    }
}
