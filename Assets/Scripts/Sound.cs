using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Sound
    {
        public enum SoundType { Default = 0 }

        public SoundType soundType;
        public Vector3 pos;
        public float range;

        public Sound(Vector3 pos, float range, SoundType type = SoundType.Default)
        {
            this.soundType = type;
            this.pos = pos;
            this.range = range;
        }
    }
}