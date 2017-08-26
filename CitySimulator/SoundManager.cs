using SFML.Audio;
using System;
using System.Collections.Generic;

namespace CitySimulator
{
    internal class SoundManager
    {
        private Dictionary<string, Sound> _sounds = new Dictionary<string, Sound>();

        internal Sound GetSound(string fileName)
        {
            if (fileName == "")
                return null;

            if (_sounds.ContainsKey(fileName))
                return _sounds[fileName];

            var sound = Load(fileName);

            _sounds.Add(fileName, sound);

            return sound;
        }

        private Sound Load(string fileName)
        {
            var buffer = new SoundBuffer(Program.AssetsFolder + fileName);
            var sound = new Sound(buffer);
            return sound;
        }
    }
}