using System.Collections.Generic;
using UnityEngine;
using SD_GameLoad;
using SD_Core;
using UnityEngine.UI;

namespace SD_Sound
{
    public class SDSoundManager : SDLogicMonoBehaviour
    {
        [SerializeField] private List<AudioScriptable> audioAssets;
        private Dictionary<SoundEffectType, AudioScriptable> audioAssetDict;
        [SerializeField] AudioSource audioSource;
        [SerializeField] Sprite muteSprite;
        [SerializeField] Sprite unmuteSprite;
        [SerializeField] Image buttonImage;

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

        public void ButtonClickSound() => PlaySound(SoundEffectType.ButtonClick);

        public void ToggleMute()
        {
            AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            buttonImage.sprite = unmuteSprite;

            if (AudioListener.volume == 0)
            {
                buttonImage.sprite = muteSprite;
            }
        }
    }

    public enum SoundEffectType
    {
        ButtonClick,
        SkullSmokeAudio,
        SlashesAudio,
        SmokeExplosionAudio,
        SkullExplosionAudio,
        ScratchesAudio,
        TornadoAudio,
        TentacleAudio,
        BossDeathAudio,
        BossRespawnAudio,
        LevelupAudio,
    }
}