using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{   
    internal class InventoryManager : GameObject
    {
        //Size of inventory
        public int _inventorySize { get; set; }
        private int _inventoryUsed;
        private int _currentItem;
        public bool _displayInventory { get; set; }

        //Array of items
        public InventoryItem[] _inventoryItems { get; }

        // Default inventory manager constructor
        public InventoryManager() 
        {
            _inventorySize = 10;
            _inventoryUsed = 0;
            _currentItem = 0;
            
            _inventoryItems = new InventoryItem[_inventorySize];
            SetSprite("pixel"); // set a sprite for the inventory
            SetVisible(true);
        }

        /**
         * The Overloaded constructor take a size
         *  The combined size of all items in the inventory should not
         *  exceed the size... this does not necessarily correspond
         *  to the number of items in the array.
         */
        public InventoryManager(int size)
        {
            _inventorySize = size;
            _inventoryUsed = 0;

            _inventoryItems = new InventoryItem[_inventorySize];
        }


        public bool AddItem(InventoryItem item)
        {
            bool success = false;

            if(_inventoryUsed < _inventorySize)
            {
                _inventoryItems[_inventoryUsed] = item;
                _inventoryUsed++;                
                success = true;
            }

            return success;
        }
        
        /**
         * Find an empty item slot
         * Only required if we use an array
         * A list or map would work without this mechanism
         */
        public int FindEmptyItemSlot()
        {
            int selectedElement = 0;

            for(int i = 0; i < _inventorySize; i++)
            {
                if(_inventoryItems[i] == null)
                {
                    selectedElement = i; 
                    break;
                }
            }
            return selectedElement;
        }

        public InventoryItem GetNextItem()
        {
            _currentItem++;

            if(_currentItem == _inventorySize)
                _currentItem = 0;

            if (_inventoryItems[_currentItem] == null)
                _currentItem = 0;

            return _inventoryItems[_currentItem];
        }

        public InventoryItem GetPreviousItem()
        {
            _currentItem--;

            if (_currentItem < 0)
                _currentItem = _inventorySize - 1;

            return _inventoryItems[_currentItem];
        }
        public bool RemoveItem(InventoryItem item) 
        {

            return false;
        }

        /**
         * Remove and item from the inventory - to do this we need a key or an index
         * the index will indicate which item needs to be removed.
         * 
         */
        public bool RemoveItem(int itemIndex) 
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


        /**
         * Draw the inventory as an overlay of the game
         */
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            if (_displayInventory == true)
            {
                GetScreen().AddText("Title", new Text("Inventory"), 100, 100);

                for (int i = 0; i < _inventoryItems.Length; i++)
                {
                    if (_inventoryItems[i] != null)
                    {
                        string id = _inventoryItems[i].GetHashCode().ToString(); // Get a unique reference for the item
                        if (i == _currentItem)
                            GetScreen().AddText(id, new Text(_inventoryItems[i].Name, new Color(255, 0, 0)), 100, 200 + (i* 20));
                        else
                            GetScreen().AddText(id, new Text(_inventoryItems[i].Name), 100, 200 + (i * 20));
                    }
                }
            }
            else 
            {
                GetScreen().RemoveAllText();
            }
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
