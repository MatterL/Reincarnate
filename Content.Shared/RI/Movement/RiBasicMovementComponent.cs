using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared.RI.Movement;

/// <summary>
/// Temporary Phase 07 movement data.
/// Server owns the movement result; the client only sends input intent.
/// </summary>
[RegisterComponent]
public sealed partial class RiBasicMovementComponent : Component
{
    [DataField("walkSpeed")] [ViewVariables(VVAccess.ReadWrite)]
    public float WalkSpeed = 4.0f;

    /// <summary>
    /// Server-side cached input vector from the owning session.
    /// Do not save this.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)] public Vector2 CurrentInput = Vector2.Zero;
}