using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_10___Text_Based_Adventure
{
    public class MagicKey
    {
        //              Initilization
        //=====================================================
        public string Name {  get; private set; }
        public string Description { get; private set; }

        // defualt constructor
        public MagicKey() 
        {
            Name = "The Magic Key of Truth";
            Description = "\nIf you take me to the start of time, I will reward you.";
        }

        /// <summary>
        /// Describe displays the magic key's name and description
        /// </summary>
        public void Describe()
        {
            Console.WriteLine(Name + ": " + Description + "\n ");
        }

    }
}
