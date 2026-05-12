using Robust.Shared.GameStates;

namespace Content.Shared.RI.Sandbox;

/// <summary>
/// Networked learning component for the Phase 02 sandbox.
///
/// The server owns Hits. Clients receive replicated state and display it.
/// Do not let client code directly mutate Hits.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TrainingDummyComponent : Component
{
    /// <summary>
    /// Total confirmed server-side training hits.
    /// This is the networked field used by the learning spike.
    /// </summary>
    [DataField, AutoNetworkedField] public int Hits = 0;

    /// <summary>
    /// Safety cap so a test session cannot overflow the counter forever.
    /// </summary>
    [DataField] public int MaxHits = 999_999;

    /// <summary>
    /// Display label used by the tiny client UI.
    /// </summary>
    [DataField] public string DisplayName = "Training Dummy";
}