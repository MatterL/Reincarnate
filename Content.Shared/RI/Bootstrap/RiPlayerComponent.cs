using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared.RI.Bootstrap;

/// <summary>
/// Marker for the temporary Phase 07 player pawn.
/// Later phases should replace this with real identity/stat components
/// </summary>
[RegisterComponent]
public sealed partial class RiPlayerComponent : Component
{
    [DataField("debugName")] [ViewVariables(VVAccess.ReadWrite)]
    public string DebugName = "RI Test Player";
}