using Robust.Shared.Serialization;

namespace Content.Shared.RI.Common;

/// <summary>
/// Runtime resources that can be spent, damaged, regenerated, drained, or displayed.
/// </summary>
[Serializable, NetSerializable]
public enum RprResourceType : byte
{
    Health = 0,
    Energy = 1,
    Oxygen = 2,

    // Reserved for later if RPR mechanics need them.
    Stamina = 10,
    Ki = 11,
    Mana = 12,
}