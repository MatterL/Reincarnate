namespace Content.Shared.RI.Stats;

public static class RiStatConstants
{
    /// <summary>
    /// Creation stats that can be affected by race/class/body and later by
    /// creation allocation UI.
    /// </summary>
    public static readonly RiStatType[] AllocatableCreationStats =
    [
        RiStatType.EnergyCapacity,
        RiStatType.Strength,
        RiStatType.Endurance,
        RiStatType.Speed,
        RiStatType.Force,
        RiStatType.Resistance,
        RiStatType.Offense,
        RiStatType.Defense,
        RiStatType.Regeneration,
        RiStatType.Recovery,
        RiStatType.Anger,
    ];

    /// <summary>
    /// All stat types tracked by the first implementation.
    /// </summary>
    public static readonly RiStatType[] AllStats =
    [
        RiStatType.BasePower,
        RiStatType.EnergyCapacity,
        RiStatType.Strength,
        RiStatType.Endurance,
        RiStatType.Force,
        RiStatType.Resistance,
        RiStatType.Speed,
        RiStatType.Offense,
        RiStatType.Defense,
        RiStatType.Regeneration,
        RiStatType.Recovery,
        RiStatType.Anger,
        RiStatType.Intelligence,
        RiStatType.Enchantment,
        RiStatType.TrainingRate,
        RiStatType.MeditationRate,
    ];

    public static float DefaultBaseValue(RiStatType stat)
    {
        return stat switch
        {
            RiStatType.BasePower => 1f,
            RiStatType.TrainingRate => 1f,
            RiStatType.MeditationRate => 1f,
            RiStatType.Intelligence => 1f,
            RiStatType.Enchantment => 1f,
            _ => 1f,
        };
    }
}