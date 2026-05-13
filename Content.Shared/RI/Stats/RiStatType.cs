using Robust.Shared.Serialization;

namespace Content.Shared.RI.Stats;

/// <summary>
/// Canonical Reincarnate stat identifiers.
///
/// EnergyCapacity is the modern name for legacy EnergyMod. It describes
/// creation affinity / capacity scaling, not current depletable Energy.
/// Anger means creation AngerMax affinity; active combat anger belongs to a
/// later runtime modifier/status system.
/// </summary>
[Serializable, NetSerializable]
public enum RiStatType : byte
{
    BasePower = 0,

    EnergyCapacity = 1,
    Strength = 2,
    Endurance = 3,
    Force = 4,
    Resistance = 5,
    Speed = 6,
    Offense = 7,
    Defense = 8,
    Regeneration = 9,
    Recovery = 10,
    Anger = 11,

    Intelligence = 12,
    Enchantment = 13,
    TrainingRate = 14,
    MeditationRate = 15,
}