# RI Phase 07 camera setup

The Phase 07 player pawn has an `Eye` component and the server attaches the session to
the spawned `RiPlayerPawn`.

For the minimum playable surface, the expected camera path is:

1. Server creates the Phase 07 test map.
2. Server spawns `RiPlayerPawn`.
3. Server calls `session.AttachToEntity(pawn)`.
4. The pawn has an `Eye` component.
5. The client view frames/follows the attached pawn.

Do not build a custom cinematic/follow camera yet unless this fails in the local
RobustToolbox version. Phase 07 should prove the controlled pawn and visible surface,
not create the final camera system.