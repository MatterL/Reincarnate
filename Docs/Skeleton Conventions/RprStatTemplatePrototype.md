using Content.Shared.RI.Common;
using Robust.Shared.Prototypes;

namespace Content.Shared.RI.Prototypes;

[Prototype("rprStatTemplate")]
public sealed partial class RprStatTemplatePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public string DisplayName { get; private set; } = string.Empty;

    [DataField]
    public RprFormulaAuditStatus AuditStatus { get; private set; } = RprFormulaAuditStatus.Unknown;
}

stat_templates.yml
- type: rprStatTemplate
  id: BootstrapOnly
  displayName: Bootstrap Only
  auditStatus: Unknown