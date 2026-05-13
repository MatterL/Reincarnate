using System.Linq;
using System.Numerics;
using Content.Shared.RI.Movement;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.IoC;

namespace Content.Client.RI.Input;

/// <summary>
/// Phase 07 client input bridge.
/// Sends movement intent only when the local player has a pawn and input changes.
/// </summary>
public sealed partial class RiInputSystem : EntitySystem
{
    [Dependency] private readonly IInputManager _input = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private Vector2 _lastDirection = Vector2.Zero;

    public override void FrameUpdate(float frameTime)
    {
        base.FrameUpdate(frameTime);

        if (_playerManager.LocalEntity is not { Valid: true })
            return;

        var direction = GetMovementDirection();

        if (Vector2.DistanceSquared(direction, _lastDirection) < 0.0001f)
            return;

        _lastDirection = direction;
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