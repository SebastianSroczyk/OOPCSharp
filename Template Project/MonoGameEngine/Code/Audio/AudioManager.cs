// The class is an example of a C# singleton, which is also thread-safe
// https://csharpindepth.com/articles/singleton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using MonoGameEngine.Maths;
using MonoGameEngine.StandardCore;
using System;
using System.Collections.Generic;

using System.Linq;

namespace MonoGameEngine
{
    /// <summary>AudioManager exposes high-level functionality for controlling audio using Monogame's underlying XNA architecture.</summary>
    public sealed class AudioManager
    {
        /// <summary>Provides access to this AudioManager object. The main way to use the built-in audio functionality.</summary>
        public static AudioManager Instance { get; } = new AudioManager();

        /// <summary>An integer property which represents the rate at which the volume of sound effects begins to drop when distanced from the center of the Camera2D.</summary>
        public float FallOff { get { return _fallOff; } set { _fallOff = Math.Clamp(value, 0, 2.0f); } }

        private const float MASTER_VOLUME = 0.2f;

        private static float _fallOff = 1.25f;

        private SFX _bgm;
        private SFX _bgmIntro;

        private readonly List<SFX> _soundEffects = new List<SFX>();

        private InterpolationData _volumeLerp;

        static AudioManager()
        {
        }

        private AudioManager()
        {
        }

        /// <summary>
        /// A method to keep the AudioManager up-to-date. <b>Called automatically by Core</b>.
        /// </summary>
        /// <param name="deltaTime">The current delta time of the frame.</param>
        internal void Update(float deltaTime)
        {
            Refresh();

            if (_volumeLerp != null)
            {
                if (_bgm != null)
                {
                    if (GetCurrentBGMVolume() != MASTER_VOLUME)
                    {
                        LerpBGMVolume(deltaTime);
                    }
                    else if (GetCurrentBGMVolume() != MASTER_VOLUME * 0.5f)
                    {
                        LerpBGMVolume(deltaTime);
                    }
                }
            }
        }

        /// <summary>
        /// Interpolates the volume of the current BGM. Creates a 'fade in' effect on the audio.
        /// </summary>
        /// <param name="deltaTime">The current delta time of the frame.</param>
        private void LerpBGMVolume(float deltaTime)
        {
            _volumeLerp.UpdateValues(deltaTime);

            _bgm.SetVolume(_volumeLerp.CurrentValues[0]);
            if (_bgmIntro != null)
                _bgmIntro.SetVolume(_volumeLerp.CurrentValues[0]);

            if (_volumeLerp.LerpTime >= 1.0f)
                _volumeLerp = null;
        }

        /// <summary>
        /// Method used to check for finished SFX objects, and disposes of them. 
        /// Also starts a BGM loop after a BGM intro finishes. 
        /// Invoked from Update().
        /// </summary>
        private void Refresh()
        {
            foreach (SFX sound in _soundEffects.ToList())
            {
                if (sound.IsStopped())
                {
                    if (sound.WillBlockBGM())
                    {
                        if (_bgmIntro != null)
                            _bgmIntro.Resume();
                        if (_bgm != null)
                            _bgm.Resume();
                    }

                    _soundEffects.Remove(sound);
                }
                else if(Settings.UsePositionalAudio)
                {
                    Apply2DSound(sound);
                }
            }

            if (_bgmIntro != null) // Check to see if a BGM intro was playing
            {
                if (_bgmIntro.IsStopped()) // If the intro is finished...
                {
                    _bgm.Play();
                    _bgmIntro.Dispose();
                    _bgmIntro = null;
                }
            }
        }

        /// <summary>
        /// Play a pre-existing SFX instance.
        /// </summary>
        /// <param name="soundEffect">An instance of a pre-existing SFX object.</param>
        /// <param name="emitter">The GameObject that has emitted the sound effect.</param>
        public void PlaySFX(SFX soundEffect, GameObject emitter)
        {
            SFX newSFX = new SFX(soundEffect);
            newSFX.Emitter = emitter.GetEmitter();
            if (Settings.UsePositionalAudio)
                Apply2DSound(newSFX);
            else
                newSFX.SetVolume(MASTER_VOLUME);
            newSFX.Play();
        }

