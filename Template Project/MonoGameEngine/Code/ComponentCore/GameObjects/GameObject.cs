using Microsoft.Xna.Framework;
using MonoGameEngine.ComponentCore.GameObjects.Components;
using System.Collections.Generic;

namespace MonoGameEngine.ComponentCore
{
    internal class GameObject
    {
        /// <summary>
        /// A collection of all the Components attached to this GameObject. 
        /// </summary>
        protected readonly List<Component> _components;

        /// <summary>
        /// A reference to the GameCore, allowing access to some high-level functionality.
        /// </summary>
        protected readonly Core _gameCore;

        /*CLASS VARIABLES*/
        protected Vector2 _position;
        protected Vector2 _velocity;

        public GameObject(Core game, Vector2 position)
        {
            _gameCore = game;
            _components = new List<Component>();

            _position = position;
        }

        public virtual void Update(float deltaTime)
        {
            foreach(Component component in _components)
            {
                component.Update(deltaTime);
            }
        }

        /// <summary>
        /// Method used to send a message to all of this GameObject's Components.
        /// </summary>
        /// <param name="messageType">The MessageType allows Components to ignore or listen for specific kinds of messages.</param>
        /// <param name="message"></param>
        public void SendMessage(MessageType messageType, string message)
        {
            if(_components != null)
            {
                foreach(Component component in _components)
                {
                    component.Receive(messageType, message);
                }
            }
        }

        /// <summary>
        /// Used to assign a new Component to this GameObject.
        /// </summary>
        /// <param name="component">A Component instance must be assigned.</param>
        public void AddComponent(Component component)
        {
            _components.Add(component);
            component.Register(this);
        }

        /// <summary>
        /// Retrieve an assigned Component of the requested type, if one exists.
        /// </summary>
        /// <typeparam name="T">The type of Component that you would like to look for.</typeparam>
        /// <returns>Returns the requested Component if found; otherwise, returns 'null' if no Component of the requested type belongs to this GameObject.</returns>
        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in _components)
            {
                if (component.GetType().Equals(typeof(T)))
                {
                    return (T)component;
                }
            }
            return null;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public Vector2 GetVelocity()
        {
            return _velocity;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        public List<Component> GetComponentList()
        {
            return _components;
        }

        public Core GetGameCore()
        {
            return _gameCore;
        }
    }
}
