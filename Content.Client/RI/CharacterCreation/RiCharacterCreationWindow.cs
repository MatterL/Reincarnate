using Content.Shared.RI.Character;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.RI.CharacterCreation;

public sealed class RiCharacterCreationWindow : DefaultWindow
{
    public event Action<RiCharacterCreationSelectionDto>? PreviewRequested;
    public event Action<RiCharacterCreationSelectionDto>? SubmitRequested;

    private readonly LineEdit _name = new();
    private readonly OptionButton _race = new();
    private readonly OptionButton _class = new();
    private readonly OptionButton _bodyType = new();
    private readonly OptionButton _spawnPlanet = new();
    private readonly Button _create = new() { Text = "Create" };
    private readonly Label _errors = new();
    private readonly BoxContainer _statList = new() { Orientation = BoxContainer.LayoutOrientation.Vertical };

    private RiCharacterCreationOptionDto[] _races = Array.Empty<RiCharacterCreationOptionDto>();
    private RiCharacterCreationOptionDto[] _classes = Array.Empty<RiCharacterCreationOptionDto>();
    private RiCharacterCreationOptionDto[] _bodyTypes = Array.Empty<RiCharacterCreationOptionDto>();
    private RiCharacterCreationOptionDto[] _spawnPlanets = Array.Empty<RiCharacterCreationOptionDto>();

    public RiCharacterCreationWindow()
    {
        Title = "Create Character";

        var root = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            Margin = new Thickness(8)
        };

        root.AddChild(new Label { Text = "Name" });
        root.AddChild(_name);

        root.AddChild(new Label { Text = "Race" });
        root.AddChild(_race);

        root.AddChild(new Label { Text = "Class" });
        root.AddChild(_class);

        root.AddChild(new Label { Text = "Body Type" });
        root.AddChild(_bodyType);

        root.AddChild(new Label { Text = "Spawn Planet" });
        root.AddChild(_spawnPlanet);

        root.AddChild(new Label { Text = "Stat Preview" });
        root.AddChild(_statList);

        root.AddChild(_errors);
        root.AddChild(_create);

        Contents.AddChild(root);

        _name.OnTextChanged += _ => RequestPreview();
        _race.OnItemSelected += _ => RequestPreview();
        _class.OnItemSelected += _ => RequestPreview();
        _bodyType.OnItemSelected += _ => RequestPreview();
        _spawnPlanet.OnItemSelected += _ => RequestPreview();

        _create.OnPressed += _ => SubmitRequested?.Invoke(BuildSelection(0));
        _create.Disabled = true;
    }

    public void SetOptions(
        RiCharacterCreationOptionDto[] races,
        RiCharacterCreationOptionDto[] classes,
        RiCharacterCreationOptionDto[] bodyTypes,
        RiCharacterCreationOptionDto[] spawnPlanets)
    {
        _races = races;
        _classes = classes;
        _bodyTypes = bodyTypes;
        _spawnPlanets = spawnPlanets;

        Populate(_race, races);
        Populate(_class, classes);
        Populate(_bodyType, bodyTypes);
        Populate(_spawnPlanet, spawnPlanets);

        RequestPreview();
    }

    public void SetState(RiCharacterCreationUiState state)
    {
        var submitting = state == RiCharacterCreationUiState.SubmitPending;
        var ready = state == RiCharacterCreationUiState.ReadyToSubmit;

        _name.Editable = !submitting;
        _race.Disabled = submitting;
        _class.Disabled = submitting;
        _bodyType.Disabled = submitting;
        _spawnPlanet.Disabled = submitting;

        _create.Disabled = !ready || submitting;
        _create.Text = submitting ? "Creating..." : "Create";
    }

    public void SetPreview(RiStatPreviewLineDto[] stats)
    {
        _statList.RemoveAllChildren();

        foreach (var stat in stats)
        {
            _statList.AddChild(new Label
            {
                Text = $"{stat.DisplayName}: {stat.FinalValue:0.##}"
            });
        }
    }

    public void SetErrors(RiCharacterCreationErrorDto[] errors)
    {
        _errors.Text = errors.Length == 0
            ? string.Empty
            : string.Join("\n", errors.Select(error => error.Message));
    }

    private void RequestPreview()
    {
        PreviewRequested?.Invoke(BuildSelection(0));
    }

    private RiCharacterCreationSelectionDto BuildSelection(int revision)
    {
        return new RiCharacterCreationSelectionDto(
            revision,
            _name.Text,
            GetSelectedId(_race, _races),
            GetSelectedId(_class, _classes),
            GetSelectedId(_bodyType, _bodyTypes),
            GetSelectedId(_spawnPlanet, _spawnPlanets));
    }

    private static void Populate(OptionButton button, RiCharacterCreationOptionDto[] options)
    {
        button.Clear();

        for (var i = 0; i < options.Length; i++)
        {
            button.AddItem(options[i].DisplayName, i);
        }

        if (options.Length > 0)
            button.SelectId(0);
    }

    private static string GetSelectedId(OptionButton button, RiCharacterCreationOptionDto[] options)
    {
        var selected = button.SelectedId;

        if (selected < 0 || selected >= options.Length)
            return string.Empty;

        return options[selected].Id;
    }
}