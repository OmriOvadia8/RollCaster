using System.Collections.Generic;
using UnityEngine;
using SD_GameLoad;
using SD_Core;

namespace SD_Sound
{
    public class SDSoundManager : SDLogicMonoBehaviour
    {
        [SerializeField] private List<AudioScriptable> audioAssets;
        private Dictionary<SoundEffectType, AudioScriptable> audioAssetDict;
        [SerializeField] AudioSource audioSource;

        private void Awake() => InitializeAudioDictionary();

        private void OnEnable() => AddListener(SDEventNames.PlaySound, PlaySound);

        private void OnDisable() => RemoveListener(SDEventNames.PlaySound, PlaySound);

        private void PlaySound(object sound)
        {
            if (sound is SoundEffectType effectType)
            {
                if (audioAssetDict.TryGetValue(effectType, out AudioScriptable asset))
                {
                    audioSource.PlayOneShot(asset.clip, asset.volume);
                }
                else
                {
                    SDDebug.LogException($"No audio asset found for sound effect type: {effectType}");
                }
            }
            else
            {
                SDDebug.LogException($"Sound object is not of type SoundEffectType");
            }
        }

        private void InitializeAudioDictionary()
        {
            audioAssetDict = new Dictionary<SoundEffectType, AudioScriptable>();

            foreach (var audioAsset in audioAssets)
            {
                if (!audioAssetDict.ContainsKey(audioAsset.effectType))
                {
                    audioAssetDict.Add(audioAsset.effectType, audioAsset);
                }
                else
                {
                    SDDebug.LogException($"Duplicate sound effect type detected: {audioAsset.effectType}");
                }
            }
        }
    }

    public enum SoundEffectType
    {
        ButtonClick,
        SlashesAudio,

    }
}