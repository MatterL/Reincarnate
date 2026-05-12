# Extracted Admin, Moderation, Reports, Notes, and Logs

Source focus: `Code/Admin.dm`, `Code/AdminOverview.dm`, `Code/Reports.dm`, `Code/Notes.dm`, `Code/LogSystem/*`, `Code/VotingSystem.dm`, `Code/Packages.dm`.

## Source concepts

| Concept | Source clues | Rewrite target |
|---|---|---|
| Coded admin list | Hardcoded names/levels in `Admin.dm` | External role config + permissions; never hardcode live admin identities. |
| Admin levels | Admin1/Admin2/Admin3/Admin4 verb groups | `RprAdminPermission` flags and role prototypes. |
| Rewarder role | Separate rewarder verbs/messages | `CanReward`, `CanEditProgression` permissions. |
| Admin vote | `obj/AdminVotes` with yes/no/abstain and timed ending | Optional server-side admin vote record/system. |
| Automated RP reward | RP frequency/character count -> base/stat rewards | Moderated reward system; balance/legal review. |
| Punishments | punishment lists/checks | Ban/mute/jail/note records. |
| Reports | report browsing and logs | AHelp/report ticket system. |
| Notes | player notes | Persistent moderation note records. |
| Logs | Admin, temp, skill, chat logs | Structured audit log service. |

## Backend assumptions to discard

- Hardcoded admin keys in source.
- Admin `input` prompts as authority boundary.
- Client UI being trusted to decide who can execute an action.
- One-off text log formats as the only audit trail.

## Target Robust shape

- `RprAdminRolePrototype` in `Resources/Prototypes/RPR/Admin/roles.yml`.
- `RprAdminPermission` enum/flags.
- `RprAdminSystem` permission checks.
- `RprAdminCommandSystem` server-only command handlers.
- `RprAuditLogSystem` structured audit records.
- `RprReportSystem` reports/ahelps.
- `RprModerationRepository` for bans/mutes/notes.

## First safe admin command

Implement `ViewCharacterStats` before teleport/summon/edit commands. It is read-only, useful, and validates the permission model without enabling destructive tools.

## Test cases

- Non-admin cannot execute admin commands even if client UI sends the event.
- Admin action creates audit log record.
- Reward/edit action requires stronger permission than view action.
- Dangerous action requires explicit permission and reason.
