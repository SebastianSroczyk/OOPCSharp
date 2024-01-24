using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class Room
    {
        public string Name {  get; private set; }
        private string Description { get; set; }
        public Player Player { get; set; }
        public Potion? Potion { get;  set; }
        public Monster Monster { get;  set; }
        public Weapon? Weapon { get;  set; }
        public MagicKey? MagicKey { get;  set; } 

        

        // default constructor
        public Room() 
        {
            Name = "noName";
            Description = "noDescription";
            Potion = new Potion();
            Monster = new Monster();
            Weapon = new Weapon();
            MagicKey = new MagicKey();
            

        }
        // overloaded constructor with a key
        public Room(string name, string description, Potion potion, Monster monster, Weapon weapon) 
        {
            
            this.Name = name;
            this.Description = description;
            this.Potion = potion;
            this.Monster = monster;
            this.Weapon = weapon;
            
            
            
        }

        /// <summary>
        /// SetKey adds a key to a room
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(MagicKey key)
        {
            this.MagicKey = key;
        }

        /// <summary>
        /// Describe displays the contents of the room depending on the rooms state
        /// </summary>
        public void Describe()
        {
            Console.Clear();
            Console.WriteLine("\n=================================================\n");
            Console.WriteLine("This is a " + Name + "\n" + Description + "\n ");
            //checks if a potion is in the room
            if(Potion != null)
            {
                Potion?.Describe();
            }
            //checks if a monster is in the room
            if (Monster != null)
            {
                Monster?.Describe();
            }
            //checks if a weapon is in the room
            if (Weapon != null)
            {
                Weapon?.describe();
            }
            //checks if a magic key is in the room
            if (MagicKey != null)
            {
                MagicKey.Describe();
            }
            
        }

        
    } 
}
