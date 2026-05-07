//====================[ Imports ]====================
using BepInEx.Configuration;
using Newtonsoft.Json;
using SPT.Common.Http;
using System;
using KeepMeAlive.Models;
using System.Threading.Tasks;
using UnityEngine;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.DI;

namespace KeepMeAlive.Helpers
{
    //====================[ KeepMeAliveSettings ]====================
    internal class KeepMeAliveSettings
    {
        //====================[ Settings Properties ]====================
        #region Settings Properties

        public static ModConfig MOD_CONFIG;

        // Key Bindings
        public static ConfigEntry<KeyCode> SELF_REVIVE_KEY;
        public static ConfigEntry<KeyCode> GIVE_UP_KEY;

        // Revival Mechanics
        public static string REVIVAL_ITEM_ID;
        public static bool SELF_REVIVAL_ENABLED;
        public static bool TEAM_REVIVE_ENABLED;
        public static float SELF_REVIVE_HOLD_TIME;
        public static float TEAM_REVIVE_HOLD_TIME;
        public static float SELF_REVIVE_ANIMATION_DURATION;
        public static float TEAMMATE_REVIVE_ANIMATION_DURATION;
        public static bool CONSUME_REVIVE_ITEM_ON_SELF_REVIVE;
        public static bool CONSUME_REVIVE_ITEM_ON_TEAMMATE_REVIVE;
        public static float CRITICAL_STATE_TIME;

        // Self-Revive Post-Revival
        public static bool SELF_REVIVE_RESTORE_BODY_PARTS;
        public static float SELF_REVIVE_HEAD_PCT;
        public static float SELF_REVIVE_CHEST_PCT;
        public static float SELF_REVIVE_STOMACH_PCT;
        public static float SELF_REVIVE_ARMS_PCT;
        public static float SELF_REVIVE_LEGS_PCT;
        public static bool SELF_REVIVE_REMOVE_BLEEDS;
        public static bool SELF_REVIVE_REMOVE_FRACTURES;
        public static float SELF_REVIVE_INVULN_DURATION;
        public static float SELF_REVIVE_INVULN_SPEED_PCT;
        public static float SELF_REVIVE_COOLDOWN;
        public static bool SELF_REVIVE_CONTUSION_ON_REVIVE;
        public static float SELF_REVIVE_CONTUSION_DURATION;
        public static bool SELF_REVIVE_PAIN_ON_REVIVE;

        // Team-Revive Post-Revival
        public static bool TEAM_REVIVE_RESTORE_BODY_PARTS;
        public static float TEAM_REVIVE_HEAD_PCT;
        public static float TEAM_REVIVE_CHEST_PCT;
        public static float TEAM_REVIVE_STOMACH_PCT;
        public static float TEAM_REVIVE_ARMS_PCT;
        public static float TEAM_REVIVE_LEGS_PCT;
        public static bool TEAM_REVIVE_REMOVE_BLEEDS;
        public static bool TEAM_REVIVE_REMOVE_FRACTURES;
        public static float TEAM_REVIVE_INVULN_DURATION;
        public static float TEAM_REVIVE_INVULN_SPEED_PCT;
        public static float TEAM_REVIVE_COOLDOWN;
        public static bool TEAM_REVIVE_CONTUSION_ON_REVIVE;
        public static float TEAM_REVIVE_CONTUSION_DURATION;
        public static bool TEAM_REVIVE_PAIN_ON_REVIVE;

        // Effects
        public static bool CONTUSION_EFFECT;
        public static bool STUN_EFFECT;
        public static float MEDICAL_RANGE;
        public static float DOWNED_MOVEMENT_SPEED;
        public static bool BLOCK_UI_WHEN_DOWNED;

        // Hardcore Mode
        public static bool DEATH_BLOCK_IN_CRITICAL;
        public static bool GOD_MODE;
        public static bool GHOST_MODE;
        public static bool HARDCORE_MODE;
        public static bool HARDCORE_HEADSHOT_DEFAULT_DEAD;
        public static float HARDCORE_CHANCE_OF_CRITICAL_STATE;

