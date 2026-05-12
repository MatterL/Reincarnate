using Robust.Shared.Serialization;

namespace Content.Shared.RI.Common;

/// <summary>
/// Tracks whether a formula/data value was preserved from DM, tuned, rewritten, deleted, or still unknown.
/// Used in extracted design docs and prototype metadata.
/// </summary>
[Serializable, NetSerializable]
public enum RprFormulaAuditStatus : byte
{
    Unknown = 0,
    Preserve = 1,
    Tune = 2,
    Rewrite = 3,
    Delete = 4,
    LegalReview = 5,
    NeedsBalanceSim = 6,
    NeedsNetworkPrototype = 7,
}