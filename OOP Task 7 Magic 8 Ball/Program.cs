namespace OOP_Task_7_Magic_8_Ball
{
    internal class Program
    {
        static void Main(string[] args)
        {
            _8_Ball m8Ball = new _8_Ball();

            // Ask Question
            m8Ball.AskQuestion();
            // Shakes the ball
            m8Ball.Shake();
        }
    }
}