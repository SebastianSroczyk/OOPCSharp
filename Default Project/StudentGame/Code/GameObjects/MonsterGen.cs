using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects
{
    internal class MonsterGen
    {
        private string[] names = { "Goblin", "Ork", "Witch" };  // Random name options
        private Vector2 spawnLocation; // The location the monsters will spawn from

        public MonsterGen() 
        {
            spawnLocation = new Vector2(100, 100);
        }

        public MonsterGen(Vector2 spawnLocation)
        {
            this.spawnLocation = spawnLocation;
        }

        public Monster generateMonster()
        {
            Random r = new Random();

            Console.WriteLine("Creating Monster");
            Monster m = new Monster();

            // Create a monster
            m._damage = r.Next(50) + 75;
            m._monsterName = names[r.Next(3)];
            m._health = r.Next(50 + 25);
            m.SetPosition(spawnLocation);

            return m;
        }
    }
}
