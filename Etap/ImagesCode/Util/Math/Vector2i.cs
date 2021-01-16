using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    class Vector2i
    {
        internal int X { get; set; }
        internal int Y { get; set; }

        public Vector2i(float x, float y)
        {
            X = (int)Math.Ceiling(x);
            Y = (int)Math.Ceiling(y);
        }
        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Vector2i(double x, double y)
        {
            X = (int)Math.Ceiling(x);
            Y = (int)Math.Ceiling(y);
        }
        public Vector2i()
        {
            X = 0;
            Y = 0;
        }

        public int getX() { return X; }
        public int getY() { return Y; }

        public static implicit operator Vector2(Vector2i myClass)
        {
            return new Vector2(myClass.X, myClass.Y);
        }
        public static Vector2i operator +(Vector2i left, Vector2i right)
        {
            return new Vector2i(left.X + right.X, left.Y + right.Y);
        }
        public static Vector2i operator -(Vector2i left, Vector2i right)
        {
            return new Vector2i(left.X - right.X, left.Y - right.Y);
        }
        public static implicit operator string(Vector2i myClass)
        {
            return myClass.X + ";" + myClass.Y;
        }
    }
}
