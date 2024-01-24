using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OOP_task_6.Questions
{
    internal class Question
    {
        // Attributes of the class
        private string question = "";
        private string[] choices;
        private int score = 0;
        private int correctAnswer = 0;


        // Constructor  - Defualt
        public Question() 
        {
            question = "NO QUESTION PROVIDED";
            score = 0;
            choices = new string[] {"NO ANSWER", "NO ANSWER", "NO ANSWER", "NO ANSWER"};
        }

        // Constructor - Parameterised
        //   Overloaded Constructor
        public Question(string questionText, string[] questionOptions, int questionScore, int correctQuestionAnswer)
        {
            question = questionText;
            choices = questionOptions;
            score = questionScore;
            correctAnswer = correctQuestionAnswer;
        }

        public void displayQuestion()
        {
            // Print question
            Console.WriteLine("What is the Capital of " + question + "?");

            // Print question options
            for (int i = 0; i < choices.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + choices[i]);
            }
        }
        public bool isCorrect(int option)
        {
            option -= 1;
            if(option == correctAnswer)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
        public int getScore()
        {
            return score;
        }
    }
}
