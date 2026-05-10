//====================[ Imports ]====================
using System;
using System.IO;
using Newtonsoft.Json;

namespace KeepMeAlive.Helpers
{
    //====================[ SyncedRuntimeConfigSnapshot ]====================
    internal sealed class SyncedRuntimeConfigSnapshot
    {
        public int SchemaVersion { get; set; }
        public string ConfigHash { get; set; } = string.Empty;
        public long GeneratedAtUnixSeconds { get; set; }
        public SyncedRuntimeConfig Config { get; set; } = new();

        public static SyncedRuntimeConfigSnapshot CreateFailClosedSnapshot(string reason)
        {
            var config = new SyncedRuntimeConfig();
            config.Normalize();
            config.Gameplay.Revival.EnableSelfRevive = false;
            config.Gameplay.Revival.EnableTeamRevive = false;
            config.Gameplay.TeamHealing.Enabled = false;
            config.Gameplay.TeamHealing.AllowLootDownedPlayers = false;
            config.Gameplay.Protection.BlockDeathInCritical = false;
            config.Gameplay.Development.NoReviveItemRequired = false;
            config.Gameplay.Development.FreeTeamHealing = false;

            return new SyncedRuntimeConfigSnapshot
            {
                SchemaVersion = SyncedServerConfigStore.SupportedSchemaVersion,
                ConfigHash = string.Empty,
                GeneratedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Config = config
            };
        }
    }

    //====================[ SyncedRuntimeConfig ]====================
    internal sealed class SyncedRuntimeConfig
    {
        public int SchemaVersion { get; set; } = SyncedServerConfigStore.SupportedSchemaVersion;
        public SyncedRevivalItemConfig RevivalItem { get; set; } = new();
        public SyncedGameplayConfig Gameplay { get; set; } = new();

        public void Normalize()
        {
            SchemaVersion = SyncedServerConfigStore.SupportedSchemaVersion;
            RevivalItem ??= new SyncedRevivalItemConfig();
            Gameplay ??= new SyncedGameplayConfig();
            RevivalItem.Normalize();
            Gameplay.Normalize();
        }
    }

    //====================[ SyncedRevivalItemConfig ]====================
    internal sealed class SyncedRevivalItemConfig
    {
        public string TemplateId { get; set; } = "5c052e6986f7746b207bc3c9";
        public SyncedTradingConfig Trading { get; set; } = new();

        public void Normalize()
        {
            TemplateId = string.IsNullOrWhiteSpace(TemplateId)
                ? "5c052e6986f7746b207bc3c9"
                : TemplateId.Trim();

            Trading ??= new SyncedTradingConfig();
            Trading.Normalize();
        }
    }

    //====================[ SyncedTradingConfig ]====================
    internal sealed class SyncedTradingConfig
    {
        public string Trader { get; set; } = "Therapist";
        public int AmountRoubles { get; set; } = 200000;

        public void Normalize()
        {
            Trader = string.IsNullOrWhiteSpace(Trader) ? "Therapist" : Trader.Trim();
            AmountRoubles = Math.Max(1, AmountRoubles);
        }
    }

    //====================[ SyncedGameplayConfig ]====================
    internal sealed class SyncedGameplayConfig
    {
        public SyncedRevivalMechanicsConfig Revival { get; set; } = new();
        public SyncedPostReviveConfig PostRevive { get; set; } = new();
        public SyncedProtectionConfig Protection { get; set; } = new();
        public SyncedTeamHealingConfig TeamHealing { get; set; } = new();
        public SyncedDevelopmentGameplayConfig Development { get; set; } = new();

        public void Normalize()
        {
            Revival ??= new SyncedRevivalMechanicsConfig();
            PostRevive ??= new SyncedPostReviveConfig();
            Protection ??= new SyncedProtectionConfig();
            TeamHealing ??= new SyncedTeamHealingConfig();
            Development ??= new SyncedDevelopmentGameplayConfig();

            Revival.Normalize();
            PostRevive.Normalize();
            Protection.Normalize();
            TeamHealing.Normalize();
        }
    }