        /// <summary>
        /// Play a new SFX instance.
        /// </summary>
        /// <param name="filename">The name of the desired sound effect</param>
        /// <param name="emitter">The GameObject that has emitted the sound effect.</param>
        /// <param name="overlapRule">[Optional] Determines what happens if another SFX instance of the same name is already active. Defaults to SFXOverlapRule.None.</param>
        /// <param name="blockBGM">[Optional] Determines if the sound effect should pause the current BGM while it plays. 'false' by default.</param>
        public void PlaySFX(string filename, GameObject emitter, SFXOverlapRule overlapRule = SFXOverlapRule.Overlay, bool blockBGM = false)
        {
            string filepath = "sfx/" + filename;

            switch (overlapRule)
            {
                case SFXOverlapRule.Overwrite:
                    if (_soundEffects.Any(sound => sound.GetName().Equals(filepath)))
                    {
                        foreach (SFX sound in _soundEffects)
                        {
                            if (sound.GetName().Equals(filepath))
                            {
                                sound.Stop(); // Refresh() will remove from list in next pass
                            }
                        }
                    }
                    goto case SFXOverlapRule.Overlay; // emulate fallthrough
                case SFXOverlapRule.Ignore:
                    if (_soundEffects.Any(sound => sound.GetName().Equals(filepath)))
                        break;
                    else
                        goto case SFXOverlapRule.Overlay; // emulate fallthrough
                case SFXOverlapRule.Overlay:
                    var newSFX = new SFX(Core.GetContent().Load<SoundEffect>(filepath), false, blockBGM);
                    newSFX.Emitter = emitter.GetEmitter();
                    
                    if(Settings.UsePositionalAudio)
                        Apply2DSound(newSFX);
                    else
                        newSFX.SetVolume(MASTER_VOLUME);

                    newSFX.Play();
                    _soundEffects.Add(newSFX);

                    if (blockBGM)
                    {
                        if (_bgmIntro != null)
                            _bgmIntro.Pause();
                        _bgm.Pause();
                    }
                    break;
            }
        }

        // Based on the code provided by Andrew Russell: https://gamedev.stackexchange.com/questions/9774/sound-emmiters-with-panning-and-volume-in-2d
        // NOTE: Panning has been disabled as MonoGame's current integration of OpenAL does not work as intended.
        private bool Apply2DSound(SFX sfx)
        {
            var position = new Vector2(sfx.Emitter.Position.X, sfx.Emitter.Position.Y);
            var cameraTranslation = Camera.Instance.GetViewMatrix(Vector2.One).Translation;
            var screenDistance = (position - Camera.Instance.Origin + new Vector2(cameraTranslation.X, cameraTranslation.Y)) / (Camera.Instance.GetRenderRectangle().Size.ToVector2() * 0.5f);

            if (screenDistance.X < 0.15 && screenDistance.X > -0.15) // Generates a small range which prevents panning from taking place.
                screenDistance.X = 0;

            float fade = Math.Clamp(FallOff - screenDistance.Length(), 0, 1);
            sfx.SetVolume(fade * fade * MASTER_VOLUME);
            //sfx.SetPan(Math.Clamp(screenDistance.X, -1, 1));
            return fade > 0;
        }

        /// <summary>
        /// Plays a piece of music of the given filename, if it isn't already playing. Has some customisable options which can be ignored if not needed.
        /// </summary>
        /// <param name="filename">The name of the piece of music that should be played.</param>
        /// <param name="loop">[Optional] Should this piece of music loop infinitely? 'true' by default.</param>
        /// <param name="fadeIn">[Optional] Should this piece of music start with a 'fade in' effect? 'true' by default.</param>
        public void PlayBGM(string filename, bool loop = true, bool fadeIn = true)
        {
            string filepath = "bgm/" + filename;

            if (_bgm == null || !_bgm.GetName().Equals(filepath))
            {
                if (fadeIn)
                    _volumeLerp = new InterpolationData(new float[] { 0.0f }, new float[] { MASTER_VOLUME }, 1.0f);

                float bgmVolume = fadeIn == true ? 0.0f : MASTER_VOLUME;
                if (_bgm != null)
                    _bgm.Stop();
                if (_bgmIntro != null)
                    _bgmIntro.Stop();

                _bgm = new SFX(Core.GetContent().Load<SoundEffect>(filepath), loop, false);

                _bgm.SetVolume(bgmVolume);
                _bgm.Play();
            }
        }

        /// <summary>
        /// Plays a piece of music (with an intro) of the given filenames, if it isn't already playing. 
        /// Has some customisable options which can be ignored if not needed.
        /// </summary>
        /// <param name="introName">The name of the intro/one-off track that should be played.</param>
        /// <param name="bgmName">The name of the main piece of music that should be played.</param>
        /// <param name="loop">[Optional] Should this piece of music loop infinitely? 'true' by default.</param>
        /// <param name="fadeIn">[Optional] Should this piece of music start with a 'fade in' effect? 'true' by default.</param>
        public void PlayBGMWithIntro(string introName, string bgmName, bool loop = true, bool fadeIn = true)
        {
            string filepath = "bgm/";

            if (_bgm == null || !_bgm.GetName().Equals(filepath + bgmName))
            {
                if (_bgm != null)
                {
                    _bgm.Stop();
                    _bgm.Dispose();
                }
                if (_bgmIntro != null)
                {
                    _bgmIntro.Stop();
                    _bgmIntro.Dispose();
                }

                _bgm = new SFX(Core.GetContent().Load<SoundEffect>(filepath + bgmName), loop);
                _bgmIntro = new SFX(Core.GetContent().Load<SoundEffect>(filepath + introName)); // NOTE: Intro should never loop

                if (fadeIn)
                    _volumeLerp = new InterpolationData(new float[] { 0.0f }, new float[] { MASTER_VOLUME }, 1.0f);

                float bgmVolume = fadeIn == true ? 0.0f : MASTER_VOLUME;
                _bgm.SetVolume(bgmVolume);
                _bgmIntro.SetVolume(bgmVolume);

                _bgmIntro.Play();
            }
        }

