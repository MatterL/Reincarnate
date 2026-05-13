using Content.Shared.RI.Movement;
using Robust.Client.Input;
using Robust.Shared.GameObjects;
using Robust.Shared.Input.Binding;
using Robust.Shared.Maths;

namespace Content.Client.RI.Input;

/// <summary>
/// Phase 07 client input bridge.
/// Reads the existing engine MoveUp/MoveDown/MoveLeft/MoveRight binds and sends
/// intent to the server.
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

        // Send immediately on changes, and also resend at a low rate while held.
        if (direction.EqualsApprox(_lastDirection) && _sendAccumulator < SendInterval)
            return;

        _lastDirection = direction;
        _sendAccumulator = 0.0f;

        RaiseNetworkEvent(new RiMoveInputEvent(direction));
    }

    private Vector2 GetMovementDirection()
    {
        var direction = Vector2.Zero;

        if (_input.CmdStates.GetState(EngineKeyFunctions.MoveUp) == BoundKeyState.Down)
            direction.Y += 1;

        if (_input.CmdStates.GetState(EngineKeyFunctions.MoveDown) == BoundKeyState.Down)
            direction.Y -= 1;

        if (_input.CmdStates.GetState(EngineKeyFunctions.MoveRight) == BoundKeyState.Down)
            direction.X += 1;

        if (_input.CmdStates.GetState(EngineKeyFunctions.MoveLeft) == BoundKeyState.Down)
            direction.X -= 1;

        if (direction.LengthSquared() > 1.0f)
            direction = direction.Normalized();

        return direction;
    }
}