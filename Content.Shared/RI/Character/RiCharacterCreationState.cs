using Robust.Shared.Serialization;

namespace Content.Shared.RI.Character;

[Serializable, NetSerializable]
public enum RiCharacterCreationUiState : byte
{
    Closed,
    LoadingOptions,
    Editing,
    PreviewPending,
    ReadyToSubmit,
    SubmitPending,
    Complete,
    Rejected
}

[Serializable, NetSerializable]
public enum RiCharacterCreationErrorCode : byte
{
    None,

    NameMissing,
    NameTooShort,
    NameTooLong,
    NameInvalidCharacters,
    NameReserved,

    RaceMissing,
    RaceNotFound,

    ClassMissing,
    ClassNotFound,
    ClassNotAllowedForRace,

    BodyTypeMissing,
    BodyTypeNotFound,
    BodyTypeNotAllowedForRace,

    SpawnPlanetMissing,
    SpawnPlanetNotFound,
    SpawnPlanetNotAllowed,

    UnlockMissing,
    StatAllocationInvalid,

    AlreadyHasCharacter,
    AlreadySubmitting,
    PrototypeMismatch,
    ServerNotReady,
    SpawnFailed,
    Unknown
}