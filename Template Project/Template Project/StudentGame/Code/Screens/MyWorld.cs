using StudentGame.Code.GameObjects;
using StudentGame.Code.GameObjects.Tiles;

namespace StudentGame.Code.Screens
{
    public class MyWorld : Screen
    {
        // Hold the TileMap instance
        private TileMap _tileMap;
        
        // Hold the Text instance for displaying a message on screen
        private Text _titleText;

        public override void Start(Core core)
        {
            base.Start(core);
            // TODO: Add your screen initialisation code between here...
            _tileMap = new TileMap();
            AddObject(new Player(), 800, 500);
            AddObject(new Other(), 20, 20);

            // Add a text object for displaying the camera center position
            _titleText = new Text("Test text", inScreenSpace: true);
            _titleText.SetScale(2.0f);
            AddText(_titleText, 800, 20);

            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
           
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // Begin a standalone spritebatch pass for the tilemap
            spriteBatch.Begin(transformMatrix: Camera.Instance.GetViewMatrix(Vector2.One));

            

            spriteBatch.End();
            // End the standalone spritebatch pass for the tilemap

            // The rest of the screen renders as normal
            base.Render(spriteBatch); 
        }

        public TileMap GetTileMap()
        {
            return _tileMap;
        }
    }
}
