namespace OOP_Conditional_Statements
{
    internal class Program
    {
        static void Main(string[] args)
        {
            float cost = 0;
            int people;


            Console.WriteLine("Pick Destenation: \na:Spain \nb:Alps \nc:USA");
            char userInput = (char)Console.Read();
            switch (userInput)
            {
                case 'a':
                    cost = 15000.0f;
                    break;
                case 'b':
                    cost = 2000.0f;
                    break;
                case 'c':
                    cost = 1700.0f;
                    break;
            }
            Console.WriteLine("Please enter the number of people: ");
            
            people = int.Parse(Console.ReadLine());

            if(people <= 2)
            {
                Console.WriteLine("Total cost: " + cost);
            }
            else if (people >= 3 &&people <= 5)
            {
                cost = cost * 0.95f;
                Console.WriteLine("Total cost: " + cost);
            }
            else
            {
                cost = cost * 0.93f;
                Console.WriteLine("Total cost: " + cost);
            }




        }
    }
}