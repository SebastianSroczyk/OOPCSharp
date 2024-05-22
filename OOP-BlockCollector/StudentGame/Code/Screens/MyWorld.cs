using StudentGame.Code.GameObjects;
using StudentGame.Code.GameObjects.Inventory;
using StudentGame.Code.GameObjects.Tiles;
using System;
<<<<<<< HEAD:Block-OOP-SebastianSroczyk/Block/Code/Screens/MyWorld.cs
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> 01b0d21af0f97f5a8b9a0a846b5168634694731a:OOP-BlockCollector/StudentGame/Code/Screens/MyWorld.cs
using System.Runtime.ExceptionServices;

namespace StudentGame.Code.Screens
{
    public class MyWorld : Screen
    {
        // Hold the TileMap instance
        private TileMap _tileMap;
        
        // Hold the Text instance for displaying a message on screen
        private Text _titleText;

        private ItemGenerator _itemGen;
        private InventoryManager _inventoryMan;
        private Player _player;


<<<<<<< HEAD:Block-OOP-SebastianSroczyk/Block/Code/Screens/MyWorld.cs
        private int numberOfMonsters = 0;
        private int currentRound = 500;

        private Dictionary<GameObject, Vector2> monsterDic = new Dictionary<GameObject, Vector2>();

=======
>>>>>>> 01b0d21af0f97f5a8b9a0a846b5168634694731a:OOP-BlockCollector/StudentGame/Code/Screens/MyWorld.cs
        //Set up level with gameobjects and engine options
        public override void Start(Core core)
        {
            base.Start(core);
            // TODO: Add your screen initialisation code between here...
            _tileMap = new TileMap();
            

            ItemGenerator itemGen = new ItemGenerator();
            _itemGen = itemGen;

            InventoryManager inventoryManager = new InventoryManager();
            _inventoryMan = inventoryManager;

            Player player = new Player(_inventoryMan);
            _player = player;

            AddObject(_inventoryMan, 0, 0);

            AddObject(_player, 100, 50);

            // Add a text object for displaying the camera center position
            _titleText = new Text(" Hello ", inScreenSpace: true);
            _titleText.SetScale(2.0f);
            AddText(_titleText, 800, 20);
            AddText("Controls", new Text("Controls: \nWASD - Move \nE - Pick Up \nI - Inventory \nZ - Move Up In Inventory \nC - Move Down In Inventory \nX - Drop Selected Item ", inScreenSpace: true), 1600, 100);

<<<<<<< HEAD:Block-OOP-SebastianSroczyk/Block/Code/Screens/MyWorld.cs
            _gameStateManager.PlayGame();
            var Object = _gameStateManager.GenerateObjects();

            monsterDic.Add(Object.Item1,Object.Item2);
            

            if (Object != null )
            {
                AddObject(Object.Item1, (int)Object.Item2.X, (int)Object.Item2.Y);
                
            }
            
=======
>>>>>>> 01b0d21af0f97f5a8b9a0a846b5168634694731a:OOP-BlockCollector/StudentGame/Code/Screens/MyWorld.cs
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
                AddObject(_itemGen.GenerateCoin(), _itemGen.objectXPos, _itemGen.objectYPos);

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
            
            GenerateObjects();

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
