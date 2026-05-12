# Extraction Automation Notes

This package was produced from the uploaded zip with lightweight text inspection. Suggested repo tool:

```text
Tools/RIContentExtractor/
  scan_dm_files.py
  extract_skill_index.py
  extract_stat_branches.py
  extract_transform_requirements.py
  emit_markdown.py
```

## Useful machine checks

- Count risky BYOND patterns: spawn, sleep, verb, input, winset, browse, savefile, world/view/range.
- Build path indexes for `obj/Skills`, `obj/Items`, `obj/Items/Tech`, `obj/Items/Enchantment`.
- Extract race/class strings and transform requirements.
- Emit CSV/Markdown, then manually review for source context.

## Warning

Text extraction can lose DM indentation/bracing context. Treat machine rows as a checklist for manual verification, especially transformation rows and class-specific stat branches.