        /// <summary>
        /// Stop the current BGM or BGM intro, if either exist.
        /// </summary>
        public void StopBGM()
        {
            if (_bgm != null)
            {
                _bgm.Stop();
                _bgm.Dispose();
                _bgm = null;
            }

            if (_bgmIntro != null)
            {
                _bgmIntro.Stop();
                _bgmIntro.Dispose();
                _bgmIntro = null;
            }
        }

        /// <summary>
        /// Check to see if a BGM loop or intro is playing currently.
        /// </summary>
        /// <returns>'true' if BGM loop or intro is playing.</returns>
        public bool IsPlayingBGM()
        {
            if (_bgm != null)
                return _bgm.GetState() == SoundState.Playing || _bgmIntro.GetState() == SoundState.Playing;
            else
                return false;
        }

        /// <summary>
        /// Check to see if the most recent BGM has stopped playing.
        /// </summary>
        /// <returns>'true' if the current BGM has stopped and no intro is playing.</returns>
        public bool IsBGMFinished()
        {
            if (_bgm != null)
                return _bgm.GetState() == SoundState.Stopped && _bgmIntro == null;
            else
                return true;
        }

        /// <summary>
        /// Get the current BGM's name.
        /// </summary>
        /// <returns>A string value containing the current BGM's name.</returns>
        public string GetCurrentBGMName()
        {
            return _bgmIntro == null ? _bgm.GetName() : _bgm.GetName();
        }

        /// <summary>
        /// Check to see if an SFX instance is currently playing with a given name.
        /// </summary>
        /// <param name="name">The name of the desired sound effect.</param>
        /// <returns>Returns 'true' if any SFX instance with the given name is currently playing. Otherwise, returns 'false'.</returns>
        public bool IsSFXPlaying(string name)
        {
            string filepath = "sound_effects/" + name;
            return _soundEffects.Any(x => x.GetName().Equals(filepath));
        }

        /// <summary>
        /// Pause all currently playing BGM and SFX.
        /// </summary>
        public void PauseAll()
        {
            if (_bgm != null)
                _bgm.Pause();
            if (_bgmIntro != null)
                _bgmIntro.Pause();

            foreach (SFX effect in _soundEffects)
            {
                effect.Pause();
            }
        }

        /// <summary>
        /// Resume all currently paused BGM and SFX.
        /// </summary>
        public void ResumeAll()
        {
            if (_bgm != null)
                _bgm.Resume();
            if (_bgmIntro != null)
                _bgmIntro.Resume();

            foreach (SFX effect in _soundEffects)
            {
                effect.Resume();
            }
        }

        /// <summary>
        /// Allows the volume of any currently playing BGM to be set. 0.0f (silent) -> 1.0f (full volume) is the expected range.
        /// </summary>
        /// <param name="volume">The desired volume of the BGM.</param>
        public void SetBGMVolume(float volume)
        {
            volume = MathHelper.Clamp(volume, 0.0f, 1.0f);

            if (_volumeLerp != null)
                _volumeLerp.LerpTime = 1.0f; // Skip the lerp

            if (_bgm != null)
                _bgm.SetVolume(MASTER_VOLUME * volume);
            if (_bgmIntro != null)
                _bgmIntro.SetVolume(MASTER_VOLUME * volume);
        }

        /// <summary>
        /// Get the current volume level of the current BGM loop or intro.
        /// </summary>
        /// <returns>Returns -1 if no BGM is playing. Otherwise returns a floating-point value between 0.0f and 1.0f representing the volume of the audio file.</returns>
        public float GetCurrentBGMVolume()
        {
            if (_bgmIntro != null)
                return _bgmIntro.GetVolume();
            else if (_bgm != null)
                return _bgm.GetVolume();
            else
                return -1;
        }

        /// <summary>
        /// Allows the pitch of any currently playing BGM. 0.0f is the default. -1.0f -> 1.0f is the acceptable range of pitch.
        /// </summary>
        /// <param name="pitch">The value  desired pitch of the audio file. </param>
        public void SetBGMPitch(float pitch)
        {
            pitch = Math.Clamp(pitch, -1.0f, 1.0f);
            if (_bgm != null)
                _bgm.SetPitch(pitch);
            if (_bgmIntro != null)
                _bgmIntro.SetPitch(pitch);
        }

        /// <summary>
        /// Allows an audio file to be loaded into the game before being played. This method should be called if your audio files are being delayed when first played.
        /// </summary>
        /// <param name="filename">The name of the desired sound effect.</param>
        public void PreLoadSFX(string filename)
        {
            Core.GetResource<SoundEffect>("sfx/" + filename);
        }
    }
}
