using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inheritance2_overriding
{
    internal class Circle : AbstractShape
    {
        public Circle()
        {
            

        }

        public Circle(string shapeType) : base(shapeType)
        {
            
        }

        public override void drawShape()
        {
            Console.WriteLine("       ...       ");
            Console.WriteLine("      .   .      ");
            Console.WriteLine("     .     .      ");
            Console.WriteLine("     .     .      ");
            Console.WriteLine("      .   .      ");
            Console.WriteLine("       ...       ");
        }
    }
}
