# Extracted Races and Spawn Planets

Source focus: `Code/Creation.dm`, `Code/statpointsystems.dm`, `Code/Stats.dm`, `Code/Variables.dm`, `Code/TransNew.dm`.

## Creation-screen race availability

| Spawn area / planet | Race | Initial class | Notes |
| --- | --- | --- | --- |
| Earth | Human | default | base race |
| Earth | Spirit Doll | default | base race |
| Earth | Neko | Domestic | base race, class cycle has Schrodinger unlock |
| Earth | Makyo | Fighter | base race |
| Earth | Half Saiyan | Goten | unlock: Half Saiyan |
| Earth | Quarter Saiyan | Fighter | unlock: Quarter Saiyan |
| Earth | Popo | Fighter | unlock: Popo; high legal/content risk |
| Earth | Nobody | Fighter | unlock: Nobody |
| Earth | 1/16th Saiyan | Fighter | unlock: 1/16th Saiyan |
| Earth | Throwback | Fighter | unlock: Throwback |
| Namek | Namekian | Fighter | base race |
| Vegeta | Saiyan | Normal | base race |
| Vegeta | Tsufurujin | Fighter | base race |
| Afterlife | Kaio | Fighter | base race |
| Afterlife | Demi | Fighter | base race |
| Afterlife | Demon | Pride | base race, sin class cycle |
| Afterlife | Hollow | Fighter | unlock: Hollow |
| Afterlife | Half Demon | Pride | unlock: Half Demon |
| Afterlife | Majin | Fighter | unlock: Majin |
| Afterlife | Dragon | Fighter | unlock: Dragon |
| Afterlife | Makaioshin | Courage | unlock: Makaioshin |
| Afterlife | God of Destruction | Fighter | unlock: God of Destruction |
| Afterlife | Mazoku Demon | Fighter | unlock: Mazoku |
| Arconia | Alien | Fighter | base race |
| Arconia | Heran | Fighter | base race |
| Arconia | Youkai | Kitsune | base race, Youkai class cycle |
| Arconia | Lycan | Fighter | unlock: Lycan |
| Arconia | Vampire | Fighter | unlock: Vampire |
| Arconia | Aethirian | Fighter | unlock: Aethirian |
| Arconia | Golem | Fighter | unlock: Golem |
| Android Ship | Android | Fighter | base race |
| Android Ship | Bio Android | Fighter | unlock: Bio |
| Ice | Changeling | Frieza | base race |

## Races with stat branches found in `statpointsystems.dm`

1/16th Saiyan, Aethirian, Alien, Android, Anti-Spiral, Bio Android, Changeling, Demi, Demon, Dragon, Galvan, God of Destruction, Golem, Half Demon, Half Saiyan, Heran, Hollow, Human, Kaio, Lycan, Majin, Makaioshin, Makyo, Mazoku Demon, Morphling, Namekian, Neko, Nobody, Pathfinder, Popo, Quarter Saiyan, Saiyan, Spirit Doll, Throwback, Trueseer, Tsufurujin, Vampire, Youkai

## Extraction notes

- Creation uses numeric `Plan` and `Rac` counters to rotate through planets/races. Rewrite this as explicit `RprPlanetPrototype` + `RprRacePrototype` relations.
- Unlocks are embedded as checks such as `CheckUnlock(...)`. Rewrite as server-authoritative unlock predicates.
- Several names are franchise-derived or meme-heavy. Public-facing content requires rename/legal review before Phase 18.
- Some races appear in stats/blurbs but are not clearly reachable through the active planet cycle, such as `Anti-Spiral`, `Pathfinder`, `Trueseer`, and `Great Old Admin`. Treat them as `Internal`, `Deprecated`, or `Legal Review` until confirmed.

## Target Robust data model

- `RprRacePrototype`: id, displayName, description, defaultSpawnPlanet, defaultClass, allowedClasses, bodyTypeRules, statTemplate, progressionMods, unlockRequirement, tags, legalStatus.
- `RprRaceUnlockComponent` or account/character unlock record: owned unlock IDs.
- `RprCharacterCreationSystem`: server validates race/class/body/spawn choices.

## Immediate Phase 05 candidates

Use a small public-safe sample first: Human, Alien, Namekian-equivalent-originalized, Android-equivalent-originalized. Do not start with the full race list.
