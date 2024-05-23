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
        private int _firstItemIndex = 1;
        private int _currentItem;
        
        public bool _displayInventory { get; set; }

        //Array of items
        public InventoryItem[] _inventoryItems { get; }

        

        // Default inventory manager constructor
        public InventoryManager() 
        {
            _inventorySize = 10;
            _currentItem = _firstItemIndex;
            
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
            _inventorySize = size + 1;
            _inventoryItems = new InventoryItem[_inventorySize];
        }
        
        /// <summary>
        /// Searches the Array and adds an item into the empty slots
        /// </summary>
        /// <param name="item"></param>
        /// <returns> The item the we are wanting to add</returns>
        public bool AddItem(InventoryItem item)
        {
            bool success = false;
            int emptySlot = FindEmptyItemSlot();

            if (emptySlot > _inventorySize)
            {
                success = false;
                
            }
            else if (emptySlot == 0)
            {
                success = false;
            }
            else
            {
                
                _inventoryItems[emptySlot] = item;
                success = true;
            }
            return success;
            
        }
        
        /// <summary>
        /// Finds an emptry slot in the inventroy and sets the next item slot to that slot
        /// </summary>
        /// <returns></returns>
        public int FindEmptyItemSlot()
        {
            int selectedElement = 0;

            // loops through the list
            for(int i = _firstItemIndex; i < _inventorySize; i++)
            {
                if(_inventoryItems[i] == null)
                {
                    selectedElement = i; 
                    break;
                }
            }
            return selectedElement;
        }

        /// <summary>
        /// Checks if the inventory is empty
        /// </summary>
        /// <returns></returns>
        public bool CheckIfEmpty()
        {
            int numEmptySlot = 0;
            
            for (int i = 1; i < _inventorySize; i++)
            {
                if (_inventoryItems[i] == null)
                {
                    numEmptySlot++;
                    
                }
                if (_inventorySize - 1 == numEmptySlot)
                {
                    //Console.WriteLine("Array is Empty");
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Gets the next item
        /// </summary>
        /// <returns></returns>
        public InventoryItem GetNextItem()
        {
            _currentItem++;

            if(_currentItem == _inventorySize)
                _currentItem = _firstItemIndex;

            if (_inventoryItems[_currentItem] == null)
                _currentItem = _firstItemIndex;

            return _inventoryItems[_currentItem];
        }

        /// <summary>
        /// Get the previous item
        /// </summary>
        /// <returns></returns>
        public InventoryItem GetPreviousItem()
        {
            _currentItem--;

            if (_currentItem < 0)
                _currentItem = _inventorySize - 1;

            return _inventoryItems[_currentItem];
        }

        /// <summary>
        /// Removes the Selected item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(InventoryItem item) 
        {
            _inventoryItems[_currentItem] = item;
            
            bool success = false;

            // checks if item isn't null ( i.e. if slot is populated)
            if (_inventoryItems[_currentItem] == null)
            {
                success = false;
            }
            else
            {
                _inventoryItems[_currentItem] = null;
                success = true;
            }

            return success;

        }

        /// <summary>
        /// Removes the item via item Index
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public bool RemoveItem(int itemIndex) 
        {
            bool success = false;

            // checks if item index isn't null ( i.e. if slot is populated)
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
            


        /**
         * Draw the inventory as an overlay of the game
         */
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            
        }

        // List of items on screen
        private List<string> _itemTextOnScreen = new List<string>();

        /// <summary>
        /// Displays the Inventory
        /// </summary>
        public void DisplayInventory()
        {
            // toggles display inventory
            _displayInventory = !_displayInventory;

            // checks if inventory is displaying
            if (_displayInventory == true)
            {
                // adds text to screen
                AddTextToScreen();
            }
            else
            {
                //Console.WriteLine("Inventory manager removing text");
                // removes text from screen
                RemoveTextToScreen();
            }
        }

        /// <summary>
        /// Adds Text to screen
        /// </summary>
        private void AddTextToScreen()
        {
            // adds text to screen
            GetScreen().AddText("Inventory", new Text("Inventory"), 100, 100);

            // loops for the length of items in inventory
            for (int i = _firstItemIndex; i < _inventoryItems.Length; i++)
            {
                // checks if items arn't null
                if (_inventoryItems[i] != null)
                {
                    // sets the item id
                    string id = _inventoryItems[i].GetHashCode().ToString(); // Get a unique reference for the item
                    // adds itm if to lsit of items ojn screen
                    _itemTextOnScreen.Add(id);
                    // check if index is the current item
                    if (i == _currentItem)
                    {
                        // adds text 
                        GetScreen().AddText(id, new Text(_inventoryItems[i].Name, new Color(255, 0, 0)), 100, 200 + (i * 20));
                    }
                    else
                    {
                        // adds text 
                        GetScreen().AddText(id, new Text(_inventoryItems[i].Name), 100, 200 + (i * 20));
                    }
                        
                }
            }
        }

        /// <summary>
        /// Removes the text from the screen
        /// </summary>
        private void RemoveTextToScreen()
        {
            //removes the "Inventory" text from screen
            GetScreen().RemoveText("Inventory");

            // Loops thorugh all items in the list 
            foreach (var item in _itemTextOnScreen)
            {
                // removes said items 
                GetScreen().RemoveText(item);
            }
            // clears the list
            _itemTextOnScreen.Clear();
        }
        
        public override void Update(float deltaTime)
        {
            
        }

        
    }
}
