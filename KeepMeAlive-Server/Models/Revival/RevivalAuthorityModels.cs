using KeepMeAlive.Server;

namespace KeepMeAlive.Server.Models.Revival;

//====================[ RevivalState ]====================
public enum RevivalState
{
    None = 0,
    BleedingOut = 1,
    Reviving = 2,
    Revived = 3,
    CoolDown = 4
}

//====================[ RevivalSourceKind ]====================
public enum RevivalSourceKind
{
    Self = 0,
    Team = 1
}

//====================[ RevivalStateEntry ]====================
public record RevivalStateEntry
{
    public string PlayerId { get; init; } = string.Empty;
    public RevivalState State { get; set; } = RevivalState.None;
    public RevivalSourceKind Source { get; set; } = RevivalSourceKind.Self;
    public string ReviverId { get; set; } = string.Empty;
    public long LastUpdatedUnixSeconds { get; set; }
    public long CooldownUntilUnixSeconds { get; set; }
}

//====================[ RevivalAuthorityResponse ]====================
public record RevivalAuthorityResponse
{
    public bool Success { get; init; }
    public RevivalDeniedCode DenialCode { get; init; } = RevivalDeniedCode.None;
    public string Reason { get; init; } = string.Empty;
    public RevivalStateEntry? State { get; init; }
}

//====================[ RevivalDeniedCode ]====================
public enum RevivalDeniedCode
{
    None = 0,
    Cooldown = 1,
    InvalidState = 2,
    NotDowned = 3,
    CompleteInvalidState = 4,
    ServerError = 5,
    FeatureDisabled = 6
}

//====================[ RevivalRuntimeConfigSnapshot ]====================
public record RevivalRuntimeConfigSnapshot
{
    public int SchemaVersion { get; init; }
    public string ConfigHash { get; init; } = string.Empty;
    public long GeneratedAtUnixSeconds { get; init; }
    public RevivalServerConfig Config { get; init; } = new();
}
