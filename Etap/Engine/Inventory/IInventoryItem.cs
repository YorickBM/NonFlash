using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Engine.Inventory
{
    interface IInventoryItem
    {
        string GetFurniClass();
        int GetItemId();
    }
}
