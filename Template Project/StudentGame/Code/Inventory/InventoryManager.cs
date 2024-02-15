using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.Inventory
{
    internal class InventoryManager
    {
        public int _inventorySize {  get; set; }
        private int _inventoryUsed;

        // Array of Items

        public InventoryItem[] _inventoryItems { get; }


        // Default Invetory
        public InventoryManager() 
        {
            _inventorySize = 20;
            _inventoryUsed = 0;

            _inventoryItems = new InventoryItem[_inventorySize];
        }
        public InventoryManager(int inventorySize)
        {
            _inventorySize = inventorySize;
            _inventoryUsed = 0;
            _inventoryItems = new InventoryItem[_inventorySize];
        }

        public bool addItem(InventoryItem item)
        {
            bool success = false;

            if (_inventoryUsed <  _inventorySize)
            {
                _inventoryItems[_inventoryUsed] = item;
                _inventoryUsed++;

                success = true;
            }
            return success;
        }
        
        public bool removeItem(int itemIndex)
        {
            bool success = false;

            if (_inventoryItems[itemIndex] == null)
            {
                success = false;
            }
            else
            {
                _inventoryItems[itemIndex] = null;
                success = true;
            }
            return success;
        }

        public InventoryItem TakeItem(int itemIndex)
        {
            return _inventoryItems[itemIndex];
        }
    }
}
