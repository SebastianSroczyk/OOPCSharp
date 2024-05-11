using Block.Code.GameObjects;
using Block.Code.GameObjects.Enemy;
using Block.Code.GameObjects.Inventory;
using Block.Code.GameObjects.Managers;
using Block.Code.GameObjects.Tiles;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using ObjectManager = Block.Code.GameObjects.Managers.ObjectManager;

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
        private ObjectManager _objectManager;


        private int numberOfMonsters = 0;
        private int currentRound = 500;

        //Set up level with gameobjects and engine options
        public override void Start(Core core)
        {
            base.Start(core);
            // TODO: Add your screen initialisation code between here...
            _tileMap = new TileMap();

            _gameStateManager = new GameStateManager();

            ObjectManager objectManager = new ObjectManager();
            _objectManager = objectManager;

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


            AddObject(GenerateObjects(),(int), (int)_objectManager.ObjectPosition.Y);
            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }
        public GameObject GenerateObjects()
        {
            Vector2 BoxTopLeft = new Vector2(300, 300);
            Vector2 BoxBottomRight = new Vector2(900, 900);

            while (_gameStateManager.CurrentNumberOfMonsters != _gameStateManager.CurrentRound)
            {
                Vector2 pos = PlaceObjects(BoxTopLeft, BoxBottomRight);
                MonsterObjectPool.Add(_monsterGen.GenerateMonster());
                MonsterPoolPos.Add(ObjectPosition);
                stateManager.CurrentNumberOfMonsters++;
            }

            // Disables all objects in Object Pool
            foreach (GameObject obj in MonsterObjectPool)
            {
                obj.SetActive(false);
                obj.SetVisible(false);
            }

            return MonsterObjectPool[stateManager.CurrentNumberOfMonsters];
        }
        private Vector2 PlaceObjects(Vector2 p1, Vector2 p2)
        {
            Random r = new Random();

            Vector2 tempPos = new Vector2(r.Next((int)Settings.GameResolution.X - 30), r.Next((int)Settings.GameResolution.Y - 30));

            while ((tempPos.X > p1.X && tempPos.X < p2.X) && (tempPos.Y > p1.Y && tempPos.Y < p2.Y))
            {
                tempPos = new Vector2(r.Next((int)Settings.GameResolution.X - 30), r.Next((int)Settings.GameResolution.Y - 30));
            }

            return tempPos;
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
