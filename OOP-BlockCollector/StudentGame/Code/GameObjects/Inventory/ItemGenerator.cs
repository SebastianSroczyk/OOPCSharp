using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class ItemGenerator
    {
        private string[] coinName = { "Bronze Coin", "Silver Coin", "Gold Coin" };  // Random name options

        public int objectXPos { get; private set; } // The location the coins will spawn from
        public int objectYPos { get; private set; }

        private int numItemsInPotionArray;
        public ItemGenerator()
        {
            objectXPos = 100;
            objectYPos = 100;
            numItemsInPotionArray = coinName.Length;
        }

        /// <summary>
        /// Generates Coin
        /// </summary>
        /// <returns></returns>
        public Coin GenerateCoin()
        {
            Random r = new Random();

            //Console.WriteLine("Creating Items");
            Coin p = new Coin(64, coinName[r.Next(numItemsInPotionArray)]);
            objectXPos = r.Next(20, 1600);
            objectYPos = r.Next(20, 800);
            p.SetPosition(objectXPos,objectYPos);

            return p;
        }
        /// <summary>
        /// Generates Coin (Override) with spwan location
        /// </summary>
        /// <param name="spawnLocation"></param>
        /// <returns></returns>
        public Coin GenerateCoin(Vector2 spawnLocation)
        {
            Random r = new Random();

            //Console.WriteLine("Creating Items");
            Coin p = new Coin(64, coinName[r.Next(numItemsInPotionArray)]);
            p.SetPosition(spawnLocation);

            return p;
        } 
    }
}
