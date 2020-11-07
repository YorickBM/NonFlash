using bbbb.connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbbb
{
    public interface ICoreCommunicationManager
    {
        IConnection createConnection(IConnectionStateListener arg1 = null);
    }
}
