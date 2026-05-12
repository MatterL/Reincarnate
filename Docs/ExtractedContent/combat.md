# Extracted Combat, Vitals, KO, Death, and Melee

Source focus: `Code/BattleSystem.dm`, `Code/Gains.dm`, `Code/Variables.dm`, `Code/RPMode.dm`.

## Player-facing behaviors

- Health can drop to KO threshold.
- KO cancels training/flying/buffs/transforms in many cases.
- Recovery from KO is delayed and affected by Regeneration.
- Lethal state can convert severe KO into death.
- Anger can trigger below health pressure and temporarily raises power behavior.
- Hostility/RP mode affects whether combat state toggles out of RP protection.
- Melee damage depends on attacker power/stat multipliers, defender endurance, weapons/buffs, accuracy, and special statuses.

## Extracted state machine

| State | Source clues | Target representation |
|---|---|---|
| Conscious | `KO = 0`, normal icon state | no `RprKnockoutComponent` or state enum Conscious |
| Knocked out | `KO = 1`, `icon_state = KO`, health/energy set low | `RprKnockoutComponent`, movement/action restrictions |
| Recovering | delayed Conscious call, Last_Zenkai timestamp | scheduled event / timer component |
| Dead / Afterlife | Death proc sends to spawn/afterlife unless immune/regenerating | `RprDeathStateComponent`, explicit respawn flow |
| RP protected | PureRPMode/Hostility branches | separate `RprRpModeComponent` used by combat validation |

## Design facts to preserve

- KO should forcibly stop training, flying, digging, many buffs, and non-permanent transformations.
- KO should set vitals low, not destroy the character.
- Death should be rarer and require lethal context or special cases.
- Recovery should not be an unbounded sleep/spawn chain.

## Backend assumptions to discard

- Damage and action gating scattered inside one huge `Melee` proc.
- Client-triggerable verbs deciding final damage.
- `view()` scans for every effect without spatial budgets.
- `spawn()` timers for attack reset and KO recovery.

## Target Robust shape

- `RprVitalsComponent`: Health, MaxHealth, Energy, MaxEnergy, Oxygen, MaxOxygen.
- `RprCombatStateComponent`: hostility, lethal flag, attack lock/cooldown, target lock.
- `RprKnockoutComponent`: attacker, reason, recoveryAt, forcedRevert flags.
- Events: `BeforeKnockoutEvent`, `KnockedOutEvent`, `RegainedConsciousnessEvent`, `BeforeDeathEvent`, `DiedEvent`.
- `RprCombatValidationSystem`: server validates range, cooldown, resources, RP protection, target validity.
- `RprDamageSystem`: applies damage, armor, status, KO/death thresholds.

## Test cases

- Health <= 0 causes KO once, not repeated KO spam.
- KO cancels training/flying/transformation according to rules.
- Lethal damage to already-KO target can cause death only when lethal rules allow.
- RP-protected player cannot be damaged by normal hostile actions.
- Client cannot report a hit and force damage without server validation.
