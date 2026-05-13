# Body Types Extraction — Phase 05 Updated

Status: updated by Phase 05 final stat bible pass.

## Body type vocabulary

| Body type | Design role | First implementation |
|---|---|---|
| `Medium` | Default body profile | No multipliers. |
| `Small` | Agile, weaker, higher precision/evasion | Explicit prototype multipliers. |
| `Large` | Stronger/durable, slower, lower precision/evasion | Explicit prototype multipliers. |

## Duplicate-body-block contradiction

`Code/statpointsystems.dm:ClassBodyStats()` contains duplicate `Small` and `Large` body checks. In raw active code, non-Changeling Small and Large bodies appear to receive both matching blocks, which compounds the modifier values.

This is documented in `Docs/Generated Findings/phase05-body-modifier-audit.md`.

Phase 05 resolution:

- Do not recreate the duplicate-block behavior as hidden C# conditionals.
- Use explicit `RiBodyTypePrototype.statAffinityMultipliers` values.
- Keep the raw combined active-code values as balance audit data.
- Mark body values `Tune` / `NeedsBalanceSim` until Phase 08 tests lock the chosen seed values.

## Phase 08 seed body prototypes

```yaml
- type: riBodyType
  id: Medium
  displayName: Medium
  statAffinityMultipliers: {}
  audit:
    status: Preserve

- type: riBodyType
  id: Small
  displayName: Small
  statAffinityMultipliers:
    Strength: 0.5
    Endurance: 0.5
    Speed: 1.25
    Resistance: 0.5
    Force: 1.25
    Offense: 1.5
    Defense: 1.5
  excludedRaceTags:
    - ChangelingLike
  audit:
    status: Tune

- type: riBodyType
  id: Large
  displayName: Large
  statAffinityMultipliers:
    Strength: 1.5
    Endurance: 1.5
    Speed: 0.7
    Resistance: 1.5
    Force: 0.7
    Offense: 0.7
    Defense: 0.7
  excludedRaceTags:
    - ChangelingLike
  audit:
    status: Tune
```
