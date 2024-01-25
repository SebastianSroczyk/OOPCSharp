using StudentGame.Code.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.GameObjects
{
    internal class Mouse : GameObject
    {


        public Mouse()
        {
            SetSprite("col1");
            GetSprite().SetInWorldSpace(false);
        }
        //---------------------------------------------------
        //                U P D A T E
        //---------------------------------------------------
        public override void Update(float deltaTime)
        {
            SetPosition(GameInput.GetMousePosition());
            if (GameInput.IsMouseButtonPressed(MouseButton.Left))
            {
                CheckCollisions();
            }
        }
        //---------------------------------------------------
        //          C H E C K   C O L L I S O N S
        //---------------------------------------------------
        private void CheckCollisions()
        {
            // === checks if the mouse is over the start button===
            GameObject other = GetOneIntersectingObject<Button>();
            if (other != null)
            {
                GetScreen().RemoveObject(this);
                Transition.Instance.ToScreen<MyWorld>(TransitionType.Fade, fadeColour: Microsoft.Xna.Framework.Color.Black, 0.25f);
            }

        }
    }
}
