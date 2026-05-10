//====================[ Imports ]====================
using System;
using System.Collections.Generic;
using EFT;
using KeepMeAlive.Components;
using KeepMeAlive.Helpers;
using UnityEngine;

namespace KeepMeAlive.Features
{
    //====================[ BodyInteractableRuntime ]====================
    // Central runtime orchestration for body interactable lifecycle and cache.
    internal static class BodyInteractableRuntime
    {
        //====================[ Cache ]====================
        private static readonly Dictionary<string, BodyInteractable> Cache = new Dictionary<string, BodyInteractable>();

        //====================[ Public API ]====================
        public static bool TryRouteActions(GamePlayerOwner owner, GInterface150 interactive, ref ActionsReturnClass result)
        {
            // First route explicit EFT interactive targets that are ours.
            if (TryRouteInteractive(owner, interactive, ref result)) return true;

            // If EFT selected a teammate player, route directly via profile cache.
            if (TryRouteFromInteractablePlayer(owner, ref result)) return true;

            return false;
        }

        private static bool TryRouteInteractive(GamePlayerOwner owner, GInterface150 interactive, ref ActionsReturnClass result)
        {
            if (interactive is BodyInteractable body)
            {
                result = body.GetActions(owner);
                return result.Actions.Count > 0;
            }

            if (interactive is Component component)
            {
                var proxy = component as BodyInteractable.BodyInteractableProxy;
                if (proxy != null && proxy.Owner != null)
                {
                    result = proxy.Owner.GetActions(owner);
                    return result.Actions.Count > 0;
                }
            }

            if (interactive is MedPickerInteractable picker)
            {
                result = picker.GetActions(owner);
                return true;
            }

            return false;
        }

        private static bool TryRouteFromInteractablePlayer(GamePlayerOwner owner, ref ActionsReturnClass result)
        {
            var targetPlayer = owner?.Player?.InteractablePlayer;
            if (targetPlayer == null) return false;
            if (targetPlayer.IsAI || targetPlayer.AIData?.IsAI == true) return false;
            if (!Cache.TryGetValue(targetPlayer.ProfileId, out var body) || body == null || body.Revivee == null)
            {
                return false;
            }

            if (body.Revivee.ProfileId != targetPlayer.ProfileId)
            {
                return false;
            }

            // Enforce configurable interact range for standing teammates.
            float maxRange = SyncedGameplayValues.TEAM_HEAL_INTERACT_RANGE;
            if (maxRange > 0f)
            {
                float dist = Vector3.Distance(owner.Player.Position, targetPlayer.Position);
                if (dist > maxRange) return false;
            }

            result = body.GetActions(owner);
            return result.Actions.Count > 0;
        }

        //====================[ Lifecycle ]====================
        public static void AttachToPlayer(Player player)
        {
            try
            {
                if (player == null || player.gameObject == null)
                {
                    Plugin.LogSource.LogError("AttachToPlayer: Player or transform is null");
                    return;
                }

                BodyInteractable.AttachToPlayer(player);
                Plugin.LogSource.LogInfo($"Initiated BodyInteractable attachment routine for PlayerId {player.Id}");
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"AttachToPlayer error for player {(player != null ? player.Id.ToString() : "null")}: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static void Register(string profileId, BodyInteractable interactable)
        {
            if (string.IsNullOrEmpty(profileId) || interactable == null) return;
            Cache[profileId] = interactable;
        }

        public static void Tick(Player player)
        {
            // Tick no longer actively polls GetComponentsInChildren. 
            // BodyInteractable registers itself upon instantiation.
        }

        //====================[ Cleanup ]====================
        public static void Remove(string playerId)
        {
            if (string.IsNullOrEmpty(playerId)) return;
            Cache.Remove(playerId);
        }

        public static void ForceClosePicker(string playerId)
        {
            if (string.IsNullOrEmpty(playerId)) return;

            try
            {
                if (Cache.TryGetValue(playerId, out var interactable) && interactable != null)
                {
                    interactable.ForceClosePicker();
                }
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogWarning($"[BodyInteractableRuntime] ForceClosePicker error: {ex.Message}");
            }
        }
    }
}