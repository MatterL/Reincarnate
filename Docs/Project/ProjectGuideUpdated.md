# ProjectGuide.md — Reincarnate RobustToolbox Modernization Guide

**Project:** Reincarnate  
**Guide date:** 2026-05-12  
**Engine/foundation:** RobustToolbox + RobustToolboxTemplate  
**Source reference:** BYOND/DreamMaker ReIncarnate / RoleplayRebirth-era source  
**Current project state assumed:** post-Phase 5 guide work, with extraction/design documents present but the actual `Content.* / RI` game implementation still mostly empty.

---

## 0. What this guide is for

This guide replaces the earlier generic conversion outline. It is written for the actual Reincarnate repository shape and for the reality that RobustToolboxTemplate is not a finished game framework. It gives you a build order that starts with a visible, controllable, networked game surface before bulk-converting DreamMaker systems.

The goal is not to translate `.dm` files line-by-line. The goal is to extract the game facts from the old source and rebuild them as RobustToolbox ECS components, systems, prototypes, UI screens, networking events, persistence records, and tests.

Use this guide as the master planning document. When asking an AI assistant to expand a phase, paste the phase plus this instruction:

```text
Using ProjectGuide.md, expand Phase <number> into a concrete implementation plan for this repository. Include file paths, C# skeletons, YAML prototype examples, tests, editor/resource setup, expected compile issues, and acceptance criteria. Treat the BYOND source as design evidence, not code to copy. Assume the current project uses RobustToolboxTemplate and does not yet have a complete playable/rendered game loop.
```

---

## 1. Ground truth and project stance

### 1.1 Current repo reality

The public repository is still structurally close to RobustToolboxTemplate. It has the expected split between `Content.Client`, `Content.Server`, `Content.Shared`, `Resources`, `Docs`, `Tools`, and a `RobustToolbox` submodule. The `Content.Client/RI`, `Content.Server/RI`, and `Content.Shared/RI` roots exist, but the implementation under them should be treated as not yet built unless the local branch has unpushed work.

That means Phase 5 should be considered **design/extraction complete**, not “playable systems complete.” The next work must not jump straight into character creation or combat. First, make a minimum visible game surface:

1. server starts;
2. client starts;
3. client connects;
4. a map loads;
5. a player entity spawns;
6. the player has a visible sprite or placeholder;
7. a camera follows or frames the pawn;
8. movement input causes visible movement;
9. at least one simple interaction proves ECS/network flow.

Only after that should stat previews, character creation, persistence, combat, skills, and transformation content become useful.

### 1.2 Canonical names

Use `RI` for folder roots and namespaces.

Use `Ri*` for new C# symbols:

```text
Content.Shared.RI.Stats.RiStatType
Content.Shared.RI.Stats.RiStatBlock
Content.Shared.RI.Character.RiIdentityComponent
Content.Server.RI.Character.RiCharacterCreationSystem
Content.Client.RI.CharacterCreation.RiCharacterCreationWindow
```

The older extraction docs may mention `RPR` or `Rpr*`. Treat those as legacy labels from the source game and earlier guide. New code should prefer `RI`/`Ri*` unless you intentionally choose another prefix and update all docs consistently.

### 1.3 Rewrite rule

Do not ask:

> Where does this DM proc go in C#?

Ask:

> What player-facing rule, content definition, stat formula, UI flow, admin action, save fact, or networked event does this DM code represent?

Then rebuild that concept as one or more of:

- YAML prototypes;
- C# components;
- C# systems;
- network events;
- commands;
- client UI states;
- persistence DTOs;
- tests;
- extracted-content notes;
- balance simulator inputs.

### 1.4 Client/shared/server rule

**Server owns truth.** Saves, stats after validation, progression, damage, deaths, unlocks, admin authority, economy, world state, and anti-cheat checks belong server-side.

**Shared owns deterministic concepts used by both sides.** Stat math, prototype IDs, network event DTOs, input-intent shapes, predicted checks, enums, and pure calculation utilities belong shared-side.

**Client owns presentation.** UI, input gathering, local prediction visuals, sprites, audio, animation, and tooltips belong client-side. The client may request actions; it does not decide permanent outcomes.

### 1.5 Source quarantine rule

The raw DreamMaker source is evidence, not runtime code. Do not commit the raw ZIP or extracted `.dm` tree into the public game repo unless you intentionally keep the repository private and have reviewed legal/security concerns.

Use this workflow instead:

1. Keep `ReIncarnate.zip` outside the repo or in a private ignored folder.
2. Extract design facts into `Docs/Extracted Content/` or a new normalized `Docs/ExtractedContent/` folder.
3. Write one extraction note per system.
4. Convert extraction notes into prototypes and tests.
5. Only then write C# systems.

---

## 2. BYOND-to-Robust concept map

The old DM source should be referenced constantly, but only at the concept level. The current extraction docs already identify the highest-value source areas.

| Old DM area | Preserve as concept | RobustToolbox target |
|---|---|---|
| `Code/Creation.dm`, `Code/CreationNew.dm` | race/class/body/planet creation flow, unlock checks, initial identity | `RiRacePrototype`, `RiClassPrototype`, `RiBodyTypePrototype`, character creation UI, server validation |
| `Code/statpointsystems.dm`, `Code/Stats.dm`, `Code/Variables.dm` | stat vocabulary, race/class/body stat branches, stat points, modifiers | `RiStatType`, `RiStatBlock`, `RiStatModifier`, `RiStatsComponent`, stat bible, tests |
| `Code/Gains.dm`, `Code/GameEvents/Training.dm`, `Code/Constants/TrainingConstants.dm` | training, meditation, EXP, passive gains, recovery, ranking/catch-up | progression prototypes, training systems, balance simulator |
| `Code/BattleSystem.dm` | KO, death, combat state transitions, hostility, recovery | vitals systems, combat state components, events |
| `Code/Projectiles.dm`, `Code/Shockwave.dm` | projectile behavior, hit effects, knockback, explosions, pierce | projectile prototypes, projectile systems, server hit validation |
| `Code/Skills.dm`, `Code/SkillSystems.dm`, `Code/SkillTreeSEXY.dm`, `Code/SkillProcs.dm` | skill catalog, costs, cooldowns, unlocks, activation styles | `RiSkillPrototype`, handlers, hotbar, server validation |
| `Code/TransNew.dm`, `Code/BeastTransforms.dm`, `Code/Ascension.dm`, `Code/Cyberize.dm` | forms, transformations, drains, buffs, requirements | `RiTransformationPrototype`, modifier stacks, status effects |
| `Code/Chat&Verbs.dm`, `Code/Utils/ChatUtils/*`, `Code/RPMode.dm`, `Code/SaySpark.dm` | OOC/LOOC/say/yell/emote, RP mode, chat range, social rules | chat channels, server routing, logs, RP-mode component |
| `Code/Items.dm`, `Code/Tech.dm`, `Code/Enchantment.dm`, `Code/Build*.dm` | items, crafting, technology, enchantment, buildables | item prototypes, inventory, crafting, equipment, build systems |
| `Code/World.dm`, `Code/Map.dm`, `Code/Weather.dm`, `Code/Years.dm`, `Code/Systems/SpawnPoints.dm` | planets, maps, spawn points, weather/time/world settings | map prototypes, planet prototypes, world systems, persistence |
| `Code/Admin.dm`, `Code/Reports.dm`, `Code/Notes.dm`, `Code/LogSystem/*`, `Code/VotingSystem.dm` | admin tools, reports, notes, logs, permissions, votes | secure admin commands, permissions, audit log, report UI |

### 2.1 DreamMaker patterns to avoid

