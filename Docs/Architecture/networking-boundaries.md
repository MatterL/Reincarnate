# Reincarnate Client / Shared / Server Boundaries

## Core rule

The server owns final truth.

The client may request actions, display UI, predict safe visuals, and show local feedback. The client does not decide permanent outcomes.

## Content.Shared

Use `Content.Shared.RI` for:

- networked components;
- replicated state;
- pure stat/resource formula helpers;
- prediction-safe checks;
- enum definitions;
- event DTOs;
- prototype definitions;
- shared validation helpers that do not require server-only services.

Shared code must not reference `Content.Server` or `Content.Client`.

## Content.Server

Use `Content.Server.RI` for:

- saves and persistence;
- progression changes;
- damage application;
- resource spending;
- skill unlocks;
- transformation unlocks;
- inventory ownership;
- admin authority;
- moderation and logging;
- economy;
- anti-cheat and rate limits;
- final validation of all client requests.

## Content.Client

Use `Content.Client.RI` for:

- UI windows;
- HUDs;
- input collection;
- cosmetic effects;
- local sound/visual feedback;
- predicted visuals that can be corrected by the server.

Client code must not contain final gameplay authority.

## Network event checklist

Every client-to-server message must answer:

1. Who sent it?
2. Which entity does it affect?
3. Is the sender allowed to control that entity?
4. Is the action on cooldown?
5. Can the sender afford the resource cost?
6. Is the target valid?
7. Is the target in range / line of sight if required?
8. Is the sender in a state that allows the action?
9. Is the request rate-limited?
10. What is logged if the request is invalid or suspicious?

## Component replication rule

Replicated gameplay state should generally live in `Content.Shared` components marked for networking.

Do not manually invent custom networking until the simple component-state path has been proven insufficient.