using Microsoft.Xna.Framework;
using MonoGameEngine.StandardCore;
using StudentGame.Code.Screens;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StudentGame.Code.GameObjects
{
    internal class Player : GameObject
    {
        // Player Movement Settings
        public int MovementSpeed {  get; private set; }

        public Player() 
        {

            SetSprite("Pixel");
            GetSprite().SetScale(64, 64);
            GetSprite().SetTint(Color.Aqua);

            SetBounds(64, 64);
            MovementSpeed = 5;
            Camera.Instance.Easing = 0;

            SetDrawDebug(true, Color.Black);

            
        }

        public override void OnceAdded()
        {
            base.OnceAdded();
            Camera.Instance.LookAt(GetCenter());
        }

        public override void Update(float deltaTime)
        {
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