    //====================[ SyncedRevivalMechanicsConfig ]====================
    internal sealed class SyncedRevivalMechanicsConfig
    {
        public bool EnableSelfRevive { get; set; } = true;
        public bool EnableTeamRevive { get; set; } = true;
        public float SelfReviveHoldSeconds { get; set; } = 2f;
        public float TeamReviveHoldSeconds { get; set; } = 2f;
        public float SelfReviveProgressSeconds { get; set; } = 10f;
        public float TeamReviveProgressSeconds { get; set; } = 5f;
        public bool ConsumeReviveItemOnSelfRevive { get; set; } = true;
        public bool ConsumeReviveItemOnTeamRevive { get; set; } = false;
        public float CriticalStateSeconds { get; set; } = 180f;
        public bool RestoreVitalsOnDowned { get; set; } = true;
        public bool ApplyContusionOnDowned { get; set; } = true;
        public bool ApplyStunOnDowned { get; set; } = true;
        public float MaxStunSeconds { get; set; } = 20f;
        public float DownedMovementSpeedPercent { get; set; } = 50f;
        public bool BlockUiWhenDowned { get; set; } = true;
        public bool UnconsciousOnDowned { get; set; } = false;

        public void Normalize()
        {
            SelfReviveHoldSeconds = Math.Max(0.1f, SelfReviveHoldSeconds);
            TeamReviveHoldSeconds = Math.Max(0.1f, TeamReviveHoldSeconds);
            SelfReviveProgressSeconds = Math.Max(3f, SelfReviveProgressSeconds);
            TeamReviveProgressSeconds = Math.Max(3f, TeamReviveProgressSeconds);
            CriticalStateSeconds = Math.Max(1f, CriticalStateSeconds);
            MaxStunSeconds = Math.Max(0f, MaxStunSeconds);
            DownedMovementSpeedPercent = Math.Clamp(DownedMovementSpeedPercent, 0f, 100f);
        }
    }

    //====================[ SyncedPostReviveConfig ]====================
    internal sealed class SyncedPostReviveConfig
    {
        public SyncedPostReviveSourceConfig Self { get; set; } = SyncedPostReviveSourceConfig.CreateSelfDefaults();
        public SyncedPostReviveSourceConfig Team { get; set; } = SyncedPostReviveSourceConfig.CreateTeamDefaults();

        public void Normalize()
        {
            Self ??= SyncedPostReviveSourceConfig.CreateSelfDefaults();
            Team ??= SyncedPostReviveSourceConfig.CreateTeamDefaults();
            Self.Normalize();
            Team.Normalize();
        }
    }

    //====================[ SyncedPostReviveSourceConfig ]====================
    internal sealed class SyncedPostReviveSourceConfig
    {
        public bool RestoreBodyParts { get; set; } = true;
        public SyncedBodyRestorePercentConfig RestorePercent { get; set; } = SyncedBodyRestorePercentConfig.CreateNeutralDefaults();
        public bool RemoveBleeds { get; set; }
        public bool RemoveFractures { get; set; }
        public float InvulnerabilityDurationSeconds { get; set; } = 5f;
        public float InvulnerabilitySpeedPercent { get; set; } = 100f;
        public float CooldownSeconds { get; set; }
        public bool ApplyContusionOnRevive { get; set; } = true;
        public float ContusionDurationSeconds { get; set; }
        public bool ApplyPainOnRevive { get; set; }
        public float PainDurationSeconds { get; set; } = 30f;

        public static SyncedPostReviveSourceConfig CreateSelfDefaults() => new()
        {
            RestoreBodyParts = true,
            RestorePercent = SyncedBodyRestorePercentConfig.CreateSelfDefaults(),
            RemoveBleeds = false,
            RemoveFractures = false,
            InvulnerabilityDurationSeconds = 5f,
            InvulnerabilitySpeedPercent = 100f,
            CooldownSeconds = 240f,
            ApplyContusionOnRevive = true,
            ContusionDurationSeconds = 10f,
            ApplyPainOnRevive = true,
            PainDurationSeconds = 30f
        };

        public static SyncedPostReviveSourceConfig CreateTeamDefaults() => new()
        {
            RestoreBodyParts = true,
            RestorePercent = SyncedBodyRestorePercentConfig.CreateTeamDefaults(),
            RemoveBleeds = true,
            RemoveFractures = true,
            InvulnerabilityDurationSeconds = 5f,
            InvulnerabilitySpeedPercent = 100f,
            CooldownSeconds = 180f,
            ApplyContusionOnRevive = true,
            ContusionDurationSeconds = 5f,
            ApplyPainOnRevive = false,
            PainDurationSeconds = 30f
        };

