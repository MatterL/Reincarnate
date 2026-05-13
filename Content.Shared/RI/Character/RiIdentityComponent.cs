using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Character;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RiIdentityComponent : Component
{
    [DataField, AutoNetworkedField]
    public string CharacterName = string.Empty;

    [DataField, AutoNetworkedField]
    public string RaceId = string.Empty;

    [DataField, AutoNetworkedField]
    public string ClassId = string.Empty;

    [DataField, AutoNetworkedField]
    public string BodyTypeId = string.Empty;

    [DataField, AutoNetworkedField]
    public string SpawnPlanetId = string.Empty;
}