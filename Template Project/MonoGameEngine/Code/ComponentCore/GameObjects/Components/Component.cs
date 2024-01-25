namespace MonoGameEngine.ComponentCore.GameObjects.Components
{
    internal abstract class Component
    {
        protected GameObject _gameObject;

        public virtual void Receive(MessageType messageType, string message) { }
        public abstract void Update(float deltaTime);

        public void Register(GameObject gameObject) => _gameObject = gameObject;
    }
}
