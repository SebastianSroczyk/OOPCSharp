using StudentGame.Code.Screens;

namespace StudentGame
{
    public class MyGame : Core
    {
        

        protected override void Initialize()
        {
<<<<<<< HEAD:Block-OOP-SebastianSroczyk/Block/MyGame.cs
            
=======
>>>>>>> 01b0d21af0f97f5a8b9a0a846b5168634694731a:OOP-BlockCollector/StudentGame/MyGame.cs
            Window.Title = "MyGame";
            // TODO: Add your initialization logic between here...
            Camera.Instance.ClampWithinWorld = true;
            Settings.ScreenDimensions = new Vector2(1920, 1080);

            Settings.LetterboxFill = Color.Black;
            Settings.IsMouseVisible = false;

<<<<<<< HEAD:Block-OOP-SebastianSroczyk/Block/MyGame.cs
            
=======
>>>>>>> 01b0d21af0f97f5a8b9a0a846b5168634694731a:OOP-BlockCollector/StudentGame/MyGame.cs
            // TODO: Use this code to set the initial screen
            StartScreen<MyWorld>();
            base.Initialize();
        }
    }
}