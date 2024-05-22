using Block.Code.GameObjects;
using Block.Code.GameObjects.Enemy;
using Block.Code.GameObjects.Inventory;
using Block.Code.GameObjects.Managers;
using Block.Code.GameObjects.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;

namespace Block.Code.Screens
{
    public class MyWorld : Screen
    {
        // Hold the TileMap instance
        private TileMap _tileMap;
        
        // Hold the Text instance for displaying a message on screen
        private Text _titleText;

        private GameStateManager _gameStateManager;
        private ItemGenerator _itemGen;
        private MonsterGen _monsterGen;
        private InventoryManager _inventoryMan;
        private Player _player;


        private int numberOfMonsters = 0;
        private int currentRound = 500;

        private Dictionary<GameObject, Vector2> monsterDic = new Dictionary<GameObject, Vector2>();

        //Set up level with gameobjects and engine options
        public override void Start(Core core)
        {
            base.Start(core);
            // TODO: Add your screen initialisation code between here...
            _tileMap = new TileMap();

            _gameStateManager = new GameStateManager();


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
            _titleText = new Text("Block", inScreenSpace: true);
            _titleText.SetScale(2.0f);
            AddText(_titleText, 800, 20);

            _gameStateManager.PlayGame();
            var Object = _gameStateManager.GenerateObjects();

            monsterDic.Add(Object.Item1,Object.Item2);
            

            if (Object != null )
            {
                AddObject(Object.Item1, (int)Object.Item2.X, (int)Object.Item2.Y);
                
            }
            
            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }
        
        




        /**
         * Update the level including any random elements.
         */
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
