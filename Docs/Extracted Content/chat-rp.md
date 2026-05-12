# Extracted Chat and RP Social Layer

Source focus: `Code/Chat&Verbs.dm`, `Code/Utils/ChatUtils/*`, `Code/RPMode.dm`, `Code/SaySpark.dm`, `Code/Reports.dm`.

## Channels and flows found

| Channel/flow | Source behavior | Rewrite target |
|---|---|---|
| OOC | Global out-of-character chat with toggle and admin controls. | `RprChatChannel.Ooc`, server broadcast, user preference. |
| LOOC | Local OOC, routed to nearby viewers, shares OOC visibility. | `RprChatChannel.Looc`, range-limited. |
| Say | Local in-character speech; parentheses can redirect to LOOC. | `RprChatChannel.Say`, range-limited, sanitized. |
| Yell | Larger-range speech. | `RprChatChannel.Yell`, range-limited. |
| Whisper | Small-range/private-ish speech. | optional `RprChatChannel.Whisper`. |
| Think | Self/internal channel. | client-local or server logged depending design. |
| Emote | IC emote with quote colorization and MRP special handling. | `RprChatChannel.Emote`, server-routed. |
| Skill Emote | Emote tied to a skill name and logging. | skill system event -> chat/audit. |
| Telepathy/communication tech | Device/skill routed remote speech. | later channel router with permissions/range/device IDs. |
| RP Mode | toggles RP protection/hostility interactions. | `RprRpModeComponent`, separate from chat routing. |

## Design facts to preserve

- Chat must sanitize/HTML-encode user input.
- Chat should have logs for moderation.
- OOC can be globally disabled/admin controlled.
- LOOC/Say/Yell/Emote must be server-routed by range, not client-filtered.
- RP mode affects combat/social boundaries but should not be a random flag checked everywhere.

## Target Robust shape

- `RprChatChannel` enum.
- `RprChatMessage` shared DTO.
- `RprChatSystem` server-side validation and routing.
- `RprChatPreferencesComponent`: show/hide OOC, text color if retained.
- `RprRpModeComponent`: RP mode/privacy/hostility state.
- `RprChatLogSystem`: persistent moderation logs.

## Test cases

- OOC disabled rejects normal users but allows authorized admin override if desired.
- LOOC reaches nearby players only.
- Say/Yell ranges differ.
- Emotes are logged and sanitized.
- Spam check/rate limit blocks flood attempts.
