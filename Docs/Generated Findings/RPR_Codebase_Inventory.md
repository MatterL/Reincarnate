# RoleplayRebirth Codebase Inventory

Assessment date: 2026-05-12

## Scope

Uploaded file assessed: `roleplayrebirth.zip`.

The zip contains BYOND/DM source and a DME project file. It does not contain the full playable project assets referenced by the DME, such as the interface and map files noted in the project context. This assessment is therefore static/source-based.

## Static metrics

Approximate source metrics from the uploaded package:

| Metric | Count |
|---|---:|
| `.dm` files | 90 |
| Total DM lines | ~59,261 |
| Proc definitions / proc-like declarations | ~449 |
| Verb definitions / verb-like declarations | ~551 |
| `mob` type definitions | ~388 |
| `obj` type definitions | ~130 |
| `turf` type definitions | ~18 |
| `area` type definitions | ~2 |
| `spawn()` calls | ~895 |
| `sleep()` calls | ~596 |
| `for(... in world)` style loops | ~229 |
| `input()` calls | ~696 |
| `browse()` calls | ~49 |
| `winset()` calls | ~252 |
| `savefile` references | ~46 |

These counts are regex-based and should be treated as orientation rather than compiler-grade facts.

## Largest files

| File | Approx. lines | Notes |
|---|---:|---|
| `Code/Skills.dm` | 13,563 | Largest hotspot; skills, verbs, effects, likely many timing loops. |
| `Code/Tech.dm` | 4,684 | Technology/crafting/science features and many admin/input interactions. |
| `Code/Map.dm` | 4,356 | Map/turf/planet/world logic. |
| `Code/TransNew.dm` | 2,889 | Transformations/forms. |
| `Code/Creation.dm` | 2,732 | Character creation, race/class/body setup, saving/loading. |
| `Code/Admin.dm` | 2,501 | Admin verbs/tools. |
| `Code/Gains.dm` | 2,370 | Progression, gain loop, regen/status timers. |
| `Code/Enchantment.dm` | 2,144 | Enchantment system. |
| `Code/Items.dm` | 2,032 | Items/equipment/interactions. |
| `Code/BattleSystem.dm` | 1,865 | KO, conscious/unconscious, combat state, death-related flow. |
| `Code/statpointsystems.dm` | 1,446 | Stat point distribution and race/class/body stat changes. |
| `Code/Customization.dm` | 1,423 | Customization UI/content. |
| `Code/World.dm` | 1,160 | World boot, global save/load, periodic world systems. |
| `Code/Stats.dm` | 1,109 | Stat display/calculation helpers. |
| `Code/Chat&Verbs.dm` | 912 | Chat verbs and player-facing commands. |
| `Code/Projectiles.dm` | 708 | Projectile object, movement, collision, damage effects. |
| `Code/Variables.dm` | 671 | Large global/mob variable bag. |

## Main architectural shape of the BYOND code

The RPR codebase follows classic BYOND/DM patterns:

- large `mob/var` and global variable bags;
- inheritance-heavy `mob`, `obj`, `turf`, and `area` types;
- many player-facing verbs;
- modal `input()` prompts;
- browser/skin UI via `browse()` and `winset()`;
- persistent world and player data via BYOND `savefile` serialization;
- asynchronous behavior via `spawn()` and `sleep()`;
- world queries via `for(... in world)`, `view()`, `range()`, and object-location checks.

This is natural for BYOND, but it is exactly what should be avoided in a RobustToolbox rewrite.

## DME/project observations

The DME includes all major code files and references interface/map resources that were not present in the upload. Because those assets are missing, this assessment did not attempt a BYOND compile/run.

The world configuration in `Code/World.dm` defines the game as Roleplay Rebirth, sets tick/fps/view-style BYOND runtime values, and starts boot/save systems in `world.New()`. It also contains a hardcoded secret/credential. Do not publish that value; remove and rotate it if it was ever used.

