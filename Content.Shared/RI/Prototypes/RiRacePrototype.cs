using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Prototypes;

[Prototype("riRace")]
public sealed partial class RiRacePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string DisplayName = string.Empty;

    [DataField]
    public string SourceRaceId = string.Empty;

    [DataField]
    public string DefaultSpawnPlanet = "Terra";

    [DataField(required: true)]
    public ProtoId<RiClassPrototype> DefaultClass;

    [DataField(required: true)]
    public List<ProtoId<RiClassPrototype>> AllowedClasses = new();

    [DataField]
    public string BodyTypePolicy = "StandardHumanoid";

    [DataField(required: true)]
    public ProtoId<RiStatTemplatePrototype> StatTemplate;

    [DataField]
    public string? ProgressionProfile;

    [DataField]
    public int StartingStatPoints;

    [DataField]
    public string? UnlockRequirement;

    [DataField]
    public string LegalStatus = "NotAssessed";

    [DataField]
    public HashSet<string> Tags = new();

    [DataField]
    public RiPrototypeAudit Audit = new();
}