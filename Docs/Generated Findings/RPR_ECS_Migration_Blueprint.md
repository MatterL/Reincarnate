# RoleplayRebirth RobustToolbox ECS Migration Blueprint

Assessment date: 2026-05-12

## Guiding principle

Do not ask “where does this DM proc go in C#?” Ask “what game fact or player action does this represent?” Then encode the fact as data and the behavior as an ECS system.

RPR's current code is highly object/proc/verb/global driven. RobustToolbox wants the opposite: components as data, entity systems as behavior, events for communication, and explicit client/shared/server boundaries.

## Recommended repository starting point

Start from `RobustToolboxTemplate` unless you explicitly want a game that inherits most SS14 gameplay assumptions. Keep SS14 nearby as a reference repo, not as the main content base.

Suggested project structure:

```text
Content.Shared/RPR/
  Character/
  Stats/
  Progression/
  Combat/
  Projectiles/
  Transformations/
  Skills/
  Chat/
  World/
  Persistence/

Content.Server/RPR/
  Persistence/
  Admin/
  Progression/
  WorldEvents/
  AntiCheat/

Content.Client/RPR/
  UI/
  CharacterCreation/
  StatsWindow/
  CombatEffects/
  Chat/

Resources/Prototypes/RPR/
  races.yml
  classes.yml
  body_types.yml
  stats.yml
  skills.yml
  transformations.yml
  projectiles.yml
  planets.yml
  items.yml
  recipes.yml
```

## Core data prototypes

### RacePrototype

Represents RPR race identity and baseline modifiers.

Fields to consider:

```yaml
- type: rprRace
  id: Saiyan
  displayName: Saiyan
  baseMod: 1.0
  trainingRate: 1.0
  meditationRate: 1.0
  zenkaiRate: 1.0
  intelligence: 1.0
  enchantment: 1.0
  gravityMod: 1.0
  masteredGravity: 1.0
  temperatureTolerance: 1.0
  temperatureType: Normal
  spawnPlanet: Vegeta
  statMods:
    energy: 1.0
    strength: 1.0
    endurance: 1.0
    speed: 1.0
    force: 1.0
    resistance: 1.0
    offense: 1.0
    defense: 1.0
    regeneration: 1.0
    recovery: 1.0
    anger: 1.0
  tags:
    - Humanoid
    - TransformingRace
```

### ClassPrototype

Represents class modifiers and unlocks.

```yaml
- type: rprClass
  id: Wizard
  displayName: Wizard
  statMods:
    energy: 1.25
    force: 1.2
    resistance: 0.9
  skillTags:
    - Magic
  startingSkills:
    - ManaSense
```

### BodyTypePrototype

Represents body-size stat bias.

```yaml
- type: rprBodyType
  id: Small
  displayName: Small
  statMods:
    strength: 0.5
    endurance: 0.5
    resistance: 0.5
    speed: 1.5
    offense: 1.25
    defense: 1.25
```

### SkillPrototype

Skills should be mostly data. Complex skills can still point to C# behavior IDs.

```yaml
- type: rprSkill
  id: BasicKiBlast
  displayName: Ki Blast
  category: Combat
  activation: TargetedProjectile
  resourceCost:
    energy: 10
  cooldown: 1.0
  projectile: KiBlastProjectile
  scaling:
    damageStat: Force
    defenseStat: Resistance
```

### TransformationPrototype

```yaml
- type: rprTransformation
  id: SuperFormExample
  displayName: Example Form
  requirements:
    raceTags: [TransformingRace]
    minimumBase: 1000
  statMultipliers:
    strength: 1.5
    speed: 1.2
    force: 1.4
  drainPerSecond:
    energy: 5
  visualState: SuperFormAura
```

## Core components

### Identity and stats

- `RprCharacterComponent`
  - Character ID, display name, account owner reference, creation state.
- `RprIdentityComponent`
  - Race prototype ID, class prototype ID, body type prototype ID.
- `RprBaseStatsComponent`
  - Raw/base stat values: Energy, Strength, Endurance, Speed, Force, Resistance, Offense, Defense, Regeneration, Recovery, Anger.
- `RprStatModifiersComponent`
  - Runtime additive/multiplicative modifiers from race, class, body, equipment, buffs, terrain, transformations.