        // Team Healing
        public static float TEAM_HEAL_HOLD_TIME;
        public static float TEAM_HEAL_MIN_HP_RESOURCE;
        public static float TEAM_HEAL_NUTRITION_MIN_DEFICIT;

        // Development
        public static bool NO_REVIVE_ITEM_REQUIRED;
        public static bool ENABLE_DEBUG_LOGS;
        public static bool DEBUG_REVIVE_FLOW;
        public static bool DEBUG_NETWORK_TRACE;
        public static bool DEBUG_SELF_REVIVE_TRACE;
        public static bool DEBUG_KEYBINDS;
        public static bool FREE_TEAM_HEALING;

        #endregion

        //====================[ Init ]====================
        public async static void Init(ConfigFile config)
        {
            MOD_CONFIG = await LoadFromServer();

            #region Key Bindings Settings

            SELF_REVIVE_KEY = config.Bind(
                "1. Key Bindings",
                "Self Revive Key",
                KeyCode.F5,
                "The key used to revive yourself."
            );

            GIVE_UP_KEY = config.Bind(
                "1. Key Bindings",
                "Give Up Key",
                KeyCode.Backspace,
                "Press this key when in critical state to die immediately"
            );

            #endregion

            #region Mechanics Settings

            // Revival Mechanics
            REVIVAL_ITEM_ID = MOD_CONFIG.RevivalItemId;
            SELF_REVIVAL_ENABLED = MOD_CONFIG.SelfRevivalEnabled;
            TEAM_REVIVE_ENABLED = MOD_CONFIG.TeamReviveEnabled;
            SELF_REVIVE_HOLD_TIME = MOD_CONFIG.SelfReviveHoldTime;
            TEAM_REVIVE_HOLD_TIME = MOD_CONFIG.TeamReviveHoldTime;
            SELF_REVIVE_ANIMATION_DURATION = MOD_CONFIG.SelfReviveAnimationDuration;
            TEAMMATE_REVIVE_ANIMATION_DURATION = MOD_CONFIG.TeammateReviveAnimationDuration;
            CONSUME_REVIVE_ITEM_ON_SELF_REVIVE = MOD_CONFIG.ConsumeReviveItemOnSelfRevive;
            CONSUME_REVIVE_ITEM_ON_TEAMMATE_REVIVE = MOD_CONFIG.ConsumeReviveItemOnTeammateRevive;
            CRITICAL_STATE_TIME = MOD_CONFIG.CriticalStateTime;

            // Self Revive
            SELF_REVIVE_RESTORE_BODY_PARTS = MOD_CONFIG.SelfReviveRestoreBodyParts;
            SELF_REVIVE_HEAD_PCT = MOD_CONFIG.SelfReviveHeadPct;
            SELF_REVIVE_CHEST_PCT = MOD_CONFIG.SelfReviveChestPct;
            SELF_REVIVE_STOMACH_PCT = MOD_CONFIG.SelfReviveStomachPct;
            SELF_REVIVE_ARMS_PCT = MOD_CONFIG.SelfReviveArmsPct;
            SELF_REVIVE_LEGS_PCT = MOD_CONFIG.SelfReviveLegsPct;
            SELF_REVIVE_REMOVE_BLEEDS = MOD_CONFIG.SelfReviveRemoveBleeds;
            SELF_REVIVE_REMOVE_FRACTURES = MOD_CONFIG.SelfReviveRemoveFractures;
            SELF_REVIVE_INVULN_DURATION = MOD_CONFIG.SelfReviveInvulnDuration;
            SELF_REVIVE_INVULN_SPEED_PCT = MOD_CONFIG.SelfReviveInvulnSpeedPct;
            SELF_REVIVE_COOLDOWN = MOD_CONFIG.SelfReviveCooldown;
            SELF_REVIVE_CONTUSION_ON_REVIVE = MOD_CONFIG.SelfReviveContusionOnRevive;
            SELF_REVIVE_CONTUSION_DURATION = MOD_CONFIG.SelfReviveContusionDuration;
            SELF_REVIVE_PAIN_ON_REVIVE = MOD_CONFIG.SelfRevivePainOnRevive;

            // Team Revive
            TEAM_REVIVE_RESTORE_BODY_PARTS = MOD_CONFIG.TeamReviveRestoreBodyParts;
            TEAM_REVIVE_HEAD_PCT = MOD_CONFIG.TeamReviveHeadPct;
            TEAM_REVIVE_CHEST_PCT = MOD_CONFIG.TeamReviveChestPct;
            TEAM_REVIVE_STOMACH_PCT = MOD_CONFIG.TeamReviveStomachPct;
            TEAM_REVIVE_ARMS_PCT = MOD_CONFIG.TeamReviveArmsPct;
            TEAM_REVIVE_LEGS_PCT = MOD_CONFIG.TeamReviveLegsPct;
            TEAM_REVIVE_REMOVE_BLEEDS = MOD_CONFIG.TeamReviveRemoveBleeds;
            TEAM_REVIVE_REMOVE_FRACTURES = MOD_CONFIG.TeamReviveRemoveFractures;
            TEAM_REVIVE_INVULN_DURATION = MOD_CONFIG.TeamReviveInvulnDuration;
            TEAM_REVIVE_INVULN_SPEED_PCT = MOD_CONFIG.TeamReviveInvulnSpeedPct;
            TEAM_REVIVE_COOLDOWN = MOD_CONFIG.TeamReviveCooldown;
            TEAM_REVIVE_CONTUSION_ON_REVIVE = MOD_CONFIG.TeamReviveContusionOnRevive;
            TEAM_REVIVE_CONTUSION_DURATION = MOD_CONFIG.TeamReviveContusionDuration;
            TEAM_REVIVE_PAIN_ON_REVIVE = MOD_CONFIG.TeamRevivePainOnRevive;

            // Effects
            CONTUSION_EFFECT = MOD_CONFIG.ContusionEffect;
            STUN_EFFECT = MOD_CONFIG.StunEffect;
            MEDICAL_RANGE = MOD_CONFIG.MedicalRange;
            DOWNED_MOVEMENT_SPEED = MOD_CONFIG.DownedMovementSpeed;
            BLOCK_UI_WHEN_DOWNED = MOD_CONFIG.BlockUiWhenDowned;

            // Hardcore
            DEATH_BLOCK_IN_CRITICAL = MOD_CONFIG.DeathBlockInCritical;
            GOD_MODE = MOD_CONFIG.GodMode;
            GHOST_MODE = MOD_CONFIG.GhostMode;
            HARDCORE_MODE = MOD_CONFIG.HardcoreMode;
            HARDCORE_HEADSHOT_DEFAULT_DEAD = MOD_CONFIG.HardcoreHeadshotDefaultDead;
            HARDCORE_CHANCE_OF_CRITICAL_STATE = MOD_CONFIG.HardcoreChanceOfCriticalState;

            // Team Healing
            TEAM_HEAL_HOLD_TIME = MOD_CONFIG.TeamHealHoldTime;
            TEAM_HEAL_MIN_HP_RESOURCE = MOD_CONFIG.TeamHealMinHpResource;
            TEAM_HEAL_NUTRITION_MIN_DEFICIT = MOD_CONFIG.TeamHealNutritionMinDeficit;

            // Debug
            NO_REVIVE_ITEM_REQUIRED = MOD_CONFIG.NoReviveItemRequired;
            ENABLE_DEBUG_LOGS = MOD_CONFIG.EnableDebugLogs;
            DEBUG_REVIVE_FLOW = MOD_CONFIG.DebugReviveFlow;
            DEBUG_NETWORK_TRACE = MOD_CONFIG.DebugNetworkTrace;
            DEBUG_SELF_REVIVE_TRACE = MOD_CONFIG.DebugSelfReviveTrace;
            DEBUG_KEYBINDS = MOD_CONFIG.DebugKeybinds;
            FREE_TEAM_HEALING = MOD_CONFIG.FreeTeamHealing;

            #endregion
        }

        public static async Task<ModConfig> LoadFromServer()
        {
            try
            {
                string payload = await RequestHandler.GetJsonAsync("/keepmealive/load");

                return JsonConvert.DeserializeObject<ModConfig>(payload);
            }
            catch (Exception)
            {
                NotificationManagerClass.DisplayWarningNotification("Failed to load Bring Me To Life server config - check the server");

                return null;
            }
        }
    }
}