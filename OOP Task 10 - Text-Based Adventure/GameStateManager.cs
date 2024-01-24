using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class GameStateManager
    {
        //              Initilization
        //=====================================================
        private PlayerGenerator? PlayerGenerator { get; set; }
        private RoomGenerator RoomGenerator { get; set; }
        private Room[] RoomArray { get; set; }

        private int numberOfRooms = 5;
        private int RoomIndex { get; set; }
        private bool CommandInput {  get; set; }    
        private bool StartCheck { get; set; }
        private bool Room1Check { get; set; }
        private bool Room2Check { get; set; }
        private bool Room3Check { get; set; }
        private bool Room4Check { get; set; }
        private bool EndCheck { get; set; }
        


        /// <summary>
        /// This is the Main game loop
        /// </summary>
        public void PlayGame()
        {
            //              Initilization
            //=====================================================

            // initilizes the player genertaor
            PlayerGenerator = new PlayerGenerator();
            // initilizes the room generator
            RoomGenerator = new RoomGenerator();
            // generates the player
            Player player = PlayerGenerator.GeneratePlayer();
            // generates all the rooms
            RoomArray = RoomGenerator.GeneratingRooms(numberOfRooms);

            RoomIndex = 0;
            StartCheck = false;
            Room1Check = false;
            Room2Check = false;
            Room3Check = false;
            Room4Check = false;
            EndCheck = false;
            


            //              Game Start Sequence
            //=====================================================
            StoryDisplay(RoomIndex, WinConditon(player));
            player.Describe();
            
            while (!WinConditon(player))
            {
                GetPlayerInput(player);
                
            }
            StoryDisplay(RoomIndex, WinConditon(player));
            PlayAgain();
            
        }

        /// <summary>
        /// Displays all the story elements in order of which room we are and if we have been in the room before
        /// </summary>
        private void StoryDisplay(int roomIndex, bool winCondition)
        {
            if(roomIndex == 0 && !StartCheck)
            {
                Console.Clear();
                Console.WriteLine("A long time ago there was a Warrior. This Warrior\nwas feared as they were the strongest Warrior in the world, and\nthis Story Begins with the Warrior without any weapons in... \n");
                StartCheck = true;
            }
            else if(roomIndex == 1 && !Room1Check)
            {
                Console.Clear();
                Console.WriteLine("The Warrior Swiftly found a weapon and fought\nthrough the enemy guarding the door. The Warrior pushs forwards with\nall thier might.\n");
                Room1Check = true;
            }
            else if (roomIndex == 2 && !Room2Check)
            {
                Console.Clear();
                Console.WriteLine("A harder battle awaits as the Warrior cuts clean\n through another monster.\n");
                Room2Check = true;
            }
            else if (roomIndex == 3 && !Room3Check)
            {
                Console.Clear();
                Console.WriteLine("At this point in the story the warrior has fought\nhard, his stamin has decreaesed. Now tired they struggle to keep moving\nforwards, but thier Journey is yet to be complete.\n");
                Room3Check = true;
            }
            else if (roomIndex == 4 && !Room4Check)
            {
                Console.Clear();
                Console.WriteLine("The Warrior stands strong as they killed the final boss,\nbut wait a new enemy has arrived, now guarding the Magic Key of Truth.\nThe Warrior signs as they pick up thier weapon.\n'One more of you!' he exclaims chagring up his attack.\n ");
                Room4Check = true;
            }
            else if (roomIndex == 1 && !EndCheck && winCondition == true)
            {
                Console.Clear();
                Console.WriteLine("And so we end our Story of the Warrior who fought\nhard and brave to get the Magic Key of Truth and they have\n succeded with minimal resistance.\n");
                EndCheck = true;
            }
        }

        /// <summary>
        /// GetPlayerInput allows the player to be able to interact with teh game, this is where the decision making is done
        /// </summary>
        /// <param name="player">Connects Player object to the GSM</param>
        private void GetPlayerInput(Player player)
        {
            //              Initilization
            //=====================================================

            Monster monster = RoomArray[RoomIndex].Monster;
            MagicKey key = RoomArray[RoomIndex].MagicKey;
            Weapon weapon = RoomArray[RoomIndex].Weapon;
            Potion potion = RoomArray[RoomIndex].Potion;
            CommandInput = false;

            // while the player hasn't inputed anything it will ask
            while (!CommandInput)
            {
                UpdateRoomIndex(player);
                StoryDisplay(RoomIndex, WinConditon(player));

                //              Display Options
                //=====================================================
                Console.WriteLine("=================================================\n");
                Console.WriteLine("Pick one of these options to continue the game.");
                Console.WriteLine("Options:\n-Move\n-Attack\n-Pick Up\n-Describe\n-Quit\n");
                Console.WriteLine("=================================================\n");

                // checks for input
                switch (Console.ReadLine())
                {
                    // when the player types "Move" move sequence is initiated
                    case "Move":
                    case "move":
                        Console.Clear();                 
                        UpdateRoomIndex(player);
                        MoveSequence(player);
                        CommandInput = true;
                        break;

                    // when the player types "Attack" fight sequence is initiated
                    case "Attack":
                    case "attack":
                        Console.Clear();
                        UpdateRoomIndex(player);
                        FightSequence(player);
                        CommandInput = true;
                        break;

                    // when the player types "pick up" pick up sequence is initiated
                    case "Pick Up":
                    case "Pick up":
                    case "pick Up":
                    case "pick up":
                        Console.Clear();
                        UpdateRoomIndex(player);
                        player.PickUp(RoomArray, RoomIndex);
                        CommandInput = true;
                        break;

                    // when the player types "Describe" describe sequence is initiated
                    case "Describe":
                    case "describe":
                        Console.Clear();
                        UpdateRoomIndex(player);
                        ObjectDescribe(player);
                        CommandInput = true;
                        break;

                    // when the player types "Quit" game is turned off   
                    case "Quit":
                    case "quit":
                        Console.Clear();
                        Environment.Exit(0);
                        break;

                    // when the player doesn't type anything the loop is restarted
                    default:
                        Console.Clear();
                        UpdateRoomIndex(player);
                        Console.WriteLine("This isn't a command, Please try again");
                        CommandInput = false;
                        break;
                }
            }
            return;
        }

        /// <summary>
        /// ObjectDescribe Allows the player to pick a object they would like to describe
        /// </summary>
        /// <param name="player"></param>
        private void ObjectDescribe(Player player)
        {
            //              Initilization
            //=====================================================

            MagicKey magicKey = RoomArray[RoomIndex].MagicKey;
            Weapon weapon = RoomArray[RoomIndex].Weapon;
            Potion potion = RoomArray[RoomIndex].Potion;
            Room room = RoomArray[RoomIndex];

            //              Display Options
            //=====================================================
            Console.WriteLine("=================================================\n");
            Console.WriteLine("What would you like to describe:");
            Console.WriteLine("Room\nSelf\nMagic Key\nWeapon\nPotion\n");
            Console.WriteLine("=================================================\n");

            bool correctAnswer = false;
            
            // checking for correct answer
            while (!correctAnswer)
            {
                string answer = Console.ReadLine();

                // checks if answer is not null
                if (answer != null)
                {
                    // checks the answers 
                    switch (answer)
                    {
                        // when the player types "Room" room description is Initilised
                        case "Room":
                        case "room":
                            Console.Clear();
                            correctAnswer = true;
                            room.Describe();
                            break;

                        // when the player types "Self" Self description is Initilised
                        case "Self":
                        case "self":
                            Console.Clear();
                            correctAnswer = true;
                            player.Describe();
                            break;

                        // when the player types "Magic Key" magic key description is Initilised
                        case "Magic Key":
                        case "Magic key":
                        case "magic Key":
                        case "magic key":
                            Console.Clear();
                            correctAnswer = true;
                            // checks if the key is in the room or on the player
                            if (magicKey != null)
                            {
                                magicKey.Describe();
                            }
                            else if (player.ItemHeld != null)
                            {
                                player.ItemHeld.Describe();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("=================================================\n");
                                Console.WriteLine("There is no Magic key with you or in the Room!\n");
                            }
                            
                            break;

                        // when the player types "Waepon" weapon description is Initilised
                        case "Weapon":
                        case "weapon":
                            Console.Clear();
                            correctAnswer = true;
                            // checks if the weapon is in the room or on the player
                            if (weapon != null)
                            {
                                Console.WriteLine("=================================================\n");
                                weapon.describe();
                            }
                            else if (player.WeaponHeld != null)
                            {
                                
                                player.WeaponHeld.describe();
                            }
                            else
                            {
                                Console.WriteLine("There is no Weapon with you or in the room!");
                            }
                            break;

                        // when the player types "potion" potion description is Initilised
                        case "Potion":
                        case "potion":
                            Console.Clear();
                            correctAnswer = true;
                            // checks if the potion is in the room or on the player
                            if (potion != null)
                            {
                                Console.WriteLine("=================================================\n");
                                potion.Describe();
                            }
                            else
                            {
                                Console.WriteLine("The Potion does not exist int this room!");
                            }
                            break;

                        // if the input isn't correct message is displayed and options re-displayed
                        default:
                            Console.Clear();
                            Console.WriteLine("This isn't a correct input! Try again:\n");

                            //              Display Options
                            //=====================================================
                            Console.WriteLine("=================================================\n");
                            Console.WriteLine("What would you like to describe:");
                            Console.WriteLine("Room\nSelf\nMagic Key\nWeapon\nPotion\n");
                            Console.WriteLine("=================================================\n");
                            correctAnswer = false;
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// UpdateRoomIndex checks the index of the room, this makes sure the player is in the right room
        /// </summary>
        /// <param name="player">Connects Player object to the GSM</param>
        private void UpdateRoomIndex(Player player)
        {
            // updates room index
            RoomIndex = player.RoomIndex;
        }

        /// <summary>
        /// Move changes the players position in the RoomArray (aka moves the player)
        /// </summary>
        /// <param name="player">Connects Player object to the GSM</param>
        private void MoveSequence(Player player)
        {
            //              Display Options
            //=====================================================
            Console.WriteLine("=================================================\n");
            Console.WriteLine("What direction would you like to go? Forwards or Backwards\n");
            Console.WriteLine("=================================================\n");

            string direction = Console.ReadLine();
            // checks if the direction isn't null and if the monster is alive in the room
            if(direction != null && RoomArray[RoomIndex].Monster != null )
            {
                // confines the player movement within the roomArray
                if (player.RoomIndex <= 4 && player.RoomIndex >= 0)
                {   
                    player.MoveRoom(direction, numberOfRooms, RoomArray[RoomIndex].Monster);
                    Console.WriteLine("\nType 'Describe' to \nshow the contents of the room.");
                }
                else
                {
                    Console.WriteLine("\nThis isn't the right direction. Try again: Forwards or Backwards");
                }
                
            }
            
        }

        /// <summary>
        /// FightSequence calls the player and monster to attack each other
        /// </summary>
        /// <param name="player">Connects Player object to the GSM</param>
        private void FightSequence(Player player)
        {
            if (player.WeaponHeld != null)
            {
                while (player.CheckIfAlive() && RoomArray[RoomIndex].Monster.CheckIfAlive())
                {
                    Console.Clear();

                    //              Display Fight
                    //=====================================================
                    Console.WriteLine("=================================================\n");
                    player.Attack(RoomArray[RoomIndex].Monster);
                    RoomArray[RoomIndex].Monster.Attack(player);
                    Console.WriteLine("=================================================\n");
                    FightContinuationInputCheck();
                }
                // check if player is alive
                if (player.CheckIfAlive() && !RoomArray[RoomIndex].Monster.CheckIfAlive())
                {
                    //describes the mosnter and player
                    RoomArray[RoomIndex].Monster.Describe();
                    player.Describe();
                }
                else
                {
                    // display death message
                    player.Describe();
                    Console.WriteLine("\nYOU HAVE DIED, TRY AGAIN!\n");
                    
                    PlayAgain();
                }
            }
            else
            {
                Console.WriteLine("You're not Holding a Weapon, use: 'Pick Up' to pick up a weapon\n");
            }
            
        }

        /// <summary>
        /// Checks for input from the player to move to the next stage of the fight
        /// </summary>
        private static void FightContinuationInputCheck()
        {
            Console.WriteLine("Press enter to continue the fight...");
            Console.ReadLine();
            Console.Clear();
            
        }

        /// <summary>
        /// PlayAgain Allows the player to play the game again if they would like to.
        /// </summary>
        private void PlayAgain()
        {
            Console.WriteLine("Would you like to play again? (Yes/No)");
            string? answer = Console.ReadLine();
            // checks if answer is correct
            if (answer == "Yes" || answer == "yes")
            {
                PlayGame();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Checks for win condition and if the game is complete displays message and runs PlayAgain method
        /// </summary>
        /// <param name="player">references the player</param>
        private  bool WinConditon(Player player)
        {
            // checks if the player is holding a key and they are in the first room
            if (player.ItemHeld != null && player.RoomIndex == 0)
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
