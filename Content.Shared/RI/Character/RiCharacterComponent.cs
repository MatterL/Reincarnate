using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.RI.Character;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RiCharacterComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Created;

    [DataField, AutoNetworkedField]
    public int CreationRevision;
}