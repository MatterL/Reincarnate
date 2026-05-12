# Reincarnate System Conventions

## Components

Components are mostly data.

Good component responsibilities:

- replicated fields;
- current runtime state;
- prototype IDs;
- simple flags;
- timers/cooldown timestamps;
- references to other entities when runtime-only.

Bad component responsibilities:

- large methods;
- service lookups;
- persistence writes;
- admin permission checks;
- direct UI control.

## Systems

Systems own behavior.

Good system responsibilities:

- subscribing to events;
- validating actions;
- applying state changes;
- calling `Dirty` when replicated state changes;
- handling server/client/shared boundaries;
- translating prototypes into behavior.

## Prototypes

Prototypes hold tunable content.

Use prototypes for:

- races;
- classes;
- body types;
- skills;
- transformations;
- items;
- planets;
- admin roles.

## Events

Use events to decouple systems.

Examples:

- `RprBeforeDamageEvent`
- `RprDamageAppliedEvent`
- `RprBeforeKnockoutEvent`
- `RprKnockedOutEvent`
- `RprSkillAttemptEvent`
- `RprSkillActivatedEvent`

## Client-to-server rule

Client messages are requests, not facts.

Bad:

- client says “I hit this player for 500 damage”
- client says “I gained 10 strength”
- client says “I unlocked this transformation”

Good:

- client says “I want to use skill X”
- server validates cooldown/resource/range/state
- server applies final result