| DM pattern | New shape |
|---|---|
| `spawn` and `sleep` loops | ECS systems with timers, accumulators, scheduled events, cooldown components |
| `verb` | commands, interactions, UI actions, or admin tools |
| `input` | client UI request + server validation |
| `winset` and `browse` | Robust UI controls/windows and bound UI states |
| `savefile` | explicit versioned save DTOs and repositories |
| global `world` scans | ECS queries, spatial queries, map/grid-local systems, cached registries |
| `view()`/`range()` scans | physics/spatial queries, visibility-aware queries, PVS-conscious routing |
| giant string switches | prototypes, tags, requirement predicates, data-driven registries |
| mob variables as schema | explicit components, save records, and migration docs |

---

## 3. Stat model: how to carry the old stat concepts forward

This needs special attention because the BYOND source uses overlapping ideas: current stat, base stat, stat modifier, stat bonus, race/class/body modifiers, temporary multipliers, and resource values.

### 3.1 Do not flatten everything into one number

The new system should preserve the distinction between:

- **base value** — permanent baseline on the character;
- **earned value** — permanent training/progression increases;
- **prototype multipliers** — race/class/body modifiers from creation choices;
- **temporary modifiers** — buffs, transformations, equipment, injuries, status effects;
- **current resource values** — health/energy/oxygen-style values that can be partially depleted;
- **derived preview/final value** — what the character currently uses for calculations.

### 3.2 Recommended stat records

Use a small number of explicit structures rather than one giant component full of unrelated floats.

```csharp
public enum RiStatType
{
    BasePower,
    Energy,
    Strength,
    Endurance,
    Force,
    Resistance,
    Speed,
    Offense,
    Defense,
    Regeneration,
    Recovery,
    Anger,
    Intelligence,
    Enchantment,
    TrainingRate,
    MeditationRate,
}
```

```csharp
public readonly record struct RiStatLine(
    float BaseValue,
    float EarnedBonus,
    float CreationMultiplier,
    float PermanentAddBonus,
    float TemporaryAddBonus,
    float TemporaryMultiplier
);
```

```csharp
public sealed class RiStatBlock
{
    public Dictionary<RiStatType, RiStatLine> Lines = new();
}
```

For the first implementation, keep the calculation boring and deterministic:

```text
creationAdjusted = (baseValue + earnedBonus + permanentAddBonus) * creationMultiplier
currentFinal = (creationAdjusted + temporaryAddBonus) * temporaryMultiplier
```

Do not lock this formula forever. The important part is to make the order explicit and testable. The extraction notes say race/class stats are computed first, then body/class modifiers are applied. Preserve that order until balance testing says otherwise.

### 3.3 Racial stats and class variants

The extracted stat docs identify race stat branches for many races and mark some races as class-variant-dependent. Do not put all of that directly into code. Use prototypes.

Recommended split:

```text
RiRacePrototype
  id
  displayName
  defaultSpawnPlanet
  allowedClassIds
  bodyTypePolicy
  statTemplateId
  startingStatPoints
  progressionModifiers
  unlockRequirementId
  legalStatus
  tags

RiClassPrototype
  id
  displayName
  allowedRaceIds / requiredRaceTags
  statTemplateOverlayId
  skillGrantIds
  transformationPathIds
  unlockRequirementId
  legalStatus
  tags

RiBodyTypePrototype
  id
  displayName
  statMultipliers
  excludedRaceTags
  legalStatus

RiStatTemplatePrototype
  id
  statMultipliers or base stats
  formulaAuditStatus
  sourceNotes
```

### 3.4 Stat point policy

The old source includes unusual starting stat point counts, such as common races receiving around 10 points while Alien/Morphling/Android/Hollow have much larger values in the extraction notes. Treat these as `startingStatPoints` on race/class/body or on a dedicated creation rule prototype. Do not hide them in UI code.

### 3.5 Current stat vs current resource

For combat stats like `Strength`, `Endurance`, `Speed`, `Force`, `Resistance`, `Offense`, and `Defense`, prefer derived current values from the stat system.

For resources like `Health`, `Energy`, `Oxygen`, `ManaAmount`, and similar, use vitals/resource components:

```text
RiVitalsComponent
  healthCurrent
  healthMax
  energyCurrent
  energyMax
  oxygenCurrent
  oxygenMax

RiResourceRegenComponent
  regenerationRate
  recoveryRate
  oxygenRecoveryRate
```

Do not treat `EnergyMod` as the same thing as current energy. `EnergyMod` belongs to stat/progression math. `EnergyCurrent` belongs to vitals/resources.

---

## 4. Target repository shape

Keep the actual repo’s `RI` structure and grow it deliberately.

```text
Reincarnate/
  Content.Client/
    RI/
      Bootstrap/
      Camera/
      CharacterCreation/
      Chat/
      Combat/
      Debug/
      HUD/
      Input/
      Stats/
      UI/
      Visuals/

  Content.Shared/
    RI/
      Admin/
      Bootstrap/
      Character/
      Chat/
      Combat/
      Common/
      Input/
      Items/
      Movement/
      Persistence/
      Progression/
      Projectiles/
      Prototypes/
      Skills/
      Stats/
      Transformations/
      Vitals/
      World/

  Content.Server/
    RI/
      Admin/
      Bootstrap/
      Character/
      Chat/
      Combat/
      Persistence/
      Progression/
      Skills/
      Vitals/
      World/

  Resources/
    Prototypes/
      RI/
        Bootstrap/
        Character/
          races.yml
          classes.yml
          body_types.yml
          stat_templates.yml
        Entities/
          player.yml
          debug_entities.yml
        Maps/
          test_map.yml
        Combat/
          damage_types.yml
          projectiles.yml
        Skills/
          skills.yml
          skill_categories.yml
        Transformations/
          transformations.yml
        Items/
          items.yml
          equipment.yml
        World/
          planets.yml
          spawn_points.yml
          environment.yml
        Admin/
          roles.yml
          permissions.yml

  Docs/
    ADR/
    Architecture/
    Design/
    Extracted Content/
    Generated Findings/
    Networking/
    Project/
    Security/
    Setup/
    Testing/

  Tools/
    RIContentExtractor/
    RIBalanceSim/
    RISaveMigrator/
```

Prefer `Docs/ExtractedContent` without a space for new docs if you are comfortable renaming; otherwise keep the current `Docs/Extracted Content` folder to avoid churn. The guide below uses the existing folder name when referencing existing docs.

---

## 5. Phase map from here forward

The earlier guide’s Phase 00–05 work is useful, but the next order needs correction. Rendering, player spawn, map bootstrap, and a minimum playable surface come before serious character creation or combat.

| Phase | Name | Main output |
|---:|---|---|
| 00 | Governance and source quarantine | Legal/security/rewrite policy |
| 01 | Development environment and build baseline | Clone/build/run docs |
| 02 | RobustToolbox learning spike | Tiny networked ECS feature |
| 03 | BYOND source inventory and extraction workflow | Source map and extraction docs |
| 04 | Architecture skeleton | Folders, conventions, ADRs |
| 05 | Stat/identity extraction and stat bible draft | Extracted races/classes/body/stats and first YAML drafts |
| 06 | Post-Phase-5 repo reality check | Verify current repo, compile, normalize docs/names |
| 07 | Minimum playable surface | Visible map, pawn, camera, movement, debug spawn |
| 08 | Prototype infrastructure and stat system implementation | Real `Ri*` prototypes and deterministic stat math |
| 09 | Character creation vertical slice | Client UI + server validation + spawn identity |
| 10 | Persistence v0 | Explicit character save/load |
| 11 | Vitals, resources, regen, KO foundation | Health/energy/oxygen/KO state machine |
| 12 | Movement gates, flight, and environment basics | Movement restrictions, flight, water/oxygen hooks |
| 13 | Chat and RP social layer | OOC/LOOC/say/yell/emote/RP mode |
| 14 | Combat networking prototype | Melee + projectile under fake lag |
| 15 | Skill framework | Prototype-driven skill activation/cooldowns/resources |
| 16 | Transformations, buffs, and status effects | Forms/drains/modifier stacks |
| 17 | Items, equipment, crafting, tech, enchanting | Item/equipment/crafting/enchant v0 |
| 18 | Worlds, planets, maps, and environment | Planet/map/spawn/weather/time systems |
| 19 | Admin, moderation, logging, reports | Secure replacement for admin verbs |
| 20 | UI replacement program | Native Robust UI for old BYOND workflows |
| 21 | Content legality and asset replacement | Names/sprites/audio/lore cleanup |
| 22 | Testing, balance, and simulation harnesses | Unit/integration tests and balance sim |
| 23 | Performance and anti-cheat hardening | Query budgets, validation, rate limits |
| 24 | Packaging, hosting, update pipeline | Build/deploy/update flow |
| 25 | Bulk content migration factory | Repeatable conversion for skills/races/items/forms |
| 26 | Closed alpha and live-ops readiness | Playtest loop, telemetry, policies |

