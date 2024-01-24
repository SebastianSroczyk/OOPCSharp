using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Task_7_Magic_8_Ball
{
    internal class _8_Ball
    {

        //    Attributes: 
        //          string[] responsesArray
        //          string response
        //          int randomPrediction (random number)
        //          

        //    Methods: 
        //          Shake()
        //          InputQuestion()
        //          SetResponses();
        //          DisplayPrediction()

        private string[] responseArray = { "Yes", "No", "You will meet a stanger", "Go for it", "Doubt it", "This might be your lucky day", "Consider things differently", "A dream you have might come true" };
        private string response;
        private int randomPrediction;
        private string question;
        
        

        // Costructor - Defualt
        public _8_Ball() 
        {
           
        }
        // AskQuestion Method
        public void AskQuestion()
        {
            Console.WriteLine("Ask your question and press 'ENTER' ");
            question = Console.ReadLine();
        }
        // Shake Method
        public void Shake()
        {
            response = SetResponse();
            DisplayPrediction();
        }
        // setResponse Method
        private string SetResponse()
        {
            return responseArray[GetRandomPrediction()];
        }
        // getRandomPrediction Method
        private int GetRandomPrediction()
        {
            Random random = new Random();

            int randomPredition = random.Next(8);

            return randomPredition;
        }
        // DisplayPrediction
        private void DisplayPrediction()
        {
            Console.WriteLine("\n" + response);
        }


    }
}
