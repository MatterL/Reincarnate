# Extracted Body Types

Source focus: `Code/Creation.dm`, `Code/statpointsystems.dm`.

## Body type rotation

Creation cycles `Medium -> Large -> Small -> Medium`.

## Extracted stat effects

| Body type | Preserved design | Extracted modifier concept |
|---|---|---|
| Medium | Default body | No modifier. |
| Small | Agile, physically weaker | For non-Changeling races: Strength x0.5, Endurance x0.5, Speed x1.25, Resistance x0.5, Force x1.25, Offense x1.5, Defense x1.5. |
| Large | Strong/durable, slower | For non-Changeling races: Strength x1.5, Endurance x1.5, Speed x0.7, Resistance x1.5, Force x0.7, Offense x0.7, Defense x0.7. |

## Rewrite guidance

- Implement body type as `RprBodyTypePrototype`, not as post-hoc multiplication in the character creation UI.
- Keep the `raceExclusions` concept, because Changeling is exempt in the DM logic.
- Phase 05 should test stat preview determinism for Human + Small/Medium/Large before adding rare races.
