# RoleplayRebirth → RobustToolbox Feasibility Assessment

Assessment date: 2026-05-12

## Verdict

**Viable, but only as a rebuild.** RoleplayRebirth's BYOND source is useful as a design archive: races, classes, body types, stat growth, transformations, skills, crafting, enchantment, planet/world concepts, RP systems, admin tools, and progression pacing. It should not become the backend for the new game.

The most realistic modernization path is:

1. Start from `RobustToolboxTemplate`, not from a full Space Station 14 fork.
2. Recreate RPR mechanics as data-driven C# ECS systems and YAML prototypes.
3. Borrow architectural patterns from Space Station 14 where they help: networking, prediction, chat, UI, admin permissions, entity interaction, maps, physics, and prototypes.
4. Keep RPR's content concepts but rewrite the runtime model around server authority, prediction, component state replication, and explicit save schemas.

## Why RobustToolbox is a good fit

RobustToolbox is the multiplayer engine used by Space Station 14, and its README describes it as an engine being developed for SS14 while also being made usable for other singleplayer and multiplayer projects. Its own README also stresses that the engine needs a content repo rather than running alone, which maps well to treating RPR as content/design rather than engine code.

The Space Station 14 repo is a complete C# RobustToolbox game/content repo and is therefore useful as a working reference implementation. The separate RobustToolbox template is especially relevant because it is a barebones starting point for a new game with a functional client/server split and examples for components, systems, and prototypes.

For a developer who dislikes network coding, RobustToolbox is attractive because it already provides the server/client/shared project split, networked components, replication through component states, prediction patterns, network entity conversion helpers, PVS/visibility culling, and an established ECS architecture. That does not eliminate networking work, but it gives you rails to stay on.

## Why Space Station 14 itself may be the wrong base

Using the full SS14 codebase as a fork is tempting, but it carries a lot of assumptions:

- station rounds, jobs, departments, station maps, atmos, power, body/organ simulation, construction, inventories, and SS13-flavored interactions;
- many content prototypes that are irrelevant to an RPR-style action/RP game;
- a content culture and code organization optimized around SS14's game design.

A full SS14 fork is useful if the target game remains SS13-like. For RoleplayRebirth, RobustToolbox plus selected SS14 examples is cleaner than mutating SS14 into a DBZ/anime-action RP game.

## System-by-system fit

| RPR domain | RobustToolbox fit | Port strategy | Difficulty |
|---|---:|---|---:|
| Character identity: race, class, body type | Excellent | YAML prototypes + identity/stat components | Medium |
| Baseline stats and stat modifiers | Excellent | Data tables + shared stat system | Medium |
| Progression/gains/training | Good | Server systems; no per-mob infinite BYOND loops | Medium-High |
| Transformations/forms/buffs | Good | Components for active forms, drains, tags, multipliers | Medium-High |
| RP chat, OOC/LOOC/say/yell/emote | Good | Borrow SS14 chat concepts; customize ranges/channels | Medium |
| Admin verbs/moderation/logging | Good | Rebuild as commands/UI/permissions | Medium-High |
| Movement | Good | Use Robust movement/physics/grid systems | Medium |
| Fast action combat | Possible but risky | Requires prediction and careful server authority | High |
| Projectile spam, beams, AOEs, knockback | Possible but risky | Shared predicted systems + server validation | High |
| Planets/maps/building | Good | Maps/grids/prototypes; rebuild map assets | High due to content volume |
| BYOND UI panels/macros/winset/browser UI | Poor direct fit | Rebuild UI in Robust/Avalonia patterns | Medium-High |
| BYOND savefiles | Poor direct fit | Replace with explicit database/save schema | High |
| BYOND verbs | Poor direct fit | Convert to interactions, commands, UI actions | Medium-High |
| BYOND spawn/sleep loops | Poor direct fit | Replace with systems, timers, updates, event queues | High |

## Major modernization recommendation

The rewrite should use three layers:

### 1. Content data layer

Races, classes, body types, skills, transformations, enchantments, crafting recipes, planets, NPC archetypes, and admin permission definitions should become data. Most of this should live in YAML prototypes or structured data files, not in C# switch forests.

### 2. Shared simulation layer

Anything that needs prediction or client-visible calculation should live in `Content.Shared`: movement-affecting stat modifiers, active combat intents, predicted cooldown checks, simple projectile movement, form multipliers, and UI-visible stat calculations.

### 3. Server authority layer

The server should own permanent truth: saves, progression awards, race/class selection validation, death/KO state, admin commands, world events, planet destruction, economy/crafting outcomes, and anti-cheat-sensitive combat results.

## Recommended starting approach

Start with a narrow vertical slice rather than trying to recreate all of RPR at once:

1. Account/character creation with race, class, and body type.
2. Stat computation for a small subset of races/classes/bodies.
3. Movement and basic map/grid spawning.
4. Local chat + OOC/LOOC.
5. Basic melee or single projectile attack.
6. Training action that increases a stat.
7. Save/load to explicit schema.
8. One transformation/buff that affects movement/combat.

That slice proves the hardest architectural pieces: ECS data design, prediction boundary, server authority, persistence, and UI.

## Direct port blockers

- RPR is built around DM inheritance, global variables, verbs, input prompts, BYOND browser windows, savefile object serialization, and implicit runtime behavior.
- The source uses many `spawn()`/`sleep()` patterns and repeated loops that should not be translated literally.
- Several systems scan `world` or `view()` directly. In RobustToolbox, those become entity queries, events, physics queries, PVS-conscious effects, or map/grid queries.
- Character persistence currently serializes whole mobs. Robust should use explicit save records and versioned migration logic.
- The uploaded package omits map/interface/sound assets, so maps and UI must be recreated or recovered separately.
- A hardcoded secret/credential was present in the uploaded source. Remove it, rotate it if it was ever real, and avoid publishing it.

## Overall viability

| Strategy | Viability | Comment |
|---|---:|---|
| Directly translate DM files to C# | Low | Recreates BYOND patterns inside a better engine and keeps most problems. |
| Fork SS14 and heavily mutate it | Medium-Low | Gives many working systems but brings a lot of irrelevant genre assumptions. |
| New RobustToolboxTemplate game, using SS14 as examples | Medium-High | Best balance of clean architecture and existing engine support. |
| Build a custom engine from scratch | Low-Medium | More freedom, but highest networking burden. |

**Best answer:** build a new RobustToolbox game and treat RPR as content/design input.


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

