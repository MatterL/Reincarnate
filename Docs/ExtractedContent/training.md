# Extracted Training, Meditation, Gains, and Regen

Source focus: `Code/Gains.dm`, `Code/GameEvents/Training.dm`, `Code/GameEvents/GainLoop/GainLoopEvent.dm`, `Code/Constants/TrainingConstants.dm`, `Code/Systems/TrainingVariety.dm`, `Code/Systems/GlobalSoftcap.dm`.

## Player-facing behaviors

- Train action consumes energy and contributes to physical/stat growth.
- Meditate recovers health/energy/mana and contributes to force/resistance/intelligence/enchantment depending on state.
- Study is a focused meditation mode for Intelligence or Enchantment EXP.
- Fly consumes energy and improves flight-related skill and progression.
- Dig/mining consumes energy, grants money/resources, and may interact with tech/enchantment dig tools.
- Sparring/melee grants combat growth with fatigue/catch-up behavior.

## Extracted training types

| Type | Source concept | Stat focus | Notes |
|---|---|---|---|
| Training | `TrainingTypeConstants/Training` | Strength, Endurance, EXP | Uses Training_Rate and ControlTrainRate. |
| Medding | `TrainingTypeConstants/Medding` | Force, Resistance, EXP, age decline changes | Uses Meditation_Rate and ControlMedRate. |
| IntMedding | Subtype of Medding | Intelligence/Enchantment focus | Force/resistance gains disabled. |
| Sparring | `TrainingTypeConstants/Sparring` | Broad combat stats, energy, EXP | Strong candidate for balance sim. |

## Backend assumptions to discard

- Global `WorldTrainingEventQueue` with spawned while loop.
- Checking `icon_state` to decide whether someone is training/meditating.
- Directly mutating dozens of mob vars inside a long gain loop.
- World/player scans inside rank calculations.

## Target Robust shape

- `RprTrainingComponent`: current mode, fatigue values, focus, active training target.
- `RprProgressionComponent`: EXP, base, stat EXP, training/meditation rates.
- `RprTrainingSystem` server-side tick with bounded intervals.
- `RprTrainingTypePrototype`: amounts, probabilities, resource costs, stat focus.
- `RprBalanceSim`: reproduces training over time for balance review.

## Test cases

- Training stops when energy is insufficient.
- Meditation recovers health and energy only when allowed by current state.
- Study chooses Intelligence or Enchantment focus and increments the correct EXP.
- Sparring growth is server-authoritative and does not require trusting client hit claims.
- Under 50 simulated players, rank/catch-up calculations are bounded and cached.
