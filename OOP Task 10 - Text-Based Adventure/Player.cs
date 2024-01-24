using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class Player
    {
        //              Initilization
        //=====================================================
        public MagicKey ItemHeld { get; set; }
        public Potion Potion { get; set; }
        public Weapon WeaponHeld { get; set; }
        public string Name { get; private set; }
        public bool IsAttacking {  get; private set; }
        public string Title {  get; private set; }
        private float damageMultiplier {  get; set; }
        public int RoomIndex { get; private set; }
        private float health { get; set; }
        public float PHealth
        {
            get { return health; }
            set
            {
                health = value;
                if (health > 100)
                {
                    health = 100;
                }

            }
        }

        // Defualt Construtor
        public Player() 
        {
            Name = "NoName";
            Title = "NoTitle";
            PHealth = 10;
            ItemHeld = null;
            WeaponHeld = null;
        }

        // overloaded Constructor
        public Player(string name, string title, int health, MagicKey itemHeld, Weapon weaponHeld)
        {
            this.Name = name;
            this.Title = title;
            this.PHealth = health;
            this.ItemHeld = itemHeld;
            this.WeaponHeld = weaponHeld;
        }

        /// <summary>
        /// Checks the player's current state and displays information accordingly
        /// </summary>
        public void Describe()
        {
            //checks if player is alive and not holding any items
            if (CheckIfAlive() && ItemHeld == null && WeaponHeld == null)
            {
                
                Console.WriteLine("=================================================\n");
                Console.WriteLine("My name is " + Name + " the " + Title);
                Console.WriteLine("My Health is currently at: " + PHealth + " and I am carrying nothing\n");
            }
            // checks if the player is alive and holding a weapon
            else if (CheckIfAlive() && ItemHeld == null && WeaponHeld != null)
            {
                
                Console.WriteLine("=================================================\n");
                Console.WriteLine("My name is " + Name + " the " + Title);
                Console.WriteLine("My Health is currently at: " + PHealth + " and I am carrying " + WeaponHeld.Name);
            }
            // checks if the player is alive and holding a weapon and the key
            else if (CheckIfAlive() && ItemHeld != null && WeaponHeld != null)
            {
                
                Console.WriteLine("=================================================\n");
                Console.WriteLine("My name is " + Name + " the " + Title);
                Console.WriteLine("My Health is currently at: " + PHealth + " and I am carrying a " + ItemHeld.Name + " and a " + WeaponHeld.Name + "\n");
            }
            // Displays the death description of the player
            else
            {
                
                Console.WriteLine("=================================================\n");
                Console.WriteLine("Here lies the body of " + Name + " the " + Title + ". They were a great Worrier");
            }
            
        }

        /// <summary>
        /// Attack allows the player to attack the monster based on hit chance.
        /// </summary>
        /// <param name="monster"></param>
        public void Attack(Monster monster)
        {
            //              Initilization
            //=====================================================
            Random randomAttack = new Random();
            
            // check if player holding a weapon
            if (this.WeaponHeld != null)
            {
                // takes a random value to pick an attack
                switch (randomAttack.Next(1,7))
                {
                    
                    case 1:
                        // Complete Miss
                        damageMultiplier = 0f;
                        Console.WriteLine(Name + " the " + Title + " Completely missed");
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;

                    case 2:
                        // Lowest Level Attack
                        damageMultiplier = 0.2f;
                        Console.WriteLine(Name + " the " + Title + " hit " + monster.Name + "'s legs, this made the enemy stumble");
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;

                    case 3:
                        // Low Level Attack
                        damageMultiplier = 0.6f;
                        Console.WriteLine(Name + " the " + Title + " hit " + monster.Name + "'s arm, this hurt the enemy");
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;

                    case 4:
                        // Mid Level Attack
                        damageMultiplier = 0.9f;
                        Console.WriteLine(Name + " the " + Title + " hit " + monster.Name + "'s chest, they took a lot of damage");
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;

                    case 5:
                        // High Level Attack
                        damageMultiplier = 1.2f;
                        Console.WriteLine(Name + " the " + Title + " penetrated " + monster.Name + "'s armour");
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;

                    case 6:
                        // Highest Level Attack
                        damageMultiplier = 1.7f;
                        Console.WriteLine(Name + " the " + Title + " destroyed " + monster.Name + "'s armour");
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;

                    case 7:
                        // God-Like Level Attack
                        damageMultiplier = 10f;
                        Console.WriteLine(Name + " the " + Title + " annihilate " + monster.Name);
                        monster.PHealth -= (WeaponHeld.Hitpoints * damageMultiplier * CharacterTitleCheck(monster));
                        break;
                }
            }
            else
            {
                Console.WriteLine("You do not have a weapon\n");
            }
            
        }

        /// <summary>
        /// CharacterTitleCheck checks the players title and then assigns a classMultiplier, which is used as a damage modifier
        /// </summary>
        /// <param name="monster"></param>
        /// <returns>classMultiplier</returns>
        private float CharacterTitleCheck(Monster monster)
        {
            //              Initilization
            //=====================================================
            float classMultiplier = 0;

            //checks the title
            switch (Title)
            {
                
                // when DaBaby is the title a modifier of 0.3 is used
                case "DaBaby":
                    classMultiplier = 0.3f;
                    break;

                // when Bard is the title a modifier of 0.5 is used
                case "Bard":
                    classMultiplier = 0.5f;
                    break;

                // when Warlock is the title a modifier of 0.8 is used
                case "Warlock":
                    classMultiplier = 0.8f;
                    break;

                // when Barbarian is the title a modifier of 1 is used
                case "Barbarian":
                    classMultiplier = 1f;
                    break;

                // when Druid is the title a modifier of 1.2 is used
                case "Druid":
                    classMultiplier = 1.2f;
                    break;

                // when Paladin is the title a modifier of 1.4 is used
                case "Paladin":
                    classMultiplier = 1.4f;
                    break;

                // when Wizard is the title a modifier of 2 is used
                case "Wizard":
                    classMultiplier = 2f;
                    break;
            }
            return classMultiplier;
        }

        /// <summary>
        /// DisplayItems displays avaiable Items to pick up
        /// </summary>
        /// <returns>correctPickUp</returns>
        private bool DisplayItems()
        {
            //              Initilization
            //=====================================================
            bool correctPickUp = false;

            Console.WriteLine("=================================================\n");
            Console.WriteLine("What Item would you like to pick up?");
            Console.WriteLine("\nOptions:\n-Weapon\n-Key\n-Potion\n-No Item\n");
            Console.WriteLine("=================================================\n");
            return correctPickUp;
        }

        /// <summary>
        /// PickUp Gets the players input on what item to pick up and picks it up
        /// </summary>
        /// <param name="RoomArray"></param>
        /// <param name="RoomIndex"></param>
        public void PickUp(Room[] RoomArray, int RoomIndex)
        {
            //              Initilization
            //=====================================================
            bool correctPickUp = DisplayItems();

            // checks answer while the pick up is not true
            while (!correctPickUp)
            {
                //              Initilization
                //=====================================================
                string answer = Console.ReadLine();
                
                // checks answer for correct inputs
                switch (answer)
                {

                    // when the player types "weapon" the weapon is added to the player's inventory
                    case "Weapon":
                    case "weapon":
                        // checks if the room's weapon isn't null
                        if (RoomArray[RoomIndex].Weapon != null)
                        {
                            Console.Clear();
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("You picked up a weapon.\n");
                            WeaponHeld = RoomArray[RoomIndex].Weapon;
                            Weapon weapon = null;
                            RoomArray[RoomIndex].Weapon = weapon;
                            correctPickUp = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("This isn't a valid option Try again");
                            correctPickUp = DisplayItems();
                        }
                        break;
                    // when the player types "Potion" the when is taken by the player
                    case "Potion":
                    case "potion":
                        // checks if the room's potion isn't null
                        if (RoomArray[RoomIndex].Potion != null)
                        {

                            Console.Clear();
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("\nYou picked up a potion.");
                            PHealth += RoomArray[RoomIndex].Potion.HealthBoost;
                            Potion potion = null;
                            RoomArray[RoomIndex].Potion = potion;
                            correctPickUp = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("\nThis isn't a valid option Try again");
                            correctPickUp = DisplayItems();
                        }
                        break;

                    // when the player types "Key" the Key is added to the player's inventory
                    case "Key":
                    case "key":
                        // checks if the room's magic key isn't null
                        if (RoomArray[RoomIndex].MagicKey != null)
                        {
                            Console.Clear();
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("You picked up a key.\n");
                            ItemHeld = RoomArray[RoomIndex].MagicKey;
                            MagicKey key = null;
                            RoomArray[RoomIndex].MagicKey = key;
                            correctPickUp = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("This isn't a valid option Try again\n");
                            correctPickUp = DisplayItems();
                        }
                        break;

                    // when the player types "No Item" the player gets reverted to the main options
                    case "No Item":
                    case "No item":
                    case "no Item":
                    case "no item":
                        Console.Clear();
                        correctPickUp = true;
                        break;

                    // when the player doesn't type the right options, tells the plaeyr to try again
                    default:
                        
                        Console.Clear();
                        Console.WriteLine("\n=================================================\n");
                        Console.WriteLine("This isn't a valid option Try again\n");
                        correctPickUp = DisplayItems();
                        break;
                }
            }
        }

        /// <summary>
        /// MoveRoom checks if the player can move as well as moves the player from room to room
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="numberOfRooms"></param>
        /// <param name="monster"></param>
        public void MoveRoom(string direction,int numberOfRooms, Monster monster)
        {
            //              Initilization
            //=====================================================
            int forwardsCheck = RoomIndex + 1;
            int backwardsCheck = RoomIndex - 1;
            bool isCorrect = false;

            // runs direction check while the iscorrect input is false
            while (!isCorrect)
            {
                
                // checks if direction is forwards and moves the player if true
                if (direction == "forwards" || direction == "Forwards")
                {
                    // checks if we can move forwards and if the monster is not alive
                    if (forwardsCheck < numberOfRooms && !monster.CheckIfAlive())
                    {
                        RoomIndex++;
                        isCorrect = true;
                    }
                    // checks if monster is alive then displays a blocking message
                    else if (monster.CheckIfAlive())
                    {
                        Console.Clear();
                        Console.WriteLine("\nThe " + monster.Name + " is blocking the way forwards!");
                        isCorrect = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\nYou Cannot move further");
                        isCorrect = true;
                    }

                }
                // checks if direction is backwards and moves teh player if true
                else if (direction == "backwards" || direction == "Backwards")
                {
                    // checks if the player can move backwards
                    if (backwardsCheck > -1)
                    {
                        RoomIndex--;
                        isCorrect = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("You Cannot move further");
                        isCorrect = true;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nThis isn't a correct movement! Try Forwards or Backwards");
                    isCorrect = true;
                }
            }

        }

        /// <summary>
        /// CheckIfAlive checks if the player is alive, used for checking player state
        /// </summary>
        /// <returns>true if health grater then 0</returns>
        public bool CheckIfAlive()
        { 
            // checks if health greater then 0
            if (this.PHealth > 0)
            {
                return true;
                
            }
            return false;
        }

    }

}
