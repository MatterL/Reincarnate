# Extracted Classes

Source focus: `Code/Creation.dm`, `Code/statpointsystems.dm`, `Code/TransNew.dm`, `Code/Skills.dm`.

## Class cycles from creation UI

| Race/context | Classes in cycle | Source concept |
| --- | --- | --- |
| Generic | Fighter, Technologist, Healer, Wizard, Deus, Dhampir, Shifter, Quincy, Sage | Creation class-cycle logic |
| Changeling | Frieza, Cooler, Chilled, King Kold | Creation class-cycle logic |
| Anti-Spiral | Messenger, Scout, Destroyer, King | Creation class-cycle logic |
| Neko | Domestic, Schrodinger | Creation class-cycle logic |
| Youkai | Kitsune, Tanuki, Hell Raven | Creation class-cycle logic |
| Golem | Fighter, Technologist, Healer, Wizard | Creation class-cycle logic |
| Namekian | Fighter, Healer, Technologist, Ancient | Creation class-cycle logic |
| Makaioshin | Courage, Power, Wisdom | Creation class-cycle logic |
| Saiyan | Low-Class, Normal, Elite, Legendary, Hellspawn, Fire God, Savage | Creation class-cycle logic |
| Half Saiyan | Gohan, Goten, Trunks | Creation class-cycle logic |
| Demon / Half Demon | Wrath, Pride, Gluttony, Sloth, Lust, Greed, Envy | Creation class-cycle logic |
| Galvan | Prime, Albedo | Creation class-cycle logic |
| Alien | Fighter, Technologist, Healer, Wizard, Blue Mage, Vampire, Lycan, Bojack | Creation class-cycle logic |

## Classes referenced in stat branches

Albedo, Ancient, Bibarel, Blue Mage, Courage, Dhampir, Domestic, Elite, Envy, Fighter, Fire God, Freelancer, Gluttony, Gohan, Goten, Greed, Healer, Hell Raven, Hellspawn, Kitsune, Legendary, Low-Class, Lust, Normal, Power, Pride, Prime, Quarter, Quincy, Sage, Savage, Schrodinger, Sloth, Spiritualist, Tanuki, Technologist, Trunks, Warrior, Wisdom, Wrath

## Rewrite guidance

- A class should not be a string switched over in random systems. Use `RprClassPrototype` with tags and modifiers.
- Race-specific classes should be represented as allowed class IDs or class groups.
- Unlock-only classes should be hidden until server validation confirms the unlock.
- Classes that only exist to enable a form/skill should probably be tags or requirements rather than new code paths.

## Candidate prototype fields

- `id`, `displayName`, `description`
- `allowedRaceIds` or `requiredRaceTags`
- `statMods` and `progressionMods`
- `skillGrantIds`
- `transformationPathIds`
- `unlockRequirementId`
- `legalStatus`
