using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGameEngine.StandardCore;
using StudentGame.Code.GameObjects.Inventory;
using System;

namespace StudentGame.Code.GameObjects
{
    internal class Player : GameObject
    {
        //Backpack
        private InventoryManager _inventoryManager { get; set; }
        private InventoryItem _currentItem { get;set; }
        private Coin _coin { get; set; }


        public int Speed {  get; set; }
        public Vector2 PlayerPosition{  get; private set; }

        private bool canMove = true;
        public bool CanMove { get { return canMove; } set { canMove = value; } }


        private bool _collect = false;
        private int score = 0;
        public int Score { get { return score; } set { score = value; } }

        // Default Constructor
        public Player(InventoryManager inventory)
        { 
            SetSprite("player");
            SetBounds(64, 64);

            Camera.Instance.Easing = 0;

            SetDrawDebug(true, Color.Black);

            // Initialise Inventory manager
            _inventoryManager = inventory ;
            _currentItem = null;


            Speed = 5;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
           
        }

        public override void OnceAdded()
        {
            base.OnceAdded();
            Camera.Instance.LookAt(GetCenter());
        }

        public override void Update(float deltaTime)
        {
            PlayerPosition = new Vector2(GetX(), GetY());
            GetInput();
            Collisions();
 
        }
        /// <summary>
        /// Gets the inputs from the player and moves the player 
        /// </summary>
        private void GetInput()
        {
            if(canMove == true)
            {
                if (GameInput.IsKeyHeld("a"))
                {
                    Vector2 pos = new Vector2();
                    pos.X = GetX() - Speed;
                    pos.Y = GetY();

                    SetPosition(pos);
                }

                if (GameInput.IsKeyHeld("d"))
                {
                    Vector2 pos = new Vector2();
                    pos.X = GetX() + Speed;
                    pos.Y = GetY();

                    SetPosition(pos);
                }

                if (GameInput.IsKeyHeld("w"))
                {
                    Vector2 pos = new Vector2();
                    pos.X = GetX();
                    pos.Y = GetY() - Speed;

                    SetPosition(pos);
                }

                if (GameInput.IsKeyHeld("s"))
                {
                    Vector2 pos = new Vector2();
                    pos.X = GetX();
                    pos.Y = GetY() + Speed;

                    SetPosition(pos);
                }

                //Check to see if the user has indicated collect 
                // Set a boolean to hold the choice.
                if (GameInput.IsKeyHeld("e"))
                {
                    _collect = true;
                }

                // When the e key is released - turn of the collect flag
                if (GameInput.IsKeyReleased("e"))
                {

                    _collect = false;
                }

                // Display the inventory!
                if (GameInput.IsKeyPressed("i"))
                {
                    _inventoryManager.DisplayInventory();
                }

                if (GameInput.IsKeyReleased("i"))
                {
                    _inventoryManager.DisplayInventory();
                }

                if (GameInput.IsKeyPressed("z"))
                {
                    _currentItem = _inventoryManager.GetPreviousItem();
                }

                if (GameInput.IsKeyPressed("c"))
                {
                    _currentItem = _inventoryManager.GetNextItem();
                }

                // C to drop an Item
                if (GameInput.IsKeyPressed("x"))
                {

                    if (!_inventoryManager.CheckIfEmpty())
                    {
                        // gets refence for item selected
                        _inventoryManager.RemoveItem(_currentItem);
                        DropItem(_currentItem, PlayerPosition);
                    }
                }
            }
            
        }

        /// <summary>
        /// Checks for collisions
        /// </summary>
        private void Collisions()
        {
            GameObject go = (GameObject) GetOneIntersectingObject<GameObject>();

            // If there's a collision with an inventory item then collect it
            // 
            if(_collect == true && go is InventoryItem) // Check if collision is true and collision object is an inventory item
            {
                InventoryItem ii = (InventoryItem)go;

                // updates the score on collision
                UpdateScore(ii);

                PickUpItem(ii);
            }
        }

        /// <summary>
        /// Updates the score on collison and pick up
        /// </summary>
        /// <param name="ii"></param>
        private void UpdateScore(InventoryItem ii)
        {
            if (ii is Coin)
            {
                Coin c = (Coin)ii;
                Score += c.SellValue;
            }
        }

        /// <summary>
        ///  Pick up items 
        /// </summary>
        /// <param name="item"></param>
        public void PickUpItem(InventoryItem item)
        {
            if (_inventoryManager.AddItem(item))
            {
                item.SetVisible(false); // hide the item because it's now in the inventory
                item.SetPosition(new Vector2(0, 0)); // Update the position - if we drop it we'll need to change

                
                _collect = false;
            }
        }

        /// <summary>
        /// Drops items
        /// </summary>
        /// <param name="item"></param>
        /// <param name="playerPos"></param>
        public void DropItem(InventoryItem item, Vector2 playerPos)
        {
            
            if (item != null)
            {
                item.SetVisible(true); // hide the item because it's now in the inventory
                item.SetPosition(playerPos); // Update the position - if we drop it we'll need to change
                _currentItem = null;
                _collect = false;
                
            }
        }
    }
}
