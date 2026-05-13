using Content.Shared.RI.Stats;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Prototypes;

/// <summary>
/// Final race/class stat affinity template.
///
/// Phase 05 drafts used riStatProfile with nested class replacements.
/// Phase 08 normalizes that into direct templates such as Human, Human.Sage,
/// Alien.Fighter, Alien.Technologist, and Saiyan.Fighter.
/// </summary>
[Prototype("riStatTemplate")]
public sealed partial class RiStatTemplatePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public string SourceRaceId = string.Empty;

    [DataField]
    public string? SourceClassId;

    [DataField]
    public Dictionary<RiStatType, float> BaseAffinities = new();

    [DataField]
    public RiPrototypeAudit Audit = new();
}