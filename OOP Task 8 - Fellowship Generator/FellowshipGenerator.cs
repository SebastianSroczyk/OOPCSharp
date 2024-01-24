using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_8___Fellowship_Generator
{
    internal class FellowshipGenerator
    {
        Character[] members = new Character[5];
        public FellowshipGenerator() 
        {
            
        }

        private void GenerateFellowship()
        {
            CharacterGenerator CG = new CharacterGenerator();

            for (int i = 0; i < 5; i++)
            {
                members[i] = CG.GenerateCharacter();
            }      
        }
        public void DisplayFellowship()
        {
            GenerateFellowship();

            for (int i = 0;i < members.Length;i++)
            {
                members[i].DisplayAttributes();
            }
        }

    }
}
