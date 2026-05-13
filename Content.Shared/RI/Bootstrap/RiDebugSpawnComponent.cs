using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared.RI.Bootstrap;

/// <summary>
/// Marker for a temporary debug spawn location
/// This is not persistence-safe; Phase 18 should replace this with stable spawn IDs.
/// </summary>
[RegisterComponent]
public sealed partial class RiDebugSpawnComponent : Component
{
    [DataField("spawnId")] [ViewVariables(VVAccess.ReadWrite)]
    public string SpawnId = "ri-test-spawn";
}