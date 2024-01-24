namespace inheritance2_overriding
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Circle circle = new Circle("Circle");
            Square square = new Square("Square");
            Triangle triangle = new Triangle("Triangle");

            triangle.drawShape();
            triangle.describeShape();

            square.drawShape();
            square.describeShape();

            circle.drawShape();
            circle.describeShape();

            // Abstract Classes cannot be created on their own, but can be made in an array using polymorphism
            AbstractShape[] shapes = new AbstractShape[5];

            shapes[0] = new Square("Square");

            shapes[0].drawShape();
            shapes[0].describeShape();
        }
    }
}