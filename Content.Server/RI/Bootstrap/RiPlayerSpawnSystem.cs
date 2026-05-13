using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Player;

namespace Content.Server.RI.Bootstrap;

/// <summary>
/// Phase 07 bootstrap: creates a tiny visible test surface and attaches each in-game session
/// to a temporary player spawn.
/// </summary>
public sealed partial class RiPlayerSpawnSystem : EntitySystem
{
    [Dependency] private readonly IMapManager _mapManager = default!;

    private MapId? _testMapId;
    private EntityUid? _testMapEntity;

    private const string PlayerPrototype = "RiPlayerPawn";
    private const string SpawnMarkerPrototype = "RiDebugSpawnMarker";
    private const string FloorMarkerPrototype = "RiDebugFloorMarker";
    private const string WallMarkerPrototype = "RiDebugWallMarker";

    public override void Initialize()
    {
        base.Initialize();
        
        // This event is available on Robust player sessions.
        // If your exact RobustToolbox submodule has a different session lifecycle enum,
        // see the compile-risk note below.
        var players = IoCManager.Resolve<IPlayerManager>();
        players.PlayerStatusChanged += OnPlayerStatusChanged;

        Logger.InfoS("ri.phase07", "RI Phase 07 player spawn system initialized.");
    }

    private void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs args)
    {
        if (args.NewStatus != SessionStatus.InGame)
            return;

        SpawnForSession(args.Session);
    }

    private void SpawnForSession(ICommonSession session)
    {
        var spawnCoordinates = EnsureTestSurface();

        if (session.AttachedEntity is { Valid: true } existing)
        {
            Logger.InfoS("ri.phase07",
                $"Session {session.Name} already attached to {existing}; skipping Phase 07 spawn.");
            return;
        }

        var pawn = Spawn(PlayerPrototype, spawnCoordinates);
        session.AttachToEntity(pawn);

        Logger.InfoS(
            "ri.phase07",
            $"Spawned {PlayerPrototype} entity {pawn} for session {session.Name} at {spawnCoordinates}.");
    }

    private EntityCoordinates EnsureTestSurface()
    {
        if (_testMapId is { } existingMap && _testMapEntity is { Valid: true } existingMapEntity)
            return new EntityCoordinates(existingMapEntity, Vector2.Zero);

        var mapId = _mapManager.CreateMap();
        var mapEntity = _mapManager.GetMapEntityId(mapId);

        _testMapId = mapId;
        _testMapEntity = mapEntity;

        Logger.InfoS("ri.phase07", $"Created RI Phase 07 test map {mapId} / entity {mapEntity}.");
        
        // Spawn a visible 9x9 carpet of placeholder sprites so the camera is not looking at emptiness.
        for (var x in -4; x <= 4; x++)
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