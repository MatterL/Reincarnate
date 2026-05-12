# Persistence Candidates From Implicit DM Variables

Source focus: `Code/Variables.dm`, `Code/World.dm`, `Code/Creation.dm`, `Code/OfflinePlayers.dm`, `Code/Systems/OfflineCatchup.dm`, items/skills/transforms files.

## Character save record candidates

| Category | Fields/concepts to preserve | Notes |
|---|---|---|
| Identity | account ID/key, character ID, display name, Race, Class, BodyType, Gender, SpawnPlanet, SpawnPointName | Names need validation/sanitization. |
| Progression | Base, BaseMod, EXP, Potential, RewardPoints, RPPower, Training_Rate, Meditation_Rate | Use explicit schema version. |
| Stats | Strength/Endurance/Force/Resistance/Speed/Offense/Defense and mods | Separate base values from temporary modifiers. |
| Resources | Health, Energy, EnergyMax, Oxygen, MaxOxygen, ManaAmount, ManaMax | Decide whether health/energy persist on logout. |
| Skills | known skill IDs, learning %, cooldowns if long-term, mastery | Convert object contents to skill book records. |
| Transformations | unlocked stages, active form, mastery values, special flags | Active forms may be reverted on load unless design says otherwise. |
| Statuses | poison/burning/chilled/frozen/etc. | Most temporary statuses should not persist. |
| Inventory | item instance IDs, prototype IDs, equipment slot, durability, enchantments, custom names/icons if allowed | Never persist live entity IDs. |
| Admin/moderation | notes, punishments, reward logs | Separate moderation DB/table. |
| Location | map/planet/spawn or safe grid coordinates | Avoid raw BYOND x/y/z assumptions. |
| World state | year/time, destroyed planets, conquer state, world settings | Separate world save record. |

## Rewrite guidance

- Do not serialize components blindly as the permanent save format.
- Create DTOs that are stable, versioned, and migratable.
- Most `tmp/` variables in DM should become non-persistent runtime component state.
- Split account-level, character-level, item-instance, moderation, and world-state records.
