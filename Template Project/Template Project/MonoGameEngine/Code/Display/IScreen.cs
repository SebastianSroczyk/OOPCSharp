using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine
{
    public interface IScreen
    {
        /// <summary>
        /// Method used to automatically setup the screen before it begins.
        /// </summary>
        public abstract void Start(Core core);

        /// <summary>
        /// Method used to clear any created instances held by this Screen. Automatically called before a new Screen is started.
        /// </summary>
        public abstract void End();

        /// <summary>
        /// Method used to send out update requests to all registered GameObjects belonging to this Screen. Called (roughly) 60 frames per second.
        /// </summary>
        /// <param name="deltaTime">The amount of time that has passed since the last frame, in seconds. Taken from MonoGame's GameTime object.</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// Method used to render any registered GameObjects.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch of the current GameCore.</param>
        public abstract void Render(SpriteBatch spriteBatch);

        public abstract void PreRender(SpriteBatch spriteBatch);

        public abstract void PostRender(SpriteBatch spriteBatch);
    }
}
