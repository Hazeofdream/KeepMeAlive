//====================[ Imports ]====================
using System;
using System.Collections;
using EFT;
using EFT.HealthSystem;
using KeepMeAlive.Components;
using KeepMeAlive.Helpers;
using UnityEngine;

namespace KeepMeAlive.Features
{
    //====================[ DownedHealthAndEffectsManager ]====================
    internal static class DownedHealthAndEffectsManager
    {
        //====================[ Constants ]====================
        private static readonly EBodyPart[] TrackedBodyParts =
        {
            EBodyPart.Head, EBodyPart.Chest, EBodyPart.Stomach,
            EBodyPart.LeftArm, EBodyPart.RightArm, EBodyPart.LeftLeg, EBodyPart.RightLeg
        };

        //====================[ Public API ]====================
        // Restore destroyed body parts to 1 HP on downed entry (toggleable via config).
        public static void RestoreVitalsToMinimum(Player player)
        {
            if (player?.ActiveHealthController is not { } hc) return;
            try
            {
                for (int i = 0; i < TrackedBodyParts.Length; i++)
                {
                    var part = TrackedBodyParts[i];
                    if (hc.IsBodyPartDestroyed(part) && hc.FullRestoreBodyPart(part))
                    {
                        float delta = 1f - hc.GetBodyPartHealth(part).Current;
                        if (delta < -0.01f) hc.ChangeHealth(part, delta, default);
                    }
                }
            }
            catch (Exception ex) { Plugin.LogSource.LogError($"[DownedHealthAndEffects] RestoreVitalsToMinimum error: {ex.Message}"); }
        }

        // Apply critical visual effects and store original movement speed for later restoration.
        public static void ApplyCriticalEffects(Player player)
        {
            try
            {
                var st = RMSession.GetPlayerState(player.ProfileId);
                PlayerRestorations.StoreOriginalMovementSpeed(player);

                if (player?.ActiveHealthController != null)
                {
                    if (SyncedGameplayValues.CONTUSION_EFFECT) player.ActiveHealthController.DoContusion(SyncedGameplayValues.CRITICAL_STATE_TIME, 1f);
                    if (SyncedGameplayValues.STUN_EFFECT) player.ActiveHealthController.DoStun(Math.Min(SyncedGameplayValues.CRITICAL_STATE_TIME, SyncedGameplayValues.MAX_STUN_DURATION), 1f);
                }

                DownedMovementController.ApplyDownedMovementSpeed(player, st);
            }
            catch (Exception ex) { Plugin.LogSource.LogError($"[DownedHealthAndEffects] ApplyCriticalEffects error: {ex.Message}"); }
        }

        // Restore awareness if it was previously stored before downed-state modifiers.
        public static void RemoveRevivableState(Player player)
        {
            try
            {
                var st = RMSession.GetPlayerState(player.ProfileId);
                if (st.HasStoredAwareness) PlayerRestorations.RestoreAwareness(player);
            }
            catch (Exception ex) { Plugin.LogSource.LogError($"[DownedHealthAndEffects] RemoveRevivableState error: {ex.Message}"); }
        }
    }
}
