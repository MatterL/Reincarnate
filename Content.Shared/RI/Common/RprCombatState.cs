using Robust.Shared.Serialization;

namespace Content.Shared.RI.Common;

/// <summary>
/// Coarse combat/vulnerability state. Do not overload this with every possible buff/debuff.
/// Temporary effects should become status components later.
/// </summary>
[Serializable, NetSerializable]
public enum RprCombatState : byte
{
    Peaceful = 0,
    Ready = 1,
    InCombat = 2,
    KnockedOut = 3,
    Dead = 4,
}