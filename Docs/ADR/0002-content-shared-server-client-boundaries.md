# ADR 0002 — Content.Shared / Content.Server / Content.Client Boundaries

## Status

Accepted

## Context

Reincarnate is a RobustToolbox multiplayer game. The original RoleplayRebirth BYOND code mixed player input, UI prompts, world state, combat state, persistence, and admin actions in ways that should not be copied directly.

The rewrite needs clear boundaries so client code cannot become authoritative over progression, combat, saves, inventory, transformations, or admin actions.

## Decision

Use the following ownership model:

### Content.Shared.RI

Shared code contains:

- pure gameplay data structures;
- networked components;
- serializable events and DTOs;
- prediction-safe calculations;
- prototype definitions;
- enums and stable IDs;
- validation helpers that do not need server-only state.

Shared code must not reference `Content.Server` or `Content.Client`.

### Content.Server.RI

Server code owns:

- final gameplay validation;
- saves and persistence;
- progression;
- damage;
- resource spending;
- character creation approval;
- inventory ownership;
- transformations and unlocks;
- admin/moderation/logging authority;
- anti-cheat and rate limits.

### Content.Client.RI

Client code owns:

- UI;
- input collection;
- local presentation;
- cosmetic effects;
- prediction-safe feedback.

Client code must not grant permanent outcomes.

## Consequences

Positive:

- New developers know where code belongs.
- Network authority is easier to review.
- Cheating risk is reduced.
- Systems can be tested independently.

Negative:

- Some features require more upfront design.
- Client UI must wait for server approval for real outcomes.
- Shared code must stay clean of server-only dependencies.

## Enforcement checklist

Before merging a new feature:

- Does it put final state changes on the server?
- Are client-to-server messages treated as untrusted?
- Are replicated fields in shared networked components?
- Does shared code avoid server/client references?
- Is persistence represented as explicit DTOs rather than live entity serialization?