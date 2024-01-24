using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class PlayerGenerator
    {
        //              Initilization
        //===================================================== 

        private string[] name = { "David", "Josh", "Lucy", "Bart", "Henry", "Steve" };
        private string[] title = { "DaBaby", "Bard", "Warlock", "Barbarian", "Druid", "Paladin", "Wizard"  };

        // constructor
        public PlayerGenerator()
        {
            
        }

        /// <summary>
        /// GeneratePlayer Generates the player
        /// </summary>
        /// <returns>player object</returns>
        public Player GeneratePlayer()
        {
            //              Initilization
            //=====================================================
            Random random = new Random();
            string Name = name[random.Next(name.Length)];
            string Title = title[random.Next(title.Length)];
            int Health = 100;
            MagicKey? magicKey = null;
            Weapon? WeaponHeld = null;
            


            return new Player(Name, Title, Health, magicKey, WeaponHeld);
        }
    }
}
