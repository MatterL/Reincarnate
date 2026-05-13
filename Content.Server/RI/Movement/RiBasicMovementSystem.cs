using Content.Shared.RI.Movement;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Player;

namespace Content.Server.RI.Movement;

/// <summary>
/// Temporary Phase 07 movement.
/// This is intentionally simple: no physics, no prediction, no collision.
/// The goal is to prove visible server-owned movement before adding real locomotion.
/// </summary>
public sealed partial class RiBasicMovementSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<RiMoveInputEvent>(OnMoveInput);
    }

    private void OnMoveInput(RiMoveInputEvent ev, EntitySessionEventArgs args)
    {
        var session = args.SenderSession;
        var attached = session.AttachedEntity;

        if (attached is not { Valid: true } uid)
            return;

        if (!TryComp<RiBasicMovementComponent>(uid, out var movement))
            return;

        var direction = ev.Direction;

        // Reject bad input. Client sends intent, not truth.
        if (!direction.X.IsFinite() || !direction.Y.IsFinite())
        {
            movement.CurrentInput = Vector2.Zero;
            return;
        }

        if (direction.LengthSquared() > 1.0f)
            direction = direction.Normalized();

        movement.CurrentInput = direction;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<RiBasicMovementComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var movement, out var xform))
        {
            if (movement.CurrentInput.LengthSquared() <= 0.0001f)
                continue;

            var delta = movement.CurrentInput * movement.WalkSpeed * frameTime;
            var next = xform.WorldPosition + delta;

            _transform.SetWorldPosition(uid, next);
        }
    }
}