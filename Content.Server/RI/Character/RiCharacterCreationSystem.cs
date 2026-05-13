using Content.Shared.RI.Character;
using Content.Shared.RI.Prototypes;
using Content.Shared.RI.Stats;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.RI.Character;

public sealed class RiCharacterCreationSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    // Replace with the Phase 08 injection/access pattern used by your local RiStatSystem.
    [Dependency] private readonly RiStatSystem _stats = default!;

    private readonly Dictionary<NetUserId, TimeSpan> _nextPreviewAllowed = new();
    private readonly HashSet<NetUserId> _createdThisRuntime = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<RiCharacterCreationPreviewRequest>(OnPreviewRequest);
        SubscribeNetworkEvent<RiCharacterCreationSubmitRequest>(OnSubmitRequest);
    }

    public void OpenForSession(ICommonSession session)
    {
        var options = BuildOptionsMessage();
        RaiseNetworkEvent(options, session.Channel);
    }

    private void OnPreviewRequest(
        RiCharacterCreationPreviewRequest message,
        EntitySessionEventArgs args)
    {
        var session = args.SenderSession;

        if (!CanPreview(session))
            return;

        var errors = ValidateSelection(session, message.Selection, finalSubmit: false);

        if (errors.Count > 0)
        {
            RaiseNetworkEvent(
                new RiCharacterCreationPreviewResponse(
                    message.Selection.ClientRevision,
                    false,
                    Array.Empty<RiStatPreviewLineDto>(),
                    errors.ToArray()),
                session.Channel);
            return;
        }

        var preview = BuildStatPreview(message.Selection);

        RaiseNetworkEvent(
            new RiCharacterCreationPreviewResponse(
                message.Selection.ClientRevision,
                true,
                preview,
                Array.Empty<RiCharacterCreationErrorDto>()),
            session.Channel);
    }

    private void OnSubmitRequest(
        RiCharacterCreationSubmitRequest message,
        EntitySessionEventArgs args)
    {
        var session = args.SenderSession;

        var errors = ValidateSelection(session, message.Selection, finalSubmit: true);

        if (errors.Count > 0)
        {
            RaiseNetworkEvent(
                new RiCharacterCreationSubmitResponse(
                    message.Selection.ClientRevision,
                    false,
                    errors.ToArray()),
                session.Channel);
            return;
        }

        var finalStats = BuildFinalStats(message.Selection);

        var handoff = new RiCharacterSpawnHandoffEvent(session, message.Selection, finalStats);
        RaiseLocalEvent(handoff);

        if (!handoff.Handled)
        {
            RaiseNetworkEvent(
                new RiCharacterCreationSubmitResponse(
                    message.Selection.ClientRevision,
                    false,
                    new[]
                    {
                        new RiCharacterCreationErrorDto(
                            RiCharacterCreationErrorCode.SpawnFailed,
                            "spawn",
                            handoff.FailureReason ?? "The server could not spawn your character.")
                    }),
                session.Channel);
            return;
        }

        _createdThisRuntime.Add(session.UserId);

        RaiseNetworkEvent(
            new RiCharacterCreationSubmitResponse(
                message.Selection.ClientRevision,
                true,
                Array.Empty<RiCharacterCreationErrorDto>()),
            session.Channel);

        RaiseNetworkEvent(new RiCharacterCreationCompletedMessage(), session.Channel);
    }

    private bool CanPreview(ICommonSession session)
    {
        var now = _timing.CurTime;

        if (_nextPreviewAllowed.TryGetValue(session.UserId, out var next) && now < next)
            return false;

        _nextPreviewAllowed[session.UserId] = now + TimeSpan.FromMilliseconds(150);
        return true;
    }

    private List<RiCharacterCreationErrorDto> ValidateSelection(
        ICommonSession session,
        RiCharacterCreationSelectionDto selection,
        bool finalSubmit)
    {
        var errors = new List<RiCharacterCreationErrorDto>();

        if (finalSubmit && _createdThisRuntime.Contains(session.UserId))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.AlreadyHasCharacter,
                "character",
                "This session already has a created character."));
            return errors;
        }

        if (!RiCharacterCreationValidation.TryValidateName(selection.Name, out _, out var nameError)
            && nameError != null)
        {
            errors.Add(nameError);
        }

        // Prototype validation shape.
        // Adjust method names to match the exact Phase 08 prototypes you implemented.

        if (!_prototype.TryIndex<RiRacePrototype>(selection.RaceId, out var race))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.RaceNotFound,
                "race",
                "Selected race does not exist."));
        }

        if (!_prototype.TryIndex<RiClassPrototype>(selection.ClassId, out var characterClass))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.ClassNotFound,
                "class",
                "Selected class does not exist."));
        }

        if (!_prototype.TryIndex<RiBodyTypePrototype>(selection.BodyTypeId, out var bodyType))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.BodyTypeNotFound,
                "bodyType",
                "Selected body type does not exist."));
        }

        if (errors.Count > 0)
            return errors;

        // Example policy checks. Replace property names with your Phase 08 prototype fields.
        if (!race!.AllowedClassIds.Contains(selection.ClassId))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.ClassNotAllowedForRace,
                "class",
                "That class is not available for the selected race."));
        }

        if (bodyType!.ExcludedRaceTags.Any(tag => race.Tags.Contains(tag)))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.BodyTypeNotAllowedForRace,
                "bodyType",
                "That body type is not available for the selected race."));
        }

        if (!IsSpawnPlanetAllowed(race, characterClass!, bodyType, selection.SpawnPlanetId))
        {
            errors.Add(new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.SpawnPlanetNotAllowed,
                "spawnPlanet",
                "That spawn planet is not valid for this character."));
        }

        return errors;
    }

    private RiStatPreviewLineDto[] BuildStatPreview(RiCharacterCreationSelectionDto selection)
    {
        var block = BuildFinalStats(selection);

        // Convert Phase 08 RiStatBlock into simple UI-safe DTOs.
        // Replace with your actual RiStatBlock API.
        return block.Lines
            .OrderBy(pair => pair.Key.ToString())
            .Select(pair => new RiStatPreviewLineDto(
                pair.Key.ToString(),
                pair.Key.ToString(),
                pair.Value.BaseValue,
                _stats.GetFinalValue(pair.Value)))
            .ToArray();
    }

    private RiStatBlock BuildFinalStats(RiCharacterCreationSelectionDto selection)
    {
        // This must call the Phase 08 deterministic stat calculation.
        // Do not duplicate stat math here or in the UI.
        return _stats.BuildCreationStats(
            selection.RaceId,
            selection.ClassId,
            selection.BodyTypeId);
    }

    private bool IsSpawnPlanetAllowed(
        RiRacePrototype race,
        RiClassPrototype characterClass,
        RiBodyTypePrototype bodyType,
        string spawnPlanetId)
    {
        // First slice can allow "RiTestPlanet" or the race default.
        // Later Phase 18 should move this into planet/spawn prototypes.
        return !string.IsNullOrWhiteSpace(spawnPlanetId);
    }

    private RiCharacterCreationOpenMessage BuildOptionsMessage()
    {
        var races = _prototype.EnumeratePrototypes<RiRacePrototype>()
            .Select(proto => new RiCharacterCreationOptionDto(proto.ID, proto.DisplayName))
            .OrderBy(option => option.DisplayName)
            .ToArray();

        var classes = _prototype.EnumeratePrototypes<RiClassPrototype>()
            .Select(proto => new RiCharacterCreationOptionDto(proto.ID, proto.DisplayName))
            .OrderBy(option => option.DisplayName)
            .ToArray();

        var bodyTypes = _prototype.EnumeratePrototypes<RiBodyTypePrototype>()
            .Select(proto => new RiCharacterCreationOptionDto(proto.ID, proto.DisplayName))
            .OrderBy(option => option.DisplayName)
            .ToArray();

        var spawnPlanets = new[]
        {
            new RiCharacterCreationOptionDto("RiTestPlanet", "Test Planet")
        };

        return new RiCharacterCreationOpenMessage(races, classes, bodyTypes, spawnPlanets);
    }
}