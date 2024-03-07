using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class Weapon : InventoryItem
    {
        public int hitPoints {get; set;}
        public Weapon() : base()
        { 
            hitPoints = 0;
        }

        public Weapon(int size, string name, string description, int hitpoints):
            base(size, name, description)
        {
            this.hitPoints = hitpoints;
        }

        public override void Use(Player p)
        {
            base.Use(p); 
            
            p.damage = hitPoints;
        }
    }
}
