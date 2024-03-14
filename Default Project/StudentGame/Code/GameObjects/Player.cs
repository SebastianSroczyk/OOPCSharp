using Microsoft.Xna.Framework;
using MonoGameEngine.StandardCore;
using System;

namespace StudentGame.Code.GameObjects
{
    internal class Player : GameObject
    {

        public int health { get; set; }
        public int damage { get; set; }

        // Default Constructor
        public Player()
        { 
            SetSprite("player");
            SetBounds(64, 64);

            Camera.Instance.Easing = 0;

            SetDrawDebug(true, Color.Black);

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
                
            }

            // When the e key is released - turn of the collect flag
            if(GameInput.IsKeyReleased("e"))
            {

            }

            // Display the inventory!
            if(GameInput.IsKeyPressed("i"))
            {
            
            }

            if(GameInput.IsKeyReleased("i"))
            {
             
            }

            if(GameInput.IsKeyPressed("z"))
            {
                
            }

            if(GameInput.IsKeyPressed("x"))
            {
                
            }

            // C to drop an Item
            if(GameInput.IsKeyPressed("c"))
            {
             
            }

            if(GameInput.IsKeyPressed("space"))
            {
                
            }
        }

        private void Collisions()
        {
            GameObject go = (GameObject) GetOneIntersectingObject<GameObject>();

        }

        public void UseItem()
        {
                  
        }

        public void Attack(Monster m)
        {
            
        }

        /**
         * Code to pick up an item and add it to the backpack
         */
        public void PickUpItem()
        {
            
        }

        public void DropItem()
        {
     
        }
    }
}