- `RprDerivedStatsComponent`
  - Cached computed stats used by UI/combat/movement.

### Progression

- `RprProgressionComponent`
  - EXP values, base/power progression, stat EXP, decline/prime/age values if retained.
- `RprTrainingComponent`
  - Current training activity, intensity, target stat, start time, tick accumulator.
- `RprSkillBookComponent`
  - Known skill IDs, skill levels/EXP, hotbar slots.
- `RprPowerRankingComponent`
  - Cached relative rank, update timer.

### Combat and resources

- `RprVitalsComponent`
  - Health, energy, mana, oxygen, max values, regen timers.
- `RprCombatStateComponent`
  - In combat, hostility target, RP mode state, attack cooldowns.
- `RprKnockoutComponent`
  - KO state, revival time, attacker information.
- `RprDamageableComponent`
  - Defense flags, armor/equipment hooks, damage category resistances.
- `RprProjectileComponent`
  - Shooter, damage packet, speed, range, pierce/explosion/status flags.
- `RprGrabComponent`
  - Grabber/grabbed references, strength contest state.

### Transformations and effects

- `RprTransformationComponent`
  - Active form, unlocked forms, drain state, visual state.
- `RprBuffComponent`
  - Generic temporary multipliers/status effects.
- `RprAuraComponent`
  - Visual aura state and movement/combat modifiers.
- `RprElementalAffinityComponent`
  - Elemental damage/resistance interactions.

### World/content

- `RprPlanetComponent`
  - Planet ID, gravity, atmosphere/oxygen, temperature, conquer/destroy state.
- `RprEnvironmentHazardComponent`
  - Water/space/heat/cold/gravity hazard data.
- `RprBuildableComponent`
  - Build ownership, permissions, decay/savable state.
- `RprTechComponent`
  - Crafting/science data for objects/machines.
- `RprEnchantableComponent`
  - Enchantment slots, enchant IDs, quality/level.

## Core systems

| System | Location | Responsibility |
|---|---|---|
| `RprCharacterCreationSystem` | Server + Client UI | Validate and create race/class/body choices. |
| `RprStatSystem` | Shared | Combine prototypes, base stats, buffs, equipment, forms into derived stats. |
| `RprProgressionSystem` | Server | Award and persist EXP/stat gains. |
| `RprTrainingSystem` | Server, Shared for UI prediction if needed | Replace BYOND training loops. |
| `RprVitalsSystem` | Shared/Server | Regen, oxygen, health, energy, KO thresholds. |
| `RprCombatSystem` | Shared/Server | Attack requests, cooldowns, damage formulas. |
| `RprProjectileSystem` | Shared/Server + Client visuals | Predicted movement/visuals, authoritative hits. |
| `RprTransformationSystem` | Shared/Server | Form requirements, activation, drains, stat modifiers. |
| `RprSkillSystem` | Shared/Server | Skill activation, costs, cooldowns, targeting. |
| `RprEnvironmentSystem` | Server | Gravity, temperature, atmosphere, water, terrain damage. |
| `RprChatSystem` | Server + Client UI | OOC, LOOC, say, yell, emote, local range, comms. |
| `RprAdminSystem` | Server + Client UI | Permission-checked admin commands and panels. |
| `RprPersistenceSystem` | Server | Character/world save/load with schema migrations. |
| `RprWorldEventSystem` | Server | Day/night/year, wipes, scheduled events, planet events. |

## Networking model

### Put these in `Shared`

- stat calculation structs;
- component definitions that need replication;
- predicted movement/combat inputs;
- cooldown checks that affect responsiveness;
- projectile visuals/movement where safe;
- network events and serializable DTOs;
- UI state objects for character/stat windows.

### Put these in `Server`

- final save/load;
- admin authority;
- progression rewards;
- anti-cheat validation;
- permanent inventory/crafting outcomes;
- final damage application;
- world events and database writes.

### Put these in `Client`

- UI screens;
- local visuals;
- prediction-only cosmetic feedback;
- input gathering;
- client-side presentation of replicated state.

### Component replication pattern

For network-visible components, prefer Robust's generated component-state support where possible. A typical pattern is:

