using StudentGame.Code.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.Inventory
{
    internal class InventoryItem
    {

        private Player player;
        public int size {  get; set; }
        public string name { get; set; }

        public string description { get; set; }

        public InventoryItem()
        {

        }
        public InventoryItem(int size, string name, string description)
        {
            this.size = size;
            this.description = description;
            this.name = name;
        }
        public virtual void USE(Player p)
        {

        }

    }
}
