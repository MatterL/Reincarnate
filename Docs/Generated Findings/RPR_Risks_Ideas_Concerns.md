# RPR Modernization Risks, Ideas, and Concerns

Assessment date: 2026-05-12

## Top risks

### 1. Treating this as a code port

The largest risk is trying to translate DM procs into C# line-by-line. That will preserve BYOND-specific architecture inside RobustToolbox and produce a fragile game that is harder to maintain than the original.

**Mitigation:** treat RPR as a design/content source. Rewrite systems around ECS, prototypes, events, and server authority.

### 2. Action combat and network prediction

RPR has fast combat, projectiles, transformations, movement modifiers, knockback, status effects, and high-frequency skill use. These are exactly the systems where multiplayer networking becomes hardest.

**Mitigation:** prototype combat early under simulated lag. Keep final authority on the server. Predict visuals and inputs where safe. Do not trust the client for hits, resources, cooldowns, or damage.

### 3. BYOND timing patterns

The codebase uses many `spawn()` and `sleep()` calls. These often hide implicit state machines, cooldowns, delayed effects, and race conditions.

**Mitigation:** replace them with explicit systems, timers, events, cooldown components, and update loops with budgets.

### 4. World scans and performance

The source contains many world/view/range-style scans. In BYOND, this is idiomatic. In a high-player Robust server, equivalent broad scans can become expensive.

**Mitigation:** use ECS queries, spatial/physics queries, map/grid-local logic, event subscriptions, and cached state instead of repeated global scans.

### 5. Savefile migration

BYOND savefiles serialize DM objects and runtime state. RobustToolbox should not persist live entity instances as the core save format.

**Mitigation:** define a versioned save schema and write import/conversion tools only for data you truly need.

### 6. Missing assets and maps

The uploaded zip does not contain the full playable asset set. Interface/map/sound resources referenced by the BYOND project were not available for this assessment.

**Mitigation:** plan for map/UI/audio recreation unless those assets are recovered separately and legally usable.

### 7. Legal/IP/licensing concerns

RPR appears to include anime-inspired race/transformation/content concepts. Some may be derivative of existing franchises. RobustToolbox and SS14 also have licensing considerations, including MIT/GPL-related files/discussions in RobustToolbox.

**Mitigation:** get a proper legal review before public release, monetization, or closed-source distribution. Use original names/assets/lore where possible. Do not assume old BYOND private-server norms are safe for a modern release.

### 8. Hardcoded secrets and sensitive data

A hardcoded secret/credential was found in the source. This should not be copied into any new repository or public artifact.

**Mitigation:** remove secrets from source, rotate anything that may have been used, and load credentials from environment/config/secrets management.

### 9. Offensive/debug variable names and old code quality issues

The source contains old/debug/offensive naming and many legacy patterns. These should not be carried forward.

**Mitigation:** use the rewrite as a cleanup boundary. Adopt naming standards, code review, linting, analyzers, and tests.

### 10. Balance preservation versus modernization

RPR's formulas may rely on hidden BYOND timing assumptions, dead code, disabled branches, or old balance exploits.

**Mitigation:** preserve design intent, not every number. Convert formulas into tests and tuneable prototypes.

## Specific codebase concerns

### `Skills.dm` as a mega-hotspot

At over 13k lines, `Skills.dm` is too large to port directly. It should be decomposed into:

- skill prototypes;
- activation systems;
- projectile/effect factories;
- buff/status systems;
- cooldown/resource systems;
- client visual effects.

### `Gains.dm` per-player loops

The gain loop does too many things: stats, regen, status timers, terrain, gravity, oxygen, grabs, AFK-ish logic, training, and more.

Break it into separate systems with clear ownership and update frequency.

### `Variables.dm` as hidden schema

`Variables.dm` effectively defines the game's implicit schema. It should become the starting point for explicit components and save records.

### `Creation.dm` as content goldmine

Race/class/body logic is one of the most useful parts of the codebase. It should be extracted into prototypes and unit-tested stat calculations.

### `Projectiles.dm` as networking testbed

Do not delay projectile work until after all content is converted. Projectiles will reveal whether the chosen combat model feels good online.

### `World.dm` as boot/save anti-pattern and feature list

`World.dm` is useful for discovering required systems, but its boot/save architecture should be replaced.

## Ideas worth preserving

- rich race/class/body identity;
- stat modifiers tied to character concept;
- training, meditation, study, and combat growth;
- transformations as long-term progression milestones;
- RP-mode/hostility boundaries;
- local chat and roleplay-focused emotes;
- planets as meaningful gameplay spaces;
- admin/rewarder moderation culture;
- crafting/tech/enchantment as parallel progression;
- soft caps/hard caps to control runaway scaling.

## Ideas to consider changing

### Data-driven balance tools

Create a balance harness that can simulate stat growth, training rates, transformation multipliers, and combat outcomes outside the live server. This can be a console tool or test suite.

### Modern character creation UI

RPR's creation logic is valuable, but BYOND prompt/window flow should be replaced with a modern multi-step UI:

1. concept/name;
2. race;
3. class;
4. body type;
5. stat preview;
6. starting planet;
7. final confirmation.

### Formula audit tags

When extracting formulas, tag them:

- `Preserve`: intentional and core to RPR identity;
- `Tune`: likely good but numbers need adjustment;
- `Rewrite`: concept is good but formula is old/fragile;
- `Delete`: dead, disabled, exploit-prone, or not worth keeping.

### Prototype-first content conversion

Before implementing all skills, convert a small representative set:

- one instant melee action;
- one projectile;
- one charge-up action;
- one beam/channel action;
- one buff;
- one transformation;
- one crafting/enchantment interaction.

This proves the content model for the rest.

### Separate RP and combat layers

RPR appears to mix RP mode, hostility, combat state, KO, admin intervention, and social systems. In the rewrite, keep clear boundaries:

- RP/social state;
- combat eligibility;
- damage/KO/death state;
- admin/moderation state.

## Recommended first deliverables

These are the first artifacts I would create before large-scale coding:

1. **RPR Stat Bible** — race/class/body/stat definitions extracted into tables.
2. **Combat Networking Prototype** — movement + one projectile + one melee action under fake lag.
3. **Save Schema v0** — explicit character/world schema with migrations from day one.
4. **Prototype Conversion Rules** — how DM skills/items/transforms become YAML + systems.
5. **Admin/Permission Model** — secure replacement for BYOND admin verbs.
6. **Content Legality Audit** — names, assets, lore, and licensing review.
7. **Performance Budget** — limits for projectiles, AOEs, active training events, world events, and tick/update systems.

## Migration trap checklist

Avoid these traps:

- copying BYOND variable names directly into components;
- making one giant `RprPlayerComponent` with hundreds of fields;
- preserving every special case before proving the base loop;
- making the client authoritative for combat because it feels easier;
- porting admin verbs before designing permissions;
- storing EntityUid-like runtime references in long-term saves;
- building UI before the data model is stable;
- balancing around single-player/local latency;
- trying to support every race/form/skill before the first vertical slice works.

## Practical modernization stance

The strongest version of this project is not “RPR but no BYOND.” It is “a new RobustToolbox multiplayer action/RP game that inherits RPR's best mechanics and community design lessons.”

That framing lets you keep the soul of the game while dropping the parts that made BYOND limiting: implicit networking, runtime object serialization, modal UI, global state, and proc-loop spaghetti.


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

