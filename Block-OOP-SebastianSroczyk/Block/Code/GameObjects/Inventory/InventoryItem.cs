using MonoGameEngine.StandardCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block.Code.GameObjects.Inventory
{
    internal class InventoryItem : GameObject
    {
        public int Size { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        

        public InventoryItem() 
        {
            Size = 0;
            Name = "Default Item";
            Description = "Default Description";
        }

        public InventoryItem(int size, string name, string description )
        {
            this.Size = size;
            this.Name = name;
            this.Description = description;
        }

        public virtual void Use(Player p)
        {
            
        }

        public override void Update(float deltaTime)
        {
           
        }
    }
}
