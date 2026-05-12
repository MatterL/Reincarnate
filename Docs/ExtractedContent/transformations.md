# Extracted Transformations, Forms, Buffs, and Statuses

Source focus: `Code/TransNew.dm`, `Code/BeastTransforms.dm`, `Code/Ascension.dm`, `Code/Cyberize.dm`, transformation-related skill entries in `Code/Skills.dm`.

## Extracted transformation parameter rows

The table is a machine-assisted extraction of `req`, `give`, and `multi` assignments. Rows marked `context parser could not retain race` need manual confirmation before becoming YAML.

| Race/context | Class/context | Kind | Stage | Req | Give | Multi | Note |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 1/16th Saiyan | - | trans | 1 | 1000000 | 10000000 | 2 |  |
| Aethirian | - | trans | 1 | 4000000 | 30000000 | 4 |  |
| Changeling | Ascended Chilled | trans | 1 | 350000 | 17500000 | 1.5 |  |
| Changeling | Ascended Chilled | trans | 2 | 12500000 | 62500000 | 3 |  |
| Changeling | Ascended Cooler | trans | 1 | 50000 | 1000000 | 1.5 |  |
| Changeling | Ascended Cooler | trans | 2 | 500000 | 9000000 | 1.5 |  |
| Changeling | Ascended Cooler | trans | 3 | 5000000 | 25000000 | 1.5 |  |
| Changeling | Ascended Cooler | trans | 4 | 15000000 | 70000000 | 1.75 |  |
| Changeling | Ascended Frieza | trans | 1 | 100000 | 5000000 | 1.5 |  |
| Changeling | Ascended Frieza | trans | 2 | 1000000 | 15000000 | 1.5 |  |
| Changeling | Ascended Frieza | trans | 3 | 7500000 | 50000000 | 1.5 |  |
| Changeling | Ascended King Kold | trans | 1 | 3000000 | 100000000 | 3 |  |
| Changeling | Chilled | trans | 1 | 350000 | 7500000 | 1.5 |  |
| Changeling | Chilled | trans | 2 | 12500000 | 22500000 | 2 |  |
| Changeling | Cooler | trans | 1 | 50000 | 200000 | 1.45 |  |
| Changeling | Cooler | trans | 2 | 500000 | 2000000 | 1.45 |  |
| Changeling | Cooler | trans | 3 | 5000000 | 5000000 | 1.45 |  |
| Changeling | Cooler | trans | 4 | 15000000 | 20000000 | 1.5 |  |
| Changeling | Frieza | trans | 1 | 100000 | 500000 | 1.5 |  |
| Changeling | Frieza | trans | 2 | 1000000 | 4000000 | 1.5 |  |
| Changeling | Frieza | trans | 3 | 7500000 | 10000000 | 1.5 |  |
| Changeling | King Kold | trans | 1 | 3000000 | 35000000 | 1.85 |  |
| Demi | Asura | trans | 1 | 30000000 | 50000000 | 2 |  |
| Demi | Cronus | trans | 1 | 3000000 | 60000000 | 1.5 |  |
| Demi | Fighter | trans | 1 | 3000000 | 20000000 | 1.25 |  |
| Demi | Hades | trans | 1 | 3000000 | 15000000 | 1.25 |  |
| Demi | Hercules | trans | 1 | 3000000 | 20000000 | 1.25 |  |
| Demi | Hyperion | trans | 1 | 3000000 | 30000000 | 2.5 |  |
| Demi | Mantra | trans | 1 | 30000000 | 50000000 | 2 |  |
| Demi | Oceanus | trans | 1 | 3000000 | 45000000 | 2 |  |
| Demi | Zeus | trans | 1 | 3000000 | 10000000 | 1.25 |  |
| Demon | Mazoku | trans | 1 | 5000000 | 4000000 | 1.1 |  |
| Half Demon | - | trans | 1 | 1500000 | 15000000 | 1.5 |  |
| Half Saiyan | Gohan | ssj | 1 | 1500000 | 10000000 | 2 |  |
| Heran | - | trans | 1 | 50000 | 300000 | 1.5 |  |
| Hollow | Gillian Arrancar | trans | 1 | 3750000 | 10000000 | 2 |  |
| Kaio | - | trans | 1 | 2500000 | 7500000 | 1.5 |  |
| Lycan | - | trans | 1 | 2500000 | 15000000 | 3 |  |
| Namekian | Ancient | trans | 1 | 10000000 | 75000000 | 2 |  |
| Namekian | Fighter | trans | 1 | 2500000 | 26000000 | 1.5 |  |
| Namekian | Healer | trans | 1 | 1500000 | 20000000 | 1.25 |  |
| Pathfinder | - | trans | 1 | 20000000 | 30000000 | 3 |  |
| Quarter Saiyan | - | ssj | 1 | 20000000 | 50000000 | 5 |  |
| Saiyan | Divine | ssj | 1 | 2000000 | 12000000 | 1.25 |  |
| Throwback | Shifter | trans | 1 | 0 | 4500000 | 1.15 |  |
| Vampire | - | trans | 1 |  | 15000000 | 1.5 |  |
| Youkai | Hell Raven | trans | 1 | 20000000 | 15000000 | 1.75 |  |
| Youkai | Kitsune | trans | 1 | 0 | 0 | 1 |  |
| context-needs-review | - | ssj | 1 | 5000000 | 20000000 | 3 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | - | ssj | 2 | 45000000 | 100000000 | 2.25 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | - | ssj | 3 | 300000000 | 100000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | - | trans | 1 | 750000 | 3500000 | 2 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | - | trans | 2 | 10000000 | 2500000 | 11.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | - | trans | 3 | 15000000 | 5000000 | 1.15 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | - | trans | 4 | 25000000 | 10000000 | 1.15 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Adjucha Arrancar | trans | 1 | 5625000 | 7500000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Ascended Arrancar | trans | 1 | 1 | 50000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Deus | trans | 1 | 1250000 | 2500000 | 1.15 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Dhampir | trans | 1 | 1250000 | 2500000 | 1.15 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Elite | ssj | 1 | 2500000 | 20000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Fighter | trans | 1 | 1000000 | 1000000 | 1.15 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Fire God | ssj | 1 | 5000000 | 20000000 | 2 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Goten | ssj | 1 | 1400000 | 6000000 | 1.75 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Hellspawn | ssj | 1 | 5000000 | 5000000 | 5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Legendary | ssj | 1 | 200000 | 0 | 5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Low-Class | ssj | 1 | 1500000 | 15000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Mazoku | trans | 1 | 7500000 | 10000000 | 2 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Normal | ssj | 1 | 2000000 | 12000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Quincy | trans | 1 | 25000000 | 60000000 | 5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Spiral | ssj | 1 | 3350000 | 20000000 | 2.45 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Tanuki | trans | 1 | 20000000 | 15000000 | 2 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Technologist | trans | 1 | 1250000 | 3000000 | 1.45 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Transcended Arrancar | trans | 1 | 1 | 50000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Trunks | ssj | 1 | 2000000 | 25000000 | 2.45 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Vasto Arrancar | trans | 1 | 7500000 | 7500000 | 1.25 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Vegito | ssj | 1 | 5000000 | 25500000 | 1.5 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Wizard | trans | 1 | 1250000 | 4500000 | 1.15 | context parser could not retain race; verify in TransNew.dm |
| context-needs-review | Wrath | trans | 1 | 5000000 | 13000000 | 1.5 | context parser could not retain race; verify in TransNew.dm |

