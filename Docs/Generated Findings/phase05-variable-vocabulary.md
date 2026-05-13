# Phase 05 Variable Vocabulary

Source: `Code/_Variables.dm`, with behavior references from `Code/Stats.dm`, `Code/statpointsystems.dm`, and `Code/__core/progression_calculators.dm`.

This file normalizes the legacy mob variable vocabulary into the terms used by the Reincarnate RobustToolbox rewrite.

| Canonical field/stat | Legacy field(s) | Category | Save? | Phase target | Notes |
|---|---|---|---:|---|---|
| CharacterId | save key / ckey-derived path | Identity | Yes | Phase 10 | New stable ID; never save Robust `EntityUid`. |
| DisplayName | `name` | Identity | Yes | Phase 09 | Server-validated name. |
| RaceId | `Race` | Identity | Yes | Phase 08/09 | Prototype ID. |
| ClassId | `Class` | Identity | Yes | Phase 08/09 | Race-scoped prototype ID, e.g. `Human.Fighter`. |
| BodyTypeId | `BodyType` | Identity | Yes | Phase 08/09 | Body policy selection. |
| BasePower | `Base` | Permanent progression | Yes | Phase 08+ | Grows through training/progression; actual permanent base. |
| BasePowerMultiplier | `BaseMod` | Creation/progression multiplier | Yes | Phase 08+ | Race/class natural power multiplier; distinct from temporary `Power_Multiplier`. |
| PowerMultiplier | `Power_Multiplier` | Temporary runtime modifier | Usually no | Phase 16 | Buff/form/status multiplier, not base race power. |
| ControlPower | `ControlPower` | Runtime control/power-up state | Yes/derived | Phase 11+ | Percentage-style power-up/control value; display and drains need separate systems. |
| EnergyCapacity | `EnergyMod` | Creation affinity | Yes | Phase 08 | Determines energy growth/max scaling; not current energy. |
| EnergyCurrent | `Energy` | Current resource | Yes | Phase 11 | Spent by moves and recovers toward `EnergyMax`. |
| EnergyMax | `EnergyMax` | Current resource cap | Yes | Phase 11 | Grows through progression using `EnergyMod`. |
| Strength | `StrengthMod`, `Strength` | Creation affinity + trained stat | Yes | Phase 08 | Affinity mod scales trained/base Strength. |
| Endurance | `EnduranceMod`, `Endurance` | Creation affinity + trained stat | Yes | Phase 08 | Affinity mod scales trained/base Endurance. |
| Force | `ForceMod`, `Force` | Creation affinity + trained stat | Yes | Phase 08 | Affinity mod scales trained/base Force. |
| Resistance | `ResistanceMod`, `Resistance` | Creation affinity + trained stat | Yes | Phase 08 | Affinity mod scales trained/base Resistance. |
| Offense | `OffenseMod`, `Offense` | Creation affinity + trained stat | Yes | Phase 08 | Accuracy/attack skill affinity. |
| Defense | `DefenseMod`, `Defense` | Creation affinity + trained stat | Yes | Phase 08 | Evasion/guard skill affinity. |
| Speed | `SpeedMod` | Direct affinity/runtime stat | Yes | Phase 08 | No separate base stat in first pass; use current modifier directly unless temporarily changed. |
| Regeneration | `Regeneration`, `RegenerationMultiplier` | Direct affinity + temp multiplier | Yes/No | Phase 08/11 | Regen value is direct; multiplier is temporary/status-owned. |
| Recovery | `Recovery`, `RecoveryMultiplier` | Direct affinity + temp multiplier | Yes/No | Phase 08/11 | Energy/control recovery value is direct; multiplier is temporary/status-owned. |
| AngerMax | `AngerMax` | Creation cap / max burst | Yes | Phase 08/11 | Presented as percent; not the current active anger buff. |
| AngerCurrent | `Anger` | Runtime burst buff | Usually no | Phase 11/16 | Applied when anger triggers; affects current BP/power. |
| Potential | `Potential`, `OriginalPotential` | Progression reserve | Yes | Phase 08+ | Numeric in rewrite; original retained for restoration/refund style effects. |
| ExperienceMultiplier | `EXP`, `OriginalEXP`, `CurrentEXP`, `NextLevelEXP` | Progression/optional level system | Yes | Phase 08+ | Legacy `EXP` is a growth multiplier and optional level system input, not only generic XP. |
| TrainingRate | `Training_Rate` | Progression rate | Yes | Phase 08+ | Primarily BasePower growth through train/equipment. |
| MeditationRate | `Meditation_Rate` | Progression rate | Yes | Phase 08+ | BasePower, Intelligence EXP, Enchantment EXP, study/meditation. |
| ZenkaiRate | `Zenkai_Rate` | Progression rate | Yes | Phase 08+ | Spar/KO growth rate. |
| Intelligence | `Intelligence`, `IntelligenceLevel`, `IntelligenceEXP` | Progression/crafting | Yes | Later | Tech/cybernetics/building capability. Not allocatable in first pass. |
| Enchantment | `Enchantment`, `EnchantmentLevel`, `EnchantmentEXP` | Progression/crafting | Yes | Later | Magical item/spell/enchant capability. Not allocatable in first pass. |
| GravityMastery | `GravityMastered`, `GravityMod` | Environment/training | Yes | Later | Mastered gravity threshold and mastery rate. |
| TemperatureTolerance | `TemperatureTolerance`, `TemperatureToleranceType` | Environment | Yes | Later | Hot/cold/vacuum tolerance from race/class/spawn. |
| HealthCurrent | `Health` | Current resource/vital | Yes | Phase 11 | Percent-like health in legacy; rewrite should split current/max. |
| HealthMax | `100 + ChangieMaxHealth - TotalInjury` concept | Derived vital cap | Yes/derived | Phase 11 | Needs explicit max/cap model. |
| OxygenCurrent | `Oxygen` | Current environment resource | Yes | Phase 12 | Water/vacuum survival. |
| OxygenMax | `MaxOxygen` | Resource cap | Yes | Phase 12 | Environment resource max. |
| ManaCurrent/ManaMax | `ManaAmount`, `ManaMax` | Current resource | Yes | Later | Do not overload Energy. |
| Fatigue/Injury | `TotalFatigue`, `TotalInjury` | Vitals/status | Yes | Phase 11 | Resource caps and recovery penalties. |

## Source-resolution decisions

- Use `Code/_Variables.dm`; there is no separate inspected `Code/Variables.dm` in the uploaded archive.
- Use `Code/Races/*.dm` and `Code/__core/race_stats.dm` as current stat profile sources.
- Ignore `GenRaces` for Phase 05/08 and park it for a future child/genetics system.