---

# Phase 00 — Governance and source quarantine

## Purpose

Keep the rewrite legally safe, security-safe, and architecturally clean.

## Inputs

- Raw `ReIncarnate.zip` or equivalent private BYOND source archive.
- Existing extraction docs under `Docs/Extracted Content/`.
- `Docs/Generated Findings/` if present.
- Existing `ProjectGuide.md`.

## Main work

1. Keep raw DM source outside the public repo or in an ignored private folder.
2. Write or update `Docs/Project/rewrite-rules.md`.
3. Write or update `Docs/Security/source-secret-audit.md`.
4. Write or update `Docs/Legal/content-risk-register.md`.
5. Create `Docs/ADR/0001-rewrite-not-port.md`.
6. Mark franchise-derived names, inherited assets, and questionable content as `NeedsReview` or `ReplaceBeforePublic`.
7. Add `.gitignore` entries for raw source archives and extracted DM folders.

## Deliverables

- `Docs/ADR/0001-rewrite-not-port.md`
- `Docs/Project/rewrite-rules.md`
- `Docs/Security/source-secret-audit.md`
- `Docs/Legal/content-risk-register.md`
- `.gitignore` entries for private source artifacts

## Exit criteria

- The public repo contains no raw DM source archive.
- Every developer understands that DreamMaker source is design evidence, not C# source material.
- All future source references use file paths and summaries, not copied chunks of DM implementation.

## Expansion prompt

```text
Expand Phase 00 into a checklist for source quarantine, legal review, secret scanning, rewrite rules, and public-repo safety. Include exact document templates and .gitignore entries.
```

---

# Phase 01 — Development environment and build baseline

## Purpose

Prove the template-derived project builds and the client/server can connect before adding gameplay.

## Inputs

- Public Reincarnate repository.
- RobustToolboxTemplate setup expectations.
- Local OS/toolchain.

## Main work

1. Clone with submodules.
2. Verify the `RobustToolbox` submodule is present.
3. Restore and build.
4. Run server.
5. Run client.
6. Connect locally.
7. Document exact SDK/tool versions.
8. Record common errors and fixes.

## Commands

```bash
git clone --recurse-submodules https://github.com/MatterL/Reincarnate.git
cd Reincarnate
git submodule update --init --recursive
dotnet restore
dotnet build
dotnet run --project Content.Server
dotnet run --project Content.Client
```

If the local project has already been cloned, run:

```bash
git pull
git submodule update --init --recursive
dotnet build
```

## Deliverables

- Working local build.
- `Docs/Setup/development-environment.md`
- `Docs/Setup/troubleshooting.md`
- A “known good” commit or tag.

## Exit criteria

- A new developer can clone, build, run server, run client, and connect locally using only the setup doc.
- No gameplay work depends on a broken or ambiguous toolchain.

## Expansion prompt

```text
Expand Phase 01 into OS-specific setup steps for Windows, Linux, and macOS. Include required SDKs, submodule steps, build/run commands, IDE setup, and a troubleshooting table for RobustToolboxTemplate projects.
```

---

# Phase 02 — RobustToolbox learning spike

## Purpose

Learn the minimum ECS/network/prototype/UI loop before porting any real Reincarnate system.

## Main work

Build a tiny `RiTrainingDummy` or `RiDebugButton` feature:

1. one prototype;
2. one shared component;
3. one shared or server system;
4. one networked field or network event;
5. one client popup/UI feedback;
6. one simple test or manual fake-lag checklist.

## Deliverables

- `Content.Shared/RI/Sandbox/RiTrainingDummyComponent.cs`
- `Content.Shared/RI/Sandbox/RiTrainingDummySystem.cs`
- optional `Content.Client/RI/Sandbox/` UI/visuals
- `Resources/Prototypes/RI/Bootstrap/training_dummy.yml`
- `Docs/Learning/robust-ecs-notes.md`

## Exit criteria

- You have proven component registration, prototype loading, system behavior, server/client split, and a simple replicated or network-triggered result.

## Expansion prompt

```text
Expand Phase 02 into a tiny RobustToolbox learning feature with component, system, prototype, network event if needed, client feedback, and a fake-lag test checklist.
```

---

# Phase 03 — BYOND source inventory and extraction workflow

## Purpose

Turn the raw DreamMaker codebase into structured design data without copying its architecture.

## Inputs

- `ReIncarnate.zip`
- `Docs/Extracted Content/source-map.md`
- `Docs/Extracted Content/extraction-template.md`
- source files listed in the hotspot map

## Main work

1. Maintain a source map by system area.
2. For each system, create extraction notes with:
   - DM files inspected;
   - concept name;
   - player-facing behavior;
   - formulas/fields to preserve;
   - old runtime assumptions to discard;
   - target Robust components/prototypes/systems;
   - networking risk;
   - persistence risk;
   - legal/IP risk;
   - test cases.
3. Convert extraction notes into a content backlog.
4. Tag each item as `Preserve`, `Tune`, `Rewrite`, `Delete`, `Legal Review`, `Needs Network Prototype`, or `Needs Balance Sim`.

## Deliverables

- `Docs/Extracted Content/source-map.md`
- `Docs/Extracted Content/content-backlog.md`
- extraction docs for stats, races, classes, body types, training, combat, projectiles, skills, transformations, chat/RP, admin, items, worlds

## Exit criteria

- Any future phase can name the exact DM files it is using as evidence.
- No future phase has to hunt blindly through the old source.

## Expansion prompt

```text
Expand Phase 03 into a repeatable extraction workflow. Include the template, tagging rules, source-map update rules, and a sample extraction for stats or combat using the ReIncarnate BYOND source.
```

---

# Phase 04 — Architecture skeleton

## Purpose

Give every future feature an obvious home.

## Main work

1. Normalize folder names under `Content.Client/RI`, `Content.Shared/RI`, and `Content.Server/RI`.
2. Create namespace conventions.
3. Decide `RI`/`Ri*` naming and update docs that still say `RPR`/`Rpr*` where appropriate.
4. Create base IDs/enums in `Content.Shared/RI/Common`.
5. Write ADRs for:
   - client/shared/server boundary;
   - data-driven prototypes;
   - persistence boundary;
   - naming and folder conventions.
6. Add a minimal build verification script.

## Deliverables

- Folder skeleton.
- `Docs/Architecture/naming-and-folder-conventions.md`
- `Docs/Architecture/networking-boundaries.md`
- `Docs/Architecture/system-conventions.md`
- `Docs/ADR/0002-client-shared-server-boundaries.md`
- `Docs/ADR/0003-prototype-first-content.md`
- `Tools/build.ps1` or similar local verification script.

## Exit criteria

- New code has an obvious folder and namespace.
- A build catches compile failures quickly.
- No new feature is placed in a generic dumping ground.

## Expansion prompt

```text
Expand Phase 04 into folder creation, namespace conventions, ADR text, base enums/IDs, and a local build verification script for the current Reincarnate repo.
```

---

# Phase 05 — Stat/identity extraction and stat bible draft

## Purpose

Extract the old game’s race/class/body/stat identity system into a written design spec before implementation.

## Inputs

