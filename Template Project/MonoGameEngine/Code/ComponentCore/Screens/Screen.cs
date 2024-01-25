using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.ComponentCore.GameObjects;
using MonoGameEngine.ComponentCore.GameObjects.Components;
using System.Collections.Generic;

namespace MonoGameEngine.ComponentCore.Screens
{
    internal abstract class Screen : IScreen
    {
        /// <summary>
        /// A reference to the GameCore, allowing access to some high-level functionality.
        /// </summary>
        protected Core _core;

        /// <summary>
        /// A collection of all the GameObjects existing in this Screen. All new GameObjects should be placed in this collection for automatic updating and rendering.
        /// </summary>
        private List<GameObject> _gameObjects;

        public virtual void Start(Core core) 
        { 
            _core = core; 
            _gameObjects = new List<GameObject>(); 
        }

        public virtual void End()
        {
            _core = null;
            for(int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i] = null;
            }
            _gameObjects.Clear();
            _gameObjects = null;
        }

        
        public virtual void Update(float deltaTime)
        {
            for (int i = 0; i < _gameObjects.Count; i++)
                _gameObjects[i].Update(deltaTime);
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            // Currently utilises nested 'for' loops rather than 'foreach' loops, for performance purposes.
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                List<Component> components = _gameObjects[i].GetComponentList();
                for(int j = 0; j < components.Count; j++)
                {
                    if (components[j] is DrawableComponent component)
                        component.Render(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Allows a GameObject to be added to the automatic update and render collection.
        /// </summary>
        /// <param name="obj">The new GameObject for the Screen to handle.</param>
        protected void AddObject(GameObject obj)
        {
            _gameObjects.Add(obj);
        }

        /// <summary>
        /// Allows access to the entire collection of GameObjects currently handled by the Screen.
        /// </summary>
        /// <returns>A standard array of GameObjects.</returns>
        protected GameObject[] GetObjects()
        {
            return _gameObjects.ToArray();
        }

        public virtual void PreRender(SpriteBatch spriteBatch)
        {
        }

        public virtual void PostRender(SpriteBatch spriteBatch)
        {
            
        }
    }
}
