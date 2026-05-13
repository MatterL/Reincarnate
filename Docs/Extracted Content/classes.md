# Classes Extraction — Phase 05 Updated

Status: updated by Phase 05 final stat bible pass.

## Phase 05 class prototype policy

Classes are race-scoped whenever behavior differs by race. Do not use one global string `Fighter` when the current registry defines `Human:Fighter`, `Alien:Fighter`, `Android:Fighter`, etc.

Rewrite ID format:

```text
<SourceRaceId>.<SourceClassNameWithoutSpacesOrPunctuation>
```

Examples:

- `Human.Fighter`
- `Human.Sage`
- `Alien.Technologist`
- `Android.Juggernaut`
- `Namekian.Ancient`

## Class prototype fields

`classId | displayName | sourceClassName | allowedRaceIds | statMode | statTemplate | unlockRequirement | grants | legalStatus | auditStatus`

## Current class matrix

|classId|displayName|sourceClassName|allowedRaceIds|statMode|statTemplate|unlockRequirement|grants|legalStatus|auditStatus|
|---|---|---|---|---|---|---|---|---|---|
|Aethirian.Fighter|Fighter|Fighter|Aethirian|Base|Aethirian|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien.Fighter|Fighter|Fighter|Alien|Replacement|Alien.Fighter|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien.Healer|Healer|Healer|Alien|Replacement|Alien.Healer|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien.Technologist|Technologist|Technologist|Alien|Replacement|Alien.Technologist|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien.Wizard|Wizard|Wizard|Alien|Replacement|Alien.Wizard|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien.BlueMage|Blue Mage|Blue Mage|Alien|Replacement|Alien.BlueMage|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Alien.Spiritualist|Spiritualist|Spiritualist|Alien|Replacement|Alien.Spiritualist|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Android.Juggernaut|Juggernaut|Juggernaut|Android|Replacement|Android.Juggernaut|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Android.Hunter|Hunter|Hunter|Android|Replacement|Android.Hunter|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Android.Engineer|Engineer|Engineer|Android|Replacement|Android.Engineer|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Android.Slayer|Slayer|Slayer|Android|Replacement|Android.Slayer|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Android.Fighter|Fighter|Fighter|Android|Replacement|Android.Fighter|Old Model Wardroid||NotAssessedPhase05|PreserveOrderTuneNumbers|
|AntiSpiral.Messenger|Messenger|Messenger|Anti-Spiral|Base|Anti-Spiral|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|AntiSpiral.Scout|Scout|Scout|Anti-Spiral|Base|Anti-Spiral|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|AntiSpiral.Destroyer|Destroyer|Destroyer|Anti-Spiral|Base|Anti-Spiral|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|AntiSpiral.King|King|King|Anti-Spiral|Base|Anti-Spiral|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|BioAndroid.Fighter|Fighter|Fighter|Bio Android|Base|Bio Android|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Changeling.Frieza|Frieza|Frieza|Changeling|Adjustment|Changeling.Frieza|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Changeling.Cooler|Cooler|Cooler|Changeling|Adjustment|Changeling.Cooler|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Changeling.KingKold|King Kold|King Kold|Changeling|Adjustment|Changeling.KingKold|King Kold||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Changeling.Chilled|Chilled|Chilled|Changeling|Adjustment|Changeling.Chilled|King Kold||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demi.Fighter|Fighter|Fighter|Demi|Base|Demi|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Pride|Pride|Pride|Demon|Replacement|Demon.Pride|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Envy|Envy|Envy|Demon|Replacement|Demon.Envy|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Gluttony|Gluttony|Gluttony|Demon|Replacement|Demon.Gluttony|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Greed|Greed|Greed|Demon|Replacement|Demon.Greed|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Hellfire|Hellfire|Hellfire|Demon|Replacement|Demon.Hellfire|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Lust|Lust|Lust|Demon|Replacement|Demon.Lust|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Sloth|Sloth|Sloth|Demon|Replacement|Demon.Sloth|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Demon.Wrath|Wrath|Wrath|Demon|Replacement|Demon.Wrath|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Dragon.Fighter|Fighter|Fighter|Dragon|Base|Dragon|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|GodofDestruction.Fighter|Fighter|Fighter|God of Destruction|Replacement|GodofDestruction.Fighter|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|GodofDestruction.Locked|Locked|Locked|God of Destruction|Replacement|GodofDestruction.Locked|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Golem.Fighter|Fighter|Fighter|Golem|Base|Golem|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Wrath|Wrath|Wrath|Half Demon|Replacement|HalfDemon.Wrath|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Pride|Pride|Pride|Half Demon|Replacement|HalfDemon.Pride|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Gluttony|Gluttony|Gluttony|Half Demon|Replacement|HalfDemon.Gluttony|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Sloth|Sloth|Sloth|Half Demon|Replacement|HalfDemon.Sloth|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Lust|Lust|Lust|Half Demon|Replacement|HalfDemon.Lust|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Greed|Greed|Greed|Half Demon|Replacement|HalfDemon.Greed|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfDemon.Envy|Envy|Envy|Half Demon|Replacement|HalfDemon.Envy|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfSaiyan.Gohan|Gohan|Gohan|Half Saiyan|Replacement|HalfSaiyan.Gohan|Gohan||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfSaiyan.Goten|Goten|Goten|Half Saiyan|Replacement|HalfSaiyan.Goten|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|HalfSaiyan.Trunks|Trunks|Trunks|Half Saiyan|Replacement|HalfSaiyan.Trunks|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Heran.Fighter|Fighter|Fighter|Heran|Replacement|Heran.Fighter|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Heran.Captain|Captain|Captain|Heran|Replacement|Heran.Captain|Captain||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Hollow.Fighter|Fighter|Fighter|Hollow|Base|Hollow|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Fighter|Fighter|Fighter|Human|Base|Human|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Technologist|Technologist|Technologist|Human|Base|Human|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Wizard|Wizard|Wizard|Human|Base|Human|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Deus|Deus|Deus|Human|Adjustment|Human.Deus|Deus||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Dhampir|Dhampir|Dhampir|Human|Adjustment|Human.Dhampir|Vampire||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Shifter|Shifter|Shifter|Human|Base|Human|Shifter||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Quincy|Quincy|Quincy|Human|Replacement|Human.Quincy|Quincy||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Sage|Sage|Sage|Human|Replacement|Human.Sage|Sage||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Chara|Chara|Chara|Human|Adjustment|Human.Chara|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Human.Frisk|Frisk|Frisk|Human|Adjustment|Human.Frisk|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Kaio.Fighter|Fighter|Fighter|Kaio|Base|Kaio|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Kaio.Healer|Healer|Healer|Kaio|Base|Kaio|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Kaio.Wizard|Wizard|Wizard|Kaio|Base|Kaio|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Lycan.Fighter|Fighter|Fighter|Lycan|Base|Lycan|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Lycan.UltimateLycan|Ultimate Lycan|Ultimate Lycan|Lycan|Adjustment|Lycan.UltimateLycan|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Majin.Fighter|Fighter|Fighter|Majin|Base|Majin|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makaioshin.Courage|Courage|Courage|Makaioshin|Replacement|Makaioshin.Courage|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makaioshin.Power|Power|Power|Makaioshin|Replacement|Makaioshin.Power|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makaioshin.Wisdom|Wisdom|Wisdom|Makaioshin|Replacement|Makaioshin.Wisdom|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makaioshin.Bibarel|Bibarel|Bibarel|Makaioshin|Replacement|Makaioshin.Bibarel|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makyo.Fighter|Fighter|Fighter|Makyo|Base|Makyo|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Makyo.Wizard|Wizard|Wizard|Makyo|Base|Makyo|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Manakete.Fighter|Fighter|Fighter|Manakete|Base|Manakete|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Manakete.Mage|Mage|Mage|Manakete|Base|Manakete|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Mazoku.Pride|Pride|Pride|Mazoku|Base|Mazoku|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Namekian.Fighter|Fighter|Fighter|Namekian|Base|Namekian|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Namekian.Ancient|Ancient|Ancient|Namekian|Replacement|Namekian.Ancient|Ancient||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Namekian.Healer|Healer|Healer|Namekian|Base|Namekian|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Neko.Fighter|Fighter|Fighter|Neko|Base|Neko|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Neko.Domestic|Domestic|Domestic|Neko|Base|Neko|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Neko.Schrodinger|Schrodinger|Schrodinger|Neko|Base|Neko|Schrodinger||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.Normal|Normal|Normal|Saiyan|Replacement|Saiyan.Normal|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.LowClass|Low-Class|Low-Class|Saiyan|Replacement|Saiyan.LowClass|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.Elite|Elite|Elite|Saiyan|Replacement|Saiyan.Elite|Elite||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.Legendary|Legendary|Legendary|Saiyan|Replacement|Saiyan.Legendary|Legendary||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.Hellspawn|Hellspawn|Hellspawn|Saiyan|Replacement|Saiyan.Hellspawn|Hellspawn||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.FireGod|Fire God|Fire God|Saiyan|Replacement|Saiyan.FireGod|Fire God||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.Savage|Savage|Savage|Saiyan|Replacement|Saiyan.Savage|Savage||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Saiyan.FallenAngel|Fallen Angel|Fallen Angel|Saiyan|Replacement|Saiyan.FallenAngel|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|SpiritDoll.Fighter|Fighter|Fighter|Spirit Doll|Base|Spirit Doll|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|SpiritDoll.Wizard|Wizard|Wizard|Spirit Doll|Base|Spirit Doll|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Tsufurujin.Fighter|Fighter|Fighter|Tsufurujin|Base|Tsufurujin|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Tsufurujin.Technologist|Technologist|Technologist|Tsufurujin|Base|Tsufurujin|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Vampire.Fighter|Fighter|Fighter|Vampire|Base|Vampire|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Vampire.Volodarskii|Volodarskii|Volodarskii|Vampire|Base|Vampire|Volodarskii||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Youkai.Kitsune|Kitsune|Kitsune|Youkai|Replacement|Youkai.Kitsune|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Youkai.Tanuki|Tanuki|Tanuki|Youkai|Replacement|Youkai.Tanuki|||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Youkai.HellRaven|Hell Raven|Hell Raven|Youkai|Replacement|Youkai.HellRaven|Hell Raven||NotAssessedPhase05|PreserveOrderTuneNumbers|
|Youkai.Fairy|Fairy|Fairy|Youkai|Replacement|Youkai.Fairy|||NotAssessedPhase05|PreserveOrderTuneNumbers|


## Notes

- `statProfileMode = Replacement` means class stats override the base race stat set.
- `statProfileMode = Adjustment` means base race stats are applied first, then named fields are adjusted.
- Generic legacy `ClassBodyStats()` class modifiers such as Wizard/Healer/Technologist should become explicit class overlays instead of C# string checks.
