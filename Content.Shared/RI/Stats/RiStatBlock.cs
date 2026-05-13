using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Stats;

/// <summary>
/// Full set of stat lines for preview, runtime inspection, or save reconstruction.
/// </summary>
[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class RiStatBlock
{
    [DataField]
    public Dictionary<RiStatType, RiStatLine> Lines = new();

    public RiStatLine GetOrDefault(RiStatType stat)
    {
        return Lines.TryGetValue(stat, out var line)
            ? line
            : new RiStatLine(RiStatConstants.DefaultBaseValue(stat));
    }

    public float GetFinalValue(RiStatType stat)
    {
        return RiStatMath.GetFinalValue(GetOrDefault(stat));
    }

    public Dictionary<RiStatType, float> ToFinalValueDictionary()
    {
        var values = new Dictionary<RiStatType, float>(RiStatConstants.AllStats.Length);

        foreach (var stat in RiStatConstants.AllStats)
            values[stat] = GetFinalValue(stat);

        return values;
    }

    public RiStatBlock Clone()
    {
        var copy = new RiStatBlock();

        foreach (var (stat, line) in Lines)
            copy.Lines[stat] = line.Clone();

        return copy;
    }
}