        public void Normalize()
        {
            RestorePercent ??= SyncedBodyRestorePercentConfig.CreateNeutralDefaults();
            RestorePercent.Normalize();

            InvulnerabilityDurationSeconds = Math.Max(0f, InvulnerabilityDurationSeconds);
            InvulnerabilitySpeedPercent = (!float.IsNaN(InvulnerabilitySpeedPercent) && !float.IsInfinity(InvulnerabilitySpeedPercent))
                ? InvulnerabilitySpeedPercent
                : 100f;
            CooldownSeconds = Math.Max(0f, CooldownSeconds);
            ContusionDurationSeconds = Math.Max(0f, ContusionDurationSeconds);
            PainDurationSeconds = Math.Max(0f, PainDurationSeconds);
        }
    }

    //====================[ SyncedBodyRestorePercentConfig ]====================
    internal sealed class SyncedBodyRestorePercentConfig
    {
        public float Head { get; set; }
        public float Chest { get; set; }
        public float Stomach { get; set; }
        public float Arms { get; set; }
        public float Legs { get; set; }

        public static SyncedBodyRestorePercentConfig CreateNeutralDefaults() => new()
        {
            Head = 0f,
            Chest = 35f,
            Stomach = 35f,
            Arms = 35f,
            Legs = 35f
        };

        public static SyncedBodyRestorePercentConfig CreateSelfDefaults() => CreateNeutralDefaults();

        public static SyncedBodyRestorePercentConfig CreateTeamDefaults() => new()
        {
            Head = 50f,
            Chest = 50f,
            Stomach = 50f,
            Arms = 50f,
            Legs = 50f
        };

        public void Normalize()
        {
            Head = Math.Clamp(Head, 0f, 100f);
            Chest = Math.Clamp(Chest, 0f, 100f);
            Stomach = Math.Clamp(Stomach, 0f, 100f);
            Arms = Math.Clamp(Arms, 0f, 100f);
            Legs = Math.Clamp(Legs, 0f, 100f);
        }
    }

    //====================[ SyncedProtectionConfig ]====================
    internal sealed class SyncedProtectionConfig
    {
        public bool BlockDeathInCritical { get; set; } = true;
        public bool EnableGodMode { get; set; }
        public bool EnableGhostMode { get; set; } = true;
        public bool EnableHardcoreMode { get; set; }
        public bool HardcoreHeadshotsAreFatal { get; set; }
        public float HardcoreCriticalStateChance { get; set; } = 0.75f;

        public void Normalize()
        {
            HardcoreCriticalStateChance = Math.Clamp(HardcoreCriticalStateChance, 0f, 1f);
        }
    }

    //====================[ SyncedTeamHealingConfig ]====================
    internal sealed class SyncedTeamHealingConfig
    {
        public bool Enabled { get; set; } = true;
        public float InteractRangeMeters { get; set; } = 1f;
        public float HoldSeconds { get; set; } = 1f;
        public float UseTimeMultiplier { get; set; } = 1f;
        public float MinHpResourceToDisplay { get; set; } = 50f;
        public float NutritionMinDeficitToDisplay { get; set; } = 0.5f;
        public bool AllowLootDownedPlayers { get; set; } = true;

        public void Normalize()
        {
            InteractRangeMeters = Math.Max(0f, InteractRangeMeters);
            HoldSeconds = Math.Max(0.1f, HoldSeconds);
            UseTimeMultiplier = Math.Max(0.01f, UseTimeMultiplier);
            MinHpResourceToDisplay = Math.Max(0f, MinHpResourceToDisplay);
            NutritionMinDeficitToDisplay = Math.Max(0f, NutritionMinDeficitToDisplay);
        }
    }

    //====================[ SyncedDevelopmentGameplayConfig ]====================
    internal sealed class SyncedDevelopmentGameplayConfig
    {
        public bool NoReviveItemRequired { get; set; }
        public bool FreeTeamHealing { get; set; }
    }

    //====================[ SyncedServerConfigStore ]====================
    internal static class SyncedServerConfigStore
    {
        public const int SupportedSchemaVersion = 2;

        private const string CacheFileName = "com.KeepMeAlive.server-runtime-config.json";
        private static readonly object SyncRoot = new();

        private static SyncedRuntimeConfigSnapshot _activeSnapshot = SyncedRuntimeConfigSnapshot.CreateFailClosedSnapshot("startup");
        private static bool _initialized;
        private static bool _hasAuthoritativeSnapshot;

        public static bool HasAuthoritativeSnapshot => _hasAuthoritativeSnapshot;
        public static SyncedRuntimeConfig Config => _activeSnapshot.Config;
        public static string LastStatus { get; private set; } = "Runtime config not initialized.";

