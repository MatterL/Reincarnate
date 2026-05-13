#nullable enable

using System.Numerics;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Map;
using Robust.Shared.Player;

namespace Content.Server.RI.Bootstrap;

/// <summary>
/// Phase 07 bootstrap: creates a tiny visible test surface and attaches each connected
/// session to a temporary player pawn.
/// </summary>
public sealed partial class RiPlayerSpawnSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    private ISawmill _sawmill = default!;

    private MapId? _testMapId;
    private EntityUid? _testMapEntity;

    private const string PlayerPrototype = "RiPlayerPawn";
    private const string SpawnMarkerPrototype = "RiDebugSpawnMarker";
    private const string FloorMarkerPrototype = "RiDebugFloorMarker";
    private const string WallMarkerPrototype = "RiDebugWallMarker";

    public override void Initialize()
    {
        base.Initialize();

        _sawmill = _logManager.GetSawmill("ri.phase07");
        _playerManager.PlayerStatusChanged += OnPlayerStatusChanged;

        _sawmill.Info("RI Phase 07 player spawn system initialized.");
    }

    public override void Shutdown()
    {
        base.Shutdown();

        _playerManager.PlayerStatusChanged -= OnPlayerStatusChanged;
    }

    private void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs args)
    {
        // The template connection flow first reaches Connected. We spawn and then explicitly
        // move the session into InGame after attaching a pawn.
        if (args.NewStatus != SessionStatus.Connected)
            return;

        SpawnForSession(args.Session);
    }

    private void SpawnForSession(ICommonSession session)
    {
        if (session.AttachedEntity is { Valid: true } existing)
        {
            _sawmill.Info($"Session {session.Name} already attached to {existing}; joining game.");
            _playerManager.JoinGame(session);
            return;
        }

        var spawnCoordinates = EnsureTestSurface();
        var pawn = Spawn(PlayerPrototype, spawnCoordinates);

        _playerManager.SetAttachedEntity(session, pawn, force: true);
        _playerManager.JoinGame(session);

        _sawmill.Info($"Spawned {PlayerPrototype} entity {pawn} for session {session.Name} at {spawnCoordinates}.");
    }

    private EntityCoordinates EnsureTestSurface()
    {
        if (_testMapId is { } && _testMapEntity is { Valid: true } existingMapEntity)
            return new EntityCoordinates(existingMapEntity, Vector2.Zero);

        // Phase 07 temporary map bootstrap.
        // IMapManager map creation is obsolete in current Robust, but this is isolated
        // here so Phase 18 can replace it with the real map/grid loading path.
#pragma warning disable CS0618
        var mapId = _mapManager.CreateMap();
        var mapEntity = _mapManager.GetMapEntityId(mapId);
#pragma warning restore CS0618

        _testMapId = mapId;
        _testMapEntity = mapEntity;

        _sawmill.Info($"Created RI Phase 07 test map {mapId} / entity {mapEntity}.");

        for (var x = -4; x <= 4; x++)
        {
            for (var y = -4; y <= 4; y++)
            {
                var edge = x == -4 || x == 4 || y == -4 || y == 4;
                var prototype = edge ? WallMarkerPrototype : FloorMarkerPrototype;

                Spawn(prototype, new EntityCoordinates(mapEntity, new Vector2(x, y)));
            }
        }

        Spawn(SpawnMarkerPrototype, new EntityCoordinates(mapEntity, Vector2.Zero));

        return new EntityCoordinates(mapEntity, Vector2.Zero);
    }
}