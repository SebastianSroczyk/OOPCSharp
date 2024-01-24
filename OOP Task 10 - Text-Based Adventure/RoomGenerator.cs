using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class RoomGenerator
    {
        //              Initilization
        //=====================================================
        private string[] names = { "Kitchen", "Living Room", "Bathroom", "Corridor", "Bedroom", "Laundry Room", "The Dungeon" };
        private string[] descriptions = { "\nA place too cook things, contains: ", "\nA place to relax, contains: ", "\nA Place to wash yourself, contains: ", "\nA place to store garmits, contains: ", "\nA place to sleep, contains: ", "\nA place to do Laundry in, contains: ", "\nA place...for things..., contains: Questionable objects, " };

        // Default Constructor
        public Room GenerateRoom()
        {
            PotionGenerator potionGenerator = new PotionGenerator();
            MonsterGenerator monsterGenerator = new MonsterGenerator();
            WeaponGenerator weaponGenerator = new WeaponGenerator();
 

            Random random = new Random();
            int arrayNumber = random.Next(names.Length); 
            string roomName = names[arrayNumber];
            string roomDescription = descriptions[arrayNumber];
            

            return new Room(roomName, roomDescription, potionGenerator.GeneratePotions(), monsterGenerator.GenerateMonster(), weaponGenerator.GenerateWeapons());
        }

        /// <summary>
        /// GeneratingRooms generates all the rooms for the game, as well as gets rid of any duplicates
        /// </summary>
        public Room[] GeneratingRooms(int roomCount)
        {
            Room[] roomArray = new Room[roomCount];

            // Generates Rooms
            for (int i = 0; i < roomCount; i++)
            {
                roomArray[i] = GenerateRoom();  
            }
            // Checking for duplicate rooms
            for (int j = 0; j < roomCount - 1; j++)
            {
                foreach (Room room in roomArray)
                {
                    while (roomArray[j].Name == room.Name)
                    {
                        roomArray[j] = GenerateRoom();
                    }
                }
            }
            
            // Adds Key to last room
            roomArray[roomCount - 1].SetKey(new MagicKey());

            return roomArray;
        }
    }
}
