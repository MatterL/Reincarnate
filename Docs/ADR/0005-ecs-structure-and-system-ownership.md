# ADR 0005: ECS structure and system ownership

## Status
Accepted

## Context
RoleplayRebirth's DM source often combines data, behavior, UI prompts, timers, world scans, and persistence in the same object/proc flow. RobustToolbox uses an ECS architecture where components hold data and systems own behavior. Reincarnate needs a clear rule so later ports do not create giant monolithic systems that recreate BYOND-style coupling.

## Decision
Use the following ECS conventions:

- Components are mostly data.
- Systems own behavior.
- Events describe things that happened or are being requested.
- Prototypes hold tunable content.
- Server systems own final authority.
- Shared systems may implement deterministic, prediction-safe behavior.
- Client systems own UI/presentation and local feedback.
- Long-running BYOND loops become systems, timers, accumulators, events, or scheduled updates.

## Component rules
Components should:

- be small and domain-specific;
- expose serialized/networked fields where appropriate;
- avoid complex behavior methods;
- avoid direct service lookups;
- avoid persistence code;
- avoid client-only rendering logic.

## System rules
Systems should:

- subscribe to events;
- own queries and state changes;
- validate entity state before applying changes;
- avoid broad world scans unless explicitly budgeted;
- separate server authority from client presentation;
- keep per-frame allocations low.

## Domain grouping
Group code by gameplay domain first:

Good:

```text
Content.Shared/RI/Combat/RprCombatComponent.cs
Content.Server/RI/Combat/RprCombatSystem.cs
Content.Client/RI/Combat/RprCombatEffectsSystem.cs
```

Bad:

```text
Content.Shared/RI/Components/CombatThing.cs
Content.Server/RI/Misc/RprStuff.cs
Content.Client/RI/Utils/CombatHelper.cs
```

The `Components` folder is reserved for truly cross-domain components. If a component clearly belongs to Stats, Combat, Vitals, Chat, Character, Items, Movement, or another domain, put it in that domain folder.

## Consequences
- Later phases have obvious homes for new files.
- DM source can be translated into gameplay concepts instead of proc-by-proc ports.
- Systems remain testable and replaceable.
- The codebase remains navigable as mechanics expand.