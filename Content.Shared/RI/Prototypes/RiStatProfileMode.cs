using Robust.Shared.Serialization;

namespace Content.Shared.RI.Prototypes;

/// <summary>
/// How the class relationship should be interpreted for audit and UI.
/// Calculation itself uses the class's resolved StatTemplate directly.
/// </summary>
[Serializable, NetSerializable]
public enum RiStatProfileMode : byte
{
    Base = 0,
    Replacement = 1,
    AdditiveOverlay = 2,
}