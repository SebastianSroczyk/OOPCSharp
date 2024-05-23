using MonoGameEngine.StandardCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class InventoryItem : GameObject
    {
        public int Size { get; set; }
        public string Name { get; set; }

        protected float Speed = 10;

        public InventoryItem() 
        {
            Size = 0;
            Name = "Default Item";
        }

        public InventoryItem(int size, string name)
        {
            this.Size = size;
            this.Name = name;
        }

        /// <summary>
        /// This method moves the item.
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Move(float deltaTime)
        {
            
        }

        public override void Update(float deltaTime)
        {
           Move(deltaTime);
        }
    }
}
