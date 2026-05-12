using Content.Shared.RI.Sandbox;
using Robust.Shared.Network;

namespace Content.Server.RI.Sandbox;

/// <summary>
/// Server-authoritative training dummy logic.
///
/// Clients may request a hit, but this system decides whether the
/// target is real and whether the counter changes.
/// </summary>
public sealed class TrainingDummySystem : SharedTrainingDummySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<TrainingDummyHitRequestEvent>(OnHitRequest);
    }

    private void OnHitRequest(TrainingDummyHitRequestEvent message, EntitySessionEventArgs args)
    {
        // Convert network entity ID back to server entity.
        if (!TryGetEntity(message.Dummy, out var dummy) || dummy == null)
            return;
        
        // Validate the requested entity is actually a training dummy.
        if (!TryComp<TrainingDummyComponent>(dummy.Value, out var dummyComp))
            return;
        
        // In this Phase 02 spike, we intentionally do not require a player pawn yet.
        // In Phase 08+ you should also validate:
        // - the sender has an attached player entity;
        // - the player is close enough;
        // - the player is not KO/frozen/stunned;
        // - the action is rate-limited.
        if (!TryAddHit(dummy.Value, dummyComp, out var hits))
            return;

        var feedback = new TrainingDummyHitFeedbackEvent(
            GetNetEntity(dummy.Value),
            hits,
            FormatHits(hits));
        
        // For the learning spike, broadcasting is fine because the replicated
        // component is visible to clients anyway. Later, prefer targeted feedback
        // to only the interacting player where appropriate.
        RaiseNetworkEvent(feedback);
    }
}