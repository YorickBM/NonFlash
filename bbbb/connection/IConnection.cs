using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbbb.connection
{
    public interface IConnection
    {
        bool disposed();
        void dispose();
        void processReceivedData();
    }
}