- `Code/Creation.dm`
- `Code/CreationNew.dm`
- `Code/statpointsystems.dm`
- `Code/Stats.dm`
- `Code/Variables.dm`
- `Docs/Extracted Content/races.md`
- `Docs/Extracted Content/classes.md`
- `Docs/Extracted Content/body-types.md`
- `Docs/Extracted Content/stats.md`
- `Docs/Extracted Content/first-yaml-drafts.md`

## Main work

1. Confirm the stat vocabulary.
2. Confirm race stat branches and class variants.
3. Confirm body type modifiers and exclusions.
4. Draft prototype shapes.
5. Draft the stat calculation order.
6. Mark formulas as `Preserve`, `Tune`, `Rewrite`, or `Delete`.
7. Write a stat bible.

## Deliverables

- `Docs/Design/stat-bible.md`
- updated `Docs/Extracted Content/stats.md`
- updated `Docs/Extracted Content/races.md`
- updated `Docs/Extracted Content/classes.md`
- updated `Docs/Extracted Content/body-types.md`
- draft YAML examples, but not necessarily final committed content

## Exit criteria

- You can explain what a stat is, how race/class/body affect it, what gets saved, what gets derived, and what is temporary.
- You have not yet assumed that the C# implementation exists.

## Expansion prompt

```text
Expand Phase 05 into a final stat bible pass. Use Creation.dm, statpointsystems.dm, Stats.dm, Variables.dm, and the existing extracted docs. Define stat vocabulary, calculation order, race/class/body prototype fields, and audit labels for formulas.
```

---

# Phase 06 — Post-Phase-5 repo reality check

## Purpose

Stop and reconcile the guide with the actual repository before writing gameplay code.

This phase exists because design docs can make a project feel farther along than it is. After Phase 5, verify what is truly implemented.

## Inputs

- Current GitHub/local repo.
- Existing docs.
- Current `Content.Client/RI`, `Content.Shared/RI`, `Content.Server/RI` folders.
- `Resources/Prototypes/RI` folders.

## Main work

1. Run `dotnet build`.
2. List actual `RI` files under `Content.Client`, `Content.Shared`, `Content.Server`, and `Resources/Prototypes`.
3. Mark each phase as:
   - `Docs only`;
   - `Skeleton exists`;
   - `Compiles`;
   - `Playable`;
   - `Tested`.
4. Update `Docs/Project/current-state.md`.
5. Update this guide if local code differs from public GitHub.
6. Normalize `RI` vs `RPR` naming in docs or record why you keep both.
7. Create `Docs/Project/phase-status.md`.

## Deliverables

- `Docs/Project/current-state.md`
- `Docs/Project/phase-status.md`
- build log or screenshot in local notes
- list of existing `RI` source/prototype files
- update notes for this guide

## Exit criteria

- You know what exists as real code and what exists only as design.
- Phase 07 starts from the real repo, not from imagined prior work.

## Expansion prompt

```text
Expand Phase 06 into a repository audit plan. Include commands to list files, verify build, classify completed phases, inspect Resources/Prototypes/RI, and produce Docs/Project/current-state.md and Docs/Project/phase-status.md.
```

---

# Phase 07 — Minimum playable surface

## Purpose

Build the missing foundation that RobustToolboxTemplate does not provide as a finished game: a visible map, a player pawn, a camera, movement input, and a debug spawn path.

Do this before character creation. A character creator is not useful if there is no visible world to enter.

## Inputs

- Phase 06 current-state audit.
- RobustToolboxTemplate examples.
- SS14 examples only as reference, not as copied gameplay architecture.
- `Resources/Prototypes/Entities` from the template.

## Main work

1. Create a tiny test map or map-loading path.
2. Create a placeholder player entity prototype.
3. Add a placeholder texture/sprite or use an acceptable test texture.
4. Add client camera behavior.
5. Add movement input.
6. Make server spawn the player pawn after connect or via debug command.
7. Make the client view/follow the pawn.
8. Add a debug overlay or log proving spawn position and entity ID.
9. Add one debug entity such as a wall, marker, or training dummy.
10. Write a manual test checklist.

## Suggested files

```text
Content.Shared/RI/Bootstrap/RiPlayerComponent.cs
Content.Shared/RI/Bootstrap/RiDebugSpawnComponent.cs
Content.Shared/RI/Movement/RiBasicMovementComponent.cs
Content.Shared/RI/Movement/RiBasicMovementSystem.cs
Content.Server/RI/Bootstrap/RiPlayerSpawnSystem.cs
Content.Client/RI/Camera/RiCameraSystem.cs
Content.Client/RI/Input/RiInputSystem.cs
Resources/Prototypes/RI/Entities/player.yml
Resources/Prototypes/RI/Entities/debug_entities.yml
Resources/Prototypes/RI/Maps/test_map.yml
Docs/Design/minimum-playable-surface.md
```

## Exit criteria

- Server and client run.
- Client connects.
- A visible pawn exists.
- Camera shows the pawn or test map.
- WASD or chosen movement input visibly moves the pawn.
- Movement is server-authoritative enough for the first prototype.
- There is a known path to spawn debug entities.

## Expansion prompt

```text
Expand Phase 07 into a step-by-step RobustToolbox minimum playable surface implementation. Include player prototype, placeholder sprite, test map, spawn system, camera setup, movement input, resource paths, and manual verification steps.
```

---

# Phase 08 — Prototype infrastructure and stat system implementation

## Purpose

Turn the Phase 05 stat bible into real C# prototypes, YAML, deterministic stat calculation, and tests.

## Inputs

- Phase 05 stat bible.
- `Docs/Extracted Content/stats.md`
- `Docs/Extracted Content/races.md`
- `Docs/Extracted Content/classes.md`
- `Docs/Extracted Content/body-types.md`
- `Docs/Extracted Content/first-yaml-drafts.md`
- Phase 07 working playable surface.

## Main work

1. Implement `RiStatType`.
2. Implement `RiStatBlock` and `RiStatLine`.
3. Implement `RiRacePrototype`, `RiClassPrototype`, `RiBodyTypePrototype`, and `RiStatTemplatePrototype`.
4. Implement `RiStatSystem` as a deterministic shared calculation service.
5. Create first real YAML prototypes for:
   - Human;
   - Alien;
   - Saiyan;
   - Medium/Small/Large body types;
   - Fighter/Technologist/Healer/Wizard or renamed equivalents.
6. Implement unit tests for:
   - Human + Medium;
   - Human + Small;
   - Human + Large;
   - Changeling-like body exclusion if included;
   - class variant behavior;
   - deterministic ordering.
7. Document calculation order in `Docs/Design/stat-bible.md`.

## Suggested files

```text
Content.Shared/RI/Stats/RiStatType.cs
Content.Shared/RI/Stats/RiStatLine.cs
Content.Shared/RI/Stats/RiStatBlock.cs
Content.Shared/RI/Stats/RiStatModifier.cs
Content.Shared/RI/Stats/RiStatsComponent.cs
Content.Shared/RI/Stats/RiStatSystem.cs
Content.Shared/RI/Prototypes/RiRacePrototype.cs
Content.Shared/RI/Prototypes/RiClassPrototype.cs
Content.Shared/RI/Prototypes/RiBodyTypePrototype.cs
Content.Shared/RI/Prototypes/RiStatTemplatePrototype.cs
Resources/Prototypes/RI/Character/races.yml
Resources/Prototypes/RI/Character/classes.yml
Resources/Prototypes/RI/Character/body_types.yml
Resources/Prototypes/RI/Character/stat_templates.yml
Docs/Design/stat-bible.md
Docs/Testing/stat-system-tests.md
```

## Exit criteria

- The project compiles.
- Prototype YAML loads.
- Stat previews are deterministic.
- At least three race/class/body combinations pass tests.
- The stat system does not depend on UI.
- The stat system does not depend on server-only persistence.

## Expansion prompt

