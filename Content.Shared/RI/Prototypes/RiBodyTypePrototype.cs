using Content.Shared.RI.Stats;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Prototypes;

[Prototype("riBodyType")]
public sealed partial class RiBodyTypePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string DisplayName = string.Empty;

    /// <summary>
    /// Explicit body multipliers. Empty means neutral body.
    /// </summary>
    [DataField]
    public Dictionary<RiStatType, float> StatAffinityMultipliers = new();

    [DataField]
    public HashSet<string> ExcludedRaceTags = new();

    [DataField]
    public string LegalStatus = "NotAssessed";

    [DataField]
    public RiPrototypeAudit Audit = new();
}