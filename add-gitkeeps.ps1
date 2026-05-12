$dirs = @(
  "Content.Shared/RI/Common",
  "Content.Shared/RI/Admin",
  "Content.Shared/RI/Character",
  "Content.Shared/RI/Chat",
  "Content.Shared/RI/Combat",
  "Content.Shared/RI/Components",
  "Content.Shared/RI/Items",
  "Content.Shared/RI/Movement",
  "Content.Shared/RI/Persistence",
  "Content.Shared/RI/Progression",
  "Content.Shared/RI/Projectiles",
  "Content.Shared/RI/Prototypes",
  "Content.Shared/RI/Skills",
  "Content.Shared/RI/Stats",
  "Content.Shared/RI/Transformations",
  "Content.Shared/RI/Vitals",
  "Content.Shared/RI/World",

  "Content.Server/RI/Admin",
  "Content.Server/RI/Character",
  "Content.Server/RI/Chat",
  "Content.Server/RI/Combat",
  "Content.Server/RI/Logging",
  "Content.Server/RI/Moderation",
  "Content.Server/RI/Persistence",
  "Content.Server/RI/Progression",
  "Content.Server/RI/Skills",
  "Content.Server/RI/Vitals",
  "Content.Server/RI/World",

  "Content.Client/RI/CharacterCreation",
  "Content.Client/RI/Chat",
  "Content.Client/RI/Combat",
  "Content.Client/RI/Stats",
  "Content.Client/RI/UI",
  "Content.Client/RI/Visuals",

  "Resources/Prototypes/RI/Admin",
  "Resources/Prototypes/RI/Character",
  "Resources/Prototypes/RI/Combat",
  "Resources/Prototypes/RI/Entities",
  "Resources/Prototypes/RI/Items",
  "Resources/Prototypes/RI/Sandbox",
  "Resources/Prototypes/RI/Skills",
  "Resources/Prototypes/RI/Transformations",
  "Resources/Prototypes/RI/World",

  "Docs/ADR",
  "Docs/Architecture",
  "Docs/Design",
  "Docs/ExtractedContent",
  "Docs/Networking",
  "Docs/Testing",
  "Docs/Project",
  "Docs/Security",
  "Docs/Legal",

  "Tools/RIContentExtractor",
  "Tools/RIBalanceSim",
  "Tools/RISaveMigrator"
)

foreach ($dir in $dirs) {
  New-Item -ItemType Directory -Force $dir | Out-Null
  New-Item -ItemType File -Force "$dir/.gitkeep" | Out-Null
}