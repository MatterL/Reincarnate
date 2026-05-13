# Stat Bible

**Project:** Reincarnate / RobustToolbox rewrite  
**Phase:** 05 — final stat bible pass  
**Status:** Design/extraction only. No C# stat implementation is assumed to exist yet.  
**Primary output path:** `Docs/Design/stat-bible.md`

## 1. Scope and non-goals

This document defines the Phase 08-ready stat vocabulary, calculation order, prototype shapes, formula audit labels, source matrices, and acceptance tests for the Reincarnate RobustToolbox rewrite.

Phase 05 does **not** implement gameplay code. It produces enough deterministic documentation that Phase 08 can implement `RiStatType`, stat profiles, race/class/body prototypes, stat point calculation, and tests without rereading the whole BYOND source.

Non-goals:

- No C# implementation in Phase 05.
- No bulk conversion of every race into final production YAML.
- No public-release legal/name cleanup in this pass.
- No `GenRaces` hybrid/child/genetics implementation in Phase 08.

## 2. Source files and source priority

Resolved source priority:

1. `Code/Races/*.dm` and `Code/__core/race_stats.dm` are the current race/class/stat registry source of truth for Phase 05 extraction.
2. `Code/__core/progression_calculators.dm` is the current stat point formula source.
3. `Code/statpointsystems.dm` remains the body type and legacy fallback calculation source.
4. `Code/Stats.dm` remains the derived power/current resource display source.
5. `Code/_Variables.dm` is the actual variables file in the uploaded archive and replaces the guide's older `Code/Variables.dm` reference.
6. Existing extracted docs are treated as historical notes and update targets, not tie-breakers over current registry files.

User resolutions applied:

- Use `Code/_Variables.dm` instead of `Code/Variables.dm`.
- Use `Code/Races/*.dm` and `Code/__core/race_stats.dm` over legacy `statpointsystems.dm` race branches.
- Use current `RI` / `Ri` naming conventions.
- Do not evaluate race/class names for legal release risk in Phase 05.
- Ignore `GenRaces` for now; park it for a possible future child/genetics update.

Generated evidence files:

- `Docs/Generated Findings/phase05-race-stat-matrix.csv`
- `Docs/Generated Findings/phase05-race-class-matrix.csv`
- `Docs/Generated Findings/phase05-stat-point-policy-matrix.csv`
- `Docs/Generated Findings/phase05-body-modifier-audit.md`
- `Docs/Generated Findings/phase05-variable-vocabulary.md`

## 3. Stat vocabulary

The legacy source uses “stat” for identity, creation affinity, trained/base stat, resource, temporary multiplier, and display value. The rewrite splits these explicitly.

### 3.1 Identity fields

| New field | Legacy field(s) | Save? | Prototype-owned? | Notes |
|---|---|---:|---:|---|
| `CharacterId` | save key / ckey-derived path | Yes | No | New stable ID. Never use live Robust `EntityUid` as persistent identity. |
| `DisplayName` | `name` | Yes | No | Server validates. |
| `RaceId` | `Race` | Yes | Yes | Uses current registry source race IDs as migration values. |
| `ClassId` | `Class` | Yes | Yes | Race-scoped rewrite ID, such as `Human.Fighter`. |
| `BodyTypeId` | `BodyType` | Yes | Yes | Medium/Small/Large first. |
| `SpawnPlanetId` / `SpawnPointId` | creation planet/spawn | Yes | Yes | Stable prototype IDs. |
| `VisualProfile` | icon/hair/tail/custom vars | Yes | Partly | Not part of stat math. |
| `UnlockStateRef` | `CheckUnlock`, `unlock_key` | Account/service | No | Server-owned unlock predicates. |

### 3.2 Creation affinity stats

Creation affinities are race/class/body/stat-point multipliers or direct creation values. They are saved because they define how future growth scales.

