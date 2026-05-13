using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Stats;

/// <summary>
/// Authoritative stat state attached to a character entity.
///
/// The component is not networked in Phase 08. Future UI should expose a small
/// server-built DTO instead of automatically networking every stat dictionary.
/// </summary>
[RegisterComponent]
public sealed partial class RiStatsComponent : Component
{
    [DataField]
    public Dictionary<RiStatType, float> BaseValues = new();

    [DataField]
    public Dictionary<RiStatType, float> EarnedBonuses = new();

    [DataField]
    public Dictionary<RiStatType, float> CreationMultipliers = new();

    [DataField]
    public Dictionary<RiStatType, float> PermanentAddBonuses = new();

    [DataField]
    public Dictionary<RiStatType, float> TemporaryAddBonuses = new();

    [DataField]
    public Dictionary<RiStatType, float> TemporaryMultipliers = new();

    public RiStatBlock BuildBlock()
    {
        return RiStatMath.BuildBlock(
            BaseValues,
            EarnedBonuses,
            CreationMultipliers,
            PermanentAddBonuses,
            TemporaryAddBonuses,
            TemporaryMultipliers);
    }
}