using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inheritance2_overriding
{
    //       abstract - means its a super class
    internal abstract class AbstractShape
    {

        private string shapeType {  get; set; }

        public AbstractShape()
        {

        }
        public AbstractShape(string ShapeType) 
        {
            this.shapeType = ShapeType;
        }

        // allows us to create a method without defining the code
        public abstract void drawShape();


        //     virtual - Overridable method
        public virtual void describeShape()
        {
            Console.WriteLine("This is a " + shapeType);
        }
    }
}