| `RiStatType` | UI label | Legacy field(s) | First-pass meaning |
|---|---|---|---|
| `EnergyCapacity` | Energy | `EnergyMod` | Energy growth/max affinity, not current energy. |
| `Strength` | Strength | `StrengthMod`, `Strength` | Physical power affinity applied to trained/base Strength. |
| `Endurance` | Endurance | `EnduranceMod`, `Endurance` | Physical durability affinity applied to trained/base Endurance. |
| `Force` | Force | `ForceMod`, `Force` | Energy/magic output affinity applied to trained/base Force. |
| `Resistance` | Resistance | `ResistanceMod`, `Resistance` | Energy/magic durability affinity applied to trained/base Resistance. |
| `Speed` | Speed | `SpeedMod` | Direct speed multiplier; no separate base stat in first pass. |
| `Offense` | Offense | `OffenseMod`, `Offense` | Attack accuracy/effectiveness affinity. |
| `Defense` | Defense | `DefenseMod`, `Defense` | Guard/evasion effectiveness affinity. |
| `Regeneration` | Regeneration | `Regeneration` | Direct recovery stat used by health/wound recovery. |
| `Recovery` | Recovery | `Recovery` | Direct recovery stat used by energy/control recovery. |
| `Anger` | Anger | `AngerMax`, `Anger` | Creation stores `AngerMax`; runtime active anger uses a separate current buff. |

### 3.3 Progression and growth fields

| Canonical field/stat | Legacy field(s) | Save? | Notes |
|---|---|---:|---|
| `BasePower` | `Base` | Yes | Grows at the same baseline rate for all characters before multipliers. |
| `BasePowerMultiplier` | `BaseMod` | Yes | Race/class natural power multiplier. Can later change permanently or temporarily by explicit sources. |
| `Potential` | `Potential`, `OriginalPotential` | Yes | Numeric race/class potential reservoir used by unlock-like effects. |
| `ExperienceMultiplier` | `EXP`, `OriginalEXP`, `CurrentEXP`, `NextLevelEXP` | Yes | Optional level/RP growth system; not just generic XP. |
| `TrainingRate` | `Training_Rate` | Yes | BasePower growth through Train/equipment. |
| `MeditationRate` | `Meditation_Rate` | Yes | BasePower, Intelligence EXP, Enchantment EXP through study/meditation. |
| `ZenkaiRate` | `Zenkai_Rate` | Yes | Spar/KO growth rate. |
| `Intelligence` | `Intelligence`, `IntelligenceLevel`, `IntelligenceEXP` | Yes | Tech/crafting/cybernetic capability. Progression-only in first pass. |
| `Enchantment` | `Enchantment`, `EnchantmentLevel`, `EnchantmentEXP` | Yes | Enchanting/magic item/spell capability. Progression-only in first pass. |
| `GravityMastery` | `GravityMastered`, `GravityMod` | Yes | Environmental/training mastery, not core stat math. |
| `TemperatureTolerance` | `TemperatureTolerance`, `TemperatureToleranceType` | Yes | Environment survival, not core stat math. |

### 3.4 Current resources and vitals

| New field | Legacy field(s) | Target | Notes |
|---|---|---|---|
| `HealthCurrent` / `HealthMax` | `Health`, `ChangieMaxHealth`, injury caps | `RiVitalsComponent` | Split current/max explicitly. |
| `EnergyCurrent` / `EnergyMax` | `Energy`, `EnergyMax` | `RiVitalsComponent` / `RiEnergyComponent` | Moves consume current energy; it recovers toward max. |
| `OxygenCurrent` / `OxygenMax` | `Oxygen`, `MaxOxygen` | Environment/vitals | Phase 12. |
| `ManaCurrent` / `ManaMax` | `ManaAmount`, `ManaMax` | Later resource component | Keep separate from Energy. |
| `Fatigue` / `Injury` | `TotalFatigue`, `TotalInjury` | Vitals/status | Affects caps/recovery. |

### 3.5 Temporary modifiers

Temporary multipliers come from transformations, buffs, equipment, injuries, statuses, and special power states. They must be represented as modifier sources rather than written into permanent creation affinity values.

Examples: `StrengthMultiplier`, `EnduranceMultiplier`, `SpeedMultiplier`, `ForceMultiplier`, `ResistanceMultiplier`, `OffenseMultiplier`, `DefenseMultiplier`, `RegenerationMultiplier`, `RecoveryMultiplier`, `Power_Multiplier`, `RPPower`, `ControlPower`, runtime `Anger`, `Body`, and transformation/form flags.

## 4. Identity fields

Identity selection is server-validated and saved as stable IDs:

- `RaceId`
- `ClassId`
- `BodyTypeId`
- `SpawnPlanetId` / `SpawnPointId`
- `DisplayName`
- `VisualProfile`
- unlock references owned by account/character services

