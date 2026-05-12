# Extracted Worlds, Planets, Maps, Spawn Points, and Environment

Source focus: `Code/Creation.dm`, `Code/Map.dm`, `Code/World.dm`, `Code/Weather.dm`, `Code/Years.dm`, `Code/Planets/PlanetMatching.dm`, `Code/PlanetConquer.dm`, `Code/PlanetDestruction.dm`, `Code/Systems/SpawnPoints.dm`, `Code/Systems/WorldSettings.dm`, `Code/Systems/WaterAndOyxgen.dm`, `Code/Utils/WorldTimeUtils.dm`.

## Planets / spawn areas extracted from creation UI

| Planet / area | Extracted description concept | Rewrite use |
| --- | --- | --- |
| Earth | young diverse planet; creation planet 1 | spawn planet, map/gravity/env |
| Namek | three-star planet with green oceans/blue grass | spawn planet, map/gravity/env |
| Vegeta | desolate high-gravity planet | spawn planet, gravity tuning |
| Afterlife | Heaven/Hell/Checkpoint realm | death, respawn, afterlife travel |
| Arconia | near-Earth-like alien settlement | spawn planet |
| Android Ship | AI-controlled starship that creates Androids | spawn map/station equivalent |
| Ice | ancient cold Changeling homeworld | cold/weather hazard |

## Design concepts to preserve

- Characters choose or derive a spawn planet from race.
- Planets can be destroyed or locked; creation has special handling for destroyed race homes.
- Planet conquer/destruction are world-level states, not per-player data.
- Environment includes gravity, oxygen/water, weather, years/time, and temperature.
- Spawn points are named/managed systems, not hardcoded location calls.

## Backend assumptions to discard

- Hardcoded numeric `z` and `locate(x,y,z)` as long-term design API.
- World save/load loops that serialize live BYOND objects.
- Global scans over every turf/object/player for routine updates.

## Target Robust shape

- `RprPlanetPrototype`: id, displayName, mapId, gravity, atmosphere, defaultSpawnPoints, legalStatus.
- `RprPlanetComponent`: runtime planet state, conqueredBy, destroyed, hazard modifiers.
- `RprSpawnPointComponent`: planetId, spawnPointId, tags.
- `RprEnvironmentSystem`: oxygen/water/weather/gravity hazards.
- `RprWorldStateRepository`: persistent planet/world state.

## Test cases

- Character creation spawns a valid race on its default planet.
- Destroyed/locked planet rejects or reroutes spawn according to server settings.
- Water/oxygen hazard changes vitals through server system.
- Gravity affects training/combat formulas only through explicit modifiers.
