using Block.Code.GameObjects;
using Block.Code.GameObjects.Inventory;
using Block.Code.GameObjects.Tiles;
using System;
using System.Runtime.ExceptionServices;

namespace Block.Code.Screens
{
    public class MyWorld : Screen
    {
        // Hold the TileMap instance
        private TileMap _tileMap;
        
        // Hold the Text instance for displaying a message on screen
        private Text _titleText;

        private ItemGenerator _itemGen;
        private MonsterGen _monsterGen;
        private InventoryManager _inventoryMan;
        private Player _player;


        //Set up level with gameobjects and engine options
        public override void Start(Core core)
        {
            base.Start(core);
            // TODO: Add your screen initialisation code between here...
            _tileMap = new TileMap();
            

            ItemGenerator itemGen = new ItemGenerator();
            _itemGen = itemGen;

            MonsterGen monsterGen = new MonsterGen();
            _monsterGen = monsterGen;

            InventoryManager inventoryManager = new InventoryManager();
            _inventoryMan = inventoryManager;

            Player player = new Player(_inventoryMan);
            _player = player;

            AddObject(_inventoryMan, 0, 0);

            AddObject(_player, 100, 50);

            // Add a text object for displaying the camera center position
            _titleText = new Text("", inScreenSpace: true);
            _titleText.SetScale(2.0f);
            AddText(_titleText, 800, 20);
            
            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }
        public void GenerateObjects()
        {
            Random r = new Random();

            int chance = r.Next(100);

            //Check to see if we add a monster while the game is running
            //
            if (chance < 1)
            {
                AddObject(_itemGen.GeneratePotion(), _itemGen.objectXPos, _itemGen.objectYPos);

            }
            else if (chance == 10)
            {

                AddObject(_monsterGen.GenerateMonster(), _monsterGen.xPos, _monsterGen.yPos);

            }
            else if (chance > 98)
            {

                AddObject(_itemGen.GenerateWeapon(), _itemGen.objectXPos, _itemGen.objectYPos);

            }
        }

        /**
         * Update the level including any random elements.
         */
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //GenerateObjects();

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
