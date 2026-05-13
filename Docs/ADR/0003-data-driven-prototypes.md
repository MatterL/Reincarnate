# ADR 0003: Data-driven prototype strategy

## Status
Accepted

## Context
The original DM code stores many gameplay definitions directly in code: race properties, stat modifiers, skill behaviors, transformation requirements, item data, spawn choices, admin permissions, and world settings. Reincarnate needs those concepts to be inspectable, testable, and tunable without burying every value in C#.

RobustToolbox already supports prototype-driven content. Reincarnate should use prototypes for stable data definitions and C# systems for behavior.

## Decision
Use YAML prototypes under `Resources/Prototypes/RI` for tunable content and shared prototype classes under `Content.Shared/RI/Prototypes` for schema definitions.

Prototype data should cover:

- races;
- classes/archetypes;
- body types;
- stat templates;
- skills;
- projectiles;
- damage categories only when they outgrow enums;
- transformations;
- items/equipment;
- planets/spawn points;
- admin roles/permissions.

C# systems should own behavior. Prototypes describe what exists and the values attached to it. Prototypes should not become scripts.

## Rules
- Put YAML under `Resources/Prototypes/RI/<Domain>/`.
- Put prototype classes under `Content.Shared/RI/Prototypes/` unless a later domain-specific split is justified.
- Use stable IDs. Once save data or external docs reference an ID, do not rename it casually.
- Prefer explicit fields over arbitrary dictionaries until the data shape truly needs extension points.
- Every value copied or derived from DM extraction should carry an audit status in docs or metadata: Preserve, Tune, Rewrite, Delete, Legal Review, Needs Balance Sim, or Needs Network Prototype.
- Do not copy raw DM source into YAML comments.

## Consequences
- Designers can tune content without touching authority systems.
- Systems can be tested against small prototype fixtures.
- Later content migration tools can generate prototype drafts.
- Prototype IDs become part of the save/network/content contract and must be treated as stable.