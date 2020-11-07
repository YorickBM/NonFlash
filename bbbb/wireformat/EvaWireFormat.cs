using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bbbb.connection;

namespace bbbb.wireformat
{
    public class EvaWireFormat : IWireFormat
    {
        private const uint MAX_DATA = (128 * 0x0400);//131072

        public void dispose()
        {

        }

        public byte[] encode(int header, List<object> messageBuffer)
        {
            byte[] buffer = new byte[128];
            buffer = BitConverter.GetBytes(0);
            buffer = BitConverter.GetBytes(header);

            foreach(Object value in messageBuffer)
            {
                if ((value is String))
                {
                    buffer = BitConverter.GetBytes(value);
                }
                else
                {
                    if ((value is int))
                    {
                        buffer = BitConverter.GetBytes((value as int));
                    }
                    else
                    {
                        if ((value is bool))
                        {
                            buffer = BitConverter.GetBytes((value as bool));
                        }
                        else
                        {
                            if ((value is short))
                            {
                                buffer. = BitConverter.GetBytes((value as short).value);
                            }
                            else
                            {
                                if ((value is byte[]))
                                {
                                    byte[] appendingBuffer = (value as byte[]);
                                    buffer.writeInt(appendingBuffer.length);
                                    buffer.writeBytes(appendingBuffer);
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<object> decode(byte[] buffer, IConnection socket)
        {
            throw new NotImplementedException();
        }
    }
}
