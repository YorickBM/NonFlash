using System;
using Etap.Communication.ConnectionManager;
using Etap.Utilities;

namespace Etap.Communication
{
    public class InitialPacketParser : IDataParser
    {
        public delegate void NoParamDelegate();

        public byte[] currentData;

        public void handlePacketData(byte[] packet)
        {
            if (packet[0] == 60 && PolicyRequest != null)
            {
                PolicyRequest.Invoke();
                Logger.DebugWarn("Policy Request Invoked");
            }
            else if (packet[0] != 67 && SwitchParserRequest != null)
            {
                currentData = packet;
                SwitchParserRequest.Invoke();
            }
        }

        public void Dispose()
        {
            PolicyRequest = null;
            SwitchParserRequest = null;
            GC.SuppressFinalize(this);
        }

        public object Clone()
        {
            return new InitialPacketParser();
        }

        public event NoParamDelegate PolicyRequest;
        public event NoParamDelegate SwitchParserRequest;
    }
}