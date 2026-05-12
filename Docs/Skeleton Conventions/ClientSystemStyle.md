using Content.Shared.RI.Stats;

namespace Content.Client.RI.Stats;

public sealed class RprStatsSystem : SharedRprStatsSystem
{
    public override void Initialize()
    {
        base.Initialize();
    }

    // Client-side presentation helpers only.
    // No permanent stat authority belongs here.
}