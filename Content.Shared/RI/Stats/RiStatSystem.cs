using Content.Shared.RI.Prototypes;

namespace Content.Shared.RI.Stats;

/// <summary>
/// Shared deterministic stat construction service.
///
/// This intentionally accepts resolved prototypes instead of reaching into the
/// prototype manager itself. That keeps tests simple and prevents UI/server code
/// from forking the formula.
/// </summary>
public static class RiStatSystem
{
    public static RiStatChoiceValidationResult ValidateChoices(
        RiRacePrototype race,
        RiClassPrototype riClass,
        RiBodyTypePrototype bodyType)
    {
        if (!race.AllowedClasses.Contains(riClass.ID))
        {
            return RiStatChoiceValidationResult.Fail(
                $"Class '{riClass.ID}' is not allowed for race '{race.ID}'.");
        }

        if (!riClass.AllowedRaceIds.Contains(race.ID))
        {
            return RiStatChoiceValidationResult.Fail(
                $"Race '{race.ID}' is not allowed for class '{riClass.ID}'.");
        }

        foreach (var excludedTag in bodyType.ExcludedRaceTags)
        {
            if (race.Tags.Contains(excludedTag))
            {
                return RiStatChoiceValidationResult.Fail(
                    $"Body type '{bodyType.ID}' is excluded by race tag '{excludedTag}'.");
            }
        }

        return RiStatChoiceValidationResult.Ok;
    }

    public static Dictionary<RiStatType, float> BuildCreationMultipliers(
        RiStatTemplatePrototype statTemplate,
        RiBodyTypePrototype bodyType)
    {
        var result = new Dictionary<RiStatType, float>();

        foreach (var stat in RiStatConstants.AllStats)
            result[stat] = 1f;

        foreach (var (stat, value) in statTemplate.BaseAffinities)
            result[stat] = value;

        foreach (var (stat, multiplier) in bodyType.StatAffinityMultipliers)
            result[stat] = RiStatMath.GetOrDefault(result, stat, 1f) * multiplier;

        return result;
    }

    public static RiStatBlock BuildInitialCharacterStats(
        RiStatTemplatePrototype statTemplate,
        RiBodyTypePrototype bodyType,
        IReadOnlyDictionary<RiStatType, float>? baseValues = null,
        IReadOnlyDictionary<RiStatType, float>? earnedBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? creationAllocations = null,
        IReadOnlyDictionary<RiStatType, float>? temporaryAddBonuses = null,
        IReadOnlyDictionary<RiStatType, float>? temporaryMultipliers = null)
    {
        var creationMultipliers = BuildCreationMultipliers(statTemplate, bodyType);

        // Creation allocation should be saved as a permanent additive fact,
        // not hidden inside race/body/class multipliers.
        var permanentAddBonuses = RiStatMath.CloneStatMap(creationAllocations);

        return RiStatMath.BuildBlock(
            baseValues,
            earnedBonuses,
            creationMultipliers,
            permanentAddBonuses,
            temporaryAddBonuses,
            temporaryMultipliers);
    }

    public static bool IsAllocatableCreationStat(RiStatType stat)
    {
        return RiStatConstants.AllocatableCreationStats.Contains(stat);
    }
}