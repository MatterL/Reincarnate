using Robust.Shared.Maths;
using Robust.Shared.Serialization;

namespace Content.Shared.RI.Movement;

/// <summary>
/// Client-to-server input intent for the temporary Phase 07 movement slice.
/// The server must validate ownership by using the sender's attached entity.
/// </summary>
[Serializable, NetSerializable]
public sealed class RiMoveInputEvent : EntityEventArgs
{
    public Vector2 Direction;

    public RiMoveInputEvent()
    {
    }

    public RiMoveInputEvent(Vector2 direction)
    {
        Direction = direction;
    }
}