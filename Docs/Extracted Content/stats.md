# Extracted Stat Vocabulary and Race Stat Branches

Source focus: `Code/Variables.dm`, `Code/statpointsystems.dm`, `Code/Stats.dm`, `Code/Gains.dm`.

## Core fields to preserve as concepts

- Identity/progression: Race, Class, BodyType, Base, BaseMod, Power_Multiplier, RPPower, EXP, Potential.
- Vitals/resources: Health, Energy, EnergyMax, Oxygen, MaxOxygen, ManaAmount, ManaMax.
- Combat stats: Strength, Endurance, Force, Resistance, Speed, Offense, Defense.
- Growth modifiers: StrengthMod, EnduranceMod, ForceMod, ResistanceMod, SpeedMod, OffenseMod, DefenseMod, EnergyMod.
- Recovery/modifiers: Regeneration, Recovery, Anger, AngerMax, Training_Rate, Meditation_Rate, Intelligence, Enchantment.
- Temporary multipliers: StrengthMultiplier, EnduranceMultiplier, SpeedMultiplier, ForceMultiplier, ResistanceMultiplier, OffenseMultiplier, DefenseMultiplier, RegenerationMultiplier, RecoveryMultiplier.

## Race stat branch inventory

The table below is a machine-assisted first pass. For races with `Class variants = Yes`, values can depend on class and must be verified in Phase 05 before becoming balance truth.

