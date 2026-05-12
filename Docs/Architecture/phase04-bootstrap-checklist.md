# Phase 04 Bootstrap Checklist

## Required

- [ ] `Content.Shared/RI/Common` exists.
- [ ] Core enums compile.
- [ ] `Docs/ADR/0002-content-shared-server-client-boundaries.md` exists.
- [ ] `Docs/ADR/0003-data-driven-prototypes.md` exists.
- [ ] `Docs/Architecture/naming-and-folder-conventions.md` exists.
- [ ] `Docs/Architecture/networking-boundaries.md` exists.
- [ ] `dotnet restore` succeeds.
- [ ] `dotnet build RobustTemplate.sln` succeeds.
- [ ] GitHub Actions or local CI script runs the same build.

## Should not happen in this phase

- [ ] No real combat implementation.
- [ ] No real character creation implementation.
- [ ] No persistence implementation.
- [ ] No bulk DM content conversion.
- [ ] No direct DM code copying.