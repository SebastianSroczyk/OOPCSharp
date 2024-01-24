using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace OOP_Task_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // variables
            int daysOfTheWeak = 7;
            float[] moneyTook = new float[daysOfTheWeak];
            string[] dayName = {"Monday", "Tuesday", "Wedensday", "Thursday", "Friday", "Saturday", "Sunday" };
            float RunningTotal = 0;
            float average = 0;

            //logic
            for (int i = 0; i < daysOfTheWeak; i++) 
            {
                Console.WriteLine("How much did you take on " + dayName[i]);
                moneyTook[i] = float.Parse(Console.ReadLine());
                RunningTotal += moneyTook[i];
            }
            // calculation
            
            average = RunningTotal / daysOfTheWeak;
            float averagePercent = (average * 0.05f) + average;
            Console.Clear();
            Console.WriteLine("Your average taking this week is: £" + Math.Round(average,2));

            for (int j = 0;  j < moneyTook.Length; j++)
            {
                if (moneyTook[j] >= averagePercent)
                {
                    Console.WriteLine("On " + dayName[j] + " You took £" + moneyTook[j] + " This is more then the average!" );
                }
            }
            


            
        }
    }
}