| Race | Energy | Strength | Endurance | Speed | Force | Resistance | Offense | Defense | Regen | Recovery | Anger | Class variants |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1/16th Saiyan | 1.5 | 2.5 | 2.5 | 1.25 | 1.5 | 0.75 | 1.75 | 1.75 | 0.5 | 2 | 0.5 | No |
| Aethirian | 2 | 1.75 | 1 | 2 | 1.5 | 0.5 | 0.8 | 3 | 1 | 5 | 0.1 | Yes |
| Alien | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.3 | 0.4 | Yes |
| Android | 0.75 | 0.25 | 0.25 | 0.25 | 0.25 | 0.25 | 0.25 | 0.25 | 0.25 | 0.5 |  | No |
| Anti-Spiral | 2 | 2 | 2 | 1 | 3.5 | 2 | 1 | 1 | 1 | 0 | 0.1 | No |
| Bio Android | 1.5 | 1.5 | 1.5 | 1.5 | 1.5 | 1.5 | 1.5 | 1.5 | 4 | 4 | 0.75 | No |
| Changeling | 0.4 | 2.75 | 4.5 | 1.1 | 0.75 | 4.5 | 0.25 | 0.25 | 0.75 | 1 | 0.15 | Yes |
| Demi | 0.6 | 2 | 2 | 0.25 | 0 | 2 | 0.5 | 1 | 1 | 0.2 | 0.5 | No |
| Demon | 0.5 | 3 | 2 | 1 | 1 | 1.5 | 1.3 | 0.5 | 1 | 0 | 0.5 | Yes |
| Dragon | 2 | 1 | 2 | 1.2 | 1 | 2 | 1 | 1 | 2 | 2 | 1 | No |
| Galvan | 1.5 | -0.5 | -0.5 | 4 | -0.75 | -0.5 | 0 | 6 | 1.1 | 4 | -0.25 | Yes |
| God of Destruction | 2 | 3.5 | 1.5 | 1 | 2.5 | 1.5 | 1 | 1 | 1 | 0 | 0.1 | No |
| Golem | 1 | 3 | 2.5 | 0.5 | 1.5 | 2 | 1.5 | 1 | -0.5 | 1 | 0.25 | No |
| Half Demon | 1 | 3 | 3 | 1.25 | 1 | 1.5 | 1.5 | 0.75 | 0.75 | 0.75 | 0.5 | Yes |
| Half Saiyan | 0.75 | 4 | 2 | 1.25 | 1.5 | 2 | 1.75 | 1.25 | 0.5 | 0.5 | 0.25 | Yes |
| Heran | 1.5 | 2 | 2.5 | 0.5 | 1.5 | 3.5 | 1.75 | 0.75 | 0.8 | 1.5 | 0.6 | No |
| Hollow | 1 | 1.25 | 1.25 | 0.5 | 1.25 | 1.25 | 1.25 | 1.25 | 1 | 0.25 | 0.5 | No |
| Human | 2.5 | 1.5 | 0.75 | 2 | 2.5 | 2.5 | 1.75 | 1.75 | 0.5 | 2 | 0.2 | Yes |
| Kaio | 4 | 1.75 | 1.75 | 1 | 1.75 | 1.75 | 1.5 | 1.5 | 0 | 2 | 0.1 | No |
| Lycan | 1 | 2.15 | 2.15 | 1 | 0.75 | 1.75 | 1 | 1 | 1.0 | 0.5 | 0.75 | No |
| Majin | 1.5 | 2.75 | 2.5 | 1.5 | 2.5 | 1.5 | 1.5 | -0.5 | 6.5 | 6.5 | 0.5 | No |
| Makaioshin | 4 | 4 | 4 | 2.5 | 40 | 4 | 9 | 9 | 9 | 14 | 10 | Yes |
| Makyo | 1.5 | 3 | 3 | 1.5 | 1 | 3 | 0.5 | 0.5 | 0.5 | 0.25 | 0.3 | No |
| Mazoku Demon | 2 | 3 | 3 | 1 | 2.5 | 3 | 1.75 | 1.75 | 1 | 0 | 2 | No |
| Morphling |  |  |  |  |  |  |  |  |  |  |  | No |
| Namekian | 1.5 | 1.5 | 2 | 1 | 1 | 2 | 1.5 | 1 | 4 | 1 | 0.25 | Yes |
| Neko | 1 | 2.5 | 0 | 2 | 0.75 | 0 | 1.5 | 1 | 1 | 2 | 0.05 | No |
| Nobody | 2 | 3 | 2 | 1.5 | 2 | 2 | 1.5 | 1.5 | 0.5 | 2 | 0.05 | No |
| Pathfinder | 2 | 2 | 2 | 1.5 | 2 | 2 | 2 | 2 | 2 | 2 | 0.5 | No |
| Popo | 6.5 | 6.5 | 6.5 | 6.5 | 6.5 | 6.5 | 6.5 | 6.5 | 6.5 | 6.5 | 4 | No |
| Quarter Saiyan | 1 | 2.5 | 2.5 | 1.25 | 1 | 0.75 | 2 | 2 | 1 | 1.5 | 2 | No |
| Saiyan | 0.5 | 4 | 1.5 | 1.5 | 2.5 | 1.5 | 1.75 | 1.5 | 1 | 1 | 0.5 | Yes |
| Spirit Doll | 2.5 | 1.25 | 1 | 2.5 | 1 | 0.75 | 1.5 | 1.5 | 0.3 | 3 | 0.4 | No |
| Throwback | 2 | 2 | 2 | 2 | 1 | 0.75 | 1.5 | 1.5 | 2 | 1.5 | 0.5 | No |
| Trueseer | 2.5 | 1.5 | 2.5 | 1.5 | 3.0 | 2.5 | 2.0 | 2.0 | 0.75 | 2.0 | 0.25 | No |
| Tsufurujin | 2 | 1.5 | 2.25 | 0.85 | 2 | 2.25 | 1 | 1.3 | 0.75 | 1.5 | 0.5 | No |
| Vampire | 1.5 | 2.5 | 1.25 | 2 | 2 | 1.25 | 2.5 | 2.5 | 1.5 | 1 | 1 | No |
| Youkai | 2 | 1 | 1 | 2 | 2.5 | 2 | 1.25 | 1.75 | 0.5 | 3 | 0.5 | Yes |

## Important formula concepts

| Concept | Extracted rule | Phase 05/19 status |
|---|---|---|
| Body modifier application | Race/class stats are computed first, then body/class modifiers are applied. | Preserve order, tune numbers later. |
| Stat points | Most races receive 10 stat points; Alien and Morphling 90, Android 70, Hollow 30. | Tune/Balance Sim. |
| Growth catch-up | `GetPowerRank` compares player stats/BP/energy to online population. | Rewrite carefully; avoid world scans. |
| Hard/soft caps | Training uses effort, average effort, world softcap settings. | Preserve concept; rewrite formulas into tested service. |
| Regen/recovery | Regeneration restores health; Recovery restores energy. | Preserve, server-owned. |

## Target Robust shape

- `RprStatType` enum.
- `RprStatBlock` serializable struct/dictionary.
- `RprStatModifier` records with source ID and operation.
- `RprStatSystem` deterministic calculation service in shared code.
- `RprStatsComponent` for replicated current values and base values.
- `RprProgressionComponent` for EXP/training/meditation values.
- Balance tests and a standalone balance simulator.
