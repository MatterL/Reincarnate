using Robust.Shared.Serialization;

namespace Content.Shared.RI.Common;

/// <summary>
/// Stable stat identifiers used by formulas, prototypes, save data, and UI.
/// Do not rename or renumber enum values after persistence depends on them.
/// </summary>
[Serializable, NetSerializable]
public enum RprStatType : byte
{
    BasePower = 0,

    Strength = 1,
    Endurance = 2,
    Force = 3,
    Resistance = 4,
    Speed = 5,
    Offense = 6,
    Defense = 7,

    Regeneration = 20,
    Recovery = 21,
    Anger = 22,

    Intelligence = 40,
    TrainingRate = 41,
    MeditationRate = 42,
    GrowthRate = 43,
    EnchantmentRate = 44,
}