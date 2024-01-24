namespace OOP_Task_10___Text_Based_Adventure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //              Initilization
            //=====================================================
            GameStateManager GSM = new GameStateManager();

            // Runs game
            GSM.PlayGame();
        }
    }
}
