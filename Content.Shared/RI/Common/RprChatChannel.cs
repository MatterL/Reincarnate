using Robust.Shared.Serialization;

namespace Content.Shared.RI.Common;

/// <summary>
/// Chat routing categories. Server decides who receives each message.
/// </summary>
[Serializable, NetSerializable]
public enum RprChatChannel : byte
{
    Ooc = 0,
    Looc = 1,
    Say = 2,
    Yell = 3,
    Whisper = 4,
    Emote = 5,

    Admin = 50,
    AHelp = 51,
    System = 52,
}