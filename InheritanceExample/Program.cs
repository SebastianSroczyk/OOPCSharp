namespace inheritance1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mammal mammal = new Mammal();
            Mouse mouse = new Mouse("Mouse", 4, 7, "Dog", 25);
            Dog pup = new Dog();

            //pup.aboutMe();
            mouse.aboutMe();
        }
    }
}