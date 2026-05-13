using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Stats;

/// <summary>
/// One deterministic stat line. Do not collapse these fields into one number:
/// permanent values, creation multipliers, and temporary modifiers need to
/// remain inspectable and testable.
/// </summary>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class RiStatLine
{
    [DataField]
    public float BaseValue = 1f;

    [DataField]
    public float EarnedBonus = 0f;

    [DataField]
    public float CreationMultiplier = 1f;

    [DataField]
    public float PermanentAddBonus = 0f;

    [DataField]
    public float TemporaryAddBonus = 0f;

    [DataField]
    public float TemporaryMultiplier = 1f;

    public RiStatLine()
    {
    }

    public RiStatLine(
        float baseValue,
        float earnedBonus = 0f,
        float creationMultiplier = 1f,
        float permanentAddBonus = 0f,
        float temporaryAddBonus = 0f,
        float temporaryMultiplier = 1f)
    {
        BaseValue = baseValue;
        EarnedBonus = earnedBonus;
        CreationMultiplier = creationMultiplier;
        PermanentAddBonus = permanentAddBonus;
        TemporaryAddBonus = temporaryAddBonus;
        TemporaryMultiplier = temporaryMultiplier;
    }

    public RiStatLine Clone()
    {
        return new RiStatLine(
            BaseValue,
            EarnedBonus,
            CreationMultiplier,
            PermanentAddBonus,
            TemporaryAddBonus,
            TemporaryMultiplier);
    }
}