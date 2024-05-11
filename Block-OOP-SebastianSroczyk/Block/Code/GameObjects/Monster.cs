using Block.Code.Screens;
using System;

namespace Block.Code.GameObjects
{
    internal class Monster : GameObject
    {
        //Attributes for the Monster
        public string _monsterName { get; private set; }
        public int _damage { get; private set; }

        private int health;
        public int PHealth
        {
            get { return health; }
            set
            {
                health = value;
                if (health < 0)
                {
                    health = 0;
                }
            }
        }


        public Monster()
        {
            SetSprite("Hero");
            SetBounds(64, 64);
            PHealth = 0;
        }

        public Monster(string name, int health, int damage)
        {
            this._monsterName = name;
            this.PHealth = health;
            this._damage = damage;
            SetSprite("Hero");
            SetBounds(64, 64);
            
        }


        public override void Update(float deltaTime)
        {
            Console.WriteLine(PHealth);
            CheckHealth();
        }

        private void CheckHealth()
        {
           if (PHealth < 0)
            {
                Console.WriteLine(PHealth);
            } 
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }
    }
}
