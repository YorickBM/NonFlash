using System;

namespace Etap.Communication.ConnectionManager
{
    public interface IDataParser : IDisposable, ICloneable
    {
        void handlePacketData(byte[] packet);
    }
}