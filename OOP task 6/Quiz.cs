using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using OOP_task_6.Questions;

namespace OOP_task_6
{
    internal class Quiz
    {
        /*  capitals knowledge quiz
         *  
         *  question1: Captial of Scotland  ans: Edinbrugh
         *  question2: Captial of America   ans: Washington DC
         *  question3: Captial of Russia    ans: Moscow
         *  question4: Captial of Bosnia    ans: Sarajevo
         *  question5: Captial of Malaysia  ans: Federal Territory of Kuala Lumpur
         */
        private Question[] questionSet;
        private int scoreTotal;
        

        public Quiz() 
        {
            questionSet = new Question[5];

            // question 1
            string text = "Scotland";
            string[] options = { "Edinburgh", "London", "Glasgow", "Leeds" };
            int points = 1;
            int correctAnswers = 0;

            questionSet[0] = new Question(text,options,points,correctAnswers);

            // question 2
            string text2 = "United States of America";
            string[] options2 = { "Texas", "Florida", "New York", "Washington DC" };
            int points2 = 1;
            int correctAnswers2 = 3;

            questionSet[1] = new Question(text2, options2, points2, correctAnswers2);

            // question 3
            string text3 = "Russia";
            string[] options3 = { "Saint Petersburg", "Kaliningrad", "Moscow", "Stalingrad" };
            int points3 = 1;
            int correctAnswers3 = 2;

            questionSet[2] = new Question(text3, options3, points3, correctAnswers3);

            // question 4
            string text4 = "Bosnia";
            string[] options4 = { "Berlin", "Paris", "Budapest", "Sarajevo" };
            int points4 = 1;
            int correctAnswers4 = 3;

            questionSet[3] = new Question(text4, options4, points4, correctAnswers4);

            // question 5
            string text5 = "Malaysia";
            string[] options5 = { "Dundee", "Federal Territory of Kuala Lumpur", "Dunajee", "Federal City of Malaysia" };
            int points5 = 1;
            int correctAnswers5 = 1;

            questionSet[4] = new Question(text5, options5, points5, correctAnswers5);

            

        }
        public void StartQuiz()
        {
            int userChoice;

            for (int i = 0; i < questionSet.Length; i++)
            {
                questionSet[i].displayQuestion();

                Console.WriteLine("Please enter your choice! ");

                userChoice = int.Parse(Console.ReadLine());

                Console.Clear();

                if (questionSet[i].isCorrect(userChoice))
                {
                    scoreTotal += questionSet[i].getScore();
                }
            }
            
        }
        public void DisplayInfo()
        {
            Console.WriteLine("Welcome to the Quiz! \n Please enter 1-4 to make your choice!");

        }
        public void DisplayScore()
        {
            Console.WriteLine("Your score is: " + scoreTotal);
        }
        
        

        

    }
}
