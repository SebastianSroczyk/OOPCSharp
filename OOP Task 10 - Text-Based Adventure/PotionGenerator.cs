using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class PotionGenerator
    {
        //              Initilization
        //=====================================================
        private string[] itemName = { "Bleach", "Vodka", "Blood", "Cheese", "Blueberries", "Yes..." };
        private string[] description = { "\nHope it doesn't hurt", "\nThe Way the big man intended", "\nYikes", "\nCHEEEZE", "\nIt looks yummy", "\nUnknown" };
        
        // Constructor
        public PotionGenerator()
        {

        }

        /// <summary>
        /// Generate Potion Generates a potion with a random name description and health boost effect
        /// </summary>
        /// <returns>Potion Object</returns>
        public Potion GeneratePotions()
        {
            //              Initilization
            //=====================================================
            Random random = new Random();
            int arrayNumber = random.Next(itemName.Length);
            string Name = itemName[arrayNumber];
            string Description = description[arrayNumber];
            int Boost = random.Next(100);


            return new Potion(Name, Boost, Description);
        }
    }
}
