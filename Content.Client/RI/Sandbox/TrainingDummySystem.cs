using Content.Shared.RI.Sandbox;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Network;

namespace Content.Client.RI.Sandbox;

/// <summary>
/// Client-side presentation for the Phase 02 training dummy.
/// 
/// This system never increments Hits directly.
/// It only:
/// - reads replicated TrainingDummyComponent state;
/// - sends untrusted requests to the server;
/// - displays server feedback.
/// </summary>
public sealed class TrainingDummySystem : SharedTrainingDummySystem
{
    [Dependency] private readonly IUserInterfaceManager _uiManager = default!;

    private TrainingDummyPanel? _panel;
    private float _refreshAccumulator;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<TrainingDummyHitFeedbackEvent>(OnHitFeedback);

        _panel = new TrainingDummyPanel();
        _panel.HitRequested += RequestHit;

        _uiManager.StateRoot.AddChild(_panel);

        LayoutContainer.SetAnchorPreset(_panel, LayoutContainer.LayoutPreset.TopLeft);
        LayoutContainer.SetMarginLeft(_panel, 16);
        LayoutContainer.SetMarginTop(_panel, 16);

        RefreshPanel();
    }

    public override void Shutdown()
    {
        base.Shutdown();

        if (_panel != null)
        {
            _panel.HitRequested -= RequestHit;
            _panel.Orphan();
            _panel = null;
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        
        // Keep the learning UI simple. Polling a few times per second is fine
        // for this debug panel. Real UI should update from the component-state events.
        _refreshAccumulator += frameTime;

        if (_refreshAccumulator < 0.25f)
            return;

        _refreshAccumulator -= 0f;
        RefreshPanel();
    }

    private void RequestHit(EntityUid dummy)
    {
        if (!HasComp<TrainingDummyComponent>(dummy))
            return;

        var message = new TrainingDummyHitRequestEvent(GetNetEntity(dummy));
        RaiseNetworkEvent(message);

        _panel?.ShowToast("Hit request sent to server...");
    }

    private void OnHitFeedback(TrainingDummyHitFeedbackEvent message, EntitySessionEventArgs args)
    {
        _panel?.ShowToast(message.Message);
        RefreshPanel();
    }

    private void RefreshPanel()
    {
        if (_panel == null)
            return;

        var rows = new List<TrainingDummyRow>();

        var query = EntityQueryEnumerator<TrainingDummyComponent>();
        while (query.MoveNext(out var uid, out var dummy))
        {
            var name = string.IsNullOrWhiteSpace(dummy.DisplayName)
                ? MetaData(uid).EntityName
                : dummy.DisplayName;

            rows.Add(new TrainingDummyRow(uid, name, dummy.Hits));
        }

        rows.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

        _panel.SetRows(rows);
    }
}