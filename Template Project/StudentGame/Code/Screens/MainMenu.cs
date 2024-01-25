using StudentGame.Code.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGame.Code.Screens
{
    internal class MainMenu : Screen
    {
        public override void Start(Core core)
        {
            base.Start(core);


            Transition.Instance.EndTransition(TransitionType.Fade, 0.25f);

            //          Adds Objects/Buttons into the Menu
            //---------------------------------------------------

            //adds start button
            AddObject(new Button(), 600, 570);
            
            GameInput.SetMousePosition(core.GraphicsDevice.Viewport.Bounds.Center.ToVector2());
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

        }

    }
}
