# Robust ECS learning notes — Phase 02 Training Dummy

## What this exercise proves

- Entity prototype:
    - `Resources/Prototypes/RI/Sandbox/training_dummy.yml`

- Shared networked component:
    - `Content.Shared/RI/Sandbox/TrainingDummyComponent.cs`

- Networked field:
    - `TrainingDummyComponent.Hits`

- Shared network messages:
    - `TrainingDummyHitRequestEvent`
    - `TrainingDummyHitFeedbackEvent`

- Server-authoritative behavior:
    - `Content.Server/RI/Sandbox/TrainingDummySystem.cs`

- Client presentation:
    - `Content.Client/RI/Sandbox/TrainingDummyPanel.cs`
    - `Content.Client/RI/Sandbox/TrainingDummySystem.cs`

## Rule learned

The client may request an action. The server validates and applies the result.

For this spike, the client asks:

> "Please hit this dummy."

The server decides:

> "That entity exists, it has TrainingDummyComponent, and the hit may count."

Then the server increments `Hits` and dirties the component so clients receive the replicated state.

## What this is not

This is not the final RPR training system.

The final training system should add:

- player pawn validation;
- range checks;
- cooldown/rate limits;
- stamina or energy costs if desired;
- stat EXP rewards;
- anti-macro rules;
- logging if training affects progression.

## Fake lag notes

Under fake lag, the client UI may show "request sent" before the replicated counter updates.
That is acceptable.

The important rule is:

- the counter eventually converges on all clients;
- the client never increments the authoritative value locally;
- one accepted server action produces one server-side count increase.