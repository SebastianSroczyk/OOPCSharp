using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inheritance1
{
    internal class Mouse : Mammal
    {
        private int desireForCheese { get; set; }

        public Mouse():base() // we call base to call upon the base class
        {
            this.desireForCheese = 0;
        }

        public Mouse(string species, int numberOfLegs, int age, string name, int desireForCheese):
            base(species, numberOfLegs, age, name)       
        {
            this.desireForCheese= desireForCheese;
        }
        public override void aboutMe()
        {
            base.aboutMe();
            Console.WriteLine("I LOVE CHEESE <3" );
            Console.WriteLine("In fact my desire for cheese is: " + desireForCheese);
        }

        public void eatCheese()
        {
            Console.WriteLine("Desire for Cheese " + desireForCheese);
            Console.WriteLine("Nom Nom");
        }

    }
}
