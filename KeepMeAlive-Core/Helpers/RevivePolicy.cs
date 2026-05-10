//====================[ Imports ]====================
using UnityEngine;
using KeepMeAlive.Components;

namespace KeepMeAlive.Helpers
{
    //====================[ RevivePolicy ]====================
    internal static class RevivePolicy
    {
        //====================[ Policy Queries ]====================
        public static bool IsEnabled(ReviveSource source)
        {
            return source switch
            {
                ReviveSource.Self => SyncedGameplayValues.SELF_REVIVAL_ENABLED,
                ReviveSource.Team => SyncedGameplayValues.TEAM_REVIVE_ENABLED,
                _ => true
            };
        }

        public static float GetHoldDuration(ReviveSource source)
        {
            float configured = source switch
            {
                ReviveSource.Self => SyncedGameplayValues.SELF_REVIVE_HOLD_TIME,
                ReviveSource.Team => SyncedGameplayValues.TEAM_REVIVE_HOLD_TIME,
                _ => 2f
            };
            return Mathf.Max(0.1f, configured);
        }

        public static float GetProgressDuration(ReviveSource source)
        {
            float configured = source switch
            {
                ReviveSource.Self => SyncedGameplayValues.SELF_REVIVE_ANIMATION_DURATION,
                ReviveSource.Team => SyncedGameplayValues.TEAMMATE_REVIVE_ANIMATION_DURATION,
                _ => 3f
            };
            return Mathf.Max(3f, configured);
        }

        public static bool ShouldConsumeReviveItem(ReviveSource source)
        {
            return source switch
            {
                ReviveSource.Self => SyncedGameplayValues.CONSUME_REVIVE_ITEM_ON_SELF_REVIVE,
                ReviveSource.Team => SyncedGameplayValues.CONSUME_REVIVE_ITEM_ON_TEAMMATE_REVIVE,
                _ => false
            };
        }

    }
}
