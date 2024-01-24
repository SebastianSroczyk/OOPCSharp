using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inheritance2_overriding
{
    internal class Triangle : AbstractShape
    {
        public Triangle()
        {
            

        }

        public Triangle(string shapeType) : base(shapeType)
        {
            
        }

        public override void drawShape()
        {
            Console.WriteLine("       .         ");
            Console.WriteLine("      . .        ");
            Console.WriteLine("     .   .       ");
            Console.WriteLine("    .     .      ");
            Console.WriteLine("   .       .     ");
            Console.WriteLine("  ...........    ");
        }
    }
}