## High-level transformation paths

| Path | Source concepts | Rewrite shape |
|---|---|---|
| SSJ-like staged path | `ssj` list with req/give/multi/active/unlocked/mastery | `RprTransformationPathPrototype` with stages and mastery gates |
| Generic race `trans` path | Heran, Namekian, Demi, Demon, Hollow, Human, Throwback, etc. | same generic stage model |
| Changeling forms | form-specific icons and up to 4 stages | stages + visual state per class |
| Mystic/Majin/Devil Trigger | buff skills route into transformations | transformation activated by skill/toggle requirement |
| Oozaru/SSJ4/Golden | special conditional form states | custom handler after generic path exists |
| CyberPower/CyberTrans | cybernetic trans gate | later cyberization module |

## Backend assumptions to discard

- Mutating icons directly in the form logic.
- Using nested race/class branches as the core transformation engine.
- Energy drains in the global gain loop with many special cases.

## Target Robust shape

- `RprTransformationPrototype`: ID, displayName, path, stage, requirements, stat modifiers, resource drain, visual state, legalStatus.
- `RprTransformationComponent`: current form/path, active stage, mastery values, locked state.
- `RprModifierStack`: forms add/removable stat modifiers.
- `RprTransformationSystem`: validates activation/deactivation server-side.
- `RprTransformationDrainSystem`: bounded server tick for energy/health drains.

## Test cases

- Cannot transform without unlock/req/mastery.
- Transform applies and removes modifiers exactly once.
- KO/death forces reversion where rules say so.
- Energy drain eventually reverts form.
- Visual state updates client without owning truth.
