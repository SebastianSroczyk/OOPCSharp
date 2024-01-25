using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects
{
    internal class Button : GameObject
    {
        public Button()
        {
            SetSprite("Pixel");
            GetSprite().SetScale(64, 64);
            GetSprite().SetTint(Color.Yellow);
            GetSprite().SetInWorldSpace(false);
        }
        public override void Update(float deltaTime)
        {
            //nothing too see here '_' (nothing is needed here)
        }

    }
}
