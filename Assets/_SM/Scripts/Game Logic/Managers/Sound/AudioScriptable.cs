using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD_Sound
{
    [CreateAssetMenu(fileName = "AudioAsset", menuName = "Audio/New Audio Asset", order = 1)]
    public class AudioScriptable : ScriptableObject
    {
        public SoundEffectType effectType;
        public AudioClip clip;
        public float volume = 0.5f;
    }
}