using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects.Inventory
{
    internal class Coin : InventoryItem
    {
        // Values used for the scoring system
        private int sellValue = 0;
        private int[] coinValue = { 10, 20, 30 };
        public int SellValue { get { return sellValue; } }
        
        // Timer used for movement
        private float timer = 1;
        // Movement direction for coins 
        private Vector2 direction;

        //Default constructor
        public Coin() : base()
        {
            sellValue = 0;
            Size = 64;
            Name = "NO Coin";

            SetBounds(this.Size, this.Size);
        }

        // Overloaded Constructor
        public Coin(int size, string name):
            base(size, name)
        {

            this.Size = size;
            this.Name = name;
            SettingSprite(name);
        }

        /// <summary>
        /// This method sets sprites depending on the name of the object
        /// </summary>
        /// <param name="name"></param>
        private void SettingSprite(string name)
        {
            
            switch (name)
            {
                case "Gold Coin":
                    SetSprite("GoldCoin");
                    sellValue = coinValue[2];
                    Speed = 7;
                    break;

                case "Silver Coin":
                    SetSprite("SliverCoin");
                    sellValue = coinValue[1];
                    Speed = 3;
                    break;

                case "Bronze Coin":
                    SetSprite("BronzeCoin");
                    sellValue = coinValue[0];
                    Speed = 1;
                    break;
            }
            SetBounds(this.Size, this.Size);
        }

        /// <inheritdoc/>
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);


        }

        /// <inheritdoc/>
        public override void Move(float deltaTime)
        {
            timer += deltaTime;
            if (timer > 1)
            {
                direction = new Vector2(Core.GetRandomNumber(-1f, 1f), Core.GetRandomNumber(-1f, 1f));
                direction.Normalize();
                timer = 0;
            }
            SetPosition(GetPosition() + (direction * Speed));
        }
    }
}
