# Stats Extraction — Phase 05 Updated

Status: updated by Phase 05 final stat bible pass. This document is an extracted-content companion to `Docs/Design/stat-bible.md`.

## Source resolution

- `Code/_Variables.dm` is the inspected variables source.
- `Code/Races/*.dm` and `Code/__core/race_stats.dm` are the current race/class stat registry source.
- `Code/statpointsystems.dm` is still used for body type modifiers, stat reset, allocation handoff, and legacy fallback behavior.
- `Code/__core/progression_calculators.dm` owns the current stat point formula.
- `GenRaces` is ignored/parked for a future child/genetics update.

## Canonical stat split

| Category | Canonical fields | Legacy examples | Notes |
|---|---|---|---|
| Identity | `RaceId`, `ClassId`, `BodyTypeId`, `SpawnPlanetId`, `DisplayName` | `Race`, `Class`, `BodyType`, `name` | Saved and server-validated. |
| Creation affinities | `EnergyCapacity`, `Strength`, `Endurance`, `Force`, `Resistance`, `Speed`, `Offense`, `Defense`, `Regeneration`, `Recovery`, `Anger` | `EnergyMod`, `StrengthMod`, etc.; `AngerMax` | Saved multiplier/direct values. |
| Trained/permanent values | `BasePower`, trained combat stats, permanent bonuses | `Base`, `Strength`, `Endurance`, etc. | Only permanent growth changes these. |
| Resources/vitals | `HealthCurrent`, `EnergyCurrent`, `EnergyMax`, `OxygenCurrent`, `ManaCurrent` | `Health`, `Energy`, `EnergyMax`, `Oxygen`, `ManaAmount` | Depletable/current values, not creation affinities. |
| Temporary modifiers | status/form/equipment multipliers | `Power_Multiplier`, `StrengthMultiplier`, runtime `Anger` | Owned by later modifier/status systems. |

## Allocatable creation stats

`EnergyCapacity`, `Strength`, `Endurance`, `Speed`, `Force`, `Resistance`, `Offense`, `Defense`, `Regeneration`, `Recovery`, and `Anger`.

`Intelligence` and `Enchantment` remain progression-only for the first pass.

## Important rules

- `EnergyCapacity` is legacy `EnergyMod`; it does not mean current Energy.
- `Speed`, `Regeneration`, and `Recovery` are direct first-pass stats unless a temporary status changes them.
- `Anger` in `RiStatType` means creation `AngerMax`. Active battle anger is a runtime buff value.
- `BasePowerMultiplier` (`BaseMod`) is separate from temporary `Power_Multiplier`.
- The client may preview stats but must never decide final stats.

## Formula audit summary

See `Docs/Design/stat-bible.md` section 13 for the full formula table.

## Generated matrices

- `Docs/Generated Findings/phase05-race-stat-matrix.csv`
- `Docs/Generated Findings/phase05-stat-point-policy-matrix.csv`
- `Docs/Generated Findings/phase05-variable-vocabulary.md`
