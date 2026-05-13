using Content.Shared.RI.Character;
using Content.Shared.RI.Stats;
using Robust.Shared.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.RI.Character;

public sealed class RiCharacterSpawnHandoffEvent : EntityEventArgs
{
    public ICommonSession Session { get; }
    public RiCharacterCreationSelectionDto Selection { get; }
    public RiStatBlock FinalStats { get; }

    public EntityUid? SpawnedPawn;
    public string? FailureReason;

    public bool Handled => SpawnedPawn != null;

    public RiCharacterSpawnHandoffEvent(
        ICommonSession session,
        RiCharacterCreationSelectionDto selection,
        RiStatBlock finalStats)
    {
        Session = session;
        Selection = selection;
        FinalStats = finalStats;
    }
}