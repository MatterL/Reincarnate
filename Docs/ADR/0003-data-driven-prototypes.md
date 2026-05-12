# ADR 0003 — Data-Driven Prototype-First Content Strategy

## Status

Accepted

## Context

RoleplayRebirth contains many races, classes, body types, skills, transformations, items, planets, and admin/content rules. Directly translating those into hardcoded C# would recreate the same maintenance problems as the old DM code.

## Decision

Use prototypes for tunable content whenever possible.

Prototype-first content includes:

- races;
- classes;
- body types;
- stat templates;
- skills;
- skill categories;
- transformations;
- items;
- equipment;
- recipes;
- enchantments;
- planets;
- spawn points;
- admin roles;
- permissions.

C# systems should implement reusable behavior. YAML prototypes should hold data.

## Rules

- Do not create one-off C# classes for every skill.
- Do not hardcode race/class/body multipliers in systems.
- Do not copy DM control flow into C#.
- Do not put balance numbers directly into systems unless they are true constants.
- Tag formulas with audit status: Preserve, Tune, Rewrite, Delete, LegalReview, NeedsBalanceSim, or NeedsNetworkPrototype.

## Consequences

Positive:

- Bulk content migration becomes repeatable.
- Balance patches require less code churn.
- Tests can load representative prototype sets.
- Legal/name review can happen at the data layer.

Negative:

- Early schema design takes extra time.
- Some complex skills will still require named handlers.
- Bad prototype schema choices may need migration later.

## Example

Good:

```yaml
- type: rprRace
  id: Human
  displayName: Human
  statMods:
    strength: 1.0
    endurance: 1.0
```

Bad:

```yaml
if (race == "Human")
{
    strength *= 1.0f;
    endurance *= 1.0f;
}
```