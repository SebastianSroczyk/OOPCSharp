using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inheritance1
{
    internal class Mammal
    {
        private string species { get; set; }
        private int numberOfLegs { get; set; }
        private int age { get; set; }
        private string name { get; set; }

        public Mammal()
        {
            species = "Undefined";
            numberOfLegs = 0;
            age = 0;
            name = "Undefined";
        }

        public Mammal(string species, int numberOfLegs, int age, string name)
        {
            this.species = species;
            this.numberOfLegs = numberOfLegs;
            this.age = age;
            this.name = name;
        }
        //     Virtual - makes the method overrideable in a Sub-Class
        public virtual void move()
        {
            Console.WriteLine("As a mammal, I am moving");
        }

        public virtual void aboutMe()
        {
            Console.WriteLine("I am a " + species);
            Console.WriteLine("My name is " + name);
            Console.WriteLine("I am " + age + " years old");
            Console.WriteLine("I have " + numberOfLegs + " legs");
        }
    }
}
