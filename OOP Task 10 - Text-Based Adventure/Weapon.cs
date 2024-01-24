using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class Weapon
    {
        public string Name { get; private set; }
        private string Description { get; set; }
        public float Hitpoints { get; }

        public Weapon() 
        {
            Name = "noName";
            Description = "noDescription";
            Hitpoints = 0;
        }
        public Weapon(string name, string description, int hitpoints)
        {
            this.Name = name;
            this.Description = description;
            this.Hitpoints = hitpoints;
        }
        // testing
        public void describe()
        {
            Console.WriteLine(Name + ": " + Description + "\nDeals " + Hitpoints + " Damage\n ");
        }
        
        
    }
}
