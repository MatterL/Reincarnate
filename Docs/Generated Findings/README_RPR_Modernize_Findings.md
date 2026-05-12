# RPR Modernize Findings

Assessment date: 2026-05-12

These markdown files assess the uploaded `roleplayrebirth.zip` BYOND/DM codebase as a design/content source for a new game built on RobustToolbox rather than as code to directly port.

## Files

1. `RPR_RobustToolbox_Feasibility_Assessment.md` — overall viability, recommended strategy, and system-by-system fit.
2. `RPR_Codebase_Inventory.md` — static inventory of the uploaded DM source and major mechanics discovered.
3. `RPR_ECS_Migration_Blueprint.md` — proposed RobustToolbox ECS/data/network architecture.
4. `RPR_Risks_Ideas_Concerns.md` — specific risks, migration traps, and practical ideas for a first vertical slice.

## Important assumptions

- The supplied zip contains DM source but not the full playable BYOND project assets. The DME references interface/map resources that were not present in the uploaded zip, so this was a static source assessment, not a compile/run audit.
- The RPR source should be treated as a mechanics/content specification, not as backend architecture.
- RobustToolbox should be treated as the game engine/backbone. Space Station 14 should be mined for patterns and examples, not necessarily used as the content base unless you explicitly want SS14's genre assumptions.

## Executive verdict

A RoleplayRebirth-inspired game is viable on RobustToolbox if rebuilt as a new C# ECS game with RPR mechanics translated into data-driven prototypes and focused systems. A direct BYOND-to-Robust code port is not viable in any healthy sense: too much of the source depends on BYOND verbs, global state, savefile serialization, `spawn`/`sleep` loops, `world` scans, and UI/runtime assumptions.

The recommended path is a greenfield RobustToolbox project based on `RobustToolboxTemplate`, with selective study of Space Station 14 systems for patterns such as client/server/shared code layout, networked components, prediction, chat, admin tooling, grids, physics, maps, and UI.
