using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Furniture
{
    class PageMeubiItem : IPageItem
    {
        Dictionary<int, int> prices;

        bool isRentable;
        bool canGift;

        int id;
        string name;
        string extraData;
        int spriteId;

        public PageMeubiItem(int id, string name, bool canRent, bool canGift, int credits, int extraIdeniftyer, int extraCosts)
        {
            prices = new Dictionary<int, int>();
            prices.Add(1, credits);
            prices.Add(extraIdeniftyer, extraCosts);

            this.id = id;
            this.name = name;
            this.canGift = canGift;
            this.isRentable = canRent;
        }

        public void AddExtraData(string data)
        {
            extraData = data;
        }

        public int Id() {  return this.id; }
        public string Name() { return this.name; }

        public bool CostsDuckets()
        {
            return prices.ContainsKey((int)PriceTypes.DUCKETS);
        }
        public bool CostsDiamonds()
        {
            return prices.ContainsKey((int)PriceTypes.DIAMONDS);
        }
        public bool CoststGOTW()
        {
            return prices.ContainsKey((int)PriceTypes.GOTW);
        }
        public bool IsDubbelPriced()
        {
            if(GetCredits() < 1)
                return false;

            if (GetDuckets() > 0 || GetDiamonds() > 0 || GetGOTW() > 0)
                return true;

            return false;
        }
        public bool CanGift()
        {
            return canGift;
        }

        public int GetCredits()
        {
            int value = 0;
            prices.TryGetValue((int)PriceTypes.CREDITS, out value);
            return value;
        }
        public int GetDiamonds()
        {
            int value = 0;
            prices.TryGetValue((int)PriceTypes.DIAMONDS, out value);
            return value;
        }
        public int GetDuckets()
        {
            int value = 0;
            prices.TryGetValue((int)PriceTypes.DUCKETS, out value);
            return value;
        }
        public int GetGOTW()
        {
            int value = 0;
            prices.TryGetValue((int)PriceTypes.GOTW, out value);
            return value;
        }
        public int GetItemId()
        {
            return id;
        }
        public int[] GetPrices()
        {
            if (IsDubbelPriced())
                if(GetDiamonds() > 0)
                    return new int[2] { GetCredits(), GetDiamonds() };
                else if (GetDuckets() > 0)
                    return new int[2] { GetCredits(), GetDuckets() };
                else if (GetGOTW() > 0)
                    return new int[2] { GetCredits(), GetGOTW() };

            if(!IsDubbelPriced())
                if(GetCredits() > 0)
                    return new int[2] { GetCredits(), 0 };
                else if (GetDiamonds() > 0)
                    return new int[2] { GetDiamonds(), 0 };
                else if (GetDuckets() > 0)
                    return new int[2] { GetDuckets(), 0 };
                else if (GetGOTW() > 0)
                    return new int[2] { GetGOTW(), 0 };

            return new int[2] { 0, 0 };
        }
        public PriceTypes[] GetPricesTypes()
        {
            if (IsDubbelPriced())
                if (GetDiamonds() > 0)
                    return new PriceTypes[2] { PriceTypes.CREDITS, PriceTypes.DIAMONDS };
                else if (GetDuckets() > 0)
                    return new PriceTypes[2] { PriceTypes.CREDITS, PriceTypes.DUCKETS };
                else if (GetGOTW() > 0)
                    return new PriceTypes[2] { PriceTypes.CREDITS, PriceTypes.GOTW };

            if (!IsDubbelPriced())
                if (GetCredits() > 0)
                    return new PriceTypes[2] { PriceTypes.CREDITS, 0 };
                else if (GetDiamonds() > 0)
                    return new PriceTypes[2] { PriceTypes.DIAMONDS, 0 };
                else if (GetDuckets() > 0)
                    return new PriceTypes[2] { PriceTypes.DUCKETS, 0 };
                else if (GetGOTW() > 0)
                    return new PriceTypes[2] { PriceTypes.GOTW, 0 };

            return new PriceTypes[2] { PriceTypes.CREDITS, 0 };
        }
        public string ExtraData() { return extraData; }

        public int GetSpriteId() { return spriteId; }
        public void SetSpriteId(int sprite) { spriteId = sprite; }
    }
}
