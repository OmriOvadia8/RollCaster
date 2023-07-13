using System.Collections.Generic;
using UnityEngine;
using SD_GameLoad;
using SD_Core;
using UnityEngine.UI;

namespace SD_Sound
{
    /// <summary>
    /// Manages the sound effects of the game, including button clicks, and allows muting and unmuting of all sounds.
    /// </summary>
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

        /// <summary>
        /// Plays the sound specified in the input argument, if it is of type SoundEffectType and exists in the audio dictionary.
        /// </summary>
        /// <param name="sound">The sound to play. This object must be castable to SoundEffectType.</param>
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

        /// <summary>
        /// Initializes the audio dictionary with the data from the serialized audio assets list.
        /// </summary>
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

        #region Mute
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
        #endregion
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