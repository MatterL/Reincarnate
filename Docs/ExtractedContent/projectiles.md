# Extracted Projectile and Beam Concepts

Source focus: `Code/Projectiles.dm`, `Code/Skills.dm`, `Code/Shockwave.dm`, `Code/Items.dm` gun sections.

## Projectile fields found

| Field | Design meaning |
| --- | --- |
| Knockback | Projectile behavior flag / tuning field |
| Explosive | Projectile behavior flag / tuning field |
| Pierce | Projectile behavior flag / tuning field |
| Big | Projectile behavior flag / tuning field |
| Paralysis | Projectile behavior flag / tuning field |
| Deflectable | Projectile behavior flag / tuning field |
| Radius | Projectile behavior flag / tuning field |
| Cutting | Projectile behavior flag / tuning field |
| DragonFisted | Projectile behavior flag / tuning field |
| Slicing | Projectile behavior flag / tuning field |
| Mugetsu | Projectile behavior flag / tuning field |
| Pushing | Projectile behavior flag / tuning field |
| Gun | Projectile behavior flag / tuning field |
| Penetration | Projectile behavior flag / tuning field |
| RPenetration | Projectile behavior flag / tuning field |
| GunType | Projectile behavior flag / tuning field |
| GunBP | Projectile behavior flag / tuning field |
| Burning | Projectile behavior flag / tuning field |
| Poisoning | Projectile behavior flag / tuning field |
| Physical | Projectile behavior flag / tuning field |
| Omega | Projectile behavior flag / tuning field |
| MegaFlare | Projectile behavior flag / tuning field |
| Controllable | Projectile behavior flag / tuning field |
| SlicingW | Projectile behavior flag / tuning field |
| Divide | Projectile behavior flag / tuning field |
| JechtShot | Projectile behavior flag / tuning field |
| WaterTrail | Projectile behavior flag / tuning field |
| BlackDragon | Projectile behavior flag / tuning field |
| Elemental | Projectile behavior flag / tuning field |
| Lethality | Projectile behavior flag / tuning field |
| SuperExplosive | Projectile behavior flag / tuning field |
| VoidProj | Projectile behavior flag / tuning field |
| Sekiha | Projectile behavior flag / tuning field |
| Distance | Projectile behavior flag / tuning field |

## Player-facing behaviors

- Ki/magic/gun projectiles travel a limited distance.
- Projectiles can be deflected, pierce, explode, apply knockback, apply elemental/status effects, or become beams.
- Gun projectiles distinguish physical/energy/ion damage and penetration.
- Beam graphics are constructed from head/tail/end states in BYOND; rewrite this as visuals separated from server damage.

## Target Robust shape

- `RprProjectilePrototype`: speed, lifetime/distance, radius, collision mask, damage specifier, visual prototype, status effects.
- `RprProjectileComponent`: owner, damage, flags, remaining lifetime, source skill ID.
- `RprProjectileSystem`: server-owned collision/damage, shared visual prediction where safe.
- `RprProjectileRateLimitComponent`: per-player projectile budget.

## Networking risks

High. Never trust client-reported hits. Client can request activation; server spawns authoritative projectile, validates cooldown/resources, and applies final damage.

## Test cases

- Projectile expires after distance/lifetime.
- Owner cannot immediately hit themselves unless a skill explicitly permits it.
- Deflect changes projectile ownership/direction server-side.
- AoE radius hits only valid entities and respects PVS/spatial query budget.
- Spam rate limits prevent projectile floods.