## Persistence model found

Character persistence is based around BYOND savefiles and whole-mob serialization. `Creation.dm` saves the player's current mob object into a savefile and stores additional metadata such as last-used state and a stat-derived integrity hash.

For RobustToolbox, this should be replaced by explicit, versioned save schema such as:

- account ID / character ID;
- selected race/class/body type;
- base stats and stat experience;
- skills known and skill experience;
- active/inactive transformations unlocked;
- inventory/equipment references by prototype ID and instance data;
- location/map/grid coordinates;
- admin/rewarder permissions outside the character save;
- migration version.

Do not serialize live entity objects as the long-term save format.

## Character creation and stats

The character system contains a large amount of useful design material:

- race selection;
- class selection;
- body type selection;
- race/class/body stat modifiers;
- stat-point distribution;
- intelligence/enchantment/training/meditation/zenkai rate modifiers;
- environmental tolerances and spawn planets;
- hard gates and special cases for races/classes.

Races identified in the creation/stat logic include:

`Human`, `Throwback`, `Neko`, `Golem`, `Youkai`, `Quarter Saiyan`, `1/16th Saiyans`, `Popo`, `Nobody`, `Spirit Doll`, `Tsufurujin`, `Makyo`, `Namekian`, `Saiyan`, `Half Saiyan`, `Changeling`, `Vampire`, `Lycan`, `Alien`, `Trueseer`, `Galvan`, `Heran`, `Kaio`, `Demon`, `Hollow`, `Mazoku Demon`, `Half Demon`, `Dragon`, `Aethirian`, `God of Destruction`, `Anti-Spiral`, `Bio Android`, `Majin`, `Makaioshin`, `Demi`, `Android`, `Pathfinder`.

This is an ideal candidate for a data-driven system:

- `RacePrototype`
- `ClassPrototype`
- `BodyTypePrototype`
- `StatTemplatePrototype`
- `ProgressionCurvePrototype`

## Progression/gains

`Code/Gains.dm` is one of the most important files for modernization. It contains:

- online power ranking;
- intelligence gain formulas;
- base/power/stat EXP growth;
- regen and recovery logic;
- KO/revival-related progression hooks;
- a per-character gain loop;
- training event logic that updates every few ticks.

The core design idea is valuable, but the implementation should be split into Robust systems:

- `RprProgressionSystem`
- `RprTrainingSystem`
- `RprRegenSystem`
- `RprStatusEffectSystem`
- `RprPowerRankingSystem`
- `RprCharacterAgingSystem`, if age/decline/prime systems remain.

Avoid recreating one infinite loop per player. Use systems with ECS queries, timers, event subscriptions, and explicit tick/update budgets.

## Combat and KO/death flow

`Code/BattleSystem.dm` handles KO/unconscious/conscious transitions, transformation/buff cleanup, hostility/RP mode implications, energy/health recovery, and special-case class/race behavior.

This is high-value design material but should be normalized as state machines and events:

- `CombatStateComponent`
- `HealthEnergyComponent`
- `KnockoutComponent`
- `DeathStateComponent`
- `Aggression/RpModeComponent`
- `TransformationComponent`
- events such as `BeforeKnockedOutEvent`, `KnockedOutEvent`, `RegainedConsciousnessEvent`, `BeforeDeathEvent`, `DiedEvent`.

## Projectiles and action combat

`Code/Projectiles.dm` is a critical risk area. RPR projectiles are BYOND `obj`s with many flags and damage effects: knockback, explosive behavior, piercing, paralysis, cutting, gun behavior, penetration, elemental interactions, lethality, controllability, and more.

In RobustToolbox, projectiles should be authoritative server entities with carefully chosen predicted visuals. For high-speed combat, do not wait until late development to test projectile prediction, reconciliation, lag, and spam load.

Suggested rewrite model:

