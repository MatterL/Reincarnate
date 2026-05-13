# Stat System Tests — Phase 08 Requirements

Status: authored during Phase 05. These tests are required before Phase 08 can be accepted.

## Required deterministic stat tests

| Test | Purpose |
|---|---|
| `Human_Fighter_Medium_IsDeterministic` | Baseline race/class/body preview. |
| `Human_Fighter_Small_AppliesBodyPolicy` | Body modifier order and exact seed values. |
| `Human_Fighter_Large_AppliesBodyPolicy` | Body modifier order and exact seed values. |
| `ChangelingLike_Small_DoesNotUseStandardBodyPolicy` | Race/body exclusion behavior. |
| `Human_Sage_UsesClassReplacement` | Class replacement overrides base race stats. |
| `Alien_Technologist_UsesClassReplacement` | Race-specific class replacement. |
| `StatPointFormula_ZeroPoints_ReturnsBase` | Formula correctness. |
| `StatPointFormula_SpeedUsesDifferentFactor` | Speed uses factor 0.05. |
| `StatPointFormula_AngerUsesDifferentFactor` | Anger uses factor 0.01. |
| `StatPointPolicy_RaceClassOverridesDefault` | Starting point policy uses class/race overrides. |
| `PreviewMatchesServerFinal` | Client preview and server final use the same shared calculation. |
| `BasePowerMultiplier_IsSeparateFromTemporaryPowerMultiplier` | `BaseMod` is not `Power_Multiplier`. |
| `EnergyCapacity_DoesNotMutateCurrentEnergy` | `EnergyMod` does not directly alter current energy. |
| `AngerMax_DisplaysAsPercent_ActiveAngerIsRuntimeOnly` | Creation anger cap and active anger buff are separate. |

## Test fixture seed data

Use `Resources/Prototypes/RI/Character/phase08_seed.yml` as the initial fixture source, then migrate it into real prototype files after `RiRacePrototype`, `RiClassPrototype`, `RiBodyTypePrototype`, `RiStatProfilePrototype`, `RiProgressionProfilePrototype`, and `RiStatPointPolicyPrototype` compile.

## Compile-risk notes

- Robust prototype serialization may require custom type serializers for dictionaries keyed by `RiStatType` or prototype IDs.
- If dictionaries are awkward in YAML, use list entries such as `{ stat: Strength, multiplier: 1.5 }` instead of raw maps.
- Dotted class IDs like `Human.Fighter` should be validated against Robust prototype ID conventions before finalizing.
- `EnergyCapacity` should be documented in XML comments as legacy `EnergyMod`, not current Energy.
- Tests must run without client UI code.

# Stat system tests

Phase 08 stat tests live in:

```text
Content.Tests/RI/Stats/RiStatSystemTests.cs
```

Run:

```powershell
dotnet test Content.Tests/Content.Tests.csproj
```

Covered cases:

- Human + Medium body uses template values.
- Human + Small body applies explicit multipliers.
- Human + Large body applies explicit multipliers.
- Changeling-like races reject Small/Large body types.
- Alien class variants use replacement templates.
- Formula order is deterministic.
- Creation allocation is permanent additive state, not a hidden multiplier.

Manual prototype-load smoke check:

```powershell
dotnet build
dotnet run --project Content.Server
```

The server must start without prototype deserialization errors for:

```text
Resources/Prototypes/RI/Character/body_types.yml
Resources/Prototypes/RI/Character/races.yml
Resources/Prototypes/RI/Character/classes.yml
Resources/Prototypes/RI/Character/stat_templates.yml
```