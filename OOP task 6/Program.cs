namespace OOP_task_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Quiz quiz = new Quiz();

            quiz.DisplayInfo();
            quiz.StartQuiz();
            quiz.DisplayScore();

            //testing
            /*
            string text = "Kane";
            string[] options = {"yes" , "no", "maybe", "Why am I here"};
            int correctOption = 1;
            int points = 1;
            Question question2 = new Question(text, options, points, correctOption);

            
            question2.displayQuestion();
            */
        }
    }
}