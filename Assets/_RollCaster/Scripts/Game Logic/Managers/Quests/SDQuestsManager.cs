using SD_GameLoad;
using SD_Core;

namespace SD_Quest
{
    /// <summary>
    /// Manages the quests in the game, specifically the Defeat Boss Quest.
    /// </summary>
    public class SDQuestsManager : SDLogicMonoBehaviour
    {
        public const int BOSS_INTERVAL = 5;
        private const int ROLL_REWARD = 10;

        private void OnEnable() => AddListener(SDEventNames.BossQuest, DefeatBossQuest);

        private void OnDisable() => RemoveListener(SDEventNames.BossQuest, DefeatBossQuest);

        /// <summary>
        /// Handles the Defeat Boss Quest.
        /// Increases the player's roll count by <see cref="ROLL_REWARD"/>.
        /// </summary>
        /// <param name="obj">Not used, but required for the event system.</param>
        private void DefeatBossQuest(object obj = null)
        {
            int defeatedBossesCount = GameLogic.BossController.GetBossLevel();

            if (defeatedBossesCount % BOSS_INTERVAL == 1)
            {
                GameLogic.PlayerController.IncreaseRoll(ROLL_REWARD);
                InvokeEvent(SDEventNames.UpdateQuest, null);
                InvokeEvent(SDEventNames.QuestToast, null);
            }
        }
    }
}
