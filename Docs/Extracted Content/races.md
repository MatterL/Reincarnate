# Races Extraction — Phase 05 Updated

Status: updated by Phase 05 final stat bible pass.

## Phase 05 registry correction

The current archive includes `Code/Races/*.dm` race definitions with `planet`, `default_class`, class definitions, race stat profiles, and stat point policies. These are the first candidate source for Phase 08 prototype generation.

`Creation.dm` remains necessary for creation flow, `RaceQualities()`, finalization grants, older fallback behavior, and save handoff, but it is no longer the preferred source for race stat profile numbers when a current registry profile exists.

Legal/public-release name risk is intentionally not assessed in this phase by user instruction. The `legalStatus` column is therefore `NotAssessedPhase05`.

## Race prototype fields

`sourceRaceId | publicNameCandidate | currentRegistryFile | defaultClass | statProfileId | progressionProfileId | startingStatPoints | legalStatus | formulaAuditStatus`

## Current registry race matrix

|sourceRaceId|publicNameCandidate|currentRegistryFile|defaultClass|statProfileId|progressionProfileId|startingStatPoints|legalStatus|formulaAuditStatus|
|---|---|---|---|---|---|---|---|---|
|Aethirian|Aethirian|Code/Races/Aethirian.dm|Fighter|Aethirian|Aethirian|110|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien|Alien|Code/Races/Alien.dm|Fighter|Alien|Alien|70|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Android|Android|Code/Races/Android.dm|Juggernaut|Android|Android|25|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Anti-Spiral|Anti-Spiral|Code/Races/AntiSpiral.dm|Messenger|AntiSpiral|AntiSpiral|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Bio Android|Bio Android|Code/Races/BioAndroid.dm|Fighter|BioAndroid|BioAndroid|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Changeling|Changeling|Code/Races/Changeling.dm|Frieza|Changeling|Changeling|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demi|Demi|Code/Races/Demi.dm|Fighter|Demi|Demi|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon|Demon|Code/Races/Demon.dm|Pride|Demon|Demon|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Dragon|Dragon|Code/Races/Dragon.dm|Fighter|Dragon|Dragon|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|God of Destruction|God of Destruction|Code/Races/GodOfDestruction.dm|Fighter|GodofDestruction|GodofDestruction|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Golem|Golem|Code/Races/Golem.dm|Fighter|Golem|Golem|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Half Demon|Half Demon|Code/Races/HalfDemon.dm|Wrath|HalfDemon|HalfDemon|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Half Saiyan|Half Saiyan|Code/Races/HalfSaiyan.dm|Gohan|HalfSaiyan|HalfSaiyan|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Heran|Heran|Code/Races/Heran.dm|Fighter|Heran|Heran|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Hollow|Hollow|Code/Races/Hollow.dm|Fighter|Hollow|Hollow|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human|Human|Code/Races/Human.dm|Fighter|Human|Human|60|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Kaio|Kaio|Code/Races/Kaio.dm|Fighter|Kaio|Kaio|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Lycan|Lycan|Code/Races/Lycan.dm|Fighter|Lycan|Lycan|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Majin|Majin|Code/Races/Majin.dm|Fighter|Majin|Majin|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makaioshin|Makaioshin|Code/Races/Makaioshin.dm|Courage|Makaioshin|Makaioshin|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makyo|Makyo|Code/Races/Makyo.dm|Fighter|Makyo|Makyo|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Manakete|Manakete|Code/Races/Manakete.dm|Fighter|Manakete|Manakete|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Mazoku|Mazoku|Code/Races/Mazoku.dm|Pride|Mazoku|Mazoku|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Namekian|Namekian|Code/Races/Namekian.dm|Fighter|Namekian|Namekian|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Neko|Neko|Code/Races/Neko.dm|Fighter|Neko|Neko|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan|Saiyan|Code/Races/Saiyan.dm|Normal|Saiyan|Saiyan|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Spirit Doll|Spirit Doll|Code/Races/SpiritDoll.dm|Fighter|SpiritDoll|SpiritDoll|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Tsufurujin|Tsufurujin|Code/Races/Tsufurujin.dm|Fighter|Tsufurujin|Tsufurujin|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Vampire|Vampire|Code/Races/Vampire.dm|Fighter|Vampire|Vampire|30|NotAssessedPhase05|PreserveOrderTuneNumbers|
|Youkai|Youkai|Code/Races/Youkai.dm|Kitsune|Youkai|Youkai|30|NotAssessedPhase05|PreserveOrderTuneNumbers|


## Prototype policy

- Use `RiRacePrototype` with stable IDs based on the current source race ID.
- Keep `sourceRaceId` during migration for auditability.
- Use race-scoped classes, such as `Human.Fighter`, `Alien.Technologist`, and `Android.Juggernaut`.
- Do not implement `GenRaces` in Phase 08.
