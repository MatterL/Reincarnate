using Robust.Shared.Serialization;

namespace Content.Shared.RI.Common;

/// <summary>
/// High-level damage categories. Keep this small until combat extraction proves more are needed.
/// </summary>
[Serializable, NetSerializable]
public enum RprDamageType : byte
{
    Physical = 0,
    Energy = 1,
    Environmental = 2,
    Suffocation = 3,
    True = 4,
}