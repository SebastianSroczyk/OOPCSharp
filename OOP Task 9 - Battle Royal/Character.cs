using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_9___Battle_Royal
{
    internal class Character
    {
        private string characterName { get; set; }
        private string characterClass { get; set; }

        public int characterHealth { get; set; }
        public int characterAttack { get; set; }
        public int characterDefence { get; set; }
        

        


        // Defualt Constructor
        public Character()
        {
            characterName = "NO NAME";
            characterClass = "NO CLASS";
            characterHealth = 0;
            characterAttack = 0;
            characterDefence = 0;
        }
        // Parameterised Constructor
        public Character(string characterName, string characterClass, int characterHealth,int characterAttack, int characterDefence)
        {
            this.characterName = characterName;
            this.characterClass = characterClass;
            this.characterHealth = characterHealth;
            this.characterAttack = characterAttack;
            this.characterDefence = characterDefence;
        }

        public void DisplayAttributes()
        {
            Console.WriteLine("Name: " + characterName + "\nClass: " + characterClass + "\nHealth: " + characterHealth + "\nAttack: " + characterAttack + "\nDefence: " + characterDefence + "\n");
        }
        public string GetCharacterName()
        {
            return characterName;
        }

        
        
    }

    
}
