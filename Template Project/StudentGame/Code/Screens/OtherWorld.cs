using StudentGame.Code.GameObjects;

namespace StudentGame.Code.Screens
{
    // Just another Screen to test transitions with
    internal class OtherWorld : Screen
    {
        public override void Start(Core core)
        {
            base.Start(core);

            AddObject(new Player(), 800, 500);

            Transition.Instance.EndTransition(TransitionType.Fade, 0.75f);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // Begin a standalone spritebatch pass for the tilemap
            spriteBatch.Begin(transformMatrix: Camera.Instance.GetViewMatrix(Vector2.One));



            spriteBatch.End();
            // End the standalone spritebatch pass for the tilemap

            // The rest of the screen renders as normal
            base.Render(spriteBatch);
        }
    }
}
