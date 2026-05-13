# ADR 0004: Explicit persistence instead of entity serialization

## Status
Accepted

## Context
BYOND `savefile` workflows encourage saving large object state directly. That approach does not map cleanly to RobustToolbox ECS entities. Live entities contain runtime-only identifiers, transient components, map-local state, and implementation details that should not become permanent character data.

Reincarnate needs long-lived characters, progression, inventory, unlocks, and world state. Those saves must survive code refactors and schema migrations.

## Decision
Persistence will use explicit server-side save records instead of serializing live entities directly.

Save records live under `Content.Server/RI/Persistence` and are separate from ECS components. ECS entities are constructed from save records during load and converted back into save records during save.

Do not treat `EntityUid`, map-local runtime IDs, UI state, temporary cooldowns, or predicted client values as permanent save IDs.

## Early save rules
- Every save record has a schema version.
- Every save record uses stable string or GUID-style IDs for accounts/characters.
- Prototype references use stable prototype IDs.
- Server owns save/load.
- Client may display save summaries but cannot decide saved contents.
- Migration code is explicit and versioned.

## Consequences
- Save data remains readable and migratable.
- Component layout can change without corrupting saves.
- Runtime-only ECS state is not confused with permanent character identity.
- Phase 07 will implement the first real save DTOs and repository interfaces using this decision.