        //====================[ Lifecycle ]====================
        public static void Initialize()
        {
            lock (SyncRoot)
            {
                if (_initialized) return;
                _initialized = true;
            }

            TryLoadCachedSnapshot();
            RefreshFromServer("plugin-load");

            if (!_hasAuthoritativeSnapshot)
            {
                LogWarning($"[SyncConfig] No authoritative snapshot available after initialization. Running fail-closed. Status={LastStatus}");
            }
        }

        public static void EnsureSnapshot(string reason)
        {
            if (_hasAuthoritativeSnapshot)
            {
                return;
            }

            RefreshFromServer(reason);
        }

        public static bool RefreshFromServer(string reason)
        {
            string validationError = string.Empty;
            bool fetched = RevivalAuthority.TryGetRuntimeConfig(out var snapshot, out var fetchError);

            if (fetched && TryAdoptSnapshot(snapshot, persistToCache: true, sourceLabel: $"server:{reason}", out validationError))
            {
                LastStatus = $"Loaded authoritative snapshot from server ({reason}).";
                return true;
            }

            string error = !string.IsNullOrWhiteSpace(fetchError)
                ? fetchError
                : validationError;

            LastStatus = string.IsNullOrWhiteSpace(error)
                ? "Failed to load runtime config snapshot from server."
                : error;

            if (_hasAuthoritativeSnapshot)
            {
                LogWarning($"[SyncConfig] Server refresh failed; keeping existing cached snapshot. {LastStatus}");
                return false;
            }

            ActivateFailClosed(LastStatus);
            return false;
        }

        //====================[ Cache ]====================
        private static bool TryLoadCachedSnapshot()
        {
            string cachePath = GetCachePath();
            if (!File.Exists(cachePath))
            {
                LastStatus = "No cached runtime snapshot found.";
                return false;
            }

            try
            {
                string json = File.ReadAllText(cachePath);
                var snapshot = JsonConvert.DeserializeObject<SyncedRuntimeConfigSnapshot>(json);
                if (!TryAdoptSnapshot(snapshot, persistToCache: false, sourceLabel: "cache", out var validationError))
                {
                    LastStatus = validationError;
                    LogWarning($"[SyncConfig] Cached snapshot rejected. {validationError}");
                    return false;
                }

                LastStatus = "Loaded authoritative snapshot from local cache.";
                return true;
            }
            catch (Exception ex)
            {
                LastStatus = $"Failed to load cached runtime snapshot: {ex.Message}";
                LogWarning($"[SyncConfig] {LastStatus}");
                return false;
            }
        }

        private static void TryPersistSnapshot(SyncedRuntimeConfigSnapshot snapshot)
        {
            try
            {
                string cachePath = GetCachePath();
                string json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);
                File.WriteAllText(cachePath, json);
            }
            catch (Exception ex)
            {
                LogWarning($"[SyncConfig] Failed to persist runtime snapshot cache: {ex.Message}");
            }
        }

        //====================[ Internal Helpers ]====================
        private static bool TryAdoptSnapshot(SyncedRuntimeConfigSnapshot snapshot, bool persistToCache, string sourceLabel, out string error)
        {
            error = string.Empty;
            if (!ValidateSnapshot(snapshot, out error))
            {
                return false;
            }

            snapshot.Config.Normalize();
            snapshot.SchemaVersion = SupportedSchemaVersion;

            lock (SyncRoot)
            {
                _activeSnapshot = snapshot;
                _hasAuthoritativeSnapshot = true;
            }

            if (persistToCache)
            {
                TryPersistSnapshot(snapshot);
            }

            LogInfo($"[SyncConfig] Adopted authoritative runtime snapshot from {sourceLabel}. hash={snapshot.ConfigHash} generated={snapshot.GeneratedAtUnixSeconds}");
            return true;
        }

        private static bool ValidateSnapshot(SyncedRuntimeConfigSnapshot snapshot, out string error)
        {
            error = string.Empty;
            if (snapshot == null)
            {
                error = "Runtime snapshot payload is null.";
                return false;
            }

            if (snapshot.Config == null)
            {
                error = "Runtime snapshot config payload is null.";
                return false;
            }

            int schemaVersion = snapshot.SchemaVersion > 0
                ? snapshot.SchemaVersion
                : snapshot.Config.SchemaVersion;

            if (schemaVersion != SupportedSchemaVersion)
            {
                error = $"Unsupported runtime config schema version: {schemaVersion}. Expected {SupportedSchemaVersion}.";
                return false;
            }

            return true;
        }

