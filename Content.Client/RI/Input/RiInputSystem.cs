using System.Linq;
using System.Numerics;
using Content.Shared.RI.Movement;
using Robust.Client.Input;
using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.IoC;

namespace Content.Client.RI.Input;

/// <summary>
/// Phase 07 client input bridge.
/// Reads the existing engine MoveUp/MoveDown/MoveLeft/MoveRight binds and sends
/// movement intent to the server.
/// </summary>
public sealed partial class RiInputSystem : EntitySystem
{
    [Dependency] private readonly IInputManager _input = default!;

    private Vector2 _lastDirection = Vector2.Zero;
    private float _sendAccumulator;

    private const float SendInterval = 1.0f / 20.0f;

    public override void FrameUpdate(float frameTime)
    {
        base.FrameUpdate(frameTime);

        _sendAccumulator += frameTime;

        var direction = GetMovementDirection();
        var unchanged = Vector2.DistanceSquared(direction, _lastDirection) < 0.0001f;

        // Send immediately on changes, and also resend at a low rate while held.
        if (unchanged && _sendAccumulator < SendInterval)
            return;

        _lastDirection = direction;
        _sendAccumulator = 0.0f;

        RaiseNetworkEvent(new RiMoveInputEvent(direction));
    }

    private Vector2 GetMovementDirection()
    {
        var direction = Vector2.Zero;

        if (IsDown(EngineKeyFunctions.MoveUp))
            direction.Y += 1;

        if (IsDown(EngineKeyFunctions.MoveDown))
            direction.Y -= 1;

        if (IsDown(EngineKeyFunctions.MoveRight))
            direction.X += 1;

        if (IsDown(EngineKeyFunctions.MoveLeft))
            direction.X -= 1;

        if (direction.LengthSquared() > 1.0f)
            direction = Vector2.Normalize(direction);

        return direction;
    }

    private bool IsDown(BoundKeyFunction function)
    {
        return _input.DownKeyFunctions.Contains(function);
    }
}