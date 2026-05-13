# Phase 07 — Minimum Playable Surface

## Goal

Prove that Reincarnate has a visible, controllable, networked world surface before
character creation, stats, combat, persistence, or real maps are built.

## Implemented in this phase

- Programmatic test map created on server.
- Placeholder player pawn prototype: `RiPlayerPawn`.
- Placeholder visible debug entities:
    - `RiDebugSpawnMarker`
    - `RiDebugFloorMarker`
    - `RiDebugWallMarker`
- Server spawn system attaches each in-game session to a pawn.
- Client sends movement input intent.
- Server applies movement to the attached pawn.
- Camera is handled by attaching the session to a pawn with an `Eye` component.

## Not implemented

- Real map resources.
- Real sprites.
- Physics collision.
- Movement prediction.
- Character creation.
- Stats.
- Persistence.
- Combat.
- Spawn persistence IDs.

## Manual verification

1. `dotnet build`
2. Start server.
3. Start client.
4. Connect locally.
5. Confirm the server logs:
    - `RI Phase 07 player spawn system initialized.`
    - `Created RI Phase 07 test map`
    - `Spawned RiPlayerPawn`
6. Confirm the client displays placeholder sprites.
7. Confirm the camera frames the pawn.
8. Press W/A/S/D.
9. Confirm the pawn visibly moves.
10. Disconnect/reconnect and confirm a pawn spawns again.

## Future replacement path

- Phase 08: add stats/prototypes.
- Phase 09: replace automatic test pawn with character creation spawn.
- Phase 10: save/load spawn map and position.
- Phase 18: replace programmatic test map with real map/planet/spawn prototypes.