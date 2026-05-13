using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Stats;

/// <summary>
/// Generic modifier record for future equipment, status effects, transformations,
/// wounds, admin tools, and scripted effects.
///
/// Phase 08 uses this mostly as a data shape; Phase 16 should build stack
/// lifecycle management on top of it.
/// </summary>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class RiStatModifier
{
    [DataField]
    public string Source = string.Empty;

    [DataField]
    public Dictionary<RiStatType, float> PermanentAddBonuses = new();

    [DataField]
    public Dictionary<RiStatType, float> TemporaryAddBonuses = new();

    [DataField]
    public Dictionary<RiStatType, float> TemporaryMultipliers = new();
}