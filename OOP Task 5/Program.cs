using System.Linq;

namespace OOP_Task_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // starName Array
            // starType Array
            // planets Array

            // Generate a random number
            // Display ( "Solar system generator " )
            // Get random number from 1 - 5
            // Ger random number from 1- 100
            // Display ( starName[random number from 1 - 5] & - & starType[random number from 1- 100])
            // loop 8 times starting at 1
            //      Display( number & - & planets[random number from 1 - 100]
            // End Loop

            // Arrays
            string[] starNames = {"Chronos", "Alpha Reticuli", "Zorothon", "Betelgeuse", "Algol"};
            string[] starTypes = {"Type O", "Type O", "Type O", "Type O", "Type O", "Type B", "Type B", "Type B", "Type B", "Type B", "Type F", "Type F", "Type F", "Type F", "Type F", "Type F", "Type F", "Type F", "Type F", "Type F", "Type G", "Type G", "Type G", "Type G", "Type G", "Type G", "Type G", "Type G", "Type G", "Type G", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type K", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M", "Type M" };
            string[] planets = {"Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Rocky Planet", "Large Planet", "Large Planet", "Large Planet", "Large Planet", "Large Planet", "Ringed Planet", "Ringed Planet", "Ringed Planet", "Ringed Planet", "Ringed Planet", "Large Ringed Planet", "Large Ringed Planet", "Large Ringed Planet", "Large Ringed Planet", "Large Ringed Planet", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Empty Space", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Asteriods", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Ringed Gas Giant Planet", "Browm Dwarf", "Browm Dwarf", "Browm Dwarf", "Browm Dwarf", "Browm Dwarf" };
           
            // Variables

            // Logic
            Random randomNumber = new Random();

            // Display
            Console.WriteLine("Solar System Generator: \n ");

            Console.WriteLine(starNames[randomNumber.Next(starNames.Length)] + " - " + starTypes[randomNumber.Next(starTypes.Length)] + "\n ");

            for (int number = 1; number < 9; number++)
            {
                Console.WriteLine(number + " - " + planets[randomNumber.Next(planets.Length)] + "\n ");
            }
            



            
            
            
        }
    }
}
