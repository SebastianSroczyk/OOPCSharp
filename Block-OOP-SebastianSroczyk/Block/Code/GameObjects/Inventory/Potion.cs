using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block.Code.GameObjects.Inventory
{
    internal class Potion : InventoryItem
    {
        public int healthPoints { get; set; }

        // SET SPRITE OF OBJECT
        //----------------------------------------
        private string sprite = "potion";



        //Default constructor
        public Potion() : base()
        {
            healthPoints = 0;
            Size = 64;
            Name = "NO WEAPON";
            Description = "NO DESCRIPTION";

            SetSprite(sprite);
            SetBounds(this.Size, this.Size);
        }

        // Overloaded Constructor
        public Potion(int size, string name, string description,  int healthPoints):
            base(size, name, description)
        {
            
            this.Size = size;
            this.Name = name;
            this.Description = description;
            this.healthPoints = healthPoints;

            SetSprite("potion");
            SetBounds(this.Size, this.Size);

        }

        //Apply the health potion to the player

        public override void Use(Player p) 
        {
            p.health = healthPoints;
        }

    }
}