The client may preview choices but never decides final permanent values.

## 5. Creation affinities vs runtime stats vs current resources

Use three separate concepts:

1. **Creation affinity** — race/class/body/stat-point value, usually corresponding to `*Mod` or direct `Regeneration`/`Recovery`/`AngerMax`.
2. **Permanent/trained stat value** — actual grown base value, such as base Strength or BasePower.
3. **Current resource or runtime value** — current Energy, Health, Oxygen, active Anger, ControlPower, temporary multipliers.

Important examples:

- `EnergyCapacity` is legacy `EnergyMod`; `EnergyCurrent` is legacy `Energy`; `EnergyMax` is the recoverable cap.
- `Speed`, `Regeneration`, and `Recovery` are direct first-pass stats unless a status temporarily changes them.
- `AngerMax` is chosen/derived at creation and shown as a percent; active `Anger` is a runtime BP/power buff.
- `BasePowerMultiplier` (`BaseMod`) is not the same as temporary `Power_Multiplier`.

## 6. Calculation order

### 6.1 Creation preview/finalization order

1. Validate race/class/body/spawn choices server-side.
2. Reset creation affinities to neutral values.
3. Apply `RiStatProfilePrototype` from current `Code/Races/*.dm` registry data.
4. If class has replacement stats, use replacement values instead of base race values.
5. Otherwise apply race base stats, then class adjustment overlay when present.
6. Resolve starting stat point policy using registry defaults and class overrides.
7. Apply explicit body type prototype modifiers.
8. Apply generic class overlays as data, not `ClassBodyStats()` string checks.
9. Snapshot allocation base values.
10. Apply player stat point allocation with the preserved formula.
11. Finalize initial runtime values: BasePower, EnergyMax, current resources, starting grants, spawn, visuals.
12. Compute derived preview/display values from the same shared calculation code the server uses.

### 6.2 Runtime effective stat order

```text
trainedPermanentValue
+ earnedPermanentAdditiveProgress
+ permanentAdditiveBonuses
=> permanentValue

permanentValue * creationAffinityMultiplier
=> creationAdjustedValue

creationAdjustedValue + temporaryAdditiveModifiers
=> additiveAdjustedValue

additiveAdjustedValue * temporaryMultipliers
=> currentEffectiveValue
```

### 6.3 Energy, Speed, Regeneration, Recovery, and Anger special cases

- Energy growth adds to `EnergyMax`; moves consume `EnergyCurrent`; recovery returns `EnergyCurrent` toward `EnergyMax`.
- Speed is a direct modifier stat in first pass; use `SpeedMod` directly unless a status temporarily changes it.
- Regeneration and Recovery are direct modifier stats in first pass.
- Anger creation stores `AngerMax`; active battle anger writes a current temporary BP/power multiplier into runtime `Anger`.

### 6.4 Derived power concept

Preserve the concept, not the giant tick implementation:

```text
baseAvailablePower = BasePower * BasePowerMultiplier
runtimePower = baseAvailablePower
  * healthAvailabilityFactor
  * energyAvailabilityFactor
  * controlPowerFactor
  * anger/form/status/equipment modifiers
```

The legacy `Stats.dm` pattern `Base * 20 * Body * Power_Multiplier`, health/energy square-root reductions, `ControlPower / 100`, and active `Anger` should be rewritten as explicit modifier/status sources.

## 7. Stat point policy

Allocatable first-pass stats:

```text
EnergyCapacity
Strength
Endurance
Speed
Force
Resistance
Offense
Defense
Regeneration
Recovery
Anger
```

`Intelligence` and `Enchantment` are progression-only in the first pass.

Formula preserved from `ProgressionCalculator.StatPointValue`:

```text
if points == 0:
    value = baseValue
else:
    value = round(
        baseValue + ((baseValue * statPointFactor) * (points * (1 / (points ** powerValue)))),
        0.01
    )
```

Factors:

| Stat | Factor | Audit |
|---|---:|---|
| Speed | 0.05 | Tune |
| Anger | 0.01 | Tune |
| All other allocatable stats | 0.10 | PreserveOrderTuneNumbers |

Exponent policy:

| Race/class | Exponent |
|---|---:|
| Default | 0.124 |
| Alien | 0.062 |
| Aethirian | 0.062 |
| Android.Fighter | 0.062 |