        private static void ActivateFailClosed(string reason)
        {
            lock (SyncRoot)
            {
                _activeSnapshot = SyncedRuntimeConfigSnapshot.CreateFailClosedSnapshot(reason);
                _hasAuthoritativeSnapshot = false;
            }

            LogWarning($"[SyncConfig] Entered fail-closed mode. {reason}");
        }

        private static string GetCachePath()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(root, "BepInEx", "config", CacheFileName);
        }

        private static void LogInfo(string message)
        {
            Plugin.LogSource?.LogInfo(message);
        }

        private static void LogWarning(string message)
        {
            Plugin.LogSource?.LogWarning(message);
        }
    }

    //====================[ SyncedGameplayValues ]====================
    internal static class SyncedGameplayValues
    {
        private static SyncedRuntimeConfig Config => SyncedServerConfigStore.Config;
        private static SyncedGameplayConfig Gameplay => Config.Gameplay;

        public static string REVIVAL_ITEM_ID => Config.RevivalItem.TemplateId;

        public static bool SELF_REVIVAL_ENABLED => Gameplay.Revival.EnableSelfRevive;
        public static bool TEAM_REVIVE_ENABLED => Gameplay.Revival.EnableTeamRevive;
        public static float SELF_REVIVE_HOLD_TIME => Gameplay.Revival.SelfReviveHoldSeconds;
        public static float TEAM_REVIVE_HOLD_TIME => Gameplay.Revival.TeamReviveHoldSeconds;
        public static float SELF_REVIVE_ANIMATION_DURATION => Gameplay.Revival.SelfReviveProgressSeconds;
        public static float TEAMMATE_REVIVE_ANIMATION_DURATION => Gameplay.Revival.TeamReviveProgressSeconds;
        public static bool CONSUME_REVIVE_ITEM_ON_SELF_REVIVE => Gameplay.Revival.ConsumeReviveItemOnSelfRevive;
        public static bool CONSUME_REVIVE_ITEM_ON_TEAMMATE_REVIVE => Gameplay.Revival.ConsumeReviveItemOnTeamRevive;
        public static float CRITICAL_STATE_TIME => Gameplay.Revival.CriticalStateSeconds;
        public static bool RESTORE_VITALS_ON_DOWNED => Gameplay.Revival.RestoreVitalsOnDowned;
        public static bool CONTUSION_EFFECT => Gameplay.Revival.ApplyContusionOnDowned;
        public static bool STUN_EFFECT => Gameplay.Revival.ApplyStunOnDowned;
        public static float MAX_STUN_DURATION => Gameplay.Revival.MaxStunSeconds;
        public static float DOWNED_MOVEMENT_SPEED => Gameplay.Revival.DownedMovementSpeedPercent;
        public static bool BLOCK_UI_WHEN_DOWNED => Gameplay.Revival.BlockUiWhenDowned;
        public static bool UNCONSCIOUS_ON_DOWNED => Gameplay.Revival.UnconsciousOnDowned;

        public static bool SELF_REVIVE_RESTORE_BODY_PARTS => Gameplay.PostRevive.Self.RestoreBodyParts;
        public static float SELF_REVIVE_HEAD_PCT => Gameplay.PostRevive.Self.RestorePercent.Head;
        public static float SELF_REVIVE_CHEST_PCT => Gameplay.PostRevive.Self.RestorePercent.Chest;
        public static float SELF_REVIVE_STOMACH_PCT => Gameplay.PostRevive.Self.RestorePercent.Stomach;
        public static float SELF_REVIVE_ARMS_PCT => Gameplay.PostRevive.Self.RestorePercent.Arms;
        public static float SELF_REVIVE_LEGS_PCT => Gameplay.PostRevive.Self.RestorePercent.Legs;
        public static bool SELF_REVIVE_REMOVE_BLEEDS => Gameplay.PostRevive.Self.RemoveBleeds;
        public static bool SELF_REVIVE_REMOVE_FRACTURES => Gameplay.PostRevive.Self.RemoveFractures;
        public static float SELF_REVIVE_INVULN_DURATION => Gameplay.PostRevive.Self.InvulnerabilityDurationSeconds;
        public static float SELF_REVIVE_INVULN_SPEED_PCT => Gameplay.PostRevive.Self.InvulnerabilitySpeedPercent;
        public static float SELF_REVIVE_COOLDOWN => Gameplay.PostRevive.Self.CooldownSeconds;
        public static bool SELF_REVIVE_CONTUSION_ON_REVIVE => Gameplay.PostRevive.Self.ApplyContusionOnRevive;
        public static float SELF_REVIVE_CONTUSION_DURATION => Gameplay.PostRevive.Self.ContusionDurationSeconds;
        public static bool SELF_REVIVE_PAIN_ON_REVIVE => Gameplay.PostRevive.Self.ApplyPainOnRevive;
        public static float SELF_REVIVE_PAIN_DURATION => Gameplay.PostRevive.Self.PainDurationSeconds;

        public static bool TEAM_REVIVE_RESTORE_BODY_PARTS => Gameplay.PostRevive.Team.RestoreBodyParts;
        public static float TEAM_REVIVE_HEAD_PCT => Gameplay.PostRevive.Team.RestorePercent.Head;
        public static float TEAM_REVIVE_CHEST_PCT => Gameplay.PostRevive.Team.RestorePercent.Chest;
        public static float TEAM_REVIVE_STOMACH_PCT => Gameplay.PostRevive.Team.RestorePercent.Stomach;
        public static float TEAM_REVIVE_ARMS_PCT => Gameplay.PostRevive.Team.RestorePercent.Arms;
        public static float TEAM_REVIVE_LEGS_PCT => Gameplay.PostRevive.Team.RestorePercent.Legs;
        public static bool TEAM_REVIVE_REMOVE_BLEEDS => Gameplay.PostRevive.Team.RemoveBleeds;
        public static bool TEAM_REVIVE_REMOVE_FRACTURES => Gameplay.PostRevive.Team.RemoveFractures;
        public static float TEAM_REVIVE_INVULN_DURATION => Gameplay.PostRevive.Team.InvulnerabilityDurationSeconds;
        public static float TEAM_REVIVE_INVULN_SPEED_PCT => Gameplay.PostRevive.Team.InvulnerabilitySpeedPercent;
        public static float TEAM_REVIVE_COOLDOWN => Gameplay.PostRevive.Team.CooldownSeconds;
        public static bool TEAM_REVIVE_CONTUSION_ON_REVIVE => Gameplay.PostRevive.Team.ApplyContusionOnRevive;
        public static float TEAM_REVIVE_CONTUSION_DURATION => Gameplay.PostRevive.Team.ContusionDurationSeconds;
        public static bool TEAM_REVIVE_PAIN_ON_REVIVE => Gameplay.PostRevive.Team.ApplyPainOnRevive;
        public static float TEAM_REVIVE_PAIN_DURATION => Gameplay.PostRevive.Team.PainDurationSeconds;

        public static bool DEATH_BLOCK_IN_CRITICAL => Gameplay.Protection.BlockDeathInCritical;
        public static bool GOD_MODE => Gameplay.Protection.EnableGodMode;
        public static bool GHOST_MODE => Gameplay.Protection.EnableGhostMode;
        public static bool HARDCORE_MODE => Gameplay.Protection.EnableHardcoreMode;
        public static bool HARDCORE_HEADSHOT_DEFAULT_DEAD => Gameplay.Protection.HardcoreHeadshotsAreFatal;
        public static float HARDCORE_CHANCE_OF_CRITICAL_STATE => Gameplay.Protection.HardcoreCriticalStateChance;

        public static bool TEAM_HEAL_ENABLED => Gameplay.TeamHealing.Enabled;
        public static float TEAM_HEAL_INTERACT_RANGE => Gameplay.TeamHealing.InteractRangeMeters;
        public static float TEAM_HEAL_HOLD_TIME => Gameplay.TeamHealing.HoldSeconds;
        public static float TEAM_HEAL_USE_TIME_MULTIPLIER => Gameplay.TeamHealing.UseTimeMultiplier;
        public static float TEAM_HEAL_MIN_HP_RESOURCE => Gameplay.TeamHealing.MinHpResourceToDisplay;
        public static float TEAM_HEAL_NUTRITION_MIN_DEFICIT => Gameplay.TeamHealing.NutritionMinDeficitToDisplay;
        public static bool ALLOW_LOOT_DOWNED_PLAYERS => Gameplay.TeamHealing.AllowLootDownedPlayers;

        public static bool NO_REVIVE_ITEM_REQUIRED => Gameplay.Development.NoReviveItemRequired;
        public static bool FREE_TEAM_HEALING => Gameplay.Development.FreeTeamHealing;
    }
}
