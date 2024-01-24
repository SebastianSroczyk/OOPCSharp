using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace inheritance1
{
    internal class Dog : Mammal
    {
        private int droolFactor {  get; set; }

        public Dog():base()
        {
            this.droolFactor = 0;
        }

        public Dog(string species, int numberOfLegs, int age, string name, int droolFactor): 
            base(species, numberOfLegs, age, name) // Initialise the base/super class
        {
            this.droolFactor = droolFactor;
        }

        public override void move()
        {
            base.move();
            Console.WriteLine("As a Dog I am bounding excitedly");
        }
        //     override - overrides parent method aboutME class
        public override void aboutMe()
        {
            base.aboutMe();
            Console.WriteLine("My slobber factor is " + droolFactor);
        }

        public void slobberEverywhere()
        {
            Console.WriteLine("Drool factor " + droolFactor);
        }
    }
}
