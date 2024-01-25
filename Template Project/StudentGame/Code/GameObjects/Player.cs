using Microsoft.Xna.Framework;
using MonoGameEngine.StandardCore;

namespace StudentGame.Code.GameObjects
{
    internal class Player : GameObject
    {

        public Player() 
        {

            SetSprite("Pixel");
            GetSprite().SetScale(64, 64);
            GetSprite().SetTint(Color.Aqua);

            SetBounds(64, 64);

            Camera.Instance.Easing = 0;

            SetDrawDebug(true, Color.Black);
        }

        public override void Render(SpriteBatch spriteBatch)
        {

           
        }

        public override void OnceAdded()
        {
            base.OnceAdded();
            Camera.Instance.LookAt(GetCenter());
        }

        public override void Update(float deltaTime)
        {
           
           
        }
    }
}
