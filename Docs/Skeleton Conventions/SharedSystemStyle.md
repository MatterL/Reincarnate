namespace Content.Shared.RI.Stats;

public abstract class SharedRprStatsSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public float GetBasePower(EntityUid uid, RprStatsComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return 0f;

        return component.BasePower;
    }
}