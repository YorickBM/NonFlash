namespace Etap.EtapEngine
{
    public class SessionClient
    {
        private readonly int _id;
        private readonly string _username;

        public SessionClient(int id, string username)
        {
            _id = id;
            _username = username;
        }

        public string getUsername()
        {
            return _username;
        }
        public int getId()
        {
            return _id;
        }
    }
}
