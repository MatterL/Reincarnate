using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Prototypes;

[Prototype("riClass")]
public sealed partial class RiClassPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string DisplayName = string.Empty;

    [DataField]
    public string SourceClassName = string.Empty;

    [DataField(required: true)]
    public List<ProtoId<RiRacePrototype>> AllowedRaceIds = new();

    [DataField]
    public RiStatProfileMode StatProfileMode = RiStatProfileMode.Base;

    [DataField(required: true)]
    public ProtoId<RiStatTemplatePrototype> StatTemplate;

    [DataField]
    public string? ProgressionOverlay;

    [DataField]
    public List<string> SkillGrantIds = new();

    [DataField]
    public List<string> TransformationPathIds = new();

    [DataField]
    public string? UnlockRequirement;

    [DataField]
    public string LegalStatus = "NotAssessed";

    [DataField]
    public HashSet<string> Tags = new();

    [DataField]
    public RiPrototypeAudit Audit = new();
}