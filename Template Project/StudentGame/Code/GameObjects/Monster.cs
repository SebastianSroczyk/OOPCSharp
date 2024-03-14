using StudentGame.Code.Screens;
using System;

namespace StudentGame.Code.GameObjects
{
    internal class Monster : GameObject
    {
        //Attributes for the Monster
        public string _monsterName { get; set; }
        public int _damage { get;set; }
        public int _health { get; set; }


        public Monster() 
        {
            SetSprite("Hero");

            //GetSprite().SetOrigin(0.5f, 0.5f);
            GetSprite().SetTint(Color.Pink);
            //GetSprite().SetInWorldSpace(false);
            SetBounds(64, 64);
            
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }
    }
}
