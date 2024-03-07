using Microsoft.Xna.Framework;
using MonoGameEngine.StandardCore;
using StudentGame.Code.GameObjects.Inventory;
using System;

namespace StudentGame.Code.GameObjects
{
    internal class Player : GameObject
    {
        //Backpack
        InventoryManager _inventoryManager { get; set; }
        InventoryItem _currentItem { get;set; }

        public int health { get; set; }
        public int damage { get; set; }

        private bool _collect = false;
        private bool _attack = false;

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

            health = 100; // initial health value
            damage = 5;
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
            GetInput();
            Collisions();
 
        }

        private void GetInput()
        {
            if(GameInput.IsKeyPressed("a"))
            {
                Vector2 pos = new Vector2();
                pos.X = GetX() - 4;
                pos.Y = GetY();

                SetPosition(pos);
            }

            if(GameInput.IsKeyPressed("d"))
            {
                Vector2 pos = new Vector2();
                pos.X = GetX() +4;
                pos.Y = GetY();

                SetPosition(pos);
            }

            if(GameInput.IsKeyPressed("w"))
            {
                Vector2 pos = new Vector2();
                pos.X = GetX();
                pos.Y = GetY() - 4;

                SetPosition(pos);
            }

            if(GameInput.IsKeyPressed("s"))
            {
                Vector2 pos = new Vector2();
                pos.X = GetX();
                pos.Y = GetY() + 4;

                SetPosition(pos);
            }

            //Check to see if the user has indicated collect 
            // Set a boolean to hold the choice.
            if(GameInput.IsKeyPressed("e"))
            {
                _collect = true;
            }

            // When the e key is released - turn of the collect flag
            if(GameInput.IsKeyReleased("e"))
            {
                _collect = false;
            }

            // Display the inventory!
            if(GameInput.IsKeyPressed("i"))
            {
                _inventoryManager._displayInventory = true;
            }

            if(GameInput.IsKeyReleased("i"))
            {
                _inventoryManager._displayInventory = false;
            }

            if(GameInput.IsKeyPressed("z"))
            {
                _currentItem = _inventoryManager.GetPreviousItem();
            }

            if(GameInput.IsKeyPressed("x"))
            {
                _currentItem = _inventoryManager.GetNextItem();
            }

            // C to drop an Item
            if(GameInput.IsKeyPressed("c"))
            {
                _inventoryManager.RemoveItem(0);
                _currentItem = null;
            }

            if(GameInput.IsKeyPressed("space"))
            {
                _attack = true;
            }
        }

        private void Collisions()
        {
            GameObject go = (GameObject) GetOneIntersectingObject<GameObject>();

            // If there's a collision with an inventory item then collect it
            // 
            if(_collect == true && go is InventoryItem) // Check if collision is true and...
            {
                InventoryItem ii = (InventoryItem)go;
               PickUpItem(ii);
            }

            if(_attack == true && go is Monster)
            {
                Monster m = (Monster) go;
                Attack(m);
            }
        }

        public void UseItem()
        {
            if(_currentItem != null)
                _currentItem.Use(this);            
        }

        public void Attack(Monster m)
        {
            
        }

        /**
         * Code to pick up an item and add it to the backpack
         */
        public void PickUpItem(InventoryItem item)
        {
            if(_inventoryManager.AddItem(item))
            {
                item.SetVisible(false); // hide the item because it's now in the inventory
                item.SetPosition(new Vector2(0, 0)); // Update the position - if we drop it we'll need to change
                _currentItem = item;
                _collect = false;
            }
        }

        public void DropItem()
        {

        }
    }
}
