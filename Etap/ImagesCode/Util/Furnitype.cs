using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Util
{
    [Serializable]
    public class Furnidata
    {
        [XmlArrayItem("furnitype")]
        public Furnitype[] roomitemtypes;
    }

    [Serializable]
    public class Furnitype
    {
        int _id;
        String _classname;
        int _defaultdir;
        int _xdim;
        int _ydim;
        String _name;
        string _description;
        int _bc;
        bool _canstandon;
        bool _cansiton;
        bool _canlayon;
        int _interactions;
        int _rotations;

        ColorData _color;

        int _offsetX;
        int _offsetY;

        public int id { get => _id; set => _id = value; }
        public string classname { get => _classname; set => _classname = value; }
        public int defaultdir { get => _defaultdir; set => _defaultdir = value; }
        public int xdim { get => _xdim; set => _xdim = value; }
        public int ydim { get => _ydim; set => _ydim = value; }
        public string name { get => _name; set => _name = value; }
        public string description { get => _description; set => _description = value; }
        public int bc { get => _bc; set => _bc = value; }
        public bool canstandon { get => _canstandon; set => _canstandon = value; }
        public bool cansiton { get => _cansiton; set => _cansiton = value; }
        public bool canlayon { get => _canlayon; set => _canlayon = value; }
        public int interactions { get => _interactions; set => _interactions = value; }
        public int rotations { get => _rotations; set => _rotations = value; }

        public ColorData color { get => _color; set => _color = value; }
        public int offsetX { get => _offsetX; set => _offsetX = value; }
        public int offsetY { get => _offsetY; set => _offsetY = value; }
    }

    public class ColorData
    {
        public int r;
        public int g;
        public int b;

        public ColorData(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public ColorData() {
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }

        public static bool Compare(ColorData data, int r, int g, int b)
        {
            return data.r == r && data.g == g && data.b == b;
        }

        public static implicit operator string(ColorData data)
        {
            return "(" + data.r + ";" + data.g + ";" + data.b + ")";
        }
    }
}
