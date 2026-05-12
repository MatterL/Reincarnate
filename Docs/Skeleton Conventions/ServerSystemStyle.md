using Content.Shared.RI.Stats;

namespace Content.Server.RI.Stats;

public sealed class RprStatsSystem : SharedRprStatsSystem
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public void SetBasePower(EntityUid uid, float value, RprStatsComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.BasePower = MathF.Max(0f, value);
        Dirty(uid, component);
    }
}