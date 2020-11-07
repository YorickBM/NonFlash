using bbbb.connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbbb
{
    public class CoreCommunicationManager : ICoreCommunicationManager
    {
        private List<IConnection> _connections;

        public CoreCommunicationManager()
        {
            this._connections = new List<IConnection>();
        }

        public void dispose()
        {
            foreach(IConnection k in this._connections)
            {
                k.dispose();
            }
            this._connections = null;
        }

        public IConnection createConnection(IConnectionStateListener k = null)
        {
            IConnection _local_2 = new SocketConnection(this, k);
            this._connections.Add(_local_2);
            return _local_2;
        }

        public void update(uint k)
        {
            IConnection _local_3;
            int _local_2 = 0;
            while (_local_2 < this._connections.Count)
            {
                _local_3 = this._connections[_local_2];
                _local_3.processReceivedData();

                if (_local_3.disposed())
                {
                    this._connections = this._connections.Splice(_local_2, 1);
    }
                else
                {
                    _local_2++;
                }
            }
        }
    }

    static class MyExtensions
    {
        public static List<T> Splice<T>(this List<T> list, int index, int count)
        {
            List<T> range = list.GetRange(index, count);
            list.RemoveRange(index, count);
            return range;
        }
    }
}
