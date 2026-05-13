# ADR 0002: Content.Shared / Content.Server / Content.Client boundaries

## Status
Accepted

## Context
Reincarnate is a server-authoritative RobustToolbox rewrite of RoleplayRebirth. The original BYOND code mixes player input, UI prompts, world mutation, savefile access, combat rules, admin actions, and presentation inside the same runtime model. RobustToolbox gives us separate shared, server, and client projects. If those boundaries are not enforced early, later systems will recreate the same coupling that made the DM source hard to reason about.

## Decision
Use the following boundary rules for all Reincarnate code:

### Content.Shared
`Content.Shared.RI` owns code that must be visible to both server and client:

- replicated/networked components;
- shared enum and ID definitions;
- network event DTOs;
- prototype classes;
- pure deterministic formulas;
- prediction-safe validation helpers;
- shared events that describe gameplay intent or state transitions.

Shared code must not reference `Content.Server` or `Content.Client`. Shared code must not read or write long-term saves. Shared code must not open UI windows or play client-only effects directly.

### Content.Server
`Content.Server.RI` owns final authority:

- persistence and save/load;
- account/character ownership;
- admin/moderation authority;
- damage application;
- resource spending;
- progression awards;
- inventory ownership;
- transformation unlocks;
- skill unlocks;
- economy;
- anti-cheat checks;
- rate limits;
- final validation of all client requests.

The server may use shared formulas and shared DTOs, but it must not trust client-provided outcomes.

### Content.Client
`Content.Client.RI` owns presentation and input:

- UI windows;
- HUDs;
- local input collection;
- cosmetic effects;
- sound and visual feedback;
- prediction visuals that can be corrected by server state.

The client may request actions. It does not decide permanent outcomes.

## Consequences
- Every client-to-server request needs server validation.
- Most gameplay state that clients need to see should start as a shared networked component.
- Saves and moderation logs stay server-only.
- UI code may display predicted or replicated state, but it must not be the source of truth.
- When unsure, put data contracts in shared code, authority in server code, and presentation in client code.