```csharp
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RprVitalsComponent : Component
{
    [DataField, AutoNetworkedField]
    public FixedPoint2 Health;

    [DataField, AutoNetworkedField]
    public FixedPoint2 Energy;

    [DataField, AutoNetworkedField]
    public FixedPoint2 MaxHealth;

    [DataField, AutoNetworkedField]
    public FixedPoint2 MaxEnergy;
}
```

Then systems call `Dirty(uid, component)` when server-side values change and clients need updated state.

## Prediction boundary for action combat

The most important engineering question is not “can RobustToolbox do action combat?” It can. The question is how much of RPR's fast combat can be made responsive without trusting the client.

Recommended rules:

- Predict local movement and non-authoritative visual effects.
- Let the client request attacks; never let it declare final hits or final damage.
- Server validates cooldowns, range, line of sight, resource cost, target validity, and damage.
- Use client-only visual projectiles/trails if authoritative projectile reconciliation feels bad.
- For beams/rapid projectiles, consider hitscan or server-tick batched simulation instead of thousands of independent entities.
- Build fake-lag testing into development from the first combat prototype.

## Persistence design

Use explicit schema, not entity serialization.

Suggested tables/documents:

- `accounts`
- `characters`
- `character_stats`
- `character_skills`
- `character_transforms`
- `character_inventory`
- `character_location`
- `world_state`
- `planet_state`
- `admin_roles`
- `moderation_notes`

Every record should have a schema version. Migrations should be boring and explicit.

## Translating RPR formulas safely

For formulas from RPR:

1. Extract the formula into a testable C# function.
2. Write unit tests using sample values taken from DM behavior.
3. Decide whether the behavior is intended or an accident.
4. Put tuneable constants in prototypes or config.
5. Only then wire the formula into gameplay systems.

This avoids accidentally preserving old bugs, dead code, or BYOND timing artifacts.

## Suggested first vertical slice

A good first slice proves the entire architecture without recreating all content:

- Create a small test map/grid.
- Add account/character creation with 3 races, 3 classes, and 3 body types.
- Implement derived stats from race + class + body.
- Spawn the player with movement and local chat.
- Implement one melee attack and one projectile.
- Implement one training action that increases a stat.
- Persist and reload the character.
- Add one transformation that modifies stats and drains energy.
- Test all of the above under simulated latency.

## Example Race/Class/Body stat flow

```text
RacePrototype + ClassPrototype + BodyTypePrototype
        ↓
RprIdentityComponent
        ↓
RprStatSystem initializes BaseStats + permanent modifiers
        ↓
Progression, equipment, buffs, transformations add runtime modifiers
        ↓
DerivedStatsComponent is recalculated and dirtied for UI/network state
        ↓
Combat, movement, training, UI read derived values
```

## What not to recreate

- one infinite loop per player;
- raw global lists as primary data model;
- whole-mob save serialization;
- modal input prompts as the core UI;
- admin powers as client-visible verbs;
- hidden state stored in hundreds of loosely related variables;
- projectile hit logic that depends on BYOND movement semantics;
- direct world scans for frequent gameplay checks.


## Sources consulted

- RobustToolbox repository: https://github.com/space-wizards/RobustToolbox
- Space Station 14 repository: https://github.com/space-wizards/space-station-14
- RobustToolboxTemplate repository: https://github.com/space-wizards/RobustToolboxTemplate
- SS14/Robust ECS docs: https://docs.spacestation14.com/en/robust-toolbox/ecs.html
- SS14 basic networking docs: https://docs.spacestation14.com/en/ss14-by-example/basic-networking-and-you.html
- SS14 prediction guide: https://docs.spacestation14.com/en/ss14-by-example/prediction-guide.html
- Robust grids docs: https://docs.spacestation14.com/en/robust-toolbox/transform/grids.html
- Robust physics docs: https://docs.spacestation14.com/en/robust-toolbox/transform/physics.html
- Robust net entities docs: https://docs.spacestation14.com/en/robust-toolbox/netcode/net-entities.html
- SS14 codebase organization docs: https://docs.spacestation14.com/en/general-development/codebase-info/codebase-organization.html
- RobustToolbox GPL/closed-source discussion, for legal-review awareness only: https://github.com/space-wizards/RobustToolbox/discussions/5728

