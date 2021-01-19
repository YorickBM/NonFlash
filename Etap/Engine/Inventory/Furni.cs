using Etap.ImagesCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Inventory
{
    class Furni : IInventoryItem
    {
        private int _itemId;
        private int _spriteId;

        private string _itemType;

        private bool _canTrade;
        private bool _canRecycle;
        private bool _canStack;
        private bool _isRare;
        private bool _isWallItem;

        public Furni(int itemId, int spriteId, string itemType, bool canTrade, bool canRecycle, bool canStack, bool isRare, bool isWallItem)
        {
            _itemId = itemId;
            _spriteId = spriteId;

            _itemType = itemType;

            _canTrade = canTrade;
            _canRecycle = canRecycle;
            _canStack = canStack;
            _isRare = isRare;
            _isWallItem = isWallItem;
        }

        public int GetItemId()
        {
            return _itemId;
        }
        public int GetSpriteId()
        {
            return _spriteId;
        }

        public string GetFurniClass()
        {
            if(GameScreenManager.Instance.GetFurniTypeBySpriteId(_spriteId) != null) return GameScreenManager.Instance.GetFurniTypeBySpriteId(_spriteId).classname;
            return String.Empty;
        }
        public string GetFurniName()
        {
            if (GameScreenManager.Instance.GetFurniTypeBySpriteId(_spriteId) != null) return GameScreenManager.Instance.GetFurniTypeBySpriteId(_spriteId).name;
            return "#N/A";
        }

        public bool CanStack()
        {
            return _canStack;
        }
        public bool CanTrade()
        {
            return _canTrade;
        }
        public bool CanRecycle()
        {
            return _canRecycle;
        }
    }
}
