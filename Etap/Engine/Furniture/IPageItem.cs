using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Furniture
{
    enum PriceTypes { CREDITS = 1, DUCKETS = 0, DIAMONDS = 5, GOTW = 103 };

    interface IPageItem
    {
        int GetCredits();
        int GetDiamonds();
        int GetDuckets();
        int GetGOTW();
        int GetSpriteId();
        int GetItemId();
        int[] GetPrices();
        PriceTypes[] GetPricesTypes();

        bool CostsDiamonds();
        bool CostsDuckets();
        bool CoststGOTW();
        bool CanGift();

        string Name();
        string ExtraData();
        int Id();

        void SetSpriteId(int sprite);
        bool IsDubbelPriced();
    }
}
