using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared.RI.Character;

[Serializable, NetSerializable]
public sealed class RiCharacterCreationSelectionDto
{
    public int ClientRevision { get; }
    public string Name { get; }
    public string RaceId { get; }
    public string ClassId { get; }
    public string BodyTypeId { get; }
    public string SpawnPlanetId { get; }

    public RiCharacterCreationSelectionDto(
        int clientRevision,
        string name,
        string raceId,
        string classId,
        string bodyTypeId,
        string spawnPlanetId)
    {
        ClientRevision = clientRevision;
        Name = name;
        RaceId = raceId;
        ClassId = classId;
        BodyTypeId = bodyTypeId;
        SpawnPlanetId = spawnPlanetId;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationOptionDto
{
    public string Id { get; }
    public string DisplayName { get; }
    public string Description { get; }

    public RiCharacterCreationOptionDto(string id, string displayName, string description = "")
    {
        Id = id;
        DisplayName = displayName;
        Description = description;
    }
}

[Serializable, NetSerializable]
public sealed class RiStatPreviewLineDto
{
    public string StatId { get; }
    public string DisplayName { get; }
    public float BaseValue { get; }
    public float FinalValue { get; }

    public RiStatPreviewLineDto(string statId, string displayName, float baseValue, float finalValue)
    {
        StatId = statId;
        DisplayName = displayName;
        BaseValue = baseValue;
        FinalValue = finalValue;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationErrorDto
{
    public RiCharacterCreationErrorCode Code { get; }
    public string Field { get; }
    public string Message { get; }

    public RiCharacterCreationErrorDto(RiCharacterCreationErrorCode code, string field, string message)
    {
        Code = code;
        Field = field;
        Message = message;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationOpenMessage : EntityEventArgs
{
    public RiCharacterCreationOptionDto[] Races { get; }
    public RiCharacterCreationOptionDto[] Classes { get; }
    public RiCharacterCreationOptionDto[] BodyTypes { get; }
    public RiCharacterCreationOptionDto[] SpawnPlanets { get; }

    public RiCharacterCreationOpenMessage(
        RiCharacterCreationOptionDto[] races,
        RiCharacterCreationOptionDto[] classes,
        RiCharacterCreationOptionDto[] bodyTypes,
        RiCharacterCreationOptionDto[] spawnPlanets)
    {
        Races = races;
        Classes = classes;
        BodyTypes = bodyTypes;
        SpawnPlanets = spawnPlanets;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationPreviewRequest : EntityEventArgs
{
    public RiCharacterCreationSelectionDto Selection { get; }

    public RiCharacterCreationPreviewRequest(RiCharacterCreationSelectionDto selection)
    {
        Selection = selection;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationPreviewResponse : EntityEventArgs
{
    public int ClientRevision { get; }
    public bool Valid { get; }
    public RiStatPreviewLineDto[] Stats { get; }
    public RiCharacterCreationErrorDto[] Errors { get; }

    public RiCharacterCreationPreviewResponse(
        int clientRevision,
        bool valid,
        RiStatPreviewLineDto[] stats,
        RiCharacterCreationErrorDto[] errors)
    {
        ClientRevision = clientRevision;
        Valid = valid;
        Stats = stats;
        Errors = errors;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationSubmitRequest : EntityEventArgs
{
    public RiCharacterCreationSelectionDto Selection { get; }

    public RiCharacterCreationSubmitRequest(RiCharacterCreationSelectionDto selection)
    {
        Selection = selection;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationSubmitResponse : EntityEventArgs
{
    public int ClientRevision { get; }
    public bool Accepted { get; }
    public RiCharacterCreationErrorDto[] Errors { get; }

    public RiCharacterCreationSubmitResponse(
        int clientRevision,
        bool accepted,
        RiCharacterCreationErrorDto[] errors)
    {
        ClientRevision = clientRevision;
        Accepted = accepted;
        Errors = errors;
    }
}

[Serializable, NetSerializable]
public sealed class RiCharacterCreationCompletedMessage : EntityEventArgs
{
}