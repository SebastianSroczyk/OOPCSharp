namespace OOP_task_5__OOP_style_
{
    public class SolarSystem
    {
        string[] stars = {"Chronos", "Alpha Reticuli","Zarothon", "Betelgeuse", "Algol" };
        //                      5%          5%        10%      10%      20"        50%
        string[] starTypes = { "Type O", "Type B", "Type F", "Type G", "Type K", "Type M" };
        //                        20%              5%                5%              5%                   25%           15%             10%                 10%                     5%                                             
        string[] planets =  {"Rocky Planet", "Large Planet", "Ringed Planet", "Large Ringed Planet", "Empty Space", "Asteroids", "Gas Giant Planet", "Ringed Gas Giant Planet", "Brown Dwarf"};
       
        // Random number
        Random random = new Random();

        // Variables
        public string star;
        public string starType;
        public string[] planet = new string[8]; 

        // SolarSystem Constructor
        public SolarSystem()
        {
            // Logic
            star = stars[random.Next(stars.Length)];
            int rand = random.Next(100);
            if (rand > 0 && rand <= 5)
            {
                starType = starTypes[0];
            }
            else if(rand > 5 && rand <= 10)
            {
                starType = starTypes[1];
            }
            else if (rand > 10 && rand <= 20)
            {
                starType = starTypes[2];
            }
            else if (rand > 20 && rand <= 30)
            {
                starType = starTypes[3];
            }
            else if (rand > 30 && rand <= 50)
            {
                starType = starTypes[4];
            }
            else
            {
                starType = starTypes[5];
            }

            for (int i = 0; i < 8; i++)
            {
                rand = random.Next(100);

                if (rand > 0 && rand <= 20)
                {
                    planet[i] = planets[0];
                }
                else if (rand > 20 && rand <= 25)
                {
                    planet[i] = planets[1];
                }
                else if (rand > 25 && rand <= 30)
                {
                    planet[i] = planets[2];
                }
                else if (rand > 30 && rand <= 35)
                {
                    planet[i] = planets[3];
                }
                else if (rand > 35 && rand <= 60)
                {
                    planet[i] = planets[4];
                }
                else if (rand > 60 && rand <= 75)
                {
                    planet[i] = planets[5];
                }
                else if (rand > 75 && rand <= 85)
                {
                    planet[i] = planets[6];
                }
                else if (rand > 85 && rand <= 95)
                {
                    planet[i] = planets[7];
                }
                else
                {
                    planet[i] = planets[8];
                }
            }
        }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            SolarSystem solSys = new SolarSystem();
            Console.WriteLine("Solar System Generator\n----------------------\n" + solSys.star + " - " + solSys.starType + "\n ");
            for(int i = 1; i < 9; i++)
            {
                Console.WriteLine( i + " - " + solSys.planet[i-1] + "\n ") ;
            }

            
        }
    }
}