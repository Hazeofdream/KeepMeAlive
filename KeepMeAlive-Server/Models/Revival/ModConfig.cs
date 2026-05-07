using System.Text.Json.Serialization;

namespace KeepMeAlive.Models;

public class ModConfig
{
    [JsonPropertyName("revivalItemId")]
    public required string RevivalItemId { get; set; }

    [JsonPropertyName("selfRevivalEnabled")]
    public bool SelfRevivalEnabled { get; set; }

    [JsonPropertyName("teamReviveEnabled")]
    public bool TeamReviveEnabled { get; set; }

    [JsonPropertyName("selfReviveHoldTime")]
    public float SelfReviveHoldTime { get; set; }

    [JsonPropertyName("teamReviveHoldTime")]
    public float TeamReviveHoldTime { get; set; }

    [JsonPropertyName("selfReviveAnimationDuration")]
    public float SelfReviveAnimationDuration { get; set; }

    [JsonPropertyName("teammateReviveAnimationDuration")]
    public float TeammateReviveAnimationDuration { get; set; }

    [JsonPropertyName("consumeReviveItemOnSelfRevive")]
    public bool ConsumeReviveItemOnSelfRevive { get; set; }

    [JsonPropertyName("consumeReviveItemOnTeammateRevive")]
    public bool ConsumeReviveItemOnTeammateRevive { get; set; }

    [JsonPropertyName("criticalStateTime")]
    public float CriticalStateTime { get; set; }

    // Self Revive
    [JsonPropertyName("selfReviveRestoreBodyParts")]
    public bool SelfReviveRestoreBodyParts { get; set; }

    [JsonPropertyName("selfReviveHeadPct")]
    public float SelfReviveHeadPct { get; set; }

    [JsonPropertyName("selfReviveChestPct")]
    public float SelfReviveChestPct { get; set; }

    [JsonPropertyName("selfReviveStomachPct")]
    public float SelfReviveStomachPct { get; set; }

    [JsonPropertyName("selfReviveArmsPct")]
    public float SelfReviveArmsPct { get; set; }

    [JsonPropertyName("selfReviveLegsPct")]
    public float SelfReviveLegsPct { get; set; }

    [JsonPropertyName("selfReviveRemoveBleeds")]
    public bool SelfReviveRemoveBleeds { get; set; }

    [JsonPropertyName("selfReviveRemoveFractures")]
    public bool SelfReviveRemoveFractures { get; set; }

    [JsonPropertyName("selfReviveInvulnDuration")]
    public float SelfReviveInvulnDuration { get; set; }

    [JsonPropertyName("selfReviveInvulnSpeedPct")]
    public float SelfReviveInvulnSpeedPct { get; set; }

    [JsonPropertyName("selfReviveCooldown")]
    public float SelfReviveCooldown { get; set; }

    [JsonPropertyName("selfReviveContusionOnRevive")]
    public bool SelfReviveContusionOnRevive { get; set; }

    [JsonPropertyName("selfReviveContusionDuration")]
    public float SelfReviveContusionDuration { get; set; }

    [JsonPropertyName("selfRevivePainOnRevive")]
    public bool SelfRevivePainOnRevive { get; set; }

    // Team Revive
    [JsonPropertyName("teamReviveRestoreBodyParts")]
    public bool TeamReviveRestoreBodyParts { get; set; }

    [JsonPropertyName("teamReviveHeadPct")]
    public float TeamReviveHeadPct { get; set; }

    [JsonPropertyName("teamReviveChestPct")]
    public float TeamReviveChestPct { get; set; }

    [JsonPropertyName("teamReviveStomachPct")]
    public float TeamReviveStomachPct { get; set; }

    [JsonPropertyName("teamReviveArmsPct")]
    public float TeamReviveArmsPct { get; set; }

    [JsonPropertyName("teamReviveLegsPct")]
    public float TeamReviveLegsPct { get; set; }

    [JsonPropertyName("teamReviveRemoveBleeds")]
    public bool TeamReviveRemoveBleeds { get; set; }

    [JsonPropertyName("teamReviveRemoveFractures")]
    public bool TeamReviveRemoveFractures { get; set; }

    [JsonPropertyName("teamReviveInvulnDuration")]
    public float TeamReviveInvulnDuration { get; set; }

    [JsonPropertyName("teamReviveInvulnSpeedPct")]
    public float TeamReviveInvulnSpeedPct { get; set; }

    [JsonPropertyName("teamReviveCooldown")]
    public float TeamReviveCooldown { get; set; }

    [JsonPropertyName("teamReviveContusionOnRevive")]
    public bool TeamReviveContusionOnRevive { get; set; }

    [JsonPropertyName("teamReviveContusionDuration")]
    public float TeamReviveContusionDuration { get; set; }

    [JsonPropertyName("teamRevivePainOnRevive")]
    public bool TeamRevivePainOnRevive { get; set; }

    // Effects
    [JsonPropertyName("contusionEffect")]
    public bool ContusionEffect { get; set; }

    [JsonPropertyName("stunEffect")]
    public bool StunEffect { get; set; }

    [JsonPropertyName("medicalRange")]
    public float MedicalRange { get; set; }

    [JsonPropertyName("downedMovementSpeed")]
    public float DownedMovementSpeed { get; set; }

    [JsonPropertyName("blockUiWhenDowned")]
    public bool BlockUiWhenDowned { get; set; }

    // Hardcore
    [JsonPropertyName("deathBlockInCritical")]
    public bool DeathBlockInCritical { get; set; }

    [JsonPropertyName("godMode")]
    public bool GodMode { get; set; }

    [JsonPropertyName("ghostMode")]
    public bool GhostMode { get; set; }

    [JsonPropertyName("hardcoreMode")]
    public bool HardcoreMode { get; set; }

    [JsonPropertyName("hardcoreHeadshotDefaultDead")]
    public bool HardcoreHeadshotDefaultDead { get; set; }

    [JsonPropertyName("hardcoreChanceOfCriticalState")]
    public float HardcoreChanceOfCriticalState { get; set; }

    // Team Healing
    [JsonPropertyName("teamHealHoldTime")]
    public float TeamHealHoldTime { get; set; }

    [JsonPropertyName("teamHealMinHpResource")]
    public float TeamHealMinHpResource { get; set; }

    [JsonPropertyName("teamHealNutritionMinDeficit")]
    public float TeamHealNutritionMinDeficit { get; set; }

    // Development
    [JsonPropertyName("noReviveItemRequired")]
    public bool NoReviveItemRequired { get; set; }

    [JsonPropertyName("enableDebugLogs")]
    public bool EnableDebugLogs { get; set; }

    [JsonPropertyName("debugReviveFlow")]
    public bool DebugReviveFlow { get; set; }

    [JsonPropertyName("debugNetworkTrace")]
    public bool DebugNetworkTrace { get; set; }

    [JsonPropertyName("debugSelfReviveTrace")]
    public bool DebugSelfReviveTrace { get; set; }

    [JsonPropertyName("debugKeybinds")]
    public bool DebugKeybinds { get; set; }

    [JsonPropertyName("freeTeamHealing")]
    public bool FreeTeamHealing { get; set; }
}