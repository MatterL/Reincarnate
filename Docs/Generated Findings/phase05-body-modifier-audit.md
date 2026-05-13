# Phase 05 Body Modifier Audit

Source: `Code/statpointsystems.dm:ClassBodyStats()`.

Resolution for Phase 05:

- Body type behavior is **not** pulled from `Code/Races/*.dm`; it remains legacy active logic in `ClassBodyStats()`.
- The first Phase 08 implementation should use explicit `RiBodyTypePrototype` values instead of preserving hidden duplicate `if BodyType` blocks.
- The raw active-code combined values below are kept as audit evidence.
- The draft YAML seed uses a one-pass, readable body profile and marks the numbers `Tune` until balance testing.

## Raw active-code body blocks

| Body | Condition | Multipliers |
|---|---|---|
| Small block A | `BodyType == Small` | Strength x0.5; Endurance x0.5; Speed x1.4; Resistance x0.5; Force x1.25; Offense x1.4; Defense x1.4; Regeneration x1.25; Recovery x1.25 |
| Small block B | `BodyType == Small && Race != Changeling` | Strength x1; Endurance x1; Speed x1.25; Resistance x0.5; Force x1.25; Offense x1.5; Defense x1.5 |
| Large block A | `BodyType == Large && Race != Changeling` | Strength x1; Endurance x1.5; Speed x0.7; Resistance x1.5; Force x0.7; Offense x0.7; Defense x0.7 |
| Large block B | `BodyType == Large` | Strength x1.5; Endurance x2; Speed x0.75; Resistance x1.5; Force x0.75; Offense x0.75; Defense x0.75; Regeneration x0.75; Recovery x0.75 |

## Combined non-Changeling raw behavior

| Body | Strength | Endurance | Speed | Force | Resistance | Offense | Defense | Regeneration | Recovery | Audit |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---|
| Medium | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | Preserve |
| Small | 0.5 | 0.5 | 1.75 | 1.5625 | 0.25 | 2.1 | 2.1 | 1.25 | 1.25 | AuditOnly / NeedsBalanceSim |
| Large | 1.5 | 3.0 | 0.525 | 0.525 | 2.25 | 0.525 | 0.525 | 0.75 | 0.75 | AuditOnly / NeedsBalanceSim |

## Combined Changeling raw behavior

| Body | Strength | Endurance | Speed | Force | Resistance | Offense | Defense | Regeneration | Recovery | Audit |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---|
| Small | 0.5 | 0.5 | 1.4 | 1.25 | 0.5 | 1.4 | 1.4 | 1.25 | 1.25 | Park; body policy should explicitly decide Changeling-like behavior. |
| Large | 1.5 | 2.0 | 0.75 | 0.75 | 1.5 | 0.75 | 0.75 | 0.75 | 0.75 | Park; body policy should explicitly decide Changeling-like behavior. |

## Phase 08 seed choice

Use the explicit one-pass values in `Resources/Prototypes/RI/Character/phase08_seed.yml` and `Docs/Extracted Content/body-types.md` for implementation tests, while keeping the raw combined table above for comparison in the balance simulator.
