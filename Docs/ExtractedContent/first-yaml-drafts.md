# First YAML Drafts for Later Phases

These are not final schemas. They are intentionally small drafts to start Phase 05/06 without dragging the whole DM codebase into the repo.

```yaml
- type: rprRace
  id: Human
  displayName: Human
  defaultSpawnPlanet: Earth
  defaultClass: Fighter
  allowedClasses: [Fighter, Technologist, Wizard]
  bodyTypePolicy: StandardHumanoid
  statTemplate: HumanBase
  progressionMods:
    trainingRate: 1.0
    meditationRate: 1.0
  legalStatus: NeedsReview

- type: rprRace
  id: Alien
  displayName: Alien
  defaultSpawnPlanet: Arconia
  defaultClass: Fighter
  allowedClasses: [Fighter, Technologist, Healer, Wizard]
  bodyTypePolicy: StandardHumanoid
  statTemplate: AlienVariable
  startingStatPoints: 90
  legalStatus: RewriteNameOptional

- type: rprBodyType
  id: Small
  displayName: Small
  statMultipliers:
    strength: 0.5
    endurance: 0.5
    speed: 1.25
    resistance: 0.5
    force: 1.25
    offense: 1.5
    defense: 1.5
  excludedRaceTags: [ChangelingLike]

- type: rprBodyType
  id: Large
  displayName: Large
  statMultipliers:
    strength: 1.5
    endurance: 1.5
    speed: 0.7
    resistance: 1.5
    force: 0.7
    offense: 0.7
    defense: 0.7
  excludedRaceTags: [ChangelingLike]

- type: rprTrainingType
  id: Training
  displayName: Train
  baseAmount: 0.04
  energyAmount: 0.0008
  expAmount: 0.0014
  statRolls:
    - stat: Strength
      probability: 90
      amount: 0.025
    - stat: Endurance
      probability: 90
      amount: 0.025

- type: rprSkill
  id: BasicBlast
  displayName: Basic Blast
  activation: TargetedProjectile
  resourceCost:
    energyPercent: 0.01
  cooldown: 0.75
  projectile: BasicKiProjectile
  legalStatus: OriginalPlaceholder
```

## Commit recommendation

Do not commit these drafts as final game content. Use them as Phase 05/06 scaffolding and replace names/stats after the stat bible is written.
