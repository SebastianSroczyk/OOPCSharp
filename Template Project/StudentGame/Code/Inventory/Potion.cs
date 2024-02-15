using StudentGame.Code.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.Inventory
{
    internal class Potion : InventoryItem
    {
        Player player;
        public int HealthPoints {  get; set; }

        public Potion() :base()
        {
            HealthPoints = 0;            
        }
        public Potion(int size, string name, string description, int healthpoints):
            base (size, name, description) 
        {
            this.HealthPoints = healthpoints;      
        }

        public void USE(Player p)
        {
            p.PlayerHealth += HealthPoints;
        }


    }
}
