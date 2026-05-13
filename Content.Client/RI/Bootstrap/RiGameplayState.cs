using Robust.Client.State;

namespace Content.Client.RI.Bootstrap;

/// <summary>
/// Minimal Phase 07 gameplay state.
/// No linked UI screen: this lets the world render without the debug connection screen.
/// </summary>
public sealed class RiGameplayState : State
{
    protected override void Startup()
    {
    }

    protected override void Shutdown()
    {
    }
}