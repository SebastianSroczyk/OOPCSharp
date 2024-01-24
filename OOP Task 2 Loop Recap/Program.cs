namespace OOP_Task_2_Loop_Recap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int number = 0;
            int square = 0;
            int cube = 0;
            int factorial = 0;
            Console.WriteLine("Number\tSquare\tCube");
            for (int i = 0; i < 25; i++) 
            {
                number++;
                square = number * number;
                cube = number * number * number;

                Console.WriteLine(number + "\t" + square + "\t" + cube);
            }
            



            
        }
    }
}