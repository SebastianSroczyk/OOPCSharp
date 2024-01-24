using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OOP_Task_3
{
    internal class Program
    {
        
        

        static void Main(string[] args)
        {
            // Variables 
            double paintAreaCoverage = 5; // meters squared
            double cost = 0;
            string colour = "";
            Console.WriteLine(" Paint Calculation ");
            Console.WriteLine("-------------------");
            Console.WriteLine("Whatt is your wall length");
            double wallLength = int.Parse(Console.ReadLine());
            Console.WriteLine("What is your wall hight");
            double wallHight = int.Parse(Console.ReadLine());

            double wallArea = wallHight * wallLength;
            double numberOfCans = wallArea / paintAreaCoverage;
            int roundingNumber = (int)Math.Round(numberOfCans);

            if(numberOfCans < roundingNumber)
            {
                numberOfCans = Math.Round((double)numberOfCans);
                Console.WriteLine("Number of cans: " + numberOfCans);
            }
            else
            {
                numberOfCans = Math.Round((double)numberOfCans) + 1;
                Console.WriteLine("Number of cans: " + numberOfCans);
            }
            Console.Clear();
            Console.WriteLine("What colour would you like? \n--------------------------- \nA)White\n B)Green\n C)Blue");
            char userInput = (char)Console.Read();
            switch (userInput)
            {
                case 'a':
                    cost = 7.50f * numberOfCans;
                    colour = "White";
                    break;
                case 'b':
                    cost = 8f * numberOfCans;
                    colour = "Green";
                    break;
                case 'c':
                    cost = 8.20f * numberOfCans;
                    colour = "Blue";
                    break;
            }
            Console.Clear();
            Console.WriteLine("Cost Calculations \n------------------ \nWall Length: " + wallLength + "\nWall Hight: " + wallHight + "\nColour of Paint: " + colour);
            Console.WriteLine("Cost of Cans: £" + Math.Round(cost,2));




        }
    }
}