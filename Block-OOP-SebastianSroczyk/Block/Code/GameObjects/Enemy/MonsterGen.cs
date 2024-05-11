using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block.Code.GameObjects.Enemy
{
    internal class MonsterGen
    {
        private string[] names = { "Goblin", "Ork", "Witch" };  // Random name options
        public int xPos { get; private set; } // The location the monsters will spawn from
        public int yPos { get; private set; }

        private int numOfNamesArray;

        public int MaxDamage = 100;
        private int MaxHealth = 500;
        public MonsterGen()
        {

            xPos = 100;
            yPos = 100;
            numOfNamesArray = names.Length;


        }
        public Monster GenerateMonster()
        {
            Random r = new Random();
            //Console.WriteLine("Creating Monster");
            Monster m = new Monster(names[r.Next(numOfNamesArray)], r.Next(MaxHealth), r.Next(MaxDamage));

            return m;
        }
        public Monster GenerateMonster(Vector2 spawnLocation)
        {
            Random r = new Random();

            //Console.WriteLine("Creating Monster");
            Monster m = new Monster(names[r.Next(numOfNamesArray)], r.Next(MaxHealth), r.Next(MaxDamage));


            // Create a monster
            m.SetPosition(spawnLocation);

            return m;
        }
    }
}
