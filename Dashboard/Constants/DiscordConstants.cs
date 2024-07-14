namespace Dashboard.Constants;

public static class DiscordConstants
{
    public const string DisplayNameClaim = "wax:discord:user:displayname";
    public const string BannerColor = "wax:discord:user:banner_color";
    public const string Banner  = "wax:discord:user:banner";
    public const string Flags = "wax:discord:user:flags";
}

[Flags]
public enum DiscordFlags
{
    None = 0,
    Staff = 1 << 0,
    Partner = 1 << 1,
    Hypesquad = 1 << 2,
    BugHunterLevel1 = 1 << 3,
    HypesquadBravery = 1 << 6,
    HypesquadBrilliance = 1 << 7,
    HypesquadBalance = 1 << 8,
    PremiumEarlySupporter = 1 << 9,
    TeamPseudoUser = 1 << 10,
    BugHunterLevel2 = 1 << 14,
    VerifiedBot = 1 << 16,
    VerifiedDeveloper = 1 << 17,
    CertifiedModerator = 1 << 18,
    BotHttpInteractions = 1 << 19,
    ActiveDeveloper = 1 << 22,
}