```text
Expand Phase 08 into an implementation plan for Ri stat prototypes and stat math. Use the extracted stats/races/classes/body-types docs. Include C# classes, YAML examples, deterministic formula order, tests, and compile-risk notes for RobustToolbox prototype serialization.
```

---

# Phase 09 — Character creation vertical slice

## Purpose

Let a player choose identity data and spawn a pawn with computed stats.

This phase uses the Phase 07 playable surface and Phase 08 stat system. It should not invent its own stat calculations in UI code.

## Inputs

- Phase 07 playable surface.
- Phase 08 stat prototypes/system.
- `Code/Creation.dm`
- `Code/CreationNew.dm`
- `Code/UIBackend/Menu.dm`
- `Docs/Extracted Content/races.md`
- `Docs/Extracted Content/classes.md`
- `Docs/Extracted Content/body-types.md`

## Main work

1. Define character creation states.
2. Define shared request/response messages.
3. Build client UI:
   - name;
   - race;
   - class;
   - body type;
   - optional spawn planet;
   - stat preview;
   - confirmation.
4. Server validates:
   - name;
   - race exists;
   - class allowed for race;
   - body type allowed;
   - unlocks;
   - spawn planet rules.
5. Server computes final stats using `RiStatSystem`.
6. Server spawns/updates the player pawn.
7. Client closes creation UI and enters world.
8. Add rejection messages for invalid choices.

## Suggested files

```text
Content.Shared/RI/Character/RiIdentityComponent.cs
Content.Shared/RI/Character/RiCharacterComponent.cs
Content.Shared/RI/Character/RiCharacterCreationMessages.cs
Content.Shared/RI/Character/RiCharacterCreationState.cs
Content.Server/RI/Character/RiCharacterCreationSystem.cs
Content.Client/RI/CharacterCreation/RiCharacterCreationWindow.cs
Content.Client/RI/CharacterCreation/RiCharacterCreationController.cs
Docs/Design/character-creation-flow.md
```

## Exit criteria

- A player can create a character and spawn into the Phase 07 test map.
- UI preview and server final stats match.
- Invalid choices are rejected server-side.
- No BYOND modal prompt pattern is recreated.

## Expansion prompt

```text
Expand Phase 09 into a RobustToolbox character creation vertical slice. Include shared messages, UI flow, server validation, stat preview, spawn handoff, and error states.
```

---

# Phase 10 — Persistence v0

## Purpose

Replace BYOND `savefile` behavior with explicit, versioned character saves.

## Inputs

- Phase 09 character creation.
- `Code/Variables.dm`
- `Code/World.dm`
- `Code/OfflinePlayers.dm`
- `Code/Systems/OfflineCatchup.dm`

## Main work

1. Define save DTOs separate from ECS components.
2. Choose v0 storage:
   - local JSON for earliest prototype; or
   - SQLite if you want durability sooner.
3. Save:
   - account/user ID;
   - character ID;
   - display name;
   - race/class/body type IDs;
   - base/earned stat data;
   - current map/spawn point;
   - known skills later;
   - inventory later;
   - schema version.
4. Load on connect or character select.
5. Save on disconnect and shutdown.
6. Add migration hooks.
7. Do not serialize live entity IDs as permanent IDs.

## Suggested files

```text
Content.Server/RI/Persistence/RiCharacterSaveRecord.cs
Content.Server/RI/Persistence/RiSaveSchemaVersion.cs
Content.Server/RI/Persistence/IRiCharacterRepository.cs
Content.Server/RI/Persistence/RiJsonCharacterRepository.cs
Content.Server/RI/Persistence/RiPersistenceSystem.cs
Docs/Architecture/persistence-schema-v0.md
Docs/ADR/0004-explicit-persistence-not-entity-serialization.md
```

## Exit criteria

- Create character, disconnect, reconnect, reload character.
- Save data is readable and versioned.
- ECS components can be reconstructed from DTOs.
- No long-term save uses `EntityUid` as a permanent identity.

## Expansion prompt

```text
Expand Phase 10 into persistence v0 for Reincarnate. Include save DTOs, repository interface, JSON/SQLite choice, load/save flows, migration versioning, shutdown/disconnect handling, and tests.
```

---

# Phase 11 — Vitals, resources, regen, KO foundation

## Purpose

Build the health/energy/oxygen/KO foundation used by combat, training, transformations, environment hazards, and skills.

## Inputs

- `Code/BattleSystem.dm`
- `Code/Gains.dm`
- `Code/Variables.dm`
- `Code/Systems/WaterAndOyxgen.dm`
- Phase 08 stat system
- Phase 09/10 character entity

## Main work

1. Define vitals/resource component.
2. Define max-value derivation from stats.
3. Define KO/death state components.
4. Define events:
   - `RiBeforeKnockoutEvent`;
   - `RiKnockedOutEvent`;
   - `RiRegainedConsciousnessEvent`;
   - `RiBeforeDeathEvent`;
   - `RiDiedEvent`.
5. Implement regen/recovery without per-player infinite loops.
6. Add HUD display.
7. Add tests for thresholds and regen.

## Suggested files

```text
Content.Shared/RI/Vitals/RiVitalsComponent.cs
Content.Shared/RI/Vitals/RiKnockoutComponent.cs
Content.Shared/RI/Vitals/RiVitalsSystem.cs
Content.Server/RI/Vitals/RiRegenSystem.cs
Content.Client/RI/HUD/RiVitalsHud.cs
Docs/Design/vitals-ko-death.md
```

## Exit criteria

- Health/energy/oxygen can be displayed.
- Damage reduces health.
- KO triggers at the chosen threshold.
- Regen/recovery works through a system tick, not `spawn/sleep` loops.

## Expansion prompt

```text
Expand Phase 11 into a vitals/regen/KO implementation. Use BattleSystem.dm, Gains.dm, Variables.dm, and WaterAndOyxgen.dm as design references. Include components, events, server/shared split, HUD, and tests.
```

---

# Phase 12 — Movement gates, flight, and environment basics

## Purpose

Add movement rules without recreating a giant BYOND `Move()` override.

## Inputs

- Phase 07 movement.
- Phase 11 vitals/KO.
- `Code/PlayerMovement/Move.dm`
- `Code/PlayerMovement/Macros.dm`
- `Code/Flight.dm`
- `Code/Systems/WaterAndOyxgen.dm`
- `Code/Map.dm`

## Main work

1. Define movement restriction components/tags.
2. Implement gates:
   - KO prevents movement;
   - stunned/frozen prevents movement;
   - training/meditation may restrict movement;
   - flight modifies movement;
   - water/oxygen environment can apply vitals effects.
3. Keep movement gates modular.
4. Add debug commands to apply/remove movement states.
5. Add tests/manual checks.

## Suggested files

```text
Content.Shared/RI/Movement/RiMovementRestrictionComponent.cs
Content.Shared/RI/Movement/RiFlightComponent.cs
Content.Shared/RI/Movement/RiMovementGateSystem.cs
Content.Server/RI/World/RiEnvironmentHazardSystem.cs
Docs/Design/movement-rules.md
```

## Exit criteria

- KO/stun/freeze movement gates work.
- Flight can be toggled in a prototype-safe way.
- Water/oxygen hooks can affect vitals.
- The code remains split into systems, not a monolithic movement override.

## Expansion prompt

```text
Expand Phase 12 into movement gates, flight, and environment basics. Include components, systems, debug commands, tests, and how to avoid a monolithic BYOND Move.dm replacement.
```

---

# Phase 13 — Chat and RP social layer

## Purpose

Rebuild the roleplay communication backbone early. Chat/RP is central to the game identity and lower-risk than combat networking.

## Inputs

- `Code/Chat&Verbs.dm`
- `Code/Utils/ChatUtils/LOOC.dm`
- `Code/Utils/ChatUtils/Say.dm`
- `Code/Utils/ChatUtils/Yell.dm`
- `Code/Utils/ChatUtils/Emote.dm`
- `Code/RPMode.dm`
- `Code/SaySpark.dm`
- `Code/Reports.dm`

