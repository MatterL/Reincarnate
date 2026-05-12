using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.RI.Stats;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RprStatsComponent : Component
{
    /// <summary>
    /// Temporary placeholder. Phase 05 should replace this with a proper stat container.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float BasePower = 1f;
}