Starting stat point policy comes from `Code/Races/*.dm` and `Code/__core/race_stats.dm`; see `phase05-stat-point-policy-matrix.csv`.

## 8. Race prototype schema

```yaml
- type: riRace
  id: Human
  displayName: Human
  sourceRaceId: Human
  defaultSpawnPlanet: Terra
  defaultClass: Human.Fighter
  allowedClasses:
    - Human.Fighter
  bodyTypePolicy: StandardHumanoid
  statProfile: Human
  progressionProfile: Human
  startingStatPoints: 60
  unlockRequirement: null
  legalStatus: NotAssessedPhase05
  tags:
    - Humanoid
    - StandardBodyTypes
  audit:
    status: PreserveOrderTuneNumbers
    sourcePaths:
      - Code/Races/Human.dm
```

Required fields: `id`, `displayName`, `sourceRaceId`, `defaultSpawnPlanet`, `defaultClass`, `allowedClasses`, `bodyTypePolicy`, `statProfile`, `progressionProfile`, `startingStatPoints`, `unlockRequirement`, `legalStatus`, `tags`, and `audit`.

## 9. Class prototype schema

```yaml
- type: riClass
  id: Human.Sage
  displayName: Sage
  sourceClassName: Sage
  allowedRaceIds:
    - Human
  statProfileMode: Replacement
  statTemplate: Human.Sage
  progressionOverlay: Human.Sage
  skillGrantIds: []
  transformationPathIds: []
  unlockRequirement: Sage
  legalStatus: NotAssessedPhase05
  tags:
    - HumanClass
  audit:
    status: Preserve
    sourcePaths:
      - Code/Races/Human.dm
```

Race-scoped class IDs are required when behavior differs by race. Do not use one global `Fighter` when `Human.Fighter`, `Alien.Fighter`, and `Android.Fighter` differ.

## 10. Body type prototype schema

```yaml
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
    sourcePaths:
      - Code/statpointsystems.dm:ClassBodyStats
```

Body type modifiers must be explicit prototype data. The raw duplicate active-code behavior is documented in `phase05-body-modifier-audit.md`; Phase 08 should not bury that behavior in C# conditionals.

## 11. Stat/progression profile schema

```yaml
- type: riStatProfile
  id: Human
  sourceRaceId: Human
  baseAffinities:
    EnergyCapacity: 1.5
    Strength: 1
    Endurance: 1
    Speed: 1
    Force: 1
    Resistance: 1
    Offense: 1
    Defense: 1
    Regeneration: 0.5
    Recovery: 3
    Anger: 0.5
  classReplacements:
    Human.Sage:
      EnergyCapacity: 1.75
      Strength: 1.5
  classAdjustments: {}
```

```yaml
- type: riProgressionProfile
  id: Human
  basePowerMultiplier: 1
  potential: 4
  experienceMultiplier: 1
  trainingRate: 1.75
  meditationRate: 1.75
  zenkaiRate: 0.5
  intelligence: 2
  enchantment: 2
  gravityMod: 1
  gravityMastered: 1
  temperatureTolerance:
    amount: 0
    type: None
```

Progression profile fields are seed candidates; Phase 08/10 should expand extraction from `RaceQualities()` after the minimum stat framework is implemented.

## 12. Formula audit labels

| Label | Meaning |
|---|---|
| `Preserve` | Behavior and order should be preserved unless later playtests prove otherwise. |
| `PreserveOrderTuneNumbers` | Keep operation order but allow numeric rebalance. |
| `Tune` | Useful concept, numeric values are balance candidates. |
| `Rewrite` | Preserve player-facing concept, not implementation shape. |
| `Delete` | Remove old implementation. |
| `Park` | Do not implement in first pass; document for future phases. |
| `NotAssessedPhase05` | Intentionally not reviewed during this phase, such as legal release naming. |
| `NeedsSourceConfirmation` | Current source and extracted notes disagree or source path is uncertain. |
| `NeedsBalanceSim` | Requires simulator coverage before final acceptance. |
| `ImplementationBlocker` | Must be resolved before Phase 08 code can be accepted. |

## 13. Formula audit table