## Main work

1. Define channels:
   - OOC;
   - LOOC;
   - Say;
   - Yell;
   - Whisper if retained;
   - Emote;
   - Admin/help/report later.
2. Define range rules.
3. Define formatting rules.
4. Define RP mode as explicit state.
5. Route all messages server-side.
6. Log chat for moderation.
7. Add basic mutes or permission hooks.
8. Add client chat UI.

## Suggested files

```text
Content.Shared/RI/Chat/RiChatChannel.cs
Content.Shared/RI/Chat/RiChatMessage.cs
Content.Shared/RI/Chat/RiRpModeComponent.cs
Content.Server/RI/Chat/RiChatSystem.cs
Content.Client/RI/Chat/RiChatController.cs
Content.Client/RI/Chat/RiChatWindow.cs
Docs/Design/chat-and-rp-mode.md
```

## Exit criteria

- Players can use OOC, LOOC, Say, Yell, and Emote.
- Server controls range and routing.
- RP mode is explicit state.
- Chat is logged for moderation.

## Expansion prompt

```text
Expand Phase 13 into a complete chat/RP social layer. Include channel definitions, range checks, server routing, client UI, logs, mutes, RP mode, and report hooks.
```

---

# Phase 14 — Combat networking prototype

## Purpose

Resolve the hardest technical risk before bulk-converting skills: action combat under multiplayer latency.

## Inputs

- Phase 07 playable surface.
- Phase 11 vitals.
- Phase 12 movement gates.
- `Code/BattleSystem.dm`
- `Code/Projectiles.dm`
- `Code/Shockwave.dm`
- representative entries from `Code/Skills.dm`
- RobustToolbox networking/prediction examples.

## Main work

Build only two attacks:

1. basic melee strike;
2. basic projectile / energy blast.

Rules:

- client sends intent, not final hit truth;
- server validates cooldown, resources, range, collision/hit, target, and damage;
- client may predict safe visuals;
- server applies final damage and KO;
- rate-limit spam;
- test with fake lag and two clients.

## Suggested files

```text
Content.Shared/RI/Combat/RiCombatComponent.cs
Content.Shared/RI/Combat/RiAttackIntent.cs
Content.Shared/RI/Combat/RiDamageSpecifier.cs
Content.Shared/RI/Combat/RiDamageSystem.cs
Content.Shared/RI/Projectiles/RiProjectileComponent.cs
Content.Shared/RI/Projectiles/RiProjectileSystem.cs
Content.Server/RI/Combat/RiCombatValidationSystem.cs
Resources/Prototypes/RI/Combat/projectiles.yml
Resources/Prototypes/RI/Combat/damage_types.yml
Docs/Networking/combat-prototype-results.md
```

## Exit criteria

- Two clients can fight on the test map.
- Melee and projectile actions cost resources/cooldowns.
- Fake-lag behavior is documented.
- The team decides whether RobustToolbox feels acceptable for the intended action-combat feel.

## Expansion prompt

```text
Expand Phase 14 into a combat networking prototype. Include melee, projectile, prediction boundaries, network events, server validation, rate limits, fake-lag tests, and anti-cheat notes.
```

---

# Phase 15 — Skill framework

## Purpose

Convert the giant skill catalog into data-driven prototypes and handlers instead of one giant C# translation of `Skills.dm`.

## Inputs

- Phase 14 combat prototype.
- `Code/Skills.dm`
- `Code/SkillSystems.dm`
- `Code/SkillTreeSEXY.dm`
- `Code/SkillProcs.dm`
- `Code/Projectiles.dm`
- `Docs/Extracted Content/skills.md`
- `Docs/Extracted Content/skill-index.csv`

## Main work

1. Define `RiSkillPrototype`.
2. Define activation styles:
   - self buff;
   - targeted entity;
   - targeted ground point;
   - projectile;
   - beam/channel;
   - area effect;
   - toggle/form hook.
3. Define resources/cooldowns.
4. Define requirements/unlocks.
5. Define effect handlers.
6. Implement hotbar/keybind dispatch.
7. Convert only 3–5 representative skills first.
8. Build skill tests.

## Suggested files

```text
Content.Shared/RI/Skills/RiSkillPrototype.cs
Content.Shared/RI/Skills/RiSkillActivationType.cs
Content.Shared/RI/Skills/RiSkillCost.cs
Content.Shared/RI/Skills/RiSkillRequirement.cs
Content.Shared/RI/Skills/RiSkillComponent.cs
Content.Shared/RI/Skills/RiSkillSystem.cs
Content.Server/RI/Skills/RiSkillValidationSystem.cs
Content.Client/RI/Skills/RiSkillHotbarController.cs
Resources/Prototypes/RI/Skills/skills.yml
Docs/Design/skill-framework.md
```

## Exit criteria

- 3–5 skills work through the same framework.
- Cooldowns/resources are server-authoritative.
- Effects are handler-based, not hardcoded per skill in one giant switch.
- The framework can support later bulk conversion.

## Expansion prompt

```text
Expand Phase 15 into a skill framework implementation. Use Skills.dm and extracted skill docs. Include prototype schema, activation types, costs, cooldowns, requirements, handlers, hotbar, first five skills, and tests.
```

---

# Phase 16 — Transformations, buffs, and status effects

## Purpose

Rebuild forms, drains, buffs, requirements, and temporary modifiers as a general modifier/status framework.

## Inputs

- Phase 08 stat system.
- Phase 11 vitals/resources.
- Phase 15 skills.
- `Code/TransNew.dm`
- `Code/BeastTransforms.dm`
- `Code/Ascension.dm`
- `Code/Cyberize.dm`
- `Docs/Extracted Content/transformations.md`
- `Docs/Extracted Content/transformation-requirements-first-pass.csv`

## Main work

1. Define `RiModifierSource`.
2. Define stat modifier stacks.
3. Define `RiStatusEffectPrototype`.
4. Define `RiTransformationPrototype`.
5. Support:
   - activation requirements;
   - drains/upkeep;
   - stat multipliers;
   - visual/audio changes;
   - forced cancellation;
   - cooldown/lockout.
6. Convert 2–3 representative transformations.
7. Test stacking order.

## Suggested files

```text
Content.Shared/RI/Stats/RiModifierStack.cs
Content.Shared/RI/Status/RiStatusEffectPrototype.cs
Content.Shared/RI/Status/RiStatusEffectComponent.cs
Content.Shared/RI/Transformations/RiTransformationPrototype.cs
Content.Shared/RI/Transformations/RiTransformationComponent.cs
Content.Shared/RI/Transformations/RiTransformationSystem.cs
Content.Server/RI/Transformations/RiTransformationUpkeepSystem.cs
Resources/Prototypes/RI/Transformations/transformations.yml
Docs/Design/transformations-and-modifiers.md
```

## Exit criteria

- At least two forms can be activated/deactivated.
- Upkeep/drain works server-side.
- Stats update through the stat modifier framework.
- Stacking order is documented and tested.

## Expansion prompt

```text
Expand Phase 16 into transformations, buffs, and status effects. Include modifier stack design, transformation prototypes, activation requirements, drains, stat multipliers, visuals, and tests.
```

---

# Phase 17 — Items, equipment, crafting, tech, enchanting

## Purpose

Rebuild item and build systems after the character/combat core exists.

## Inputs

- Phase 08 stats.
- Phase 15 skills.
- Phase 16 modifiers.
- `Code/Items.dm`
- `Code/Tech.dm`
- `Code/Enchantment.dm`
- `Code/Build.dm`
- `Code/BuildingNewWyatt.dm`
- `Code/WyattBuild.dm`
- `Code/WyattVisual.dm`
- `Docs/Extracted Content/items-tech-enchanting.md`

## Main work

