using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_8___Fellowship_Generator
{
    internal class Character
    {
        private string characterName { get; set; }
        private string characterClass { get; set; }
        private int characterHealth { get; set; }


        // Defualt Constructor
        public Character()
        {
            characterName = "NO NAME";
            characterClass = "NO CLASS";
            characterHealth = 0;
        }
        // Parameterised Constructor
        public Character(string characterName, string characterClass, int characterHealth)
        {
            this.characterName = characterName;
            this.characterClass = characterClass;
            this.characterHealth = characterHealth;
        }

        public void DisplayAttributes()
        {
            Console.WriteLine("Name: " + characterName + "\nClass: " + characterClass + "\nHealth: " + characterHealth + "\n");
        }
    }

    
}
