using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class MonsterGenerator
    {
        //              Initilization
        //=====================================================
        private string[] name = { "Robot", "Dragon", "Slime", "Bat", "Donkey", "James" };
        private string[] description = { "\na Robot", "\nyour worst nightmare", "\nslimey", "\nthe one who knocks", "\njust a normal Donkey...", "\na high Level Threat: AI capable of Destroying the world" };
        
        //constructor
        public MonsterGenerator()
        {

        }

        /// <summary>
        /// GenerateMonster generates a monster
        /// </summary>
        /// <returns>monster object</returns>
        public Monster GenerateMonster()
        {
            //              Initilization
            //=====================================================
            Random random = new Random();
            int arrayNumber = random.Next(name.Length);
            string Name = name[arrayNumber];
            int Damage = random.Next(10,30);
            int Health = 100;
            string Description = description[arrayNumber];

            return new Monster(Name, Damage, Health, Description);
        }
    }
}
