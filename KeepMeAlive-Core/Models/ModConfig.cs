namespace KeepMeAlive.Models;

public class ModConfig
{
    // Revival Mechanics
    public string RevivalItemId { get; set; }
    public bool SelfRevivalEnabled { get; set; }
    public bool TeamReviveEnabled { get; set; }
    public float SelfReviveHoldTime { get; set; }
    public float TeamReviveHoldTime { get; set; }
    public float SelfReviveAnimationDuration { get; set; }
    public float TeammateReviveAnimationDuration { get; set; }
    public bool ConsumeReviveItemOnSelfRevive { get; set; }
    public bool ConsumeReviveItemOnTeammateRevive { get; set; }
    public float CriticalStateTime { get; set; }

    // Self-Revive Post-Revival
    public bool SelfReviveRestoreBodyParts { get; set; }
    public float SelfReviveHeadPct { get; set; }
    public float SelfReviveChestPct { get; set; }
    public float SelfReviveStomachPct { get; set; }
    public float SelfReviveArmsPct { get; set; }
    public float SelfReviveLegsPct { get; set; }
    public bool SelfReviveRemoveBleeds { get; set; }
    public bool SelfReviveRemoveFractures { get; set; }
    public float SelfReviveInvulnDuration { get; set; }
    public float SelfReviveInvulnSpeedPct { get; set; }
    public float SelfReviveCooldown { get; set; }
    public bool SelfReviveContusionOnRevive { get; set; }
    public float SelfReviveContusionDuration { get; set; }
    public bool SelfRevivePainOnRevive { get; set; }

    // Team-Revive Post-Revival
    public bool TeamReviveRestoreBodyParts { get; set; }
    public float TeamReviveHeadPct { get; set; }
    public float TeamReviveChestPct { get; set; }
    public float TeamReviveStomachPct { get; set; }
    public float TeamReviveArmsPct { get; set; }
    public float TeamReviveLegsPct { get; set; }
    public bool TeamReviveRemoveBleeds { get; set; }
    public bool TeamReviveRemoveFractures { get; set; }
    public float TeamReviveInvulnDuration { get; set; }
    public float TeamReviveInvulnSpeedPct { get; set; }
    public float TeamReviveCooldown { get; set; }
    public bool TeamReviveContusionOnRevive { get; set; }
    public float TeamReviveContusionDuration { get; set; }
    public bool TeamRevivePainOnRevive { get; set; }

    // Effects & Gameplay
    public bool ContusionEffect { get; set; }
    public bool StunEffect { get; set; }
    public float MedicalRange { get; set; }
    public float DownedMovementSpeed { get; set; }
    public bool BlockUiWhenDowned { get; set; }

    // Hardcore Mode
    public bool DeathBlockInCritical { get; set; }
    public bool GodMode { get; set; }
    public bool GhostMode { get; set; }
    public bool HardcoreMode { get; set; }
    public bool HardcoreHeadshotDefaultDead { get; set; }
    public float HardcoreChanceOfCriticalState { get; set; }

    // Team Healing
    public float TeamHealHoldTime { get; set; }
    public float TeamHealMinHpResource { get; set; }
    public float TeamHealNutritionMinDeficit { get; set; }

    // Development / Debug
    public bool NoReviveItemRequired { get; set; }
    public bool EnableDebugLogs { get; set; }
    public bool DebugReviveFlow { get; set; }
    public bool DebugNetworkTrace { get; set; }
    public bool DebugSelfReviveTrace { get; set; }
    public bool DebugKeybinds { get; set; }
    public bool FreeTeamHealing { get; set; }
}