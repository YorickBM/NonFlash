using System;

namespace Retro.Communication.ConnectionManager
{
    public interface IDataParser : IDisposable, ICloneable
    {
        void handlePacketData(byte[] packet);
    }
}