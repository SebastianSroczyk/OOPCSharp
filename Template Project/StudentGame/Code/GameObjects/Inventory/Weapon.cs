using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class Weapon : InventoryItem
    {
        
        public int HitPoints {get; private set;}

        // SET SPRITE OF OBJECT
        //----------------------------------------
        private string sprite = "col1";


        //Default constructor
        public Weapon() : base()
        {
            HitPoints = 0;
            Size = 64;
            Name = "NO WEAPON";
            Description = "NO DESCRIPTION";

            SetSprite(sprite);
            SetBounds(this.Size, this.Size);
        }

        //Default constructor
        public Weapon(int size, string name, string description, int hitpoints):
            base(size, name, description)
        {
            this.Size = size;
            this.Name = name;
            this.Description = description;
            this.HitPoints = hitpoints;

            SetSprite(sprite);
            SetBounds(this.Size, this.Size);
        }

        // APPLY DAMAGE TO PLAYER
        public override void Use(Player p)
        {
            base.Use(p); 
            
            p.damage = HitPoints;
        }
    }
}
