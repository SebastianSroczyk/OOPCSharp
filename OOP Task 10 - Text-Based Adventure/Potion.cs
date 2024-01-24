using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class Potion
    {

        //              Initilization
        //=====================================================
        public int HealthBoost {  get; private set; }  
        public string Description { get; private set; }
        public string Name { get; private set; }

        // Defualt Constructor
        public Potion()
        {
            HealthBoost = 0;
            Description = "noDescription";
            Name = "noName";
        }

        // Overloaded Constructor
        public Potion(string name, int healthBoost, string description )
        {
            this.HealthBoost = healthBoost;
            this.Description = description;
            this.Name = name;
        }

        /// <summary>
        /// Describe displays the description of the potion
        /// </summary>
        public void Describe()
        {
            
            Console.WriteLine(Name + ": " + Description + ".\nRegenerates health by: " + HealthBoost + " points.\n ");
        }



    }
}
