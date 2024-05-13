using Block.Code.GameObjects.Managers;
using Block.Code.Screens;

namespace Block
{
    public class MyGame : Core
    {
        protected override void Initialize()
        {
            GameStateManager gameStateManager = new GameStateManager();

            Window.Title = "MyGame";
            // TODO: Add your initialization logic between here...
            Camera.Instance.ClampWithinWorld = true;
            Settings.ScreenDimensions = new Vector2(1920f, 1080f);

            Settings.LetterboxFill = Color.Black;
            Settings.IsMouseVisible = false;

            gameStateManager.PlayGame();
            // TODO: Use this code to set the initial screen
            StartScreen<MyWorld>();
            base.Initialize();
        }
    }
}