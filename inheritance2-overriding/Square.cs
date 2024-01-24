using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inheritance2_overriding
{
    internal class Square : AbstractShape
    {
        public Square()
        {
            

        }

        public Square(string shapeType) : base(shapeType)
        {
            
        }

        public override void drawShape()
        {
            Console.WriteLine("  ...........  ");
            Console.WriteLine("  .         .  ");
            Console.WriteLine("  .         .  ");
            Console.WriteLine("  .         .  ");
            Console.WriteLine("  .         .  ");
            Console.WriteLine("  ...........   ");
        }
    }
}
