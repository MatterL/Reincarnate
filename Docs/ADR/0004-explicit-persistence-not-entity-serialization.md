# ADR 0004 — Explicit Persistence, Not Entity Serialization

## Status

Proposed

## Context

The original BYOND project used savefile-style persistence. Reincarnate needs explicit, versioned save records that are separate from live ECS entities.

## Decision

Phase 07 will define explicit save DTOs and schema versions.

Until then:

- do not persist live `EntityUid` values;
- do not serialize whole components as long-term saves;
- do not treat runtime map entities as permanent records;
- do not store client-provided values without server validation.

## Consequences

This ADR blocks accidental early persistence shortcuts.