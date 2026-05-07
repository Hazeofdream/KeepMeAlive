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
                ReviveSource.Self => KeepMeAliveSettings.SELF_REVIVAL_ENABLED,
                ReviveSource.Team => KeepMeAliveSettings.TEAM_REVIVE_ENABLED,
                _ => true
            };
        }

        public static float GetHoldDuration(ReviveSource source)
        {
            float configured = source switch
            {
                ReviveSource.Self => KeepMeAliveSettings.SELF_REVIVE_HOLD_TIME,
                ReviveSource.Team => KeepMeAliveSettings.TEAM_REVIVE_HOLD_TIME,
                _ => 2f
            };
            return Mathf.Max(0.1f, configured);
        }

        public static float GetProgressDuration(ReviveSource source)
        {
            float configured = source switch
            {
                ReviveSource.Self => KeepMeAliveSettings.SELF_REVIVE_ANIMATION_DURATION,
                ReviveSource.Team => KeepMeAliveSettings.TEAMMATE_REVIVE_ANIMATION_DURATION,
                _ => 3f
            };
            return Mathf.Max(3f, configured);
        }

        public static bool ShouldConsumeReviveItem(ReviveSource source)
        {
            return source switch
            {
                ReviveSource.Self => KeepMeAliveSettings.CONSUME_REVIVE_ITEM_ON_SELF_REVIVE,
                ReviveSource.Team => KeepMeAliveSettings.CONSUME_REVIVE_ITEM_ON_TEAMMATE_REVIVE,
                _ => false
            };
        }

        //====================[ Authority Routing ]====================
        public static bool UseResilientAuthority(ReviveSource source)
        {
            return source == ReviveSource.Team;
        }
    }
}
