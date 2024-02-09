using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.Inventory
{
    internal class InventoryManager
    {
        private InventoryItem InventoryItem;

        public InventoryManager() 
        {
            
        }

        public void CreateInventoryItem()
        {
            InventoryItem = new InventoryItem();
        }
    }
}
