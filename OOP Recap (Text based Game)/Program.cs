using System.ComponentModel.DataAnnotations;

namespace OOP_Recap__Text_based_Game_
{
    internal class Program
    {
        
        public static void Main(string[] args)
        {
            string name;
            int age;
            string hairColour;

            // a
            Console.WriteLine("2x1 = 2");
            Console.WriteLine("2x2 = 4");
            Console.WriteLine("2x3 = 6");
            Console.WriteLine("2x4 = 8");
            Console.WriteLine("2x5 = 10");
            Console.WriteLine("2x6 = 12");
            Console.WriteLine("2x7 = 14");
            Console.WriteLine("2x8 = 16");
            Console.WriteLine("2x9 = 18");
            Console.WriteLine("2x10 = 20");
            Console.WriteLine("                  ");

            // b)

            Console.WriteLine("-------------------------------");
            Console.WriteLine("|  GAMES  DEVELOPMENT:        |");
            Console.WriteLine("| OBJECT ORIENTED PROGRAMMING |");
            Console.WriteLine("| ROOM:Y348 TIME:9:00 - 12:00 |");
            Console.WriteLine("|-----------------------------|");
            Console.WriteLine("| LUNCH: 12:00 - 14:00        |");
            Console.WriteLine("|-----------------------------|");
            Console.WriteLine("| 3D Level Editing;           |");
            Console.WriteLine("| 3D Modelling and Animation  |");
            Console.WriteLine("| ROOM:Y348 TIME:14:00 - 17:00|");
            Console.WriteLine("|-----------------------------|");
            Console.WriteLine("                       ");


            //c

            Console.WriteLine("Please input your name: ");
            name = Console.ReadLine();
            Console.WriteLine("Please input your age: ");
            age = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter your hair colour: ");
            hairColour = Console.ReadLine();
            Console.WriteLine("Name: " + name + "\nAge: " + age + "\nHair Colour: " + hairColour + "\nThank you");
            Console.WriteLine("                       ");

            // d
            Console.WriteLine("Please input your name: ");
            name = Console.ReadLine();
            Console.WriteLine(name + "You will eat a cactus and turn into a dog in the next 3 years");

        }

        
        
    }
}