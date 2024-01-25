using Microsoft.Xna.Framework.Audio;

namespace MonoGameEngine
{
    /// <summary>
    /// Controls how sound effects are handled when a request to play an existing sound is made.
    /// <br/>- <strong>Ignore</strong> discards any extra sfx of the same name. 
    /// <br/>- <strong>Overwrite</strong> replaces the current instance of the sfx.
    /// <br/>- <strong>Overlay</strong> allows multiples of a single sfx to play at once.
    /// </summary>
    public enum SFXOverlapRule { Ignore, Overwrite, Overlay };

    /// <summary>A class representing an audio asset within the game engine. Can be used for BGM as well as sound effects.</summary>
    public sealed class SFX
    {
        private SoundEffect _effect;
        private SoundEffectInstance _effectInstance;

        internal AudioEmitter Emitter { get; set; }

        private readonly bool _blockBGM = false;

        public SFX(SFX effect, bool blockBGM = false)
        {
            _effect = effect._effect;
            _effectInstance = _effect.CreateInstance();
            _blockBGM = blockBGM;
        }

        public SFX(SoundEffect effect, bool looped = false, bool blockBGM = false)
        {
            _effect = effect;
            _effectInstance = _effect.CreateInstance();
            _effectInstance.IsLooped = looped;
            _blockBGM = blockBGM;
        }

        internal void Apply3D(AudioEmitter emitter, AudioListener listener)
        {
            _effectInstance.Apply3D(listener, emitter);
        }

        /// <summary>
        /// Method used to clean the memory allocated for this sound effect.
        /// </summary>
        internal void Dispose()
        {
            _effect = null;
            _effectInstance = null;
        }

        /// <summary>
        /// Getter method which checks to see if this SFX is currently playing.
        /// </summary>
        /// <returns>Returns 'true' if the SFX is active. Otherwise, returns 'false'.</returns>
        public bool IsPlaying()
        {
            return _effectInstance.State == SoundState.Playing;
        }

        /// <summary>
        /// Getter method which checks to see if this SFX is currently stopped.
        /// </summary>
        /// <returns>Returns 'true' if the SFX is not currently playing. Otherwise, returns 'false'.</returns>
        public bool IsStopped()
        {
            return _effectInstance.State == SoundState.Stopped;
        }

        /// <summary>
        /// Getter method which returns the name associated with this SFX.
        /// </summary>
        /// <returns>A string object representing the name of this SFX's audio file.</returns>
        public string GetName()
        {
            return _effect.Name;
        }

        /// <summary>
        /// Starts playing the audio file held by this SFX.
        /// </summary>
        public void Play()
        {
            _effectInstance.Play();
        }

        /// <summary>
        /// Stops the audio file held by this SFX.
        /// </summary>
        public void Stop()
        {
            _effectInstance.Stop();
        }

        /// <summary>
        /// Pauses the audio file held by this SFX. If resumed, file will continue from where it paused.
        /// </summary>
        public void Pause()
        {
            if (_effectInstance.State == SoundState.Playing)
                _effectInstance.Pause();
        }

        /// <summary>
        /// Resumes playback of the audio file if it was previously paused. Will resume from the point it was paused.
        /// </summary>
        public void Resume()
        {
            if (_effectInstance.State == SoundState.Paused)
                _effectInstance.Resume();
        }

        /// <summary>
        /// Getter method which checks if this SFX will has looping playback enabled.
        /// </summary>
        /// <returns>A boolean value representing whether or not the audio will repeat when finished.</returns>
        public bool IsLooping()
        {
            return _effectInstance.IsLooped;
        }

        /// <summary>
        /// Getter method which checks if this SFX will pause any background music when played.
        /// </summary>
        /// <returns>A boolean value representing whether or not the audio pauses existing background music while playing.</returns>
        public bool WillBlockBGM()
        {
            return _blockBGM;
        }

        /// <summary>
        /// Getter method which returns the SoundState of this SFX. Useful for digging into the state of this object a little deeper.
        /// </summary>
        /// <returns>A SoundState enum which contains the underlying state of this SFX's audio.</returns>
        public SoundState GetState()
        {
            return _effectInstance.State;
        }

        /// <summary>
        /// Setter method which can adjust the playback volume of this SFX's audio.
        /// </summary>
        /// <param name="volume">The volume at which to play this SFX's audio. Should be between 0.0f and 1.0f.</param>
        public void SetVolume(float volume)
        {
            _effectInstance.Volume = volume;
        }

        /// <summary>
        /// Getter method which returns the current volume that this SFX is playing at.
        /// </summary>
        /// <returns>A floating-point value (between 0.0f and 1.0f) that represents the volume of this SFX's audio playback.</returns>
        public float GetVolume()
        {
            return _effectInstance.Volume;
        }

        internal void SetPitch(float pitch)
        {
            _effectInstance.Pitch = pitch;
        }

        internal void SetPan(float pan)
        {
            _effectInstance.Pan = pan;
        }
    }
}
