using bbbb.connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbbb.wireformat
{
    public interface IWireFormat
    {
        void dispose();
        byte[] encode(int header, List<object> messageBuffer);
        List<object> decode(byte[] buffer, IConnection socket);
    }
}
