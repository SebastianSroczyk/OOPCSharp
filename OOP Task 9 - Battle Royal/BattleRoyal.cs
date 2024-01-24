using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_9___Battle_Royal
{
    internal class BattleRoyal
    {
        Character[] members = new Character[2];
        bool play = true;

        public BattleRoyal() 
        {
            
        }
        private void GenerateFighters()
        {
            CharacterGenerator CG = new CharacterGenerator();

            for (int i = 0; i < 2; i++)
            {
                members[i] = CG.GenerateCharacter();
            }      
        }
        private void Attack(Character attacking, Character defending)
        {
            int attackDamage = (attacking.characterAttack - defending.characterDefence);
            defending.characterHealth -= (attackDamage);
            Console.WriteLine("Damage inflicted: " + attackDamage);

        }
        private void BattleCalculations()
        {

            int round = 0;
 
            while (members[0].characterHealth > 0 && members[1].characterHealth > 0)
            {
                Random random = new Random();
                round++;
                Console.WriteLine(" \nRound " + round + "\n-------");

                switch(random.Next(1,3))
                {
                    case 1:
                        {
                            Console.WriteLine(members[0].GetCharacterName() + " is Attacking" + "\nAttack Roll: " + members[0].characterAttack + "\n\n" + members[1].GetCharacterName() + " is Defending" + "\nDefence Roll: " + members[0].characterDefence + "\n");
                            Attack(members[0], members[1]);
                            Console.WriteLine(members[0].GetCharacterName() + " Health: " + members[0].characterHealth + "\n" + members[1].GetCharacterName() + " Health: " + members[1].characterHealth);
                            
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine(members[1].GetCharacterName() + " is Attacking" + "\nAttack Roll: " + members[1].characterAttack + "\n\n" + members[0].GetCharacterName() + " is Defending" + "\nDefence Roll: " + members[1].characterDefence + "\n");
                            Attack(members[1], members[0]);
                            Console.WriteLine(members[1].GetCharacterName() + " Health: " + members[1].characterHealth + "\n" + members[0].GetCharacterName() + " Health: " + members[0].characterHealth);
                            
                            break;
                        }
                }
            }

        }
        private void DisplayContestants()
        {
            Console.WriteLine("Contestant 1");
            members[0].DisplayAttributes();
            Console.WriteLine("Contestant 2");
            members[1].DisplayAttributes();
        }
        private void Battle()
        {
            while (play == true)
            {
                Console.Clear();
                GenerateFighters();
                Console.WriteLine("      BATTLE ROYAL     \n-----------------------\n\nPress Enter to Begin...");
                Console.ReadLine();
                Console.Clear();
                DisplayContestants();
                Console.WriteLine("Press ENTER to fight...\n-----------------------");
                Console.ReadLine();
                Console.Clear();
                BattleCalculations();
                if (members[0].characterHealth <= 0)
                {
                    Console.WriteLine("\n=================\n  WINNER: " + members[1].GetCharacterName() + "\n=================");
                }
                if (members[1].characterHealth <= 0)
                {
                    Console.WriteLine("\n=================\n  WINNER: " + members[0].GetCharacterName()+ "\n=================");
                }
                play = false;
                Console.WriteLine("\n\nWould you like to play again? (Y/N): ");
            }
            
   
        }
        public void Play()
        {
            while (play == true)
            {
                Battle();
            }
            while (play == false)
            {


                if (Console.ReadLine() == "y")
                {
                    play = true;
                    Battle();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Thank you for playing!");
                    Environment.Exit(0);
                    
                }
            }
            
            
            
            
        }
    }
}