1. Define item prototypes.
2. Define equipment slots.
3. Define inventory model.
4. Define crafting recipes.
5. Define tech/building prototypes.
6. Define enchantment modifiers.
7. Convert a tiny vertical slice:
   - one wearable/equipment item;
   - one consumable;
   - one craftable;
   - one placeable/buildable;
   - one enchantment/modifier.

## Exit criteria

- Items can be held/stored.
- Equipment can affect stats through modifiers.
- Crafting produces an item server-side.
- Placeables/buildables use map/entity systems, not BYOND object dumping.

## Expansion prompt

```text
Expand Phase 17 into item/equipment/crafting/tech/enchanting v0. Use Items.dm, Tech.dm, Enchantment.dm, and Build*.dm as design references. Include prototype schemas and a small vertical slice.
```

---

# Phase 18 — Worlds, planets, maps, and environment

## Purpose

Rebuild the old planets/maps/world systems as explicit map, spawn, environment, time, and hazard systems.

## Inputs

- Phase 07 test map.
- Phase 12 environment basics.
- `Code/World.dm`
- `Code/Map.dm`
- `Code/Weather.dm`
- `Code/Years.dm`
- `Code/Systems/SpawnPoints.dm`
- `Code/PlanetConquer.dm`
- `Code/PlanetDestruction.dm`
- `Docs/Extracted Content/worlds-maps-environment.md`

## Main work

1. Define `RiPlanetPrototype`.
2. Define map IDs and spawn points.
3. Define environment flags/hazards.
4. Define world time/year/weather if retained.
5. Define planet travel or map transfer rules.
6. Implement at least two maps/planets as prototypes.
7. Ensure character saves reference stable map/spawn IDs.

## Exit criteria

- Player can spawn on at least one named planet/map.
- Spawn IDs are stable and save-friendly.
- Environment effects can be applied per map/area.
- No global `world` scans are required for ordinary updates.

## Expansion prompt

```text
Expand Phase 18 into worlds/planets/maps/environment implementation. Include planet prototypes, spawn point schema, map transfer rules, weather/time hooks, persistence IDs, and tests.
```

---

# Phase 19 — Admin, moderation, logging, and reports

## Purpose

Replace BYOND admin verbs with secure RobustToolbox commands, permissions, UI tools, audit logs, notes, reports, and moderation actions.

## Inputs

- `Code/Admin.dm`
- `Code/Reports.dm`
- `Code/Notes.dm`
- `Code/LogSystem/*`
- `Code/VotingSystem.dm`
- `Docs/Extracted Content/admin-moderation-logging.md`

## Main work

1. Define admin permission prototypes.
2. Define audit log events.
3. Define reports/help requests.
4. Define player notes.
5. Define moderation commands:
   - mute;
   - kick;
   - ban placeholder;
   - teleport/summon if needed;
   - inspect stats/save.
6. Build server-side authorization checks.
7. Build admin UI only after command security exists.

## Exit criteria

- Admin actions are permission-checked server-side.
- All actions are audit-logged.
- Players can submit reports/help requests.
- Old BYOND admin verbs have an explicit replacement plan.

## Expansion prompt

```text
Expand Phase 19 into admin/moderation/logging/report systems. Use Admin.dm, Reports.dm, Notes.dm, and LogSystem as references. Include permissions, commands, audit logging, reports, notes, and UI plan.
```

---

# Phase 20 — UI replacement program

## Purpose

Systematically replace BYOND windows, prompts, and browser panels with Robust native UI.

## Inputs

- All phases with UI needs.
- `Code/UIBackend/*`
- old `winset`, `browse`, and `input` usages from source inventory.

## Main work

Create a UI inventory and rebuild in priority order:

1. connection/debug status;
2. character creation;
3. HUD/vitals;
4. chat;
5. skill hotbar;
6. stat/character sheet;
7. inventory/equipment;
8. admin/report tools;
9. crafting/tech/enchanting;
10. world/map/travel screens.

Rules:

- UI sends requests/intents;
- server validates outcomes;
- UI never computes permanent truth;
- repeated UI state is driven by shared DTO/state objects.

## Exit criteria

- There is a single UI inventory doc.
- Each UI has an owner phase and status.
- No new gameplay feature relies on BYOND-style modal prompt assumptions.

## Expansion prompt

```text
Expand Phase 20 into a UI replacement roadmap. Inventory all expected screens, map old BYOND input/winset/browse usage to Robust UI, and define client/server message boundaries.
```

---

# Phase 21 — Content legality and asset replacement

## Purpose

Prepare the game for public release by replacing inherited names/assets/lore that are legally risky or not part of the new identity.

## Inputs

- `Docs/Legal/content-risk-register.md`
- extracted race/class/skill/transformation/item docs
- current assets in `Resources/Textures`, `Resources/Audio`, `Resources/Fonts`

## Main work

1. Audit race/class names.
2. Audit skill/transformation names.
3. Audit sprites/icons/audio/fonts.
4. Mark content as:
   - `Original`;
   - `Rename`;
   - `ReplaceArt`;
   - `Remove`;
   - `PrivateOnly`;
   - `NeedsLegalReview`.
5. Create replacement naming/lore themes.
6. Replace placeholder art before public alpha.

## Exit criteria

- Public-facing content has a legal status.
- High-risk names/assets are renamed, removed, or isolated.
- The game identity is Reincarnate, not a direct public clone of its source material.

## Expansion prompt

```text
Expand Phase 21 into a legal/content cleanup plan. Include audit tables for races, classes, skills, transformations, items, sprites, audio, fonts, and public-release risk statuses.
```

---

# Phase 22 — Testing, balance, and simulation harnesses

## Purpose

Prevent the rewrite from becoming untestable as content grows.

## Inputs

- All prototype systems.
- stat extraction docs.
- combat/skills/transformations docs.

## Main work

1. Add unit tests for pure stat math.
2. Add integration tests for prototype loading.
3. Add server tests for validation logic.
4. Add balance simulator for:
   - race/class/body stats;
   - stat point allocation;
   - training rates;
   - regen/recovery;
   - transformations;
   - skill damage/resource efficiency.
5. Add manual playtest checklists.
6. Add CI if not already present.

## Suggested tools

```text
Tools/RIBalanceSim/
Tools/RISaveMigrator/
Docs/Testing/manual-playtest-checklists.md
Docs/Testing/balance-sim-design.md
```

## Exit criteria

- Stat and prototype changes can be tested without launching a full game.
- Balance changes can be compared across versions.
- Core systems have regression coverage.

## Expansion prompt

```text
Expand Phase 22 into testing and balance harnesses. Include unit tests, prototype load tests, server validation tests, a balance simulator design, and manual playtest checklists.
```

---

# Phase 23 — Performance and anti-cheat hardening

## Purpose

Replace DM global loops/scans with scalable ECS/network-aware logic and secure validation.

## Inputs

- source-map counts for `spawn`, `sleep`, `world`, `view`, `range`, `verb`, and `input` patterns
- combat/skills/movement/chat systems
- profiling results

## Main work

1. Identify all per-tick systems.
2. Add query budgets.
3. Rate-limit input/network events.
4. Add server validation for:
   - movement;
   - skill activation;
   - projectile spawning;
   - resource spending;
   - cooldowns;
   - inventory/crafting;
   - admin commands.
5. Avoid world scans for normal gameplay.
6. Profile with multiple clients.
7. Document performance budgets.

## Exit criteria

- No high-frequency system does unbounded global scans.
- Network events are rate-limited where needed.
- Clients cannot grant themselves stats, damage, items, transformations, or admin actions.

## Expansion prompt

```text
Expand Phase 23 into a performance and anti-cheat hardening plan. Include query budgets, rate limits, validation rules, profiling checklists, and rewrites for old DM scan/loop patterns.
```

---

# Phase 24 — Packaging, hosting, and update pipeline

## Purpose

Make the project deployable as a game/server, not just a local development repo.

## Main work

1. Define build artifacts.
2. Define server config.
3. Define client distribution/update path.
4. Define asset/content update policy.
5. Define save backup policy.
6. Define crash/log collection.
7. Define private alpha deployment steps.

## Exit criteria

- A server can be built and launched outside the IDE.
- Client build/update process is documented.
- Save backups and logs are accounted for.

## Expansion prompt

```text
Expand Phase 24 into packaging, hosting, and update pipeline steps for a RobustToolboxTemplate-derived Reincarnate server/client project.
```

---

# Phase 25 — Bulk content migration factory

## Purpose

Only after the framework is tested, convert larger amounts of extracted content quickly and consistently.

## Inputs

- stable stat/skill/transformation/item prototypes
- extraction docs
- content backlog
- legal status register

## Main work

1. Build or script extraction helpers.
2. Convert content in batches:
   - races;
   - classes;
   - body types;
   - skills;
   - transformations;
   - items;
   - maps/worlds.
3. For each batch:
   - generate YAML draft;
   - run prototype load tests;
   - run balance sim;
   - mark legal status;
   - manually playtest representative samples.
4. Keep raw source private.

## Exit criteria

- Bulk conversion produces reviewable YAML/docs, not giant unreviewed C# changes.
- Each batch has tests and legal status.
- Content can be tuned without recompiling code.

## Expansion prompt

```text
Expand Phase 25 into a bulk content migration factory. Include extraction scripts, YAML generation, review workflow, tests, balance sim integration, and legal status checks.
```

---

# Phase 26 — Closed alpha and live-ops readiness

## Purpose

Prepare for real players, moderation, support, and iteration.

## Main work

1. Define closed alpha goals.
2. Define playtest schedule and feedback format.
3. Define server rules and moderation policy.
4. Define bug report template.
5. Define telemetry/log policy.
6. Define rollback/save backup plan.
7. Define content lock rules for alpha.
8. Run milestone playtests:
   - movement/map;
   - character creation/save;
   - chat/RP;
   - combat;
   - skills/forms;
   - items/worlds/admin.

## Exit criteria

- The game can host a small controlled group.
- Player data can be backed up and restored.
- Reports/moderation exist.
- Bugs and balance feedback have a workflow.

## Expansion prompt

```text
Expand Phase 26 into a closed-alpha readiness plan. Include playtest milestones, rules, moderation/support setup, bug templates, telemetry/log policy, backup/rollback plan, and launch checklist.
```

---

## 6. Immediate next actions

If Phase 5 is complete in the current guide sense, do these next, in order:

1. **Run Phase 06 repo reality check.** Confirm what is actually compiled and implemented.
2. **Do Phase 07 minimum playable surface.** Do not build more menus before the world is visible.
3. **Do Phase 08 stat implementation.** Turn the stat bible into real prototypes and tests.
4. **Do Phase 09 character creation.** It should feed into the playable surface and stat system.
5. **Do Phase 10 persistence.** Save the created character with explicit schema.

The first satisfying milestone should be:

> “I can connect to my server, see a placeholder character on a test map, move around, open a character creation/stat debug UI, choose Human/Fighter/Small, see deterministic stat changes, spawn with those stats, disconnect, reconnect, and reload the same character.”

That is the foundation needed before combat, skills, transformations, and bulk content conversion.

---

## 7. AI collaboration rules for this project

When asking ChatGPT or another AI to work on this project:

1. Provide the latest repo snapshot or link.
2. Provide the relevant phase from this guide.
3. Provide the exact DM files or extracted docs for the target system.
4. Ask for either:
   - a plan;
   - code skeletons;
   - unified diffs;
   - full file replacements;
   - tests;
   - documentation updates.
5. Require source references by file path.
6. Require compile-risk notes.
7. Require server/client/shared boundary notes.
8. Require acceptance criteria.

Use this default prompt:

```text
You are helping with Reincarnate, a RobustToolboxTemplate-based rewrite of a BYOND/DreamMaker game. Treat the DM source as design evidence, not code to port. Use the current repo as source of truth. Follow ProjectGuide.md. For this phase, provide file paths, C# skeletons or unified diffs, YAML prototypes, tests, networking boundaries, persistence concerns, and acceptance criteria. Do not assume a built-in finished game/rendering layer exists beyond RobustToolboxTemplate.
```

---

## 8. Definition of done by milestone

| Milestone | Done means |
|---|---|
| Foundation | Build/run/connect works; repo state documented; minimum playable surface visible |
| Identity | Race/class/body/stat prototypes load; stat math tested; character creation spawns a pawn |
| Persistence | Character saves/loads with explicit schema version |
| Social | Chat/RP channels work with server routing and logs |
| Combat | Melee/projectile prototype works under fake lag; server validates outcomes |
| Progression | Training/gains/resources affect stats through tested formulas |
| Skills/forms | Skills and transformations use prototypes and modifier stacks |
| World/items | Maps, planets, items, equipment, crafting, and environment exist as data-driven systems |
| Admin/live ops | Permissions, reports, notes, audit logs, moderation commands exist |
| Alpha | Playtestable build, backup/rollback, legal cleanup, support workflow |

---

## 9. Non-negotiables

- Do not copy BYOND runtime architecture into C#.
- Do not trust the client for permanent outcomes.
- Do not build large content catalogs before the framework is tested.
- Do not put formula logic in UI code.
- Do not serialize live ECS entities as long-term saves.
- Do not use global scans as the default answer to gameplay queries.
- Do not hide race/class/body/stat behavior in code switches when prototypes will do.
- Do not postpone legal/content review until public launch.
- Do not call Phase 5 “done” unless you clarify whether it means docs-only or compiled implementation.

---

## 10. Source documents this guide expects to stay close to

Primary project docs:

```text
Docs/Project/ProjectGuide.md
Docs/Project/current-state.md
Docs/Project/phase-status.md
Docs/Architecture/naming-and-folder-conventions.md
Docs/Architecture/networking-boundaries.md
Docs/Architecture/system-conventions.md
Docs/Extracted Content/source-map.md
Docs/Extracted Content/stats.md
Docs/Extracted Content/races.md
Docs/Extracted Content/classes.md
Docs/Extracted Content/body-types.md
Docs/Extracted Content/first-yaml-drafts.md
Docs/Extracted Content/skills.md
Docs/Extracted Content/combat.md
Docs/Extracted Content/projectiles.md
Docs/Extracted Content/transformations.md
Docs/Extracted Content/training.md
Docs/Extracted Content/chat-rp.md
Docs/Extracted Content/items-tech-enchanting.md
Docs/Extracted Content/worlds-maps-environment.md
Docs/Extracted Content/admin-moderation-logging.md
```

Primary BYOND source references:

```text
Code/Creation.dm
Code/CreationNew.dm
Code/statpointsystems.dm
Code/Stats.dm
Code/Variables.dm
Code/Gains.dm
Code/GameEvents/Training.dm
Code/Constants/TrainingConstants.dm
Code/BattleSystem.dm
Code/Projectiles.dm
Code/Shockwave.dm
Code/Skills.dm
Code/SkillSystems.dm
Code/SkillTreeSEXY.dm
Code/SkillProcs.dm
Code/TransNew.dm
Code/BeastTransforms.dm
Code/Ascension.dm
Code/Cyberize.dm
Code/Chat&Verbs.dm
Code/Utils/ChatUtils/LOOC.dm
Code/Utils/ChatUtils/Say.dm
Code/Utils/ChatUtils/Yell.dm
Code/Utils/ChatUtils/Emote.dm
Code/RPMode.dm
Code/SaySpark.dm
Code/Items.dm
Code/Tech.dm
Code/Enchantment.dm
Code/Build.dm
Code/BuildingNewWyatt.dm
Code/WyattBuild.dm
Code/WyattVisual.dm
Code/World.dm
Code/Map.dm
Code/Weather.dm
Code/Years.dm
Code/Systems/SpawnPoints.dm
Code/Admin.dm
Code/Reports.dm
Code/Notes.dm
Code/LogSystem/Events.dm
Code/LogSystem/Main.dm
```