- `RprProjectileComponent` for data;
- `RprProjectileSystem` in Shared for predicted movement where safe;
- server validation of hit, damage, status effects, and resource costs;
- client-only effects for particles, trails, beam anticipation, and impact feedback;
- cooldown/rate limit systems to prevent abuse.

## Movement

`PlayerMovement/Move.dm` overrides BYOND movement and applies many game-state checks: KO, RP mode, meditation, frozen state, movement locks, health/energy cost, aura, flight, swimming, chill, items, grabs, AFK status, edge checks, and terrain restrictions.

In RobustToolbox, movement-affecting state should be split across components and systems rather than placed inside one giant movement override.

## World and boot systems

`Code/World.dm` starts global save/load, stars, years, day/night, build systems, customizations, technology, enchantments, reports, and savable objects. It also periodically announces world saves and saves players/world data.

In RobustToolbox, this becomes:

- startup systems;
- prototype loading;
- server configuration;
- database-backed persistence;
- map initialization;
- scheduled server events;
- admin/logging systems;
- no recursive BYOND-style save proc loops.

## Chat, admin, and moderation

RPR has substantial chat and admin tooling:

- OOC/LOOC/Say/Yell/Emote-style communication;
- local range chat and comm-related mechanics;
- admin permission levels;
- admin verbs;
- reports, notes, logs, bans, and rewarder/admin state.

This is a good fit for RobustToolbox/SS14 patterns, but all player/admin commands should be server-authoritative and permission-checked. Anything originally implemented as a BYOND verb should be converted into a command, interaction, UI action, or admin panel.

## Content systems discovered

High-level systems visible in the source include:

- races/classes/body types;
- stat point systems;
- global soft caps / hard caps;
- transformations/forms;
- skill tree and many skills;
- technology/science/crafting;
- enchantment;
- building/ships/doors;
- planets/world settings/day-night/year/weather-like systems;
- dragonballs;
- alliances;
- RP mode and hostility logic;
- profiles, mates, offspring/family-ish systems;
- admin, notes, reports, logs, bans;
- offline players/catch-up concepts;
- projectiles, combat, KO, flight, grabbing, oxygen/water/terrain effects.

## Directly reusable material

Directly reusable as design/specification:

- race/class/body stat modifier tables;
- stat growth formulas after review and cleanup;
- soft-cap/hard-cap ideas;
- skill names, categories, effects, cooldown concepts;
- transformation names and mechanical intent;
- chat channel semantics;
- admin role model;
- planet/world feature list;
- crafting/enchantment recipes and progression ideas.

Not directly reusable:

- BYOND UI code;
- BYOND savefiles;
- `spawn()`/`sleep()` loop architecture;
- global variable soup;
- hardcoded world scans;
- verb declarations as gameplay API;
- hidden BYOND runtime semantics;
- any hardcoded secrets.


## Sources consulted

- RobustToolbox repository: https://github.com/space-wizards/RobustToolbox
- Space Station 14 repository: https://github.com/space-wizards/space-station-14
- RobustToolboxTemplate repository: https://github.com/space-wizards/RobustToolboxTemplate
- SS14/Robust ECS docs: https://docs.spacestation14.com/en/robust-toolbox/ecs.html
- SS14 basic networking docs: https://docs.spacestation14.com/en/ss14-by-example/basic-networking-and-you.html
- SS14 prediction guide: https://docs.spacestation14.com/en/ss14-by-example/prediction-guide.html
- Robust grids docs: https://docs.spacestation14.com/en/robust-toolbox/transform/grids.html
- Robust physics docs: https://docs.spacestation14.com/en/robust-toolbox/transform/physics.html
- Robust net entities docs: https://docs.spacestation14.com/en/robust-toolbox/netcode/net-entities.html
- SS14 codebase organization docs: https://docs.spacestation14.com/en/general-development/codebase-info/codebase-organization.html
- RobustToolbox GPL/closed-source discussion, for legal-review awareness only: https://github.com/space-wizards/RobustToolbox/discussions/5728

