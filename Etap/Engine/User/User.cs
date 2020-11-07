using Etap.Engine.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap.Engine.User
{
    public class Currency
    {
        internal int _value;

        public Currency(int value)
        {
            _value = value;
        }

        public void Add(int value) { _value += value; }
        public void Take(int value) { _value -= value; }
        public void Set(int value) { _value = value; }

        public int Amount() { return _value; }

        public static implicit operator int(Currency myClass)
        {
            return myClass.Amount();
        }
        public static implicit operator String(Currency myClass)
        {
            return myClass.Amount() + "";
        }
    }
    public class User
    {
        internal Currency _credits, _duckets, _diamonds, _gotw;

        public User(int credits, int duckets, int diamonds, int gotw) {
            _credits = new Currency(credits);
            _duckets = new Currency(duckets);
            _diamonds = new Currency(diamonds);
            _gotw = new Currency(gotw);
        }

        public Currency GetCredits() { return _credits; }
        public Currency GetDuckets() { return _duckets; }
        public Currency GetDiamonds() { return _diamonds; }
    }
}
