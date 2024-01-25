using StudentGame.Code.Screens;
using System;

namespace StudentGame.Code.GameObjects
{
    internal class Other : GameObject
    {
        public Other() 
        {
            SetSprite("Pixel");
            GetSprite().SetScale(64, 64);
            GetSprite().SetOrigin(0.5f, 0.5f);
            GetSprite().SetTint(Color.Pink);
            GetSprite().SetInWorldSpace(false);
            SetBounds(64, 64);
            
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
