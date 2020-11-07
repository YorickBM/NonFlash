using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Yorick.Core.Communication
{
    public class SocketConnection : IConnection
    {
        private Socket _socket;
        private Timer _timeOutTimer;
        private int _timeOutStarted;
        private byte[] _dataBuffer;
        private IWireFormat _wireFormat;
        private IEncryption _clientToServerEncryption;
        private IEncryption _serverToClientEncryption;
        private MessageClassManager _messageClassManager;
        private ICoreCommunicationManager _communicationManager;
        private IConnectionStateListener _stateListener;
        private bool _authenticated;
        private bool _configurationReady;
        private List<IMessageComposer> _pendingClientMessages;
        private List<IMessageDataWrapper> _pendingServerMessages;
        private IMessageDataWrapper _lastProcessedMessage;

        public SocketConnection(ICoreCommunicationManager communicationManager, IConnectionStateListener stateListener)
        {
            this._communicationManager = communicationManager;
            this._messageClassManager = new MessageClassManager();
            ///this._wireFormat = new EvaWireFormat();
            this.createSocket();
            ///this._timeOutTimer = new Timer(DEFAULT_SOCKET_TIMEOUT, 1);
           /// this._timeOutTimer.addEventListener(TimerEvent.TIMER, this.onTimeOutTimer);
            this._stateListener = stateListener;
        }

        public void addMessageEvent(IMessageEvent arg1)
        {
            throw new NotImplementedException();
        }

        public void close()
        {
            throw new NotImplementedException();
        }

        public bool connected()
        {
            throw new NotImplementedException();
        }

        public void createSocket()
        {
            this._Str_20244();
            this._dataBuffer = new byte[128];
            this._serverToClientEncryption = null;
            this._clientToServerEncryption = null;
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket.addEventListener(Event.CONNECT, this.onConnect);
            this._socket.addEventListener(Event.COMPLETE, this.onComplete);
            this._socket.addEventListener(Event.CLOSE, this.onClose);
            this._socket.addEventListener(ProgressEvent.SOCKET_DATA, this.onRead);
            this._socket.addEventListener(SecurityErrorEvent.SECURITY_ERROR, this.onSecurityError);
            this._socket.addEventListener(IOErrorEvent.IO_ERROR, this.onIOError);
        }
        private void _Str_20244()
        {
            if (this._socket != null)
            {
                this._socket.RemoveEventListener(Event.CONNECT, this.onConnect);
                this._socket.removeEventListener(Event.COMPLETE, this.onComplete);
                this._socket.removeEventListener(Event.CLOSE, this.onClose);
                this._socket.removeEventListener(ProgressEvent.SOCKET_DATA, this.onRead);
                this._socket.removeEventListener(SecurityErrorEvent.SECURITY_ERROR, this.onSecurityError);
                this._socket.removeEventListener(IOErrorEvent.IO_ERROR, this.onIOError);
                if (this._socket.Connected)
                {
                    this._socket.Close();
                }
                this._socket = null;
                this._socket.Connect()
            }
        }

        public void dispose()
        {
            throw new NotImplementedException();
        }

        public bool disposed()
        {
            throw new NotImplementedException();
        }

        public IEncryption getServerToClientEncryption()
        {
            throw new NotImplementedException();
        }

        public bool init(string arg1, uint arg2 = 0)
        {
            throw new NotImplementedException();
        }

        public void isAuthenticated()
        {
            throw new NotImplementedException();
        }

        public void isConfigured()
        {
            throw new NotImplementedException();
        }

        public void processReceivedData()
        {
            throw new NotImplementedException();
        }

        public void registerMessageClasses(IMessageConfiguration arg1)
        {
            throw new NotImplementedException();
        }

        public void removeMessageEvent(IMessageEvent arg1)
        {
            throw new NotImplementedException();
        }

        public bool send(IMessageComposer arg1)
        {
            throw new NotImplementedException();
        }

        public bool sendUnencrypted(IMessageComposer arg1)
        {
            throw new NotImplementedException();
        }

        public void setEncryption(IEncryption arg1, IEncryption _arg_2)
        {
            throw new NotImplementedException();
        }

        public void timeout(int arg1)
        {
            throw new NotImplementedException();
        }
    }
}
