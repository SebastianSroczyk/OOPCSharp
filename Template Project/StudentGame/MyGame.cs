using StudentGame.Code.Screens;

namespace StudentGame
{
    public class MyGame : Core
    {
        protected override void Initialize()
        {
            Window.Title = "MyGame";
            // TODO: Add your initialization logic between here...
            Camera.Instance.ClampWithinWorld = true;
            Settings.ScreenDimensions = new Vector2(1920, 1080);

            Settings.LetterboxFill = Color.Black;
            Settings.IsMouseVisible = false;

            // TODO: Use this code to set the initial screen
            StartScreen<MyWorld>();
            base.Initialize();
        }
    }
}