using StudentGame.Code.GameObjects;
using StudentGame.Code.GameObjects.Inventory;
using StudentGame.Code.GameObjects.Tiles;
using System;
using System.Runtime.ExceptionServices;

namespace StudentGame.Code.Screens
{
    public class MyWorld : Screen
    {
        // Hold the TileMap instance
        private TileMap _tileMap;
        
        // Hold the Text instance for displaying a message on screen
        private Text _titleText;

        private MonsterGen _monsterGen = new MonsterGen();

        //Set up level with gameobjects and engine options
        public override void Start(Core core)
        {
            base.Start(core);
            // TODO: Add your screen initialisation code between here...
            _tileMap = new TileMap();
            InventoryManager im = new InventoryManager();
            AddObject(im, 0, 0);
            AddObject(new Player(im), 100, 50);
            
            //AddObject(new Monster(), 20, 20);

            // Add a text object for displaying the camera center position
            _titleText = new Text("", inScreenSpace: true);
            _titleText.SetScale(2.0f);
            AddText(_titleText, 800, 20);

            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }

        /**
         * Update the level including any random elements.
         */
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Random r = new Random();
            int chance = r.Next(100);

            //Check to see if we add a monster while the game is running
            //
            if(chance < 1)
            {
                int x = r.Next(300);
                int y = r.Next(400);
                AddObject(new Potion(), x, y);
                //Console.WriteLine("Spawning a Monster");
                //Monster m = _monsterGen.generateMonster();
                //AddObject(m, (int)m.GetX(), (int)m.GetY());
            }     
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
