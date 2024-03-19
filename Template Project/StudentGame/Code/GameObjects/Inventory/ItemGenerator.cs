using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class ItemGenerator
    {
        private string[] potionNames = { "Vodka", "Water", "blood" };  // Random name options
        private string[] potionDescriptions = { "yes", "H20", "ew" };
        private string[] weaponNames = { "Gun", "Sword", "Hammer" };  // Random name options
        private string[] weaponDescriptions = { "Shoots", "Swords", "Hammers" };

        public int objectXPos { get; private set; } // The location the monsters will spawn from
        public int objectYPos { get; private set; }

        private int numItemsInPotionArray;
        private int numItemsInWeaponArray;
        public ItemGenerator()
        {
            objectXPos = 100;
            objectYPos = 100;
            numItemsInPotionArray = potionNames.Length;
            numItemsInWeaponArray = weaponNames.Length;

            if (potionNames.Length != potionDescriptions.Length || weaponNames.Length != weaponDescriptions.Length)
            {
                throw new ArgumentOutOfRangeException("Either the length of the potion arrays or weapon arrays don't match.");
            }
        }

        public Potion GeneratePotion()
        {
            Random r = new Random();

            Console.WriteLine("Creating Items");
            Potion p = new Potion(64, potionNames[r.Next(numItemsInPotionArray)], potionDescriptions[r.Next(numItemsInPotionArray)],r.Next(30,100));
            objectXPos = r.Next(20, 1600);
            objectYPos = r.Next(20, 800);
            p.SetPosition(objectXPos,objectYPos);

            return p;
        }
        public Potion GeneratePotion(Vector2 spawnLocation)
        {
            Random r = new Random();

            Console.WriteLine("Creating Items");
            Potion p = new Potion(64, potionNames[r.Next(numItemsInPotionArray)], potionDescriptions[r.Next(numItemsInPotionArray)], r.Next(30, 100));
            p.SetPosition(spawnLocation);

            return p;
        }
        public Weapon GenerateWeapon()
        {
            Random r = new Random();

            Console.WriteLine("Creating Items");
            Weapon w = new Weapon(64, weaponNames[r.Next(numItemsInWeaponArray)], weaponDescriptions[r.Next(numItemsInWeaponArray)], r.Next(30, 100));
            objectXPos = r.Next(20, 800);
            objectYPos = r.Next(20, 800);
            w.SetPosition(objectXPos, objectYPos);

            return w;
        }
        public Weapon GenerateWeapon(Vector2 spawnLocation)
        {
            Random r = new Random();

            Console.WriteLine("Creating Items");
            Weapon w = new Weapon(64, weaponNames[r.Next(numItemsInWeaponArray)], weaponDescriptions[r.Next(numItemsInWeaponArray)], r.Next(30, 100));
            w.SetPosition(spawnLocation);

            return w;
        }

        
    }
}
