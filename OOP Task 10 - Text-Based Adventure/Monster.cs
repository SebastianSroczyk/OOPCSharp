using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class Monster
    {
        //              Initilization
        //=====================================================
        public string Name {  get; private set; }
        public float Damage { get; private set; }
        private string Description { get; set; }
        private float Health {  get; set; }
        public float PHealth 
        {
            get { return Health; }
            set {
                Health = value;
                    if (Health < 0)
                    {
                        Health = 0;
                    }
                }
        }

        // Defualt Constructor
        public Monster() 
        {
            Name = "noName";
            Damage = 0;
            PHealth = 0;
            Description = "noDescription";
            
        }
        // Overloaded Constructor
        public Monster(string name, float damage, float health, string description)
        {
            this.Name = name;
            this.Damage = damage;
            this.PHealth = health;
            this.Description = description;       
        }
        
        /// <summary>
        /// Describe displays the monsters deatails as well as the death message
        /// </summary>
        public void Describe()
        {
            // checks if monster is alive
            if (CheckIfAlive()) 
            {
               
                Console.WriteLine("My name is " + Name + " I am " + Description + ".\nMy Health is currently at: " + PHealth + "\n ");
            }
            else
            {
                Console.WriteLine("=================================================\n");
                Console.WriteLine("Here lies the corpse of " + Name + " " + Description + "\n ");
            }
            
        }

        /// <summary>
        /// Attack lets the enemy attack the player using a random number value, each hit is different from the last. Not all attacks will hit the player
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {
            //              Initilization
            //=====================================================
            Random randomAttack = new Random(); 

            //checks the random number and attack with the right damage multiplier
            switch (randomAttack.Next(1,10)) 
            {
                // Complete miss 1#
                case 1:
                    Console.WriteLine(Name + " glanced off " + player.Name + " the " + player.Title + "'s armour!\n");
                    break;

                // complete miss 2#
                case 2:
                    Console.WriteLine(Name + " missed completely!\n");
                    break;

                // Lowest Damage
                case 3:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s toe, you stumble but stay standing!\n");
                    player.PHealth -= (Damage * 0.1f);
                    break;

                // Lower Damage
                case 4:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s arm, it hurt but they had a Hello Kitty Bandaid so they took minimum Damage!\n");
                    player.PHealth -= (Damage * 0.2f);
                    break;

                // Low Damage
                case 5:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s knee, this hurt but the blade didn't penetrate your armour!\n");
                    player.PHealth -= (Damage * 0.4f);
                    break;

                // Medium Damage 1#
                case 6:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s left leg, they stumble!\n");
                    player.PHealth -= (Damage * 0.5f);
                    break;

                // Medium Damage 2#
                case 7:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s right leg, they stumble!\n");
                    player.PHealth -= (Damage * 0.5f);
                    break;

                // High Damage
                case 8:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s backside, it took a chunck out of their bottom!\n");
                    player.PHealth -= (Damage * 0.6f);
                    break; 

                // Higher Damage
                case 9:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s chest, it winded them!\n");
                    player.PHealth -= (Damage * 0.7f);
                    break;

                // Highest Damage
                case 10:
                    Console.WriteLine(Name + " hit " + player.Name + " the " + player.Title + "'s face, they'll have a new scar!\n");
                    player.PHealth -= (Damage * 1);
                    break;
            }
        }

        /// <summary>
        /// CheckIfAlive Checks if the monster is alive
        /// </summary>
        /// <returns>returns true or false</returns>
        public bool CheckIfAlive()
        {
            // checks if health of the monster is greater then 0
            if (this.PHealth > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

    }
}
