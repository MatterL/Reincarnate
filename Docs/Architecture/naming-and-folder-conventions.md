# Reincarnate Naming and Folder Conventions

## Root convention

The project uses `RI` as the folder and namespace root.

Examples:

- `Content.Shared/RI/Stats`
- `Content.Server/RI/Persistence`
- `Content.Client/RI/CharacterCreation`
- `Resources/Prototypes/RI/Character`

Domain concepts extracted from RoleplayRebirth use the `Rpr` type prefix.

Examples:

- `RprStatType`
- `RprDamageType`
- `RprChatChannel`
- `RprCombatState`
- `RprFormulaAuditStatus`

## Namespace rules

| Location | Namespace pattern |
|---|---|
| Shared code | `Content.Shared.RI.<Domain>` |
| Server code | `Content.Server.RI.<Domain>` |
| Client code | `Content.Client.RI.<Domain>` |
| Shared common enums | `Content.Shared.RI.Common` |
| Shared prototypes | `Content.Shared.RI.Prototypes` |
| Server persistence | `Content.Server.RI.Persistence` |
| Client character creation UI | `Content.Client.RI.CharacterCreation` |

## File naming rules

- Components end with `Component`.
- Entity systems end with `System`.
- Events end with `Event`.
- Network messages/events should clearly describe intent, not outcome.
- Prototype classes end with `Prototype`.
- Save DTOs end with `SaveRecord` or `SaveDto`.
- Avoid folders named `Misc`, `Helpers`, `Stuff`, or `Old`.

## System grouping rules

Group by gameplay system first.

Good:

```text
Content.Shared/RI/Combat/RprCombatComponent.cs
Content.Server/RI/Combat/RprCombatSystem.cs
Content.Client/RI/Combat/RprCombatEffectsSystem.cs
```

Bad:

```text
Content.Shared/RI/Components/RprCombatComponent.cs
Content.Server/RI/Misc/RprDamageStuff.cs
Content.Client/RI/Utils/CombatHelper.cs
```

The `Components` folder is allowed only for very generic cross-system components. Prefer system folders.