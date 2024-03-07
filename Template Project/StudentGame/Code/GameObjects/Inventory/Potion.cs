using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class Potion : InventoryItem
    {

        public int healthPoints { get; set; }

        //Default constructor
        public Potion() : base()
        {
            healthPoints = 0;

            SetSprite("potion");
            SetBounds(64, 64);
        }

        // Overloaded Constructor
        public Potion(int size, string name, string description,  int healthPoints):
            base(size, name, description)
        {
            this.healthPoints = healthPoints;
        }

        //Apply the health potion to the player

        public override void Use(Player p) 
        {
            p.health = healthPoints;
        }

    }
}
