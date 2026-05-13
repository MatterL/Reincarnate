using Content.Shared.RI.Character;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Client.RI.CharacterCreation;

public sealed class RiCharacterCreationController : EntitySystem
{
    private RiCharacterCreationWindow? _window;
    private RiCharacterCreationOpenMessage? _lastOptions;
    private int _revision;
    private bool _latestPreviewValid;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<RiCharacterCreationOpenMessage>(OnOpen);
        SubscribeNetworkEvent<RiCharacterCreationPreviewResponse>(OnPreviewResponse);
        SubscribeNetworkEvent<RiCharacterCreationSubmitResponse>(OnSubmitResponse);
        SubscribeNetworkEvent<RiCharacterCreationCompletedMessage>(OnCompleted);
    }

    private void OnOpen(RiCharacterCreationOpenMessage message)
    {
        _lastOptions = message;
        _latestPreviewValid = false;

        _window ??= new RiCharacterCreationWindow();
        _window.PreviewRequested += OnPreviewRequested;
        _window.SubmitRequested += OnSubmitRequested;
        _window.OnClose += () => _window = null;

        _window.SetOptions(message.Races, message.Classes, message.BodyTypes, message.SpawnPlanets);
        _window.SetState(RiCharacterCreationUiState.Editing);
        _window.OpenCentered();
    }

    private void OnPreviewRequested(RiCharacterCreationSelectionDto selection)
    {
        _revision++;
        var revised = WithRevision(selection, _revision);

        _latestPreviewValid = false;
        _window?.SetState(RiCharacterCreationUiState.PreviewPending);

        RaiseNetworkEvent(new RiCharacterCreationPreviewRequest(revised));
    }

    private void OnSubmitRequested(RiCharacterCreationSelectionDto selection)
    {
        if (!_latestPreviewValid)
            return;

        _revision++;
        var revised = WithRevision(selection, _revision);

        _window?.SetState(RiCharacterCreationUiState.SubmitPending);
        RaiseNetworkEvent(new RiCharacterCreationSubmitRequest(revised));
    }

    private void OnPreviewResponse(RiCharacterCreationPreviewResponse message)
    {
        if (message.ClientRevision != _revision)
            return;

        _latestPreviewValid = message.Valid;

        _window?.SetPreview(message.Stats);
        _window?.SetErrors(message.Errors);
        _window?.SetState(message.Valid
            ? RiCharacterCreationUiState.ReadyToSubmit
            : RiCharacterCreationUiState.Editing);
    }

    private void OnSubmitResponse(RiCharacterCreationSubmitResponse message)
    {
        if (message.ClientRevision != _revision)
            return;

        if (message.Accepted)
        {
            _window?.SetState(RiCharacterCreationUiState.Complete);
            return;
        }

        _latestPreviewValid = false;
        _window?.SetErrors(message.Errors);
        _window?.SetState(RiCharacterCreationUiState.Editing);
    }

    private void OnCompleted(RiCharacterCreationCompletedMessage message)
    {
        _window?.Close();
        _window = null;
    }

    private static RiCharacterCreationSelectionDto WithRevision(
        RiCharacterCreationSelectionDto selection,
        int revision)
    {
        return new RiCharacterCreationSelectionDto(
            revision,
            selection.Name,
            selection.RaceId,
            selection.ClassId,
            selection.BodyTypeId,
            selection.SpawnPlanetId);
    }
}