using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Prototypes;

/// <summary>
/// Lightweight source/audit metadata carried by Ri prototypes.
/// This data is for designers and extraction traceability, not gameplay logic.
/// </summary>
[DataDefinition]
public sealed partial class RiPrototypeAudit
{
    [DataField]
    public string Status = "Unreviewed";

    [DataField]
    public List<string> SourcePaths = new();

    [DataField]
    public string Notes = string.Empty;
}