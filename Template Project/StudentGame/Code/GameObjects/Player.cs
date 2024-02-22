using Microsoft.Xna.Framework;
using MonoGameEngine.StandardCore;
using StudentGame.Code.Inventory;
using StudentGame.Code.Screens;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StudentGame.Code.GameObjects
{
    internal class Player : GameObject
    {
        // Player Movement Settings
        InventoryManager _inventoryManager {  get; set; }
        InventoryItem _currentItem {  get; set; }
        public int MovementSpeed {  get; private set; }
        public int PlayerHealth { get; set; }

        public int PlayerDamage { get; set; }

        public Player() 
        {

            SetSprite("Pixel");
            GetSprite().SetScale(64, 64);
            GetSprite().SetTint(Color.Aqua);

            SetBounds(64, 64);
            MovementSpeed = 5;
            Camera.Instance.Easing = 0;

            SetDrawDebug(true, Color.Black);

            _inventoryManager = new InventoryManager();
            _currentItem = new InventoryItem();

            Weapon w = new Weapon(1,"Sword","A sword", 50);
            _inventoryManager.addItem(w);
            PlayerDamage = w.hitPoints;
            _currentItem = w;


            Potion p = new Potion(1,"Health Potion", "Coke", 10);
            _inventoryManager.addItem(p);

            PlayerHealth = 100;
            

            
        }

        public override void OnceAdded()
        {
            base.OnceAdded();
            Camera.Instance.LookAt(GetCenter());
        }

        public override void Update(float deltaTime)
        {

            // ---------------------For Debug Only------------------------------ 
            Console.WriteLine(_inventoryManager.TakeItem(0).description);
            Console.WriteLine(_inventoryManager.TakeItem(1).description);
            // -----------------------------------------------------------------

            Movement();
            RemoveObejctOnCollision();
        }
        

        private void CollisionCheck()
        {
            if (IsAtScreenEdge())
            {
                RevertPosition();
            }
        }

        private void Movement()
        {
            if (GameInput.IsKeyHeld("w"))
            {
                SetPosition(GetX(), GetY() - MovementSpeed);
                CollisionCheck();
            }
            if (GameInput.IsKeyHeld("s"))
            {
                SetPosition(GetX(), GetY() + MovementSpeed);
                CollisionCheck();
            }
            if (GameInput.IsKeyHeld("a"))
            {
                SetPosition(GetX() - MovementSpeed, GetY());
                CollisionCheck();
            }
            if (GameInput.IsKeyHeld("d"))
            {
                SetPosition(GetX() + MovementSpeed, GetY());
                CollisionCheck();
            }
            
        }
        
        
        private void RemoveObejctOnCollision()
        {
            GameObject gameObject = GetOneIntersectingObject<Other>();

            if (gameObject != null)
            {
                GetScreen().RemoveObject(gameObject);
                Transition.Instance.ToScreen<OtherWorld>(TransitionType.Fade, fadeColour: Color.Black, 0.25f);
            }
        }
    }
}
