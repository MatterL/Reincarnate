using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.RI.Sandbox;

/// <summary>
/// Client-to-server request.
///
/// This is untrusted. The server must validate the target and decide
/// whether the hit actually counts.
/// </summary>
[Serializable, NetSerializable]
public sealed class TrainingDummyHitRequestEvent : EntityEventArgs
{
    public NetEntity Dummy { get; }

    public TrainingDummyHitRequestEvent(NetEntity dummy)
    {
        Dummy = dummy;
    }
}

/// <summary>
/// Server-to-client feedback.
///
/// This is just presentation. The real source of truth is still the
/// replicated TrainingDummyComponent.Hits field.
/// </summary>
[Serializable, NetSerializable]
public sealed class TrainingDummyHitFeedbackEvent : EntityEventArgs
{
    public NetEntity Dummy { get; }
    public int Hits { get; }
    public string Message { get; }

    public TrainingDummyHitFeedbackEvent(NetEntity dummy, int hits, string message)
    {
        Dummy = dummy;
        Hits = hits;
        Message = message;
    }
}