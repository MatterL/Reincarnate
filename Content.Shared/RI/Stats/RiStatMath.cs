namespace Content.Shared.RI.Stats;

/// <summary>
/// Pure deterministic stat math. This class intentionally has no ECS, UI,
/// persistence, or prototype-manager dependency so tests and balance tools can
/// call it directly.
/// </summary>
public static class RiStatMath
{
    public static float GetFinalValue(RiStatLine line)
    {
        var basePermanent = line.BaseValue + line.EarnedBonus + line.PermanentAddBonus;
        var creationAdjusted = basePermanent * line.CreationMultiplier;
        return (creationAdjusted + line.TemporaryAddBonus) * line.TemporaryMultiplier;
    }

    public static RiStatLine BuildLine(
        RiStatType stat,
        IReadOnlyDictionary<RiStatType, float>? baseValues = null,
        IReadOnlyDictionary<RiStatType, float>? earnedBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? creationMultipliers = null,
        IReadOnlyDictionary<RiStatType, float>? permanentAddBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? temporaryAddBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? temporaryMultipliers = null)
    {
        return new RiStatLine(
            baseValue: GetOrDefault(baseValues, stat, RiStatConstants.DefaultBaseValue(stat)),
            earnedBonus: GetOrDefault(earnedBonuses, stat, 0f),
            creationMultiplier: GetOrDefault(creationMultipliers, stat, 1f),
            permanentAddBonus: GetOrDefault(permanentAddBonuses, stat, 0f),
            temporaryAddBonus: GetOrDefault(temporaryAddBonuses, stat, 0f),
            temporaryMultiplier: GetOrDefault(temporaryMultipliers, stat, 1f));
    }

    public static RiStatBlock BuildBlock(
        IReadOnlyDictionary<RiStatType, float>? baseValues = null,
        IReadOnlyDictionary<RiStatType, float>? earnedBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? creationMultipliers = null,
        IReadOnlyDictionary<RiStatType, float>? permanentAddBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? temporaryAddBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? temporaryMultipliers = null)
    {
        var block = new RiStatBlock();

        foreach (var stat in RiStatConstants.AllStats)
        {
            block.Lines[stat] = BuildLine(
                stat,
                baseValues,
                earnedBonuses,
                creationMultipliers,
                permanentAddBonuses,
                temporaryAddBonuses,
                temporaryMultipliers);
        }

        return block;
    }

    public static Dictionary<RiStatType, float> CloneStatMap(IReadOnlyDictionary<RiStatType, float>? source)
    {
        var result = new Dictionary<RiStatType, float>();

        if (source == null)
            return result;

        foreach (var (stat, value) in source)
            result[stat] = value;

        return result;
    }

    public static float GetOrDefault(
        IReadOnlyDictionary<RiStatType, float>? values,
        RiStatType stat,
        float fallback)
    {
        return values != null && values.TryGetValue(stat, out var value)
            ? value
            : fallback;
    }
}