|Concept|Legacy source|Phase 05 decision|Audit label|Phase 08 action|
|---|---|---|---|---|
|Neutral stat reset|ResetStats()|Preserve neutral reset of creation affinities before race/class/body.|Preserve|Unit-test blank stat block.|
|Race stat registry|TryApplyRegisteredRaceStats(), Code/Races/*.dm|Use current registry profiles over legacy Stats() branches.|PreserveOrderTuneNumbers|Prototype RiStatProfile values from registry.|
|Class replacement stats|AddClassReplacement / race_stat_profile.ApplyToMob|Replacement overrides base race set for that class.|Preserve|Test Human.Sage and Alien.Technologist.|
|Class adjustment stats|AddClassAdjustment / race_stat_profile.ApplyToMob|Base race stats apply first, adjustment modifies named fields.|Preserve|Prototype adjustment overlays.|
|Body type modifiers|ClassBodyStats()|Do not hide duplicate if-blocks in code; use explicit body prototypes and keep raw combined behavior as audit data.|Tune / NeedsBalanceSim|Test Small/Large seed values and compare with audit matrix.|
|Generic class/body overlays|ClassBodyStats() Wizard/Healer/Technologist/God/Muscle Wizard|Move to class stat/progression overlays instead of hard-coded string checks.|Rewrite|Add class overlay schema support.|
|Starting stat points|SetDefaultStatPoints, SetClassStatPoints, class_definition.stat_points|Use current Code/Races registry and class overrides.|PreserveOrderTuneNumbers + NeedsBalanceSim|Load phase05-stat-point-policy-matrix.csv into tests/balance sim.|
|Stat point allocation formula|ProgressionCalculator.StatPointValue|Preserve formula order initially.|PreserveOrderTuneNumbers|Unit-test 0/1/10/60/70/110 points.|
|Speed point factor|StatPointFactor(PROG_STAT_SPEED)|Speed uses 0.05 instead of default 0.1.|Tune|Specific factor test.|
|Anger point factor|StatPointFactor(PROG_STAT_ANGER)|Anger uses 0.01 and displays as percent.|Tune|Specific factor and display test.|
|Race/class stat point exponent|StatPointPowerValue|Default 0.124; Alien, Aethirian, Android.Fighter use 0.062.|PreserveOrderTuneNumbers|Data-drive in RiStatPointPolicyPrototype.|
|GenRaces averaging branch|RacialStats() unreachable branch|Ignore now; park for future child/genetics update.|Park|Do not implement.|
|Base power formula|Stats.dm power tick|Preserve concepts but rewrite implementation into modifier sources.|Rewrite||
|Health/energy reducing power|Stats.dm sqrt health/energy portions|Preserve concept for most characters; special bypass via status/prototype tags.|PreserveOrderTuneNumbers||
|Recovery/regen loops|Recover(), Stat() tick|Rewrite as ECS systems/timers.|Rewrite||
|UI stat labels|winset/output/statpanel|Delete implementation; preserve displayed facts.|Delete + Rewrite||
|Savefile stat hash|Creation.dm SaveChar/EmergencySave hash|Delete implementation; preserve integrity concept as save schema/checksum/migration.|Rewrite||
|Legal release risk|Race/class names|Not assessed in this pass by user instruction.|Park||


## 14. First implementation sample set

The first sample seed set is in `Resources/Prototypes/RI/Character/phase08_seed.yml` and covers:

- Human
- Alien
- Android
- Namekian
- Medium / Small / Large
- Human.Fighter
- Human.Sage
- Human.Wizard
- Alien.Fighter
- Alien.Technologist
- Android.Juggernaut
- Android.Fighter
- Namekian.Fighter
- Namekian.Ancient

These values are Phase 08 seed candidates, not final balance truth.

## 15. Phase 08 test requirements

| Test | Expected purpose |
|---|---|
| `Human_Fighter_Medium_IsDeterministic` | Baseline race/class/body preview. |
| `Human_Fighter_Small_AppliesBodyPolicy` | Body modifier order and exact values. |
| `Human_Fighter_Large_AppliesBodyPolicy` | Body modifier order and exact values. |
| `ChangelingLike_Small_DoesNotUseStandardBodyPolicy` | Race/body exclusion behavior. |
| `Human_Sage_UsesClassReplacement` | Class replacement behavior. |
| `Alien_Technologist_UsesClassReplacement` | Race-specific class replacement. |
| `StatPointFormula_ZeroPoints_ReturnsBase` | Formula correctness. |
| `StatPointFormula_SpeedUsesDifferentFactor` | Speed special factor. |
| `StatPointFormula_AngerUsesDifferentFactor` | Anger special factor. |
| `StatPointPolicy_RaceClassOverridesDefault` | Starting point policy. |
| `PreviewMatchesServerFinal` | Client preview cannot diverge from server truth. |
| `BasePowerMultiplier_IsSeparateFromTemporaryPowerMultiplier` | `BaseMod`/`Power_Multiplier` separation. |
| `EnergyCapacity_DoesNotMutateCurrentEnergy` | `EnergyMod`/current resource split. |
| `AngerMax_DisplaysAsPercent_ActiveAngerIsRuntimeOnly` | Anger cap/current split. |

## 16. Known unresolved questions

Resolved in this pass:

1. `Code/_Variables.dm` is the variables source.
2. `Code/Races/*.dm` and `Code/__core/race_stats.dm` take priority over legacy race branches in `statpointsystems.dm`.
3. Current `RI`/`Ri` naming is used.
4. Race/class legal risk is not evaluated in Phase 05.
5. `GenRaces` is ignored/parked for future child/genetics work.
6. `EnergyCapacity` is the preferred code name for legacy `EnergyMod`.
7. `BasePowerMultiplier` and temporary `Power_Multiplier` remain separate.
8. `Intelligence` and `Enchantment` are progression-only for first pass.
9. `Potential` should be numeric in the rewrite, with tags/prototypes for special cases.

Still visible for Phase 08/10:

1. Body type numeric values need balance confirmation against the raw duplicate-block audit.
2. Progression profile values from `RaceQualities()` should be fully matrixed after the core stat framework exists.
3. Full production YAML should wait until Phase 08 prototype classes compile and load.

## 17. Save model implications

Suggested v0 stat save slice:

```csharp
public sealed record RiSavedCharacterStatsV0(
    string RaceId,
    string ClassId,
    string BodyTypeId,
    float BasePower,
    float BasePowerMultiplier,
    float Potential,
    float OriginalPotential,
    float ExperienceMultiplier,
    Dictionary<RiStatType, float> CreationAffinities,
    Dictionary<RiStatType, int> AllocatedCreationPoints,
    Dictionary<RiStatType, float> EarnedPermanentValues,
    Dictionary<RiStatType, float> PermanentAdditiveBonuses,
    float HealthCurrent,
    float EnergyCurrent,
    float OxygenCurrent,
    int SchemaVersion
);
```

Do not save temporary UI preview values or live Robust `EntityUid` values.

## Phase 08 implemented formula order

Phase 08 implements the first deterministic Ri stat calculation path.

### Source split

- `EnergyCapacity` is legacy `EnergyMod`; it is not current depletable `Energy`.
- `Anger` in `RiStatType` is creation `AngerMax`; active battle anger remains a later runtime/status value.
- `Intelligence` and `Enchantment` exist as stats but are not allocatable creation stats in this pass.
- Race/class/body behavior is prototype-defined, not switch-defined in C#.

### Prototype order

1. Resolve race.
2. Resolve class.
3. Resolve body type.
4. Validate class is allowed by race.
5. Validate race is allowed by class.
6. Validate body type is not excluded by race tags.
7. Resolve class stat template.
8. Apply stat template affinities.
9. Apply body type affinity multipliers.
10. Apply permanent creation allocations as `PermanentAddBonus`.
11. Apply temporary buffs/status/equipment only through temporary modifier fields.

### Final formula

```text
basePermanent = baseValue + earnedBonus + permanentAddBonus
creationAdjusted = basePermanent * creationMultiplier
currentFinal = (creationAdjusted + temporaryAddBonus) * temporaryMultiplier
```

### Body type contradiction resolution

The duplicate Small/Large legacy blocks are not recreated as hidden C# conditionals. The Phase 08 body prototypes carry explicit seed multiplier values and are marked `Tune` until balance simulation locks them.

### Class variant policy

Race-scoped class IDs are required when class behavior differs by race:

```text
Human.Fighter
Alien.Fighter
Alien.Technologist
Saiyan.Fighter
```

A class points to exactly one stat template. This prevents client UI, server validation, and tests from each having to understand nested replacement rules.