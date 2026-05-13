# ProjectGuide.md — RoleplayRebirth Modernization on RobustToolbox

**Project:** Reincarnate  
**Guide date:** 2026-05-12  
**Goal:** Rewrite RoleplayRebirth as a new RobustToolbox game, using the BYOND/DM codebase as design and content reference rather than as backend code.  
**Primary recommendation:** Start from `RobustToolboxTemplate`, keep Space Station 14 nearby as an example codebase, and rebuild RPR systems as C# ECS systems, networked components, and data-driven prototypes.

---

## 0. How to use this guide

This guide is intentionally split into **phases**. Each phase is written as its own self-contained project that can be expanded later.

When you want ChatGPT to expand a phase, paste that phase plus this instruction:

```text
Using ProjectGuide.md, expand Phase <number> into a detailed implementation plan with commands, file paths, code skeletons, tests, risks, and acceptance criteria. Assume I am rebuilding RoleplayRebirth as a new RobustToolbox game and I am weak at network coding.
```

Each phase contains:

- **Purpose** — why the phase exists.
- **Inputs** — files, docs, and prior phase outputs to inspect.
- **Main work** — the concrete project scope.
- **Deliverables** — artifacts that should exist when the phase is done.
- **Exit criteria** — what “done” means.
- **Expansion prompt** — a ready-to-copy prompt for a more verbose ChatGPT breakdown.

The guide is ordered, but several later phases can run in parallel once the core architecture is stable.

---

## 1. Modernization stance

### 1.1 The rewrite rule

Do **not** ask: “Where does this DM proc go in C#?”

Ask instead:

> “What game fact, rule, player action, UI flow, admin action, or content definition does this DM code represent?”

Then convert that concept into one or more of:

- YAML prototypes;
- C# components;
- C# entity systems;
- events;
- commands;
- UI states;
- persistence records;
- tests;
- content conversion notes.

### 1.2 Treat BYOND as the old runtime, not the new design

The uploaded RPR codebase has roughly **90 DM files** and about **59k lines** of DM source. Large hotspots include:

| DM source area | Approx. role in rewrite |
|---|---|
| `Code/Skills.dm` | Skill catalog, combat effects, cooldown concepts, projectile behaviors. |
| `Code/Creation.dm` | Race/class/body creation flow, spawn planets, unlocks, identity rules. |
| `Code/statpointsystems.dm` | Stat modifier tables and body/race/class stat point behavior. |
| `Code/Gains.dm` | Training, passive gains, regen, recovery, ranking, status-loop behavior. |
| `Code/TransNew.dm` | Transformations, forms, drains, multipliers, unlock requirements. |
| `Code/BattleSystem.dm` | KO, recovery, hostility, death/combat state transitions. |
| `Code/Projectiles.dm` | Projectile flags, hit effects, knockback/explosion/pierce behavior. |
| `Code/World.dm` | Boot/save loops, global world settings, world timers. |
| `Code/Variables.dm` | Implicit schema for player/world state. |
| `Code/Admin.dm`, `Code/Reports.dm`, `Code/Notes.dm`, `Code/LogSystem/*` | Moderation/admin/reporting/logging concepts. |
| `Code/Tech.dm`, `Code/Enchantment.dm`, `Code/Items.dm`, `Code/Build*.dm` | Items, crafting, technology, enchantments, buildables. |
| `Code/Chat&Verbs.dm`, `Code/Utils/ChatUtils/*`, `Code/RPMode.dm` | OOC/LOOC/say/emote, RP mode, social boundaries. |

The DM source also contains many BYOND-specific implementation patterns that should **not** be copied directly:

| Pattern | Static count observed | Rewrite stance |
|---|---:|---|
| `spawn` | ~1007 | Replace with systems, timers, events, scheduled updates. |
| `sleep` | ~604 | Replace with cooldowns, accumulators, timer components. |
| `verb` | ~645 | Replace with commands, interactions, UI actions, admin tools. |
| `input` | ~701 | Replace with client UI, validated network events, server decisions. |
| `winset` | ~252 | Replace with Robust UI/XAML-style controls and bound UI states. |
| `browse` | ~49 | Replace with native UI windows/panels. |
| `savefile` | ~46 | Replace with explicit versioned persistence schema. |
| `world` scans | ~556 | Replace with ECS queries, spatial queries, map/grid-local systems. |
| `view`/`range` | ~366 combined | Replace with visibility, physics, spatial, and PVS-conscious queries. |

### 1.3 RobustToolbox stance

Use **RobustToolboxTemplate** as the starting repo. Its README describes it as a barebones entry point for a RobustToolbox game with a functional client and server, and it includes helpful IDE settings and example resource structure.

Use **Space Station 14** as a reference, not as the main base, unless you intentionally want SS14’s gameplay assumptions. SS14 is a complete RobustToolbox game/content repo and is excellent for examples, but it includes large station-round systems that do not belong in an RPR-style action/RP game.

### 1.4 Network coding stance

RobustToolbox gives you rails, not magic. The server/client/shared split, component replication, prediction, PVS, and network events greatly reduce the need to hand-roll netcode, but the game still needs careful boundaries:

- The **server** owns permanent truth: saves, progression, damage, admin authority, economy, unlocks, anti-cheat, world state.
- **Shared** code owns predicted, replicated, or calculation-heavy logic used by both sides: stat math, predicted cooldown checks, movement-relevant modifiers, safe projectile visuals, network event DTOs.
- The **client** owns UI, input collection, local feedback, presentation, and cosmetic effects.

The core engineering discipline is: **never trust the client for final outcomes.**

---

## 2. Target repository shape

Use this as the default layout unless later phases prove that a different grouping is cleaner.

```text
Reincarnate/
  Content.Client/
    RI/
      CharacterCreation/
      Chat/
      Combat/
      Stats/
      UI/
      Visuals/

  Content.Shared/
    RI/
      Admin/
      Character/
      Chat/
      Combat/
      Components/
      Items/
      Movement/
      Persistence/
      Progression/
      Projectiles/
      Prototypes/
      Skills/
      Stats/
      Transformations/
      World/

  Content.Server/
    RI/
      Admin/
      Character/
      Chat/
      Combat/
      Persistence/
      Progression/
      Skills/
      World/

  Resources/
    Prototypes/
      RI/
        Character/
          races.yml
          classes.yml
          body_types.yml
          stat_templates.yml
        Combat/
          projectiles.yml
          damage_types.yml
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

  Tools/
    RIContentExtractor/
    RIBalanceSim/
    RISaveMigrator/

  Docs/
    ADR/
    Design/
    ExtractedContent/
    Networking/
    Testing/
```

Use the SS14 code organization principle: game code lives under `Content.Client`, `Content.Shared`, and `Content.Server`, grouped by system; avoid dumping everything into `Misc`, `Utility`, or one giant RPR folder.

---

## 3. Rewrite phase map

| Phase | Project name | Main output |
|---:|---|---|
| 00 | Project governance and source quarantine | Clean repo policy, license/IP/secrets notes, rewrite rules. |
| 01 | Development environment setup | Working RobustToolboxTemplate client/server build. |
| 02 | Robust/SS14 learning spike | Small ECS/network/UI lab project. |
| 03 | RPR source inventory and extraction workflow | DM-to-design extraction process and content backlog. |
| 04 | Architecture foundation | Folder layout, namespaces, ADRs, base ECS conventions. |
| 05 | Core prototypes and stat bible | Race/class/body/stat data model. |
| 06 | Character creation vertical slice | Create a character with race/class/body and preview stats. |
| 07 | Persistence v0 | Save/load explicit character data. |
| 08 | Player pawn, movement, and map bootstrap | Spawn, move, and constrain a player entity. |
| 09 | Chat and RP social layer | OOC/LOOC/say/yell/emote/RP mode basics. |
| 10 | Vitals, regen, KO, and death-state foundation | Health/energy/oxygen/KO state machine. |
| 11 | Combat networking prototype | One melee action and one projectile under fake lag. |
| 12 | Skill framework | Prototype-driven skill activation/cooldown/resource model. |
| 13 | Transformations, buffs, and status effects | Data-driven form activation, drains, stat modifiers. |
| 14 | Items, equipment, crafting, and enchantment | Item/inventory/equipment/crafting/enchant v0. |
| 15 | Worlds, planets, maps, and environment | Planet prototypes, spawn points, hazards, maps. |
| 16 | Admin, moderation, logging, and reports | Secure replacement for BYOND admin verbs. |
| 17 | UI replacement program | Native Robust UI for RPR workflows. |
| 18 | Content legality and asset replacement | Naming, lore, sprites, audio, license audit. |
| 19 | Testing, balance, and simulation harnesses | Unit tests, integration tests, balance simulator. |
| 20 | Performance and anti-cheat hardening | Query budgets, rate limits, validation rules. |
| 21 | Packaging, hosting, and update pipeline | Build artifacts, server deployment, update flow. |
| 22 | Bulk content migration factory | Repeatable skill/race/item/transformation conversion. |
| 23 | Closed alpha and live-ops readiness | Playtest loop, telemetry, policy, support docs. |

---

# Phase 00 — Project governance and source quarantine

## Purpose

Establish the legal, security, and engineering boundaries before any code is copied or published.

This phase prevents three common failures:

1. accidentally publishing secrets from the BYOND source;
2. copying legally risky names/assets/lore into a public modern repo;
3. turning the rewrite into a line-by-line port.

## Inputs

- `roleplayrebirth.zip`
- `RPR_Modernize_Findings_MD.zip`
- `README_RPR_Modernize_Findings.md`
- `RPR_Risks_Ideas_Concerns.md`
- BYOND files especially:
  - `Code/World.dm`
  - `Code/Variables.dm`
  - `Code/Admin.dm`
  - `Code/Creation.dm`
  - `Code/Skills.dm`

## Main work

1. Create a private archival location for the original DM source.
2. Do **not** commit the raw BYOND source into the new public repo unless you have a deliberate, private, legally reviewed reason.
3. Create a `Docs/ADR/0001-rewrite-not-port.md` decision record.
4. Create a `Docs/Security/source-secret-audit.md` note.
5. Create a `Docs/Legal/content-risk-register.md` note.
6. Write a “no direct port” policy:
   - no copying DM code comments blindly;
   - no preserving offensive/debug variable names;
   - no copying hardcoded secrets;
   - no assuming franchise-derived terms are safe for a modern release;
   - no serializing live entities as long-term saves.
7. Decide repo visibility:
   - private during extraction and legal cleanup;
   - public only after names/assets/secrets are clean.

## Deliverables

- `Docs/ADR/0001-rewrite-not-port.md`
- `Docs/Security/source-secret-audit.md`
- `Docs/Legal/content-risk-register.md`
- `Docs/Project/rewrite-rules.md`
- `.gitignore` entries for raw extracted DM archives if stored locally.

## Exit criteria

- The new repo has no raw secrets from the BYOND source.
- The team agrees RPR source is design input, not runtime architecture.
- Every future phase can cite the rewrite rules when rejecting direct DM-style implementation.

## Expansion prompt

```text
Expand Phase 00 from ProjectGuide.md into a detailed project checklist. Include exact document templates for ADR 0001, a secrets audit, a legal/content risk register, a gitignore policy for raw BYOND source, and a developer onboarding note explaining why this is a rewrite rather than a port.
```

---

# Phase 01 — Development environment setup

## Purpose

Get a clean RobustToolbox project building and running before adding RPR mechanics.

This phase is deliberately boring. Its goal is to prove that the toolchain, Git submodules, IDE, client, and server all work.

## Inputs

- RobustToolboxTemplate GitHub repo.
- RobustToolbox docs and setup docs.
- SS14 setup docs for troubleshooting.

## Main work

### 1. Install prerequisites

Recommended baseline:

- Git.
- Python 3.x, matching current SS14/Robust setup guidance.
- .NET SDK version required by the current template or `global.json`.
- Rider, Visual Studio, or VS Code/VSCodium with C# tooling.
- YAML tooling; for VS Code/VSCodium, install a YAML extension and consider Robust YAML tooling.

On Windows, the current SS14 setup docs show `winget` examples for Git, Python, and the .NET SDK. Always verify the required .NET SDK against the template and docs before locking the project.

### 2. Clone the template correctly

Do not download a GitHub zip. Use Git so submodules and history are present.

```bash
git clone --recurse-submodules https://github.com/space-wizards/RobustToolboxTemplate.git Reincarnate
cd Reincarnate
```

If the repo was cloned without submodules:

```bash
git submodule update --init --recursive
```

### 3. Build

```bash
dotnet restore
dotnet build
```

### 4. Run server and client

In two terminals:

```bash
dotnet run --project Content.Server
```

```bash
dotnet run --project Content.Client
```

Use direct connect in the client to connect to the local server.

### 5. Rename the project carefully

Only after the template builds:

- rename solution/project display names if desired;
- set game name/config values;
- create `Content.* / RPR` root folders;
- keep namespace changes small and compile after each change.

### 6. Create developer docs

Document:

- exact SDK version;
- exact clone command;
- build command;
- run command;
- common failures;
- where to put RPR code;
- how to update submodules safely.

## Deliverables

- Working local client/server build.
- `Docs/Setup/development-environment.md`
- `Docs/Setup/troubleshooting.md`
- First commit: “Bootstrap RobustToolboxTemplate”.
- Second commit: “Add RPR folder skeleton and docs”.

## Exit criteria

- A new developer can clone, build, run server, run client, and connect locally using only your setup doc.
- No RPR mechanics have been added yet.
- The repo has a clean first baseline commit.

## Expansion prompt

```text
Expand Phase 01 from ProjectGuide.md into exact Windows, Linux, and macOS setup instructions for a RobustToolboxTemplate-based RPR project. Include install commands, clone commands, submodule commands, build/run commands, IDE setup, troubleshooting, and a verification checklist.
```

---

# Phase 02 — Robust/SS14 learning spike

## Purpose

Build a tiny training feature before touching RPR systems. This phase teaches the minimum RobustToolbox concepts needed for the rewrite.

## Inputs

- RobustToolbox ECS docs.
- SS14 basic networking docs.
- SS14 prediction guide.
- SS14 codebase organization docs.
- RobustToolboxTemplate examples.

## Main work

Create a tiny feature named `RiSandboxTrainingDummy` that includes:

1. an entity prototype;
2. a component in `Content.Shared`;
3. a system in `Content.Shared` or split server/client if needed;
4. one networked field;
5. one local event or interaction;
6. one network event if UI/client input is involved;
7. one small UI or popup feedback;
8. one test if feasible.

Example feature:

- Spawn a training dummy.
- Click/use it.
- Server increments a counter.
- Counter is replicated to the client.
- Client shows “Training hits: N”.
- Under fake lag, behavior remains acceptable.

## Deliverables

- `Content.Shared/RI/Sandbox/TrainingDummyComponent.cs`
- `Content.Shared/RI/Sandbox/TrainingDummySystem.cs`
- Optional client visual/UI file.
- `Resources/Prototypes/RI/Sandbox/training_dummy.yml`
- `Docs/Learning/robust-ecs-notes.md`

## Exit criteria

- You understand where code belongs: Client vs Shared vs Server.
- You have used a component, entity system, prototype, event, and networked field.
- You have tested with at least two clients or fake lag.

## Expansion prompt

```text
Expand Phase 02 from ProjectGuide.md into a hands-on RobustToolbox learning exercise. Provide file paths, full code for a networked TrainingDummy component/system/prototype, a tiny UI or popup, and a fake-lag testing checklist.
```

---

# Phase 03 — RPR source inventory and extraction workflow

## Purpose

Create a repeatable way to extract useful design from DM files without porting bad architecture.

## Inputs

- Entire `roleplayrebirth.zip` source.
- Previous markdown findings.
- BYOND files grouped by system.

## Main work

1. Create `Docs/ExtractedContent/`.
2. Create a standard extraction template:
   - DM source files inspected;
   - concept name;
   - original behavior summary;
   - player-facing behavior;
   - backend assumptions to discard;
   - data fields to preserve;
   - formulas to preserve/tune/rewrite/delete;
   - target Robust prototypes/components/systems;
   - networking risk;
   - persistence risk;
   - legal/IP risk;
   - test cases.
3. Extract first high-value systems:
   - Race/class/body stats from `Creation.dm` and `statpointsystems.dm`.
   - Stats from `Stats.dm`, `Variables.dm`.
   - Gains/training from `Gains.dm` and `Code/GameEvents/Training.dm`.
   - Transformations from `TransNew.dm`, `BeastTransforms.dm`, `Ascension.dm`.
   - Skills from `Skills.dm`, `SkillSystems.dm`, `SkillTreeSEXY.dm`, `SkillProcs.dm`.
   - Combat/KO/projectiles from `BattleSystem.dm`, `Projectiles.dm`, `Shockwave.dm`.
   - Chat/RP from `Chat&Verbs.dm`, `RPMode.dm`, `SaySpark.dm`.
   - Admin/moderation from `Admin.dm`, `Reports.dm`, `Notes.dm`, `LogSystem/*`.
4. Create a content backlog with labels:
   - `Preserve`
   - `Tune`
   - `Rewrite`
   - `Delete`
   - `Legal Review`
   - `Needs Network Prototype`
   - `Needs Balance Sim`

## Deliverables

- `Docs/ExtractedContent/extraction-template.md`
- `Docs/ExtractedContent/source-map.md`
- `Docs/ExtractedContent/content-backlog.md`
- First extracted files:
  - `races.md`
  - `classes.md`
  - `body-types.md`
  - `stats.md`
  - `training.md`
  - `combat.md`

## Exit criteria

- You have a consistent process for turning DM into design specs.
- At least one system is extracted all the way to proposed Robust components/prototypes.
- No raw DM code is copied into C#.

## Expansion prompt

```text
Expand Phase 03 from ProjectGuide.md into a full DM content extraction from the roleplayrebirth.zip source files.
```

---

# Phase 04 — Architecture foundation

## Purpose

Create the stable skeleton of the new game before building mechanics.

## Inputs

- Phase 01 repo.
- Phase 02 learning spike.
- Phase 03 extraction process.
- SS14 code organization docs.

## Main work

1. Create root RPR folders under `Content.Shared`, `Content.Server`, and `Content.Client`.
2. Create namespace conventions:
   - `Content.Shared.RPR.Stats`
   - `Content.Server.RPR.Persistence`
   - `Content.Client.RPR.CharacterCreation`
3. Create system conventions:
   - components are mostly data;
   - systems own behavior;
   - prototypes hold tuneable content;
   - server validates final outcomes;
   - shared code must not call server-only code;
   - client code must not own permanent truth.
4. Create core enums/IDs:
   - `RprStatType`
   - `RprResourceType`
   - `RprDamageType`
   - `RprChatChannel`
   - `RprCombatState`
   - `RprFormulaAuditStatus`
5. Create ADRs:
   - ECS structure.
   - Networking boundary.
   - Persistence boundary.
   - Prototype-first content strategy.
6. Create a minimal CI build if possible.

## Deliverables

- Folder skeleton.
- `Docs/ADR/0002-content-shared-server-client-boundaries.md`
- `Docs/ADR/0003-data-driven-prototypes.md`
- `Docs/Architecture/naming-and-folder-conventions.md`
- `Docs/Architecture/networking-boundaries.md`
- `Content.Shared/RPR/Common/` with core IDs/enums.

## Exit criteria

- New systems have an obvious place to live.
- Every developer understands Client/Shared/Server boundaries.
- CI or a local build script catches compile failures.

## Expansion prompt

```text
Expand Phase 04 from ProjectGuide.md into a detailed RobustToolbox architecture bootstrap. Include folder creation, namespace conventions, ADR templates, base enums, code skeletons, and a first CI/build verification plan.
```

---

# Phase 05 — Core prototypes and stat bible

## Purpose

Convert the most valuable part of RPR — race/class/body/stat identity — into data-driven prototypes and a written stat bible.

## Inputs

- `Code/Creation.dm`
- `Code/CreationNew.dm`
- `Code/statpointsystems.dm`
- `Code/Stats.dm`
- `Code/Variables.dm`
- Phase 03 extracted `races.md`, `classes.md`, `body-types.md`, `stats.md`

## Main work

1. Define stat vocabulary.
2. Decide which original stats remain:
   - Base / Battle Power equivalent.
   - Energy.
   - Strength.
   - Endurance.
   - Force.
   - Resistance.
   - Speed.
   - Offense.
   - Defense.
   - Regeneration.
   - Recovery.
   - Anger.
   - Intelligence/enchantment/training/meditation/growth modifiers.
3. Create prototypes:
   - `RprRacePrototype`
   - `RprClassPrototype`
   - `RprBodyTypePrototype`
   - `RprStatTemplatePrototype`
   - `RprProgressionCurvePrototype`
4. Build the first YAML data set with only a few races/classes/body types.
5. Implement `RprStatSystem` in shared code.
6. Add tests for baseline stat calculation.
7. Write `Docs/Design/stat-bible.md`.

## Example prototype shape

```yaml
- type: rprRace
  id: Human
  displayName: Human
  spawnPlanet: Earth
  statMods:
    strength: 1.0
    endurance: 1.0
    force: 1.0
    resistance: 1.0
    speed: 1.0
    offense: 1.0
    defense: 1.0
    regeneration: 1.0
    recovery: 1.0
  progressionMods:
    trainingRate: 1.0
    meditationRate: 1.0
    intelligenceRate: 1.0
    enchantmentRate: 1.0
  tags:
    - Humanoid
```

## Deliverables

- `Content.Shared/RI/Stats/RprStatType.cs`
- `Content.Shared/RI/Stats/RprStatSystem.cs`
- `Content.Shared/RI/Prototypes/RprRacePrototype.cs`
- `Content.Shared/RI/Prototypes/RprClassPrototype.cs`
- `Content.Shared/RI/Prototypes/RprBodyTypePrototype.cs`
- `Resources/Prototypes/RI/Character/races.yml`
- `Resources/Prototypes/RI/Character/classes.yml`
- `Resources/Prototypes/RI/Character/body_types.yml`
- `Docs/Design/stat-bible.md`
- Stat calculation tests.

## Exit criteria

- Given race + class + body type, the shared stat system can compute a deterministic stat preview.
- At least three races, three classes, and three body types work.
- The formulas are tagged as Preserve/Tune/Rewrite/Delete.

## Expansion prompt

```text
Expand Phase 05 from ProjectGuide.md into a complete stat-prototype implementation plan. Include how to extract race/class/body data from the DM files, YAML schemas, C# prototype classes, RprStatSystem code skeleton, and unit tests for stat calculation.
```

---

# Phase 06 — Character creation vertical slice

## Purpose

Build the first real RPR feature: a player can create a character with race, class, body type, and a stat preview.

## Inputs

- Phase 05 stat prototypes.
- `Code/Creation.dm`
- `Code/CreationNew.dm`
- `Code/UIBackend/Menu.dm`
- `Code/statpointsystems.dm`

## Main work

1. Define `RprCharacterComponent`.
2. Define `RprIdentityComponent`.
3. Define character creation UI states and messages.
4. Build a client UI flow:
   - name/concept;
   - race;
   - class;
   - body type;
   - spawn planet if needed;
   - stat preview;
   - confirmation.
5. Server validates choices:
   - allowed race;
   - allowed class;
   - unlocks if implemented;
   - name constraints;
   - spawn constraints.
6. Server creates player entity/pawn using selected identity.
7. UI shows final computed stats.

## Deliverables

- `Content.Shared/RPR/Character/RprCharacterComponent.cs`
- `Content.Shared/RPR/Character/RprIdentityComponent.cs`
- `Content.Shared/RPR/Character/RprCharacterCreationMessages.cs`
- `Content.Server/RPR/Character/RprCharacterCreationSystem.cs`
- `Content.Client/RPR/CharacterCreation/` UI files.
- `Docs/Design/character-creation-flow.md`

## Exit criteria

- A local player can create a character and spawn into a test map.
- Bad choices are rejected by the server.
- Race/class/body stat preview matches server-side final stats.
- Character creation does not rely on modal BYOND-style prompts.

## Expansion prompt

```text
Expand Phase 06 from ProjectGuide.md into a step-by-step RobustToolbox character creation implementation. Include shared network messages, server validation, client UI flow, component definitions, spawn behavior, stat preview, and tests.
```

---

# Phase 07 — Persistence v0

## Purpose

Replace BYOND `savefile` object serialization with explicit, versioned save records.

## Inputs

- `Code/Creation.dm`
- `Code/World.dm`
- `Code/OfflinePlayers.dm`
- `Code/Systems/OfflineCatchup.dm`
- `Code/Variables.dm`
- Phase 06 character data.

## Main work

1. Define what must persist in v0:
   - account/user ID;
   - character ID;
   - display name;
   - race/class/body type IDs;
   - base stats;
   - stat EXP;
   - known skills;
   - current map/spawn point;
   - simple inventory later if available;
   - schema version.
2. Choose a storage backend:
   - local JSON for earliest prototype;
   - SQLite/PostgreSQL for serious multiplayer;
   - migration path from local to DB.
3. Create save DTOs separate from ECS components.
4. Write load flow:
   - account connects;
   - character list loads;
   - selected character spawns;
   - ECS entity is constructed from save DTO.
5. Write save flow:
   - periodic save;
   - save on disconnect;
   - admin save command;
   - graceful shutdown save.
6. Create migration framework from schema version 0 onward.

## Deliverables

- `Content.Server/RPR/Persistence/RprCharacterSaveRecord.cs`
- `Content.Server/RPR/Persistence/RprPersistenceSystem.cs`
- `Content.Server/RPR/Persistence/RprCharacterRepository.cs`
- `Docs/Architecture/persistence-schema-v0.md`
- `Docs/ADR/0004-explicit-persistence-not-entity-serialization.md`
- Save/load tests.

## Exit criteria

- A character can be created, saved, disconnected, reloaded, and spawned again.
- Save data is readable and versioned.
- No live `EntityUid` values are treated as permanent save IDs.

## Expansion prompt

```text
Expand Phase 07 from ProjectGuide.md into a detailed persistence v0 implementation. Include save DTOs, repository interface, local JSON or SQLite option, save/load flow, schema migrations, disconnect/shutdown handling, and tests.
```

---

# Phase 08 — Player pawn, movement, and map bootstrap

## Purpose

Rebuild the basic player entity and movement restrictions without recreating one giant BYOND `Move()` override.

## Inputs

- `Code/PlayerMovement/Move.dm`
- `Code/PlayerMovement/Macros.dm`
- `Code/Flight.dm`
- `Code/Systems/WaterAndOyxgen.dm`
- `Code/Map.dm`
- Phase 06/07 character spawn.

## Main work

1. Define the base RPR player entity prototype.
2. Add movement-relevant components:
   - identity;
   - stats;
   - vitals;
   - combat state;
   - movement restrictions;
   - flight state;
   - environment state.
3. Implement movement gates as separate systems:
   - KO prevents movement;
   - frozen/stunned prevents movement;
   - meditation/training may restrict movement;
   - flight changes movement behavior;
   - swimming/water/oxygen hazards affect movement/vitals;
   - RP mode may affect combat, not basic movement unless design says otherwise.
4. Build a test map with spawn points.
5. Avoid world scans; use entity/spatial/map queries.

## Deliverables

- `Resources/Prototypes/RPR/Entities/player.yml`
- `Content.Shared/RPR/Movement/RprMovementRestrictionComponent.cs`
- `Content.Shared/RPR/Movement/RprFlightComponent.cs`
- `Content.Shared/RPR/Movement/RprMovementSystem.cs`
- Test map/resource.
- `Docs/Design/movement-rules.md`

## Exit criteria

- A saved character spawns and moves on a test map.
- Movement restrictions are modular and testable.
- There is no monolithic equivalent of BYOND `Move.dm`.

## Expansion prompt

```text
Expand Phase 08 from ProjectGuide.md into a RobustToolbox player movement project. Include player entity prototype, movement restriction components, flight/swim/KO/frozen gates, map setup, and tests.
```

---

# Phase 09 — Chat and RP social layer

## Purpose

Recreate the roleplay communication backbone early, because it is central to RPR’s identity and easier than combat networking.

## Inputs

- `Code/Chat&Verbs.dm`
- `Code/Utils/ChatUtils/*`
- `Code/RPMode.dm`
- `Code/SaySpark.dm`
- `Code/Reports.dm`
- SS14 chat examples if useful.

## Main work

1. Define channels:
   - OOC;
   - LOOC;
   - Say;
   - Yell;
   - Whisper if retained;
   - Emote;
   - Admin/help/report channels later.
2. Define range rules and formatting rules.
3. Define RP mode state separate from chat state.
4. Server validates and routes messages.
5. Client displays messages in chat UI.
6. Add moderation hooks:
   - logs;
   - mutes;
   - admin visibility;
   - report integration later.

## Deliverables

- `Content.Shared/RPR/Chat/RprChatChannel.cs`
- `Content.Shared/RPR/Chat/RprChatMessage.cs`
- `Content.Server/RPR/Chat/RprChatSystem.cs`
- `Content.Client/RPR/Chat/RprChatUIController.cs`
- `Docs/Design/chat-and-rp-mode.md`

## Exit criteria

- Players can use OOC, LOOC, local say, yell, and emote.
- Server controls routing and range.
- Chat logs exist for moderation.
- RP mode is represented as explicit state, not mixed randomly into combat/chat code.

## Expansion prompt

```text
Expand Phase 09 from ProjectGuide.md into a complete chat/RP implementation plan. Include channel definitions, range checks, server-side validation, client UI, logs, mutes, emotes, RP mode state, and tests.
```

---

# Phase 10 — Vitals, regen, KO, and death-state foundation

## Purpose

Build the health/energy/oxygen/KO foundation that combat, training, transformations, and environment hazards will rely on.

## Inputs

- `Code/BattleSystem.dm`
- `Code/Gains.dm`
- `Code/Variables.dm`
- `Code/Systems/WaterAndOyxgen.dm`
- `Code/RPMode.dm`

## Main work

1. Define vitals:
   - health;
   - max health;
   - energy;
   - max energy;
   - oxygen;
   - max oxygen;
   - regen/recovery rates.
2. Define state components:
   - KO;
   - dead/death-pending if used;
   - stunned/frozen/status tags;
   - combat eligibility;
   - RP protection if retained.
3. Replace DM procedural flows with events:
   - `BeforeKnockoutEvent`
   - `KnockedOutEvent`
   - `RegainedConsciousnessEvent`
   - `BeforeDeathEvent`
   - `DiedEvent`
4. Implement regen as a server/shared system with tick budget.
5. Add UI state for vitals.
6. Add tests for thresholds.

## Deliverables

- `Content.Shared/RPR/Vitals/RprVitalsComponent.cs`
- `Content.Shared/RPR/Vitals/RprKnockoutComponent.cs`
- `Content.Shared/RPR/Vitals/RprVitalsSystem.cs`
- `Content.Server/RPR/Vitals/RprRegenSystem.cs`
- `Docs/Design/vitals-ko-death.md`
- Tests for KO/recovery.

## Exit criteria

- Damage can reduce health/energy.
- KO triggers at the correct threshold.
- Regen/recovery works without per-player infinite loops.
- UI can display replicated vitals.

## Expansion prompt

```text
Expand Phase 10 from ProjectGuide.md into a vitals/KO implementation. Include components, events, regen timers, server/shared split, UI state, and tests based on RPR BattleSystem.dm and Gains.dm behavior.
```

---

# Phase 11 — Combat networking prototype

## Purpose

Attack the hardest technical risk early: action combat under real multiplayer latency.

This phase should happen **before** bulk skill conversion.

## Inputs

- `Code/Projectiles.dm`
- `Code/BattleSystem.dm`
- `Code/Skills.dm` representative attacks.
- RobustToolbox prediction guide.
- Robust basic networking docs.

## Main work

Build only two combat actions:

1. Basic melee strike.
2. Basic projectile/ki blast.

For each action:

- client sends intent/input;
- shared code predicts what is safe;
- server validates cooldown, resources, range, hit, target, damage;
- server applies final damage/KO;
- client shows responsive feedback;
- misprediction is tested under fake lag.

Design rules:

- Do not trust client-reported hits.
- Do not trust client resource costs.
- Do not trust client cooldowns.
- Predict visuals and input responsiveness where safe.
- Keep final damage server-authoritative.

## Deliverables

- `Content.Shared/RPR/Combat/RprCombatComponent.cs`
- `Content.Shared/RPR/Combat/RprAttackIntent.cs`
- `Content.Shared/RPR/Combat/RprDamageSpecifier.cs`
- `Content.Shared/RPR/Projectiles/RprProjectileComponent.cs`
- `Content.Shared/RPR/Projectiles/RprProjectileSystem.cs`
- `Content.Server/RPR/Combat/RprCombatValidationSystem.cs`
- `Resources/Prototypes/RPR/Combat/projectiles.yml`
- `Docs/Networking/combat-prototype-results.md`

## Exit criteria

- Two local clients can fight on a test map.
- Fake lag has been tested.
- Mispredictions are documented.
- Projectile spam is rate-limited.
- The team has decided whether RobustToolbox feels acceptable for RPR-style action.

## Expansion prompt

```text
Expand Phase 11 from ProjectGuide.md into a combat networking prototype plan. Include melee and projectile implementation, prediction boundaries, network events, server validation, fake-lag testing, anti-cheat checks, and performance limits.
```

---

# Phase 12 — Skill framework

## Purpose

Convert RPR’s giant skill catalog into a data-driven skill system rather than a giant C# translation of `Skills.dm`.

## Inputs

- `Code/Skills.dm`
- `Code/SkillSystems.dm`
- `Code/SkillTreeSEXY.dm`
- `Code/SkillProcs.dm`
- `Code/Projectiles.dm`
- Phase 11 combat prototype.

## Main work

1. Create `RprSkillPrototype`.
2. Define activation types:
   - instant self effect;
   - targeted entity;
   - targeted position;
   - projectile;
   - beam/channel;
   - aura/buff;
   - toggle;
   - passive.
3. Define resource costs:
   - energy;
   - health;
   - cooldown;
   - charge time;
   - prerequisites;
   - required state/form/race/class.
4. Define skill effect handlers:
   - damage;
   - heal;
   - buff;
   - teleport/dash;
   - projectile spawn;
   - status effect;
   - transformation trigger;
   - build/craft trigger.
5. Convert a representative set first:
   - one melee skill;
   - one projectile;
   - one beam/channel;
   - one buff;
   - one movement skill;
   - one utility skill.
6. Build hotbar/client activation flow.

## Deliverables

- `Content.Shared/RPR/Skills/RprSkillPrototype.cs`
- `Content.Shared/RPR/Skills/RprSkillBookComponent.cs`
- `Content.Shared/RPR/Skills/RprSkillSystem.cs`
- `Content.Server/RPR/Skills/RprSkillValidationSystem.cs`
- `Resources/Prototypes/RPR/Skills/skills.yml`
- `Docs/Design/skill-conversion-rules.md`
- `Docs/ExtractedContent/skills-first-pass.md`

## Exit criteria

- At least six representative skills work.
- New simple skills can be added mostly through YAML.
- Complex skills use named handlers rather than custom one-off spaghetti.
- Skill activation uses the combat/networking boundaries proven in Phase 11.

## Expansion prompt

```text
Expand Phase 12 from ProjectGuide.md into a data-driven skill framework. Include skill prototype schema, activation types, resource/cooldown model, hotbar input, server validation, effect handlers, representative conversions from Skills.dm, and tests.
```

---

# Phase 13 — Transformations, buffs, and status effects

## Purpose

Recreate RPR’s transformations/forms as data-driven stat modifiers and state changes.

## Inputs

- `Code/TransNew.dm`
- `Code/BeastTransforms.dm`
- `Code/Ascension.dm`
- `Code/Cyberize.dm`
- `Code/BattleSystem.dm`
- `Code/Gains.dm`
- Phase 05 stats.
- Phase 12 skill framework.

## Main work

1. Create `RprTransformationPrototype`.
2. Define requirements:
   - race/class/body tags;
   - minimum base/power/stat;
   - unlock flags;
   - mastery;
   - quest/admin unlock if retained.
3. Define effects:
   - stat multipliers;
   - resource drain;
   - visual aura/sprite state;
   - movement/combat modifiers;
   - skill unlocks;
   - restrictions.
4. Define `RprBuffComponent` and generic modifier stack.
5. Define status effect lifecycle:
   - applied;
   - refreshed;
   - ticked;
   - expired;
   - removed on KO/death/logout if needed.
6. Convert one simple form and one draining form first.

## Deliverables

- `Content.Shared/RPR/Transformations/RprTransformationPrototype.cs`
- `Content.Shared/RPR/Transformations/RprTransformationComponent.cs`
- `Content.Shared/RPR/Buffs/RprBuffComponent.cs`
- `Content.Shared/RPR/Stats/RprModifierStack.cs`
- `Resources/Prototypes/RPR/Transformations/transformations.yml`
- `Docs/Design/transformations-and-buffs.md`

## Exit criteria

- A character can unlock/activate/deactivate one form.
- Form modifies stats through the same stat system used elsewhere.
- Drain and cancellation work.
- KO/death cleanup works.
- Visual state is replicated or presented correctly.

## Expansion prompt

```text
Expand Phase 13 from ProjectGuide.md into a transformation/buff/status implementation. Include transformation prototypes, requirements, stat modifiers, drains, visual state, KO cleanup, tests, and conversion examples from TransNew.dm.
```

---

# Phase 14 — Items, equipment, crafting, and enchantment

## Purpose

Create the foundation for non-character progression: items, equipment, technology, crafting, and enchantments.

## Inputs

- `Code/Items.dm`
- `Code/Tech.dm`
- `Code/Enchantment.dm`
- `Code/Build.dm`
- `Code/BuildingNewWyatt.dm`
- `Code/WyattBuild.dm`
- `Code/CarsAndOtherAddons.dm`
- Phase 05 stat system.
- Phase 07 persistence.

## Main work

1. Decide inventory model:
   - use Robust/SS14-style inventory patterns if helpful;
   - keep RPR-specific slots minimal at first.
2. Define equipment slots.
3. Define item prototypes.
4. Define enchantable item data.
5. Define crafting/tech recipes.
6. Define server validation for crafting and enchanting.
7. Add persistence for item instances only after item model is stable.
8. Convert one item, one weapon, one armor/equipment piece, one recipe, one enchantment.

## Deliverables

- `Content.Shared/RPR/Items/RprItemComponent.cs`
- `Content.Shared/RPR/Items/RprEquipmentComponent.cs`
- `Content.Shared/RPR/Items/RprEnchantableComponent.cs`
- `Content.Shared/RPR/Crafting/RprRecipePrototype.cs`
- `Content.Server/RPR/Crafting/RprCraftingSystem.cs`
- `Content.Server/RPR/Enchanting/RprEnchantingSystem.cs`
- YAML prototypes for first items/recipes/enchantments.
- `Docs/Design/items-crafting-enchanting.md`

## Exit criteria

- Player can acquire and equip a test item.
- Equipment modifies stats through the stat system.
- Player can craft one recipe.
- Player can apply one enchantment.
- Persistence plan for item instances is documented.

## Expansion prompt

```text
Expand Phase 14 from ProjectGuide.md into an item/equipment/crafting/enchantment implementation plan. Include prototype schemas, equipment slots, stat modifiers, crafting validation, enchantment data, persistence concerns, and first converted examples from Items.dm/Tech.dm/Enchantment.dm.
```

---

# Phase 15 — Worlds, planets, maps, and environment

## Purpose

Rebuild the planet/world structure and environment hazards without BYOND map/runtime assumptions.

## Inputs

- `Code/Map.dm`
- `Code/World.dm`
- `Code/Weather.dm`
- `Code/Years.dm`
- `Code/Planets/PlanetMatching.dm`
- `Code/PlanetConquer.dm`
- `Code/PlanetDestruction.dm`
- `Code/Systems/SpawnPoints.dm`
- `Code/Systems/WorldSettings.dm`
- `Code/Systems/WaterAndOyxgen.dm`

## Main work

1. Define `RprPlanetPrototype`.
2. Define map/grid strategy:
   - separate maps per planet;
   - map chunks/instances;
   - spawn points by planet;
   - admin spawn/test maps.
3. Define hazards:
   - gravity;
   - oxygen/atmosphere;
   - water;
   - heat/cold;
   - weather;
   - day/night/year if retained.
4. Define planet-level events:
   - conquer state;
   - destruction state;
   - dragonball/world objective hooks if retained.
5. Build one test planet and one hazard.

## Deliverables

- `Content.Shared/RPR/World/RprPlanetPrototype.cs`
- `Content.Shared/RPR/World/RprPlanetComponent.cs`
- `Content.Server/RPR/World/RprEnvironmentSystem.cs`
- `Resources/Prototypes/RPR/World/planets.yml`
- `Resources/Prototypes/RPR/World/spawn_points.yml`
- Test map(s).
- `Docs/Design/worlds-planets-environment.md`

## Exit criteria

- Character creation can spawn a player on a selected planet/test map.
- One environment hazard affects vitals or movement.
- Planet data lives in prototypes, not hardcoded switch chains.

## Expansion prompt

```text
Expand Phase 15 from ProjectGuide.md into a world/planet/map/environment implementation plan. Include planet prototypes, spawn point handling, map/grid approach, gravity/oxygen/water hazards, world timers, and first test planet.
```

---

# Phase 16 — Admin, moderation, logging, and reports

## Purpose

Replace BYOND admin verbs with secure commands, permissions, UI panels, logs, reports, and audit trails.

## Inputs

- `Code/Admin.dm`
- `Code/AdminOverview.dm`
- `Code/Reports.dm`
- `Code/Notes.dm`
- `Code/LogSystem/*`
- `Code/VotingSystem.dm`
- `Code/Packages.dm`
- Phase 09 chat.
- Phase 07 persistence.

## Main work

1. Define admin roles and permissions.
2. Convert verbs into categories:
   - observe/investigate;
   - teleport/summon;
   - player moderation;
   - character edit/reward;
   - world edit;
   - logs/reports;
   - dangerous/destructive commands.
3. Create server-side command handlers.
4. Create admin UI only after command handlers are safe.
5. Log all admin actions.
6. Define report/ahelp flow.
7. Define ban/mute/note records.
8. Avoid trusting client UI for permission checks.

## Deliverables

- `Content.Shared/RPR/Admin/RprAdminPermission.cs`
- `Content.Server/RPR/Admin/RprAdminSystem.cs`
- `Content.Server/RPR/Admin/RprAdminCommandSystem.cs`
- `Content.Server/RPR/Moderation/RprReportSystem.cs`
- `Content.Server/RPR/Logging/RprAuditLogSystem.cs`
- `Resources/Prototypes/RPR/Admin/roles.yml`
- `Docs/Design/admin-moderation-logging.md`

## Exit criteria

- Admin permission checks are centralized.
- At least one safe admin command works.
- At least one report/ahelp flow works.
- Admin actions are logged.
- Dangerous tools require explicit permission.

## Expansion prompt

```text
Expand Phase 16 from ProjectGuide.md into a secure admin/moderation implementation. Include permission model, command categories, server-side validation, admin UI plan, reports/ahelps, notes, bans/mutes, audit logs, and conversion mapping from Admin.dm.
```

---

# Phase 17 — UI replacement program

## Purpose

Replace BYOND `input`, `browse`, `winset`, macros, and skin windows with native Robust UI patterns.

## Inputs

- `Code/Creation.dm`
- `Code/Customization.dm`
- `Code/GridStat.dm`
- `Code/Stats.dm`
- `Code/AdminOverview.dm`
- `Code/Reports.dm`
- `Code/UIBackend/*`
- `Code/PlayerMovement/Macros.dm`

## Main work

1. Inventory every UI flow:
   - character creation;
   - stat window;
   - skill window/hotbar;
   - chat;
   - admin panels;
   - reports;
   - crafting/enchanting;
   - customization/profile;
   - spawn editor/build tools.
2. Define UI priority:
   - required for vertical slice;
   - required for alpha;
   - later.
3. Build reusable UI state DTOs.
4. Use server validation for every UI action.
5. Create UI style guidelines.
6. Avoid huge all-in-one windows.

## Deliverables

- `Docs/UI/ui-inventory.md`
- `Docs/UI/ui-style-guide.md`
- `Docs/UI/ui-networking-rules.md`
- First stable windows:
  - character creation;
  - stats;
  - chat;
  - skill hotbar.

## Exit criteria

- No new feature depends on BYOND-style modal prompts.
- UI states are explicit and network-safe.
- Server rejects invalid UI actions.

## Expansion prompt

```text
Expand Phase 17 from ProjectGuide.md into a UI replacement roadmap. Include UI inventory from BYOND files, Robust client UI structure, UI state DTOs, server validation, styling conventions, and implementation plans for character creation, stats, chat, and skill hotbar.
```

---

# Phase 18 — Content legality and asset replacement

## Purpose

Prepare the project for public release or broader distribution by replacing or reviewing risky names, lore, art, audio, and franchise-derived concepts.

## Inputs

- Extracted race/class/transformation/skill/item lists.
- Any recovered interface/map/sound/art assets.
- Licensing info for RobustToolbox/SS14/template.
- Phase 00 legal risk register.

## Main work

1. Create a content risk spreadsheet or markdown table.
2. Tag every race, transformation, skill, item, icon, sound, and lore term:
   - original-safe;
   - needs rename;
   - needs replacement art;
   - needs legal review;
   - internal-only placeholder.
3. Create replacement naming guidelines.
4. Create art direction guidelines.
5. Define attribution policy.
6. Decide what can appear in public builds.

## Deliverables

- `Docs/Legal/content-risk-register.md` updated.
- `Docs/Design/naming-and-lore-rewrite-guide.md`
- `Docs/Art/art-direction.md`
- `Docs/Audio/audio-direction.md`
- Public-safe content list.

## Exit criteria

- The public repo/build does not depend on clearly risky borrowed assets or secrets.
- Placeholder names are clearly marked.
- The project has a strategy for original lore/assets.

## Expansion prompt

```text
Expand Phase 18 from ProjectGuide.md into a content legality and asset replacement project. Include risk categories, audit tables, replacement naming/lore rules, art/audio guidance, attribution policy, and public-build gates.
```

---

# Phase 19 — Testing, balance, and simulation harnesses

## Purpose

Make the rewrite measurable. RPR’s formulas and progression loops need tests and simulations because old BYOND behavior may depend on timing quirks.

## Inputs

- `Code/Gains.dm`
- `Code/statpointsystems.dm`
- `Code/Stats.dm`
- `Code/BattleSystem.dm`
- `Code/Skills.dm`
- Phase 05 stat system.
- Phase 11 combat prototype.
- Phase 12 skill framework.

## Main work

1. Create unit tests for:
   - stat calculation;
   - modifiers;
   - transformation multipliers;
   - resource costs;
   - cooldowns;
   - damage formulas;
   - persistence migration.
2. Create integration tests for:
   - character creation;
   - save/load;
   - chat routing;
   - KO/recovery;
   - skill activation.
3. Build `Tools/RprBalanceSim`:
   - stat growth over time;
   - training rates;
   - race/class/body comparisons;
   - transformation impact;
   - combat outcome sampling.
4. Define balance dashboards or CSV outputs.
5. Tag formulas as Preserve/Tune/Rewrite/Delete.

## Deliverables

- Test projects or test folders matching Robust conventions.
- `Tools/RprBalanceSim/`
- `Docs/Testing/test-plan.md`
- `Docs/Balance/balance-simulation-plan.md`
- First balance simulation output.

## Exit criteria

- Core formulas are covered by tests.
- Balance changes can be simulated outside a live server.
- The team can detect accidental formula regressions.

## Expansion prompt

```text
Expand Phase 19 from ProjectGuide.md into a testing and balance simulation plan. Include unit/integration test lists, balance simulator architecture, sample simulations for race/class/body stat growth, CSV output, and regression workflow.
```

---

# Phase 20 — Performance and anti-cheat hardening

## Purpose

Prevent the action/RP game from collapsing under projectile spam, global scans, bad UI events, or client-authoritative exploits.

## Inputs

- Phase 11 combat prototype results.
- Phase 12 skill framework.
- Phase 15 world/environment systems.
- Robust networking/PVS docs.
- DM inventory showing world/view/range scans and spawn/sleep loops.

## Main work

1. Define performance budgets:
   - max projectiles per player;
   - max active AOEs;
   - max skill activations per second;
   - max expensive spatial queries per tick;
   - max world events per interval.
2. Add rate limits:
   - input events;
   - chat;
   - skills;
   - admin actions where relevant;
   - crafting/enchanting requests.
3. Add server validation:
   - resource costs;
   - cooldowns;
   - range/LOS;
   - target validity;
   - movement restrictions;
   - inventory ownership;
   - transformation requirements.
4. Instrument expensive systems.
5. Review all broad queries.
6. Add fake-lag and multi-client test scenarios.

## Deliverables

- `Docs/Performance/performance-budget.md`
- `Docs/Security/anti-cheat-validation-rules.md`
- Rate-limit components/systems.
- Logging/instrumentation for combat and skills.
- Load-test notes.

## Exit criteria

- Projectile/skill spam has hard server-side limits.
- Client cannot grant itself stats, damage, items, transformations, or progress.
- Expensive queries are documented and bounded.

## Expansion prompt

```text
Expand Phase 20 from ProjectGuide.md into a performance and anti-cheat hardening project. Include performance budgets, rate limits, validation rules, instrumentation, broad-query review, fake-lag/multi-client tests, and exploit test cases.
```

---

# Phase 21 — Packaging, hosting, and update pipeline

## Purpose

Prepare the game for repeatable builds and multiplayer hosting.

## Inputs

- RobustToolbox/SS14 packaging docs.
- Server hosting docs.
- Phase 01 build/run notes.
- Phase 20 performance notes.

## Main work

1. Define build modes:
   - development;
   - staging;
   - production.
2. Create packaging scripts.
3. Define server config.
4. Define secrets/config handling:
   - never hardcode credentials;
   - use environment variables or external config;
   - document local dev secrets separately.
5. Define update process.
6. Define logs/backups.
7. Decide whether to use a CDN/update mechanism for serious hosting.
8. Create deployment checklist.

## Deliverables

- `Docs/Deployment/build-and-run.md`
- `Docs/Deployment/server-config.md`
- `Docs/Deployment/secrets-policy.md`
- `Docs/Deployment/backup-and-restore.md`
- Build/package scripts.
- Staging server checklist.

## Exit criteria

- A staging server can be deployed from a clean build.
- Config/secrets are not committed.
- Backups and logs are accounted for.

## Expansion prompt

```text
Expand Phase 21 from ProjectGuide.md into a RobustToolbox packaging/hosting pipeline. Include dev/staging/prod configs, build scripts, server deployment, secrets policy, backups, logs, update strategy, and staging verification.
```

---

# Phase 22 — Bulk content migration factory

## Purpose

Once the core systems work, create a repeatable factory for converting the rest of RPR’s content.

## Inputs

- Phase 03 extraction workflow.
- Phase 05/12/13/14 prototype schemas.
- All content-heavy DM files:
  - `Skills.dm`
  - `TransNew.dm`
  - `Tech.dm`
  - `Enchantment.dm`
  - `Items.dm`
  - `Creation.dm`
  - `statpointsystems.dm`

## Main work

1. Create conversion batches:
   - races/classes/body types;
   - skills by category;
   - transformations by race/path;
   - items/equipment;
   - recipes/enchantments;
   - maps/planets/world content.
2. For each batch:
   - extract design;
   - tag formula status;
   - create YAML/prototype data;
   - implement missing handler if needed;
   - add tests;
   - playtest;
   - balance-sim if relevant;
   - legal/name review.
3. Create content review checklist.
4. Create “definition of converted.”
5. Track coverage percentage.

## Deliverables

- `Docs/Migration/content-conversion-checklist.md`
- `Docs/Migration/conversion-batches.md`
- `Docs/Migration/content-coverage.md`
- Expanded YAML prototype sets.
- New handlers only when generic handlers cannot express the content.

## Exit criteria

- Content conversion is no longer ad hoc.
- Every converted item has source notes, tests, and review status.
- Prototype coverage grows without destabilizing core systems.

## Expansion prompt

```text
Expand Phase 22 from ProjectGuide.md into a bulk content migration factory. Include batch planning, extraction-to-YAML workflow, review checklist, coverage tracking, test requirements, legal/name review, and examples for skills/transforms/items.
```

---

# Phase 23 — Closed alpha and live-ops readiness

## Purpose

Move from engineering prototype to a controlled multiplayer playtest.

## Inputs

- Completed vertical slice.
- Phase 16 moderation tools.
- Phase 20 hardening notes.
- Phase 21 deployment pipeline.

## Main work

1. Define alpha scope:
   - supported races/classes;
   - supported skills;
   - supported maps/planets;
   - enabled admin tools;
   - disabled risky systems.
2. Create playtest rules.
3. Create bug report process.
4. Create moderation escalation process.
5. Create wipe/reset policy.
6. Create telemetry/log review process.
7. Create balance patch process.
8. Run staged tests:
   - solo local;
   - two-client local;
   - private staging;
   - small group;
   - larger group.

## Deliverables

- `Docs/Alpha/alpha-scope.md`
- `Docs/Alpha/playtest-rules.md`
- `Docs/Alpha/bug-report-template.md`
- `Docs/Alpha/moderation-process.md`
- `Docs/Alpha/wipe-policy.md`
- `Docs/Alpha/balance-patch-process.md`

## Exit criteria

- Closed alpha can run without developer improvisation.
- Moderators know what tools exist and what they are allowed to do.
- Testers know what is in scope.
- Bugs and balance problems are recorded in a repeatable way.

## Expansion prompt

```text
Expand Phase 23 from ProjectGuide.md into a closed-alpha readiness plan. Include alpha scope, playtest rules, tester onboarding, bug reports, moderation process, wipe policy, telemetry/log review, balance patch workflow, and staged rollout checklist.
```

---

## 4. Suggested milestone schedule

This is not a calendar promise; it is an ordering strategy.

### Milestone A — Engine boot and learning

- Phase 00
- Phase 01
- Phase 02
- Phase 04

Goal: clean repo, working client/server, basic Robust literacy.

### Milestone B — RPR identity vertical slice

- Phase 03
- Phase 05
- Phase 06
- Phase 07
- Phase 08
- Phase 09

Goal: create, save, load, move, and talk as an RPR-style character.

### Milestone C — Combat feasibility

- Phase 10
- Phase 11
- Phase 19 first pass
- Phase 20 first pass

Goal: prove or disprove action combat feel under networking constraints before converting hundreds of skills.

### Milestone D — Content systems

- Phase 12
- Phase 13
- Phase 14
- Phase 15
- Phase 17

Goal: make the reusable systems that can receive the bulk of RPR content.

### Milestone E — Server reality

- Phase 16
- Phase 18
- Phase 20 final pass
- Phase 21
- Phase 23

Goal: safe staging server and controlled playtest.

### Milestone F — Bulk migration

- Phase 22 ongoing

Goal: convert content only after the systems are proven.

---

## 5. First vertical slice definition

The first serious playable slice should include exactly this:

1. Local server and client build.
2. Character creation:
   - name;
   - race;
   - class;
   - body type;
   - stat preview.
3. Save/load.
4. Spawn on a test map.
5. Movement.
6. Chat:
   - OOC;
   - LOOC;
   - local say;
   - emote.
7. Vitals:
   - health;
   - energy;
   - regen;
   - KO state.
8. Combat:
   - one melee attack;
   - one projectile.
9. Training action:
   - increases one stat or stat EXP.
10. One transformation:
   - modifies stats;
   - drains energy;
   - has a visible state.
11. One admin command:
   - teleport, observe, or view character stats.
12. Tests for stat calculation, save/load, KO, and combat cooldown/resource validation.

Do not add the full skill list before this slice works.

---

## 6. Recommended ChatGPT expansion workflow

Use one phase at a time. Example:

```text
I am working on RPR-Modernize. Expand Phase 11 from ProjectGuide.md. I want a detailed implementation guide for the combat networking prototype in RobustToolbox. Include exact files, code skeletons, fake lag testing, server validation, and pitfalls for someone bad at network coding.
```

For coding phases, ask for:

```text
Break the phase into small commits. For each commit, list the files to create/change, the reason, the expected compile/test result, and rollback notes.
```

For content phases, ask for:

```text
Use the DM source as design input only. Create extraction tables and YAML prototype drafts, but do not directly port the BYOND implementation style.
```

For networking phases, ask for:

```text
Identify what belongs in Content.Shared, Content.Server, and Content.Client. Mark every client-to-server message as untrusted and show where the server validates it.
```

---

## 7. Persistent design rules

1. **Prototype-first content.** If a race, class, skill, transformation, item, or planet can be data, make it data.
2. **Components hold data. Systems hold behavior.** Avoid one giant `RprPlayerComponent`.
3. **Shared code is for shared/predicted/serializable logic.** It must not depend on server-only services.
4. **Server owns final truth.** Saves, damage, progression, inventory, transforms, admin actions, and world state are server decisions.
5. **Client owns presentation.** UI and visuals are allowed to feel responsive but must not grant outcomes.
6. **No BYOND loop cloning.** Replace `spawn`/`sleep` loops with timers, events, scheduled systems, and bounded updates.
7. **No world-scan habits.** Use ECS queries, spatial/physics queries, maps/grids, events, and cached state.
8. **No implicit saves.** Use explicit save records with schema versions and migrations.
9. **No unreviewed content names/assets.** Public-facing content needs legal/IP review.
10. **Prove action combat early.** Networking feel is the highest technical risk.

---

## 8. Source and reference notes

### Project source files inspected

- Uploaded `roleplayrebirth.zip` static DM source.
- Previously generated modernization findings:
  - `README_RPR_Modernize_Findings.md`
  - `RPR_RobustToolbox_Feasibility_Assessment.md`
  - `RPR_Codebase_Inventory.md`
  - `RPR_ECS_Migration_Blueprint.md`
  - `RPR_Risks_Ideas_Concerns.md`

### Robust/SS14 references used

- RobustToolboxTemplate: https://github.com/space-wizards/RobustToolboxTemplate
- RobustToolbox: https://github.com/space-wizards/RobustToolbox
- Space Station 14: https://github.com/space-wizards/space-station-14
- SS14 setup docs: https://docs.spacestation14.com/en/general-development/setup/setting-up-a-development-environment.html
- SS14/Robust ECS docs: https://docs.spacestation14.com/en/robust-toolbox/ecs.html
- SS14 codebase organization docs: https://docs.spacestation14.com/en/general-development/codebase-info/codebase-organization.html
- Basic Networking and You: https://docs.spacestation14.com/en/ss14-by-example/basic-networking-and-you.html
- Prediction guide: https://docs.spacestation14.com/en/ss14-by-example/prediction-guide.html
- Net entities docs: https://docs.spacestation14.com/en/robust-toolbox/netcode/net-entities.html

---

## 9. Immediate next action

Start with **Phase 01** unless your machine already has a working RobustToolboxTemplate client/server.

If the template already builds, expand **Phase 02** next and build the sandbox training dummy. That small exercise will make the later RPR systems much less mysterious.
