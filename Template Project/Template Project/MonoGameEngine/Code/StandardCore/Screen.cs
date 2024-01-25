using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoGameEngine.StandardCore
{
    /// <summary>The base Screen class, from which all of a game's screens should derive.</summary>
    public abstract class Screen : IScreen
    {
        /// <summary>A reference to the GameCore, allowing access to some high-level functionality.</summary>
        private Core _core;

        /// <summary>A collection of all the GameObjects existing in this Screen. All new GameObjects should be placed in this collection for automatic updating and rendering.</summary>
        private List<GameObject> _gameObjects;

        /// <summary>A collection of all the Text objects existing in this Screen. All new Text objects should be placed in this collection for automatic rendering.</summary>
        private List<Text> _text;

        /// <summary>A collection of all the GameObjects that have been flagged to be removed at the end of this frame. <em>CAUTION WHEN HANDLING</em>.</summary>
        private List<GameObject> _removedObjects;

        /// <summary>A collection of all the Text that have been flagged to be removed at the end of this frame. <em>CAUTION WHEN HANDLING</em>.</summary>
        private List<Text> _removedText;

        /// <summary>A collection of all the GameObjects that have been flagged to be added at the end of this frame. <em>CAUTION WHEN HANDLING</em>.</summary>
        private List<GameObject> _addedObjects;

        private Background[] _backgrounds;

        private SpriteFont _font;

        Effect _spriteFill;

        private Texture2D _pixel;

        private float _saturation = 1.0f;

        private Rectangle _worldBounds;

        private float _pauseDuration = 0;

        /// <summary>
        /// Performs some basic setup required by every Screen. Can be overriden to perform more bespoke initialisation if needed.
        /// </summary>
        /// <param name="core">The game core of the current project.</param>
        public virtual void Start(Core core)
        {
            _core = core;
            _pixel = Core.GetResource<Texture2D>("misc/Pixel");

            _spriteFill = Core.GetResource<Effect>("misc/SpriteFill");

            SetWorldBounds(0, 0, (int)Settings.GameResolution.X, (int)Settings.GameResolution.Y);

            _gameObjects = new List<GameObject>();
            _removedObjects = new List<GameObject>();
            _addedObjects = new List<GameObject>();
            _backgrounds = new Background[10];

            _text = new List<Text>();
            _removedText = new List<Text>();

            try
            {
                _font = _core.Content.Load<SpriteFont>("Arial");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("No font loaded by default.");
            }
        }

        /// <summary>
        /// Performs general cleanup for this class. Can be overriden to provide more bespoke cleanup functionality as needed.
        /// </summary>
        public virtual void End()
        {
            Camera.Instance.Position = Vector2.Zero;
            _backgrounds = null;
            _core = null;
            _spriteFill = null;
            _pixel = null;

            for (int i = 0; i < _gameObjects.Count; i++)
                _gameObjects[i] = null;
            _gameObjects.Clear();
            _gameObjects = null;

            for (int i = 0; i < _removedObjects.Count; i++)
                _removedObjects[i] = null;
            _removedObjects.Clear();
            _removedObjects = null;

            for (int i = 0; i < _addedObjects.Count; i++)
                _addedObjects[i] = null;
            _addedObjects.Clear();
            _addedObjects = null;

            for (int i = 0; i < _text.Count; i++)
                _text[i] = null;
            _text.Clear();
            _text = null;

            for (int i = 0; i < _removedText.Count; i++)
                _removedText[i] = null;
            _removedText.Clear();
            _removedText = null;

            _font = null;
        }

        /// <summary>
        /// Performs the root updating functionality for the game. Can be overriden to provide more bespoke updating as needed.
        /// </summary>
        /// <param name="deltaTime">The time (in seconds) since the last frame of the game.</param>
        public virtual void Update(float deltaTime)
        {
            if(_pauseDuration == 0)
            {
                if (!Transition.Instance.IsActive())
                {
                    foreach (GameObject obj in _gameObjects)
                    {
                        if (obj.IsActive() && Camera.Instance.WithinFrustum(obj))
                        {
                            obj.Update(deltaTime);
                            if (!obj.IsKinematic)
                                obj.PhysicsUpdate(deltaTime);

                            if (obj.GetAnimatedSprite() != null)
                                obj.GetAnimatedSprite().Animate(deltaTime);
                        }
                        else
                        {
                            obj.UpdatePause(deltaTime);
                        }
                    }

                    foreach (Background background in _backgrounds)
                        if (background != null && background.IsInitialised())
                            background.Update(deltaTime);

                }

                // Check to make sure a pause wasn't started during this frame of gameplay
                if (_pauseDuration == 0)
                {
                    ClearObjects();
                    AppendObjects();
                }
            }
            else
            {
                UpdatePause(deltaTime);
                if(_pauseDuration == 0)
                {
                    ClearObjects();
                    AppendObjects();
                }
            }
        }

        /// <summary>
        /// Performs the standard rendering of this screen. Can be overriden with more bespoke rendering functionality if needed.
        /// </summary>
        /// <param name="spriteBatch">The current batch of sprites to be rendered this frame of the game.</param>
        public virtual void Render(SpriteBatch spriteBatch) 
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, // Begin World Space Rendering
                samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.BackToFront,
                transformMatrix: Camera.Instance.GetViewMatrix(Vector2.One),
                effect: _spriteFill);


            Color lastFillColour = Color.Transparent;
            float lastEffectStrength = 0;
            if (_gameObjects.Count > 0)
                lastFillColour = _gameObjects[0].GetSprite().GetFillColour();


            foreach(GameObject obj in _gameObjects)
            {
                if(obj.GetSprite().GetFillColour() != lastFillColour 
                    || obj.GetSprite().GetFillEffectStrength() != lastEffectStrength)
                {
                    lastFillColour = obj.GetSprite().GetFillColour();
                    lastEffectStrength = obj.GetSprite().GetFillEffectStrength();

                    spriteBatch.End(); // End previous rendering batch with last effect applied
                    spriteBatch.Begin(blendState: BlendState.AlphaBlend, // Begin new rendering batch
                        samplerState: SamplerState.PointClamp,
                        sortMode: SpriteSortMode.BackToFront,
                        transformMatrix: Camera.Instance.GetViewMatrix(Vector2.One),
                        effect: _spriteFill ?? null);
                }
                if (obj.GetSprite().IsInWorldSpace() && Camera.Instance.WithinFrustum(obj))
                {
                    if (_spriteFill != null)
                    {
                        _spriteFill.Parameters["fillColour"].SetValue(obj.GetSprite().GetFillColour().ToVector4());
                        _spriteFill.Parameters["effectStrength"].SetValue(obj.GetSprite().GetFillEffectStrength());
                    }
                    obj.Render(spriteBatch);
                }
            }             
                    

            for (int i = 0; i < _text.Count; i++)
                if (_text[i].IsInWorldSpace())
                    _text[i].Draw(spriteBatch);


            spriteBatch.End(); // End World Space Rendering


            spriteBatch.Begin(blendState: BlendState.AlphaBlend, // Begin Screen Space Rendering
                samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.BackToFront,
                effect: _spriteFill); 

            for (int i = 0; i < _gameObjects.Count; i++)
                if (!_gameObjects[i].GetSprite().IsInWorldSpace())
                    _gameObjects[i].Render(spriteBatch);

            for (int i = 0; i < _text.Count; i++)
                if (!_text[i].IsInWorldSpace())
                    _text[i].Draw(spriteBatch);

            spriteBatch.End(); // End Screen Space Rendering
        }

        /// <summary>
        /// Called internally for pre-render setup.
        /// </summary>
        public void PreRender(SpriteBatch spriteBatch)
        {
            Core.GetShapeBatcher().Begin();
            //Camera2D.Instance.BeginDraw();
            Camera.Instance.BeginDraw();
            _core.GraphicsDevice.Clear(Settings.BackgroundFill); // Fill in the rendered screen's background colour

            spriteBatch.Begin(blendState: BlendState.AlphaBlend, // Begin Background rendering
                samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.BackToFront,
                transformMatrix: Camera.Instance.GetViewMatrix(Vector2.One),
                effect: _spriteFill);

            foreach (Background background in _backgrounds)
                if (background != null && background.IsInitialised())
                    background.Render(spriteBatch);

            spriteBatch.End(); // End Background Rendering
        }
       
        /// <summary>
        /// Called internally for post-render cleanup.
        /// </summary>
        public void PostRender(SpriteBatch spriteBatch)
        {
            Core.GetShapeBatcher().End();
            //Camera2D.Instance.EndDraw();
            Camera.Instance.EndDraw();

            _core.GraphicsDevice.Clear(Settings.LetterboxFill); // Fill in the letterboxing colour
        }

        /// <summary>
        /// Calls the Screen's Start method again, for the purposes of resetting the current Screen to its starting state.
        /// </summary>
        public void Restart()
		{
            Camera.Instance.Position = Vector2.Zero;
            Start(_core);
		}

        private void SortDrawOrder()
        {
            _gameObjects.Sort((x, y) => x.GetSprite().GetLayerDepth().CompareTo(y.GetSprite().GetLayerDepth()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveBy"></param>
        private void ParallaxBackgrounds(Vector2 moveBy)
        {
            foreach (Background background in _backgrounds)
                if(background != null && background.IsInitialised())
                    background.Move(moveBy * -1);
        }


        /// <summary>
        /// A setter method for changing the amount of parallaxing applied to each Background layer of this Screen. '1.0f' is the default. Values are clamped between '0.1f' and '2.0f'.
        /// </summary>
        /// <param name="parallaxStrength">The parallax multiplier to be applied to each Background layer when moving. A higher number results in a higher speed, and a lower number results in a lower speed.</param>
        public void SetParallaxStrength(float parallaxStrength)
        {
            parallaxStrength = Math.Clamp(parallaxStrength, 0.1f, 2.0f);
            foreach(Background background in _backgrounds)
                if(background != null && background.IsInitialised())
                    background.SetParallaxStrength(parallaxStrength);
        }

        /// <summary>
        /// Allows a GameObject to be added to the automatic update and render collection.
        /// </summary>
        /// <param name="obj">The new GameObject for the Screen to handle.</param>
        /// <param name="x">An integer value representing the horizontal position on the Screen to place the new GameObject.</param>
        /// <param name="y">An integer value representing the vertical position on the Screen to place the new GameObject.</param>
        public void AddObject(GameObject obj, int x, int y)
        {
            obj.SetScreen(this);
            obj.SetPosition(x, y);
            _addedObjects.Add(obj);

            obj.OnceAdded();
        }

        /// <summary>
        /// [Overload] Allows a GameObject to be added to the automatic update and render collection.
        /// </summary>
        /// <param name="obj">The new GameObject for the Screen to handle.</param>
        /// <param name="x">An integer value representing the horizontal position on the Screen to place the new GameObject.</param>
        /// <param name="y">An integer value representing the vertical position on the Screen to place the new GameObject.</param>
        /// <param name="spriteName">A string value representing the name of the sprite this new GameObject should use for rendering.</param>
        public void AddObject(GameObject obj, int x, int y, string spriteName)
        {
            if (!spriteName.Equals(""))
                obj.SetSprite(spriteName);

            AddObject(obj, x, y);
        }

        /// <summary>
        /// Removes the given GameObject from the Screen at the end of the current frame of the game. 
        /// </summary>
        /// <param name="obj">The GameObject that should be removed from the Screen.</param>
        public void RemoveObject(GameObject obj)
        {
            _removedObjects.Add(obj);
        }

        /// <summary>
        /// Clears all 'removed' GameObjects and Text from the Screen. This is used in conjunction with RemoveObject() and RemoveText().
        /// </summary>
        private void ClearObjects()
        {
            if (_removedObjects.Count > 0)
            {
                for (int i = 0; i < _removedObjects.Count; i++)
                {
                    _gameObjects.Remove(_removedObjects[i]);
                }
                _removedObjects.Clear();
            }
            
            if(_removedText.Count > 0)
            {
                for (int i = 0; i < _removedText.Count; i++)
                {
                    _text.Remove(_removedText[i]);
                }
                _removedText.Clear();
            }
        }

        /// <summary>
        /// Appends the GameObjects created this frame of the game. This is used in conjunction with AddObject().
        /// </summary>
        private void AppendObjects()
        {
            if(_addedObjects.Count > 0)
            {
                _gameObjects.AddRange(_addedObjects);

                foreach (GameObject obj in _addedObjects)
                {
                    obj.OnceAdded();
                }

                _addedObjects.Clear();
                SortDrawOrder();
            }
        }

        /// <summary>
        /// Allows access to the entire collection of GameObjects currently handled by the Screen.
        /// </summary>
        /// <returns>Returns a standard array of all GameObjects that this Screen currently handles.</returns>
        public GameObject[] GetAllObjects()
        {
            return _gameObjects.ToArray();
        }

        /// <summary>
        /// Provides an array of objects of a specified type that currently exist in this Screen.
        /// </summary>
        /// <typeparam name="TClass">The subclass type of GameObjects that should be found.</typeparam>
        /// <returns>Returns an array of GameObjects of a specified type that this Screen can find.</returns>
        public GameObject[] GetAllObjectsOfType<TClass>() where TClass : class
        {
            try
            {
                var arrayOfType = _gameObjects.ToArray().OfType<TClass>().ToArray();
                return arrayOfType as GameObject[];
            }catch(InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Provides the first object of the specified type that exists in this Screen.
        /// </summary>
        /// <typeparam name="TObject">The subclass type of GameObject that should be found.</typeparam>
        /// <returns>Returns the first instance of the specified type that this Screen can find as a GameObject.</returns>
        public GameObject GetOneObjectOfType<TObject>() where TObject : class
        {
            try
            {
                return _gameObjects.OfType<TObject>().First() as GameObject;
            }
            catch(InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Allows a Text object to be added to the automatic render collection.
        /// </summary>
        /// <param name="text">The new Text object for the Screen to handle.</param>
        /// <param name="x">An integer value representing the horizontal position on the Screen to place the new Text object.</param>
        /// <param name="y">An integer value representing the vertical position on the Screen to place the new Text object.</param>
        public void AddText(Text text, int x, int y)
        {
            text.SetPosition(new Vector2(x, y));
            _text.Add(text);
        }

        /// <summary>
        /// Removes the given Text from the Screen at the end of the current frame of the game. 
        /// </summary>
        /// <param name="text">The Text object that should be removed from the Screen.</param>
        public void RemoveText(Text text)
        {
            _removedText.Add(text);
        }

        /// <summary>
        /// Allows access to the entire collection of Text objects currently handled by the Screen.
        /// </summary>
        /// <returns>A standard array of Text objects.</returns>
        protected Text[] GetText()
        {
            return _text.ToArray();
        }

        /// <summary>
        /// Provides access to changing the colour saturation of this Screen. Saturation is clamped between 0 and 1. Value set to 1.0f by default.
        /// </summary>
        /// <param name="saturation">The new saturation value (clamped between 0 and 1).</param>
        public void SetColourSaturation(float saturation)
		{
            _saturation = Math.Clamp(saturation, 0.0f, 1.0f);
		}

        /// <summary>
        /// Provides access to the colour saturation for this Screen.
        /// </summary>
        /// <returns>A floating point value from 0.0 - 1.0 representing the saturation of the colours in this Screen. </returns>
        public float GetColourSaturation()
		{
            return _saturation;
		}

        /// <summary>
        /// Allows a background image to be set for this Screen. The texture will be stretched to fit window, by default.
        /// </summary>
        /// <param name="name">A simple string for the name of the background. No filetype (.png, .jpg, etc.) required.</param>
        /// <param name="type">[Optional] An enumerated value representing how the Background should be rendered. Defaults to 'BackgroundType.Stretch'.</param>
        /// <param name="index">[Optional] The index of the Background (also used as the render layer). Defaults to '9', the furthest background from the camera..</param>
        protected void SetBackground(string name, BackgroundType type = BackgroundType.Stretch, int index = 9)
        {
            index = Math.Clamp(index, 0, _backgrounds.Length);

            if (_backgrounds[index] == null)
                _backgrounds[index] = new Background(_core);

            _backgrounds[index].Initialise(name, (index + 1) * 0.1f, type);
        }

        /// <summary>
        /// Getter method for accessing the Background instance belonging to this Screen.
        /// </summary>
        /// <param name="index">[Optional] The index of the Background in this Screen. Defaults to '9', the furthest background from the camera.</param>
        /// <returns>A Background object representing the background visual elements being rendered.</returns>
        public Background GetBackground(int index = 9)
        {
            return _backgrounds[index];
        }

        /// <summary>
        /// Getter method for accessing the primary font set for the game. This font can be adjusted in the Font.spritefont file in the Content folder of the project.
        /// </summary>
        /// <returns>A SpriteFont object for use when drawing text.</returns>
        public SpriteFont GetFont()
        {
            return _font;
        }

        /// <summary>
        /// Getter method for accessing the rectangle representing this Screen's world boundaries.
        /// </summary>
        /// <returns>Returns a Rectangle object representing the world edges.</returns>
        public Rectangle GetWorldBounds()
        {
            return _worldBounds;
        }

        /// <summary>
        /// Setter method for adjusting the world boundaries for this Screen. Used in conjunction with Camera2D scrolling.
        /// </summary>
        /// <param name="x">The on-screen horizontal position that the Rectangle should start from.</param>
        /// <param name="y">The on-screen vertical position that the Rectangle should start from.</param>
        /// <param name="width">The width of the world bounds from the x position.</param>
        /// <param name="height">The height of the world bounds from the y position.</param>
        public void SetWorldBounds(int x, int y, int width, int height)
        {
            SetWorldBounds(new Rectangle(x, y, width, height));
        }

        /// <summary>
        /// [Overload] Setter method for adjusting the world boundaries for this Screen. Used in conjunction with Camera2D scrolling.
        /// </summary>
        /// <param name="width">The width of the world bounds from x co-ordinate '0'.</param>
        /// <param name="height">The height of the world bounds from y co-ordinate '0'.</param>
        public void SetWorldBounds(int width, int height)
        {
            SetWorldBounds(new Rectangle(0, 0, width, height));
        }

        /// <summary>
        /// [Overload] Setter method for adjusting the world boundaries for this Screen. Used in conjunction with Camera2D scrolling.
        /// </summary>
        /// <param name="dimensions">A vector representing the width and height of this Screen's world, starting at origin (0,0).</param>
        public void SetWorldBounds(Vector2 dimensions)
        {
            SetWorldBounds(new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y));
        }

        /// <summary>
        /// [Overload] Setter method for adjusting the world boundaries for this Screen. Used in conjunction with Camera2D scrolling.
        /// </summary>
        /// <param name="rect">A rectangle representing the desired bounds of this Screen's world.</param>
        public void SetWorldBounds(Rectangle rect)
        {
            _worldBounds = rect;
            Camera.Instance.Limits = rect;
        }

        /// <summary>
        /// Pauses updates to the entire Screen for the specified duration, in seconds. AudioManager, Transition and Camera2D remain unaffected. <br/>
        /// <strong>NOTE:</strong> This will also pause the adding and removing of GameObjects within the Screen.
        /// </summary>
        /// <param name="pauseDuration">The amount of time, in seconds, that the Screen should remain paused.</param>
        public void Pause(float pauseDuration)
        {
            _pauseDuration = Math.Max(0, pauseDuration);
        }

        private void UpdatePause(float deltaTime)
        {
            _pauseDuration -= deltaTime;
            if(_pauseDuration <= 0)
                _pauseDuration = 0;
        }
    }
}
