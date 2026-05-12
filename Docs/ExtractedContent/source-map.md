# Source Map

This inventory exists so future phases can choose source files by design area instead of hunting through DM implementation details.

## Hotspot summary

| Area | Key files | Primary target |
| --- | --- | --- |
| Identity/stats | Creation.dm, CreationNew.dm, statpointsystems.dm, Stats.dm, Variables.dm | Phase 05/06 prototypes + stat system |
| Progression | Gains.dm, GameEvents/Training.dm, Constants/TrainingConstants.dm | Phase 10/19 progression + balance sim |
| Combat/projectiles | BattleSystem.dm, Projectiles.dm, Shockwave.dm | Phase 10/11 combat prototype |
| Skills | Skills.dm, SkillSystems.dm, SkillTreeSEXY.dm, SkillProcs.dm | Phase 12 skill framework |
| Transformations | TransNew.dm, BeastTransforms.dm, Ascension.dm, Cyberize.dm | Phase 13 transformations |
| Items/tech/enchant | Items.dm, Tech.dm, Enchantment.dm, Build*.dm | Phase 14 items/crafting/enchanting |
| Chat/RP | Chat&Verbs.dm, Utils/ChatUtils/*.dm, RPMode.dm, SaySpark.dm | Phase 09 social layer |
| Admin/live ops | Admin.dm, Reports.dm, Notes.dm, LogSystem/* | Phase 16 admin/moderation |
| World/environment | World.dm, Map.dm, Weather.dm, Years.dm, Planet*.dm, SpawnPoints.dm | Phase 15 worlds/environment |

## Full DM file inventory

| Path | Lines | Bytes | spawn | sleep | verb | input | world | Role | Target | Priority |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Code/Admin.dm | 2501 | 92704 | 11 | 10 | 25 | 168 | 138 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/AdminOverview.dm | 139 | 5386 | 1 | 1 | 2 | 3 | 3 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/Alliance.dm | 83 | 3611 | 1 | 0 | 3 | 5 | 0 | Support/misc | Review later | P3 |
| Code/Ascension.dm | 54 | 1119 | 0 | 0 | 0 | 0 | 0 | Transformations, forms, drains, buffs, unlock requirements | transformation prototypes, modifier stacks | P1 |
| Code/BattleSystem.dm | 1865 | 59561 | 62 | 22 | 2 | 2 | 6 | Combat, KO, death, melee, projectile flags | vitals, combat, projectile systems | P1 |
| Code/BeastTransforms.dm | 403 | 10441 | 20 | 10 | 3 | 2 | 0 | Transformations, forms, drains, buffs, unlock requirements | transformation prototypes, modifier stacks | P1 |
| Code/Build.dm | 623 | 18496 | 7 | 2 | 5 | 2 | 14 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/BuildingNewWyatt.dm | 533 | 17756 | 1 | 1 | 2 | 28 | 5 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/CarsAndOtherAddons.dm | 430 | 11682 | 1 | 5 | 4 | 4 | 0 | Support/misc | Review later | P3 |
| Code/Chat&Verbs.dm | 912 | 31719 | 16 | 3 | 14 | 26 | 21 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Constants/TrainingConstants.dm | 104 | 2018 | 0 | 0 | 0 | 0 | 0 | Training, meditation, passive gains, fatigue, regen, rank catch-up | progression systems, balance sim, tests | P1 |
| Code/Creation.dm | 2732 | 89498 | 36 | 2 | 2 | 6 | 19 | Character creation, stats, implicit player schema | race/class/body/stat prototypes, character components, save DTO | P1 |
| Code/CreationNew.dm | 179 | 5563 | 0 | 1 | 2 | 1 | 6 | Character creation, stats, implicit player schema | race/class/body/stat prototypes, character components, save DTO | P1 |
| Code/Customization.dm | 1423 | 37735 | 0 | 3 | 2 | 8 | 7 | Support/misc | Review later | P3 |
| Code/Cyberize.dm | 70 | 2863 | 0 | 2 | 6 | 2 | 2 | Transformations, forms, drains, buffs, unlock requirements | transformation prototypes, modifier stacks | P1 |
| Code/Dragonballs.dm | 130 | 3036 | 4 | 1 | 0 | 3 | 4 | Support/misc | Review later | P3 |
| Code/Enchantment.dm | 2144 | 69282 | 4 | 8 | 38 | 42 | 8 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/EventScheduler/CommonEvents.dm | 13 | 255 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/EventScheduler/ErrorHandling.dm | 20 | 953 | 0 | 0 | 0 | 0 | 1 | Support/misc | Review later | P3 |
| Code/EventScheduler/Event.dm | 6 | 28 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/EventScheduler/EventScheduler.dm | 97 | 2390 | 1 | 1 | 0 | 0 | 1 | Support/misc | Review later | P3 |
| Code/Extra.dm | 121 | 3210 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Flight.dm | 31 | 661 | 0 | 3 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/GainProcsOld.dm | 140 | 4049 | 4 | 1 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Gains.dm | 2370 | 96376 | 4 | 3 | 2 | 5 | 8 | Training, meditation, passive gains, fatigue, regen, rank catch-up | progression systems, balance sim, tests | P1 |
| Code/GameEvents/GainLoop/GainLoopEvent.dm | 22 | 552 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/GameEvents/GainLoop/Swimming.dm | 43 | 1659 | 1 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/GameEvents/Training.dm | 227 | 9473 | 3 | 1 | 0 | 0 | 0 | Training, meditation, passive gains, fatigue, regen, rank catch-up | progression systems, balance sim, tests | P1 |
| Code/GridStat.dm | 64 | 1293 | 1 | 0 | 2 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Guides.dm | 880 | 86506 | 1 | 0 | 4 | 0 | 3 | Support/misc | Review later | P3 |
| Code/Items.dm | 2032 | 65911 | 14 | 5 | 35 | 28 | 6 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/LogSystem/Events.dm | 166 | 5606 | 0 | 0 | 0 | 4 | 6 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/LogSystem/Main.dm | 24 | 668 | 0 | 0 | 0 | 0 | 0 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/Map.dm | 4356 | 84888 | 21 | 4 | 4 | 4 | 3 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/Mate.dm | 295 | 10230 | 1 | 0 | 4 | 5 | 0 | Support/misc | Review later | P3 |
| Code/Misc.dm | 247 | 8165 | 1 | 0 | 0 | 6 | 9 | Support/misc | Review later | P3 |
| Code/MobProcs.dm | 688 | 24785 | 0 | 11 | 4 | 7 | 1 | Support/misc | Review later | P3 |
| Code/Mobs.dm | 61 | 1714 | 0 | 0 | 0 | 0 | 1 | Support/misc | Review later | P3 |
| Code/NPC.dm | 147 | 2723 | 2 | 2 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Notes.dm | 99 | 2473 | 0 | 0 | 0 | 0 | 0 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/OfflinePlayers.dm | 44 | 1305 | 0 | 0 | 1 | 1 | 0 | Support/misc | Review later | P3 |
| Code/Packages.dm | 540 | 22921 | 1 | 0 | 6 | 26 | 2 | Support/misc | Review later | P3 |
| Code/PathFind.dm | 231 | 4650 | 0 | 1 | 1 | 0 | 0 | Support/misc | Review later | P3 |
| Code/PlaneMaster.dm | 43 | 881 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/PlanetConquer.dm | 70 | 2037 | 1 | 0 | 1 | 6 | 2 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/PlanetDestruction.dm | 402 | 12552 | 59 | 7 | 0 | 0 | 11 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/Planets/PlanetMatching.dm | 155 | 4415 | 0 | 0 | 0 | 0 | 0 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/PlayerMovement/Macros.dm | 206 | 6430 | 0 | 4 | 3 | 0 | 19 | Support/misc | Review later | P3 |
| Code/PlayerMovement/Move.dm | 271 | 7370 | 3 | 0 | 1 | 3 | 1 | Support/misc | Review later | P3 |
| Code/Profiles.dm | 39 | 1309 | 0 | 0 | 1 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Projectiles.dm | 708 | 20402 | 53 | 12 | 0 | 0 | 1 | Combat, KO, death, melee, projectile flags | vitals, combat, projectile systems | P1 |
| Code/QuickSort/QuickSort.dm | 54 | 1031 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/RPMode.dm | 25 | 618 | 4 | 0 | 0 | 0 | 0 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Reports.dm | 156 | 5691 | 0 | 0 | 1 | 1 | 6 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/ReturnProcs.dm | 192 | 4335 | 0 | 0 | 0 | 3 | 0 | Support/misc | Review later | P3 |
| Code/SaySpark.dm | 29 | 565 | 5 | 0 | 0 | 0 | 0 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Shockwave.dm | 151 | 4902 | 1 | 3 | 0 | 0 | 0 | Combat, KO, death, melee, projectile flags | vitals, combat, projectile systems | P1 |
| Code/SkillProcs.dm | 130 | 5691 | 0 | 28 | 0 | 0 | 1 | Skill catalog, activation verbs, cooldown/resource/effect clues | RprSkillPrototype, skill handlers, hotbar, validation | P1 |
| Code/SkillSystems.dm | 255 | 7457 | 5 | 0 | 5 | 0 | 0 | Skill catalog, activation verbs, cooldown/resource/effect clues | RprSkillPrototype, skill handlers, hotbar, validation | P1 |
| Code/SkillTreeSEXY.dm | 133 | 5595 | 1 | 1 | 2 | 0 | 0 | Skill catalog, activation verbs, cooldown/resource/effect clues | RprSkillPrototype, skill handlers, hotbar, validation | P1 |
| Code/Skills/AlienRacials/RacialSelection.dm | 187 | 5361 | 0 | 0 | 0 | 3 | 1 | Skill catalog, activation verbs, cooldown/resource/effect clues | RprSkillPrototype, skill handlers, hotbar, validation | P1 |
| Code/Skills.dm | 13563 | 436675 | 325 | 286 | 292 | 100 | 55 | Skill catalog, activation verbs, cooldown/resource/effect clues | RprSkillPrototype, skill handlers, hotbar, validation | P1 |
| Code/Special.dm | 453 | 17582 | 7 | 12 | 13 | 8 | 0 | Support/misc | Review later | P3 |
| Code/Stats.dm | 1109 | 40177 | 0 | 3 | 3 | 1 | 22 | Character creation, stats, implicit player schema | race/class/body/stat prototypes, character components, save DTO | P1 |
| Code/Systems/GlobalProcs.dm | 64 | 1974 | 0 | 3 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Systems/GlobalSoftcap.dm | 21 | 1024 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/Systems/OfflineCatchup.dm | 27 | 777 | 0 | 0 | 0 | 0 | 6 | Support/misc | Review later | P3 |
| Code/Systems/SpawnPoints.dm | 162 | 6200 | 0 | 0 | 0 | 0 | 0 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/Systems/TrainingVariety.dm | 71 | 2373 | 0 | 0 | 0 | 0 | 0 | Training, meditation, passive gains, fatigue, regen, rank catch-up | progression systems, balance sim, tests | P1 |
| Code/Systems/WaterAndOyxgen.dm | 63 | 1337 | 3 | 0 | 0 | 0 | 0 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/Systems/WorldSettings.dm | 14 | 393 | 0 | 0 | 0 | 0 | 0 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/Tech.dm | 4684 | 151043 | 36 | 19 | 118 | 161 | 51 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/TransNew.dm | 2889 | 87413 | 216 | 96 | 0 | 0 | 6 | Transformations, forms, drains, buffs, unlock requirements | transformation prototypes, modifier stacks | P1 |
| Code/UIBackend/Menu.dm | 15 | 357 | 0 | 0 | 0 | 0 | 0 | Support/misc | Review later | P3 |
| Code/UIBackend/SpawnEditUI.dm | 216 | 8311 | 10 | 0 | 8 | 9 | 0 | Support/misc | Review later | P3 |
| Code/Utils/ChatUtils/CommTechs.dm | 93 | 5449 | 0 | 0 | 4 | 0 | 15 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Utils/ChatUtils/Emote.dm | 131 | 4530 | 4 | 0 | 7 | 8 | 0 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Utils/ChatUtils/LOOC.dm | 22 | 961 | 0 | 0 | 0 | 0 | 1 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Utils/ChatUtils/Say.dm | 97 | 2699 | 1 | 0 | 0 | 0 | 1 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Utils/ChatUtils/SkillTreeSEXY.dm | 133 | 5595 | 1 | 1 | 2 | 0 | 0 | Skill catalog, activation verbs, cooldown/resource/effect clues | RprSkillPrototype, skill handlers, hotbar, validation | P1 |
| Code/Utils/ChatUtils/Yell.dm | 34 | 1178 | 1 | 0 | 0 | 0 | 0 | Chat, RP mode, emote, social routing | chat channels, logs, RP state | P1 |
| Code/Utils/WorldTimeUtils.dm | 48 | 1513 | 0 | 0 | 1 | 0 | 1 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/Variables.dm | 671 | 12359 | 1 | 0 | 0 | 0 | 0 | Character creation, stats, implicit player schema | race/class/body/stat prototypes, character components, save DTO | P1 |
| Code/VotingSystem.dm | 248 | 6769 | 5 | 2 | 0 | 4 | 36 | Admin, moderation, reports, notes, logs, votes | permissions, audit logs, report system | P2 |
| Code/Weather.dm | 235 | 6021 | 1 | 1 | 0 | 0 | 0 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/World.dm | 1160 | 56784 | 28 | 16 | 4 | 0 | 27 | Maps, planets, spawn points, environment, save loops | planet/map/environment prototypes, persistence | P2 |
| Code/WyattBuild.dm | 171 | 4280 | 4 | 0 | 2 | 6 | 0 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/WyattVisual.dm | 676 | 10488 | 5 | 4 | 0 | 0 | 2 | Items, equipment, crafting/tech/enchantment, buildables | item prototypes, crafting/enchant systems | P2 |
| Code/Years.dm | 278 | 7568 | 8 | 2 | 0 | 0 | 17 | Support/misc | Review later | P3 |
| Code/statpointsystems.dm | 1446 | 45230 | 0 | 1 | 4 | 0 | 0 | Character creation, stats, implicit player schema | race/class/body/stat prototypes, character components, save DTO | P1 |
