using Robust.Client.Player;
using Robust.Client.State;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Client.RI.Bootstrap;

/// <summary>
/// Switches away from the built-in connection screen once the local player has a pawn.
/// </summary>
public sealed partial class RiClientGameplayStateSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IStateManager _stateManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        _playerManager.LocalPlayerAttached += OnLocalPlayerAttached;

        if (_playerManager.LocalEntity is { Valid: true })
            _stateManager.RequestStateChange<RiGameplayState>();
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _playerManager.LocalPlayerAttached -= OnLocalPlayerAttached;
    }

    private void OnLocalPlayerAttached(EntityUid uid)
    {
        _stateManager.RequestStateChange<RiGameplayState>();
    }
}