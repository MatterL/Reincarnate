# Content Backlog

Labels used here match Phase 03: `Preserve`, `Tune`, `Rewrite`, `Delete`, `Legal Review`, `Needs Network Prototype`, `Needs Balance Sim`.

| Backlog item | Labels | Source files | Extraction result | Target phase |
| --- | --- | --- | --- | --- |
| Character creation race/planet selector | Preserve,Tune,Legal Review | Creation.dm | Convert into prototype-driven UI; review franchise names. | Phase 05/06 |
| Race/class/body stat modifiers | Preserve,Tune,Needs Balance Sim | statpointsystems.dm, Stats.dm | Extract formulas, then normalize into stat bible. | Phase 05/19 |
| Implicit player schema | Rewrite,Preserve | Variables.dm | Turn vars into explicit components/save DTO fields. | Phase 04/07 |
| Training/Meditation/Sparring gain loop | Tune,Needs Balance Sim | Gains.dm, GameEvents/Training.dm | Replace global event queue and icon_state checks with server progression systems. | Phase 10/19 |
| Vitals regen/recovery/oxygen | Preserve,Tune | Gains.dm, Systems/WaterAndOyxgen.dm | Use bounded server ticks and replicated vitals. | Phase 10 |
| KO/Death/Consciousness | Preserve,Rewrite | BattleSystem.dm | Event-driven state machine. | Phase 10 |
| Melee combat | Tune,Needs Network Prototype | BattleSystem.dm | Server-authoritative melee prototype. | Phase 11 |
| Projectiles/beams | Tune,Needs Network Prototype | Projectiles.dm, Skills.dm | Prototype flags and server validation. | Phase 11/12 |
| Skill catalog | Rewrite,Legal Review | Skills.dm | Classify into activation/effect handlers; do not port verbs. | Phase 12 |
| Transformations/forms | Preserve,Tune,Legal Review | TransNew.dm, BeastTransforms.dm, Ascension.dm | Data-driven transformation prototypes. | Phase 13 |
| Buff/status effects | Rewrite | Skills.dm, TransNew.dm, Variables.dm | Generic modifier/status framework. | Phase 13 |
| Items/equipment | Preserve,Tune | Items.dm | Use item/equipment prototypes, explicit item instances. | Phase 14 |
| Technology/crafting/buildables | Tune,Rewrite | Tech.dm, Build.dm, BuildingNewWyatt.dm | Recipe/prototype model; server validation. | Phase 14/15 |
| Enchantment/magic items | Preserve,Tune | Enchantment.dm | Enchantable components and recipes. | Phase 14 |
| Chat channels | Preserve,Rewrite | Chat&Verbs.dm, Utils/ChatUtils | Server-routed channels, logs, spam checks. | Phase 09 |
| RP mode | Preserve,Tune | RPMode.dm, Chat&Verbs.dm, BattleSystem.dm | Explicit state, not embedded in combat/chat branches. | Phase 09/10 |
| Reports/ahelp/notes/logs | Preserve,Rewrite | Reports.dm, Notes.dm, LogSystem/* | Moderation records and audit trail. | Phase 16 |
| Admin verbs | Rewrite | Admin.dm | Permissioned server commands and UI panels. | Phase 16 |
| World save/load | Rewrite | World.dm | Explicit repositories and versioned persistence. | Phase 07/21 |
| Planet/map/spawn system | Preserve,Tune | Creation.dm, Map.dm, SpawnPoints.dm, Planet*.dm | Planet prototypes and spawn point components. | Phase 15 |
| Weather/year/time | Tune | Weather.dm, Years.dm, WorldTimeUtils.dm | World systems with bounded updates. | Phase 15 |

## Definition of extracted

A backlog item is extracted when it has: source files inspected, player-facing behavior summary, data fields to preserve, formulas tagged, target Robust shape, networking/persistence/legal risks, and at least three test cases.
