using StudentGame.Code.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.Inventory
{
    internal class Weapon : InventoryItem
    {
        Player player;
        public int hitPoints {  get; set; }

        public Weapon() : base()
        {
            hitPoints = 0;
        }

        public Weapon(int size,string name, string description, int hitpoints) :
            base(size, name, description)
        {
            this.hitPoints = hitpoints;
        }

        public void USE(Player p)
        {
            base.USE(p);
            p.PlayerDamage = hitPoints;
        }
    }
}
