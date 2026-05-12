# Phase 03 DM Content Extraction — RoleplayRebirth to Reincarnate

This package is a first-pass DM-to-design extraction from the uploaded `roleplayrebirth.zip` source. It is intentionally **not** a port. It identifies game facts, rules, data, UI flows, and risks that should become RobustToolbox prototypes, components, systems, UI states, persistence records, and tests.

## What was inspected

- DM files inspected: **90**
- Largest design hotspots by bytes: Code/Skills.dm, Code/Tech.dm, Code/Gains.dm, Code/Admin.dm, Code/Creation.dm, Code/TransNew.dm, Code/Guides.dm, Code/Map.dm, Code/Enchantment.dm, Code/Items.dm
- Highest-risk BYOND implementation patterns found:

| Pattern | Count | Rewrite treatment |
| --- | --- | --- |
| spawn | 1007 | systems, timers, events, scheduled updates |
| sleep | 604 | cooldowns, accumulators, timer components |
| verb | 645 | commands, interactions, UI actions, admin tools |
| input | 701 | client UI + server validation |
| winset | 252 | native Robust UI bound state |
| winshow | 101 | native Robust UI window control |
| browse | 49 | native UI panels |
| savefile | 46 | explicit versioned persistence |
| world | 556 | ECS/spatial/map-local queries |
| view | 287 | spatial/PVS-aware queries |
| range | 79 | spatial/PVS-aware queries |

## Deliverables in this extraction package

- `extraction-template.md` — reusable source-to-design template.
- `source-map.md` — full DM file inventory with counts and target rewrite buckets.
- `content-backlog.md` — system backlog labeled Preserve/Tune/Rewrite/Delete/Legal Review/Needs Network Prototype/Needs Balance Sim.
- `races.md`, `classes.md`, `body-types.md`, `stats.md` — character identity/stat extraction.
- `training.md` — training, meditation, passive gain, fatigue, rank, and regen extraction.
- `combat.md`, `projectiles.md` — KO/death/melee/projectile extraction and networking risks.
- `skills.md`, `transformations.md` — skill catalog and form/status conversion strategy.
- `chat-rp.md` — OOC/LOOC/say/yell/emote/RP mode extraction.
- `admin-moderation-logging.md` — admin roles, commands, reports, notes, logs.
- `items-tech-enchanting.md` — item/equipment/tech/enchantment/buildable extraction.
- `worlds-maps-environment.md` — planets, maps, spawn points, weather, oxygen/water/hazards.
- `persistence-candidates.md` — explicit save record candidates from implicit DM variables.
- `first-yaml-drafts.md` — starter YAML-style prototype drafts for Phase 05/06 planning.

## Non-porting rule for this package

Copy behavior, not DM structure. A variable or proc appears here only as a clue to a design concept. Do not copy raw DM implementation style into C#.
