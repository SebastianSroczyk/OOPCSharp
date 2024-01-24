using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_9___Battle_Royal
{
    internal class CharacterGenerator
    {
        
        private string[] names = {"Bob", "Steve", "Conan", "Mary", "Jimmy", "Colin", "Bessie", "Josh", "Karim", "Fiona" };
        private string[] classes = { "Priest", "Paladin", "Mage", "Wizard", "Adventurer", "Theif", "Bard", "Executioner", "Peasant" };

        public CharacterGenerator()
        {
            
        }
        public Character GenerateCharacter()
        {
            Random random = new Random();
            string charName = names[random.Next(names.Length)];
            string charClass = classes[random.Next(classes.Length)];
            int charHealth = random.Next(80,140);
            int charAttack = random.Next(4, 10);
            int charDefence = random.Next(2, 4);

            return new Character(charName, charClass, charHealth,charAttack,charDefence);
        }

    }

   
}
