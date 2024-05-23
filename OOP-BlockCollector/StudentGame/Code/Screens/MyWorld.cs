using MonoGame.Extended;
using MonoGame.Extended.Timers;
using StudentGame.Code.GameObjects;
using StudentGame.Code.GameObjects.Inventory;
using StudentGame.Code.GameObjects.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
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

        // game timers
        private double timer = 30;
        

        //Set up level with gameobjects and engine options
        public override void Start(Core core)
        {
            base.Start(core);
            
            

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

            // text for controls
            AddText("Controls", new Text("Controls: \nWASD - Move \nE - Pick Up \nI - Inventory \nZ - Move Up In Inventory \nC - Move Down In Inventory \nX - Drop Selected Item ", inScreenSpace: true), 1600, 50);
            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }
        


        
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            CountDownTimer(deltaTime, CheckEndTimer());
            GenerateObjects(CheckEndTimer());
            
        }

        /// <summary>
        /// Check for End of the timer (if timer has reached 0)
        /// </summary>
        /// <returns></returns>
        public bool CheckEndTimer()
        {
            bool End; 

            if(timer < 0)
            {
                AddText("End", new Text($"Total Score:{_player.Score}"), 960, 540);
                
                End = true;
                _player.CanMove = false;
            }
            else
            {
                End = false;
            }
            return End;
        }

        /// <summary>
        /// Generates Objects
        /// </summary>
        /// <param name="hasFinished"></param>
        public void GenerateObjects(bool hasFinished)
        {
            if (hasFinished == false)
            {
                Random r = new Random();

                int chance = r.Next(100);

                //Check to see if we add a monster while the game is running
                //
                if (chance < 1)
                {
                    AddObject(_itemGen.GenerateCoin(), _itemGen.objectXPos, _itemGen.objectYPos);
                }
            }
        }

        /// <summary>
        /// Counts down on the timer
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="hasFinished"></param>
        public void CountDownTimer(float deltaTime, bool hasFinished)
        {
            if(hasFinished == false)
            {
                timer -= deltaTime;
                var seconds = Math.Round(timer);
                AddText("Timer", new Text($"Time Left: {seconds}"), 900, 50);
                RemoveText("Timer");
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
