using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class Coin : InventoryItem
    {
        public int healthPoints { get; set; }

        // SET SPRITE OF OBJECT
        //----------------------------------------
        private string sprite = "potion";
        private int[] coinValue = { 10, 20, 30 };

        public int[] CoinValue { get { return coinValue; } }

        //Default constructor
        public Coin() : base()
        {
            healthPoints = 0;
            Size = 64;
            Name = "NO Coin";

            SetSprite(sprite);
            SetBounds(this.Size, this.Size);
        }

        // Overloaded Constructor
        public Coin(int size, string name):
            base(size, name)
        {
            
            this.Size = size;
            this.Name = name;

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
