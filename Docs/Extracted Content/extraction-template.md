# Extraction Template

Use this file for every source-to-design pass.

## Concept name

Example: `Character race selection`, `Sparring gain`, `Basic projectile`, `OOC chat`.

## DM source files inspected

- `Code/...`

## Player-facing behavior

What a player sees, clicks, toggles, receives, spends, gains, or loses.

## Design facts to preserve

- Rule, formula, restriction, cooldown, stat field, UI state, channel, admin action, or content definition.

## Backend assumptions to discard

- BYOND `spawn`/`sleep` loops.
- BYOND `verb` as UI/command model.
- Global `world` scans.
- Modal `input` prompts.
- `savefile` live-object serialization.
- Direct writes to player vars from client-facing flows.

## Target RobustToolbox shape

- Prototypes:
- Shared components:
- Shared systems:
- Server systems:
- Client UI/states:
- Events/messages:
- Persistence records:
- Tests:

## Formula audit

| Formula / rule | Status | Notes |
|---|---|---|
|  | Preserve / Tune / Rewrite / Delete |  |

## Networking risk

Low / Medium / High. Mark every client-to-server action as untrusted.

## Persistence risk

Low / Medium / High. List fields that must persist.

## Legal/IP risk

Low / Medium / High. Note franchise-derived names, memes, art, icons, sounds, and offensive/debug names.

## Test cases

- Happy path:
- Invalid input:
- Network/fake lag:
- Persistence/load:
- Balance regression:
