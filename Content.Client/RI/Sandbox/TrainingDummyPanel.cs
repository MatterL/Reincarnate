using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;
using System.Numerics;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.RI.Sandbox;

public readonly record struct TrainingDummyRow(EntityUid Entity, string Name, int Hits);

/// <summary>
/// Tiny Phase 02 sandbox UI.
/// 
/// This is deliberately simple:
/// - lists replicated training dummies;
/// - shows each dummy's replicated hit count;
/// - sends a hit request when the user presses a button;
/// - shows small text feedback after the server responds.
/// </summary>
public sealed class TrainingDummyPanel : PanelContainer
{
    private readonly Label _status;
    private readonly BoxContainer _rows;

    public event Action<EntityUid>? HitRequested;

    public TrainingDummyPanel()
    {
        MinSize = new Vector2(360, 180);
        MouseFilter = MouseFilterMode.Stop;

        PanelOverride = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#202025ee")
        };

        var root = new BoxContainer
        {
            Orientation = LayoutOrientation.Vertical,
            SeparationOverride = 6,
            Margin = new Thickness(10)
        };

        AddChild(root);

        root.AddChild(new Label
        {
            Text = "RI Sandbox Training Dummy",
            StyleClasses = { "LabelHeading" }
        });

        root.AddChild(new Label
        {
            Text = "Press Hit to send an untrusted client request. The server owns the counter.",
            ClipText = false
        });

        _status = new Label
        {
            Text = "Waiting for replicated dummies...",
            ClipText = false
        };

        root.AddChild(_status);

        _rows = new BoxContainer
        {
            Orientation = LayoutOrientation.Vertical,
            SeparationOverride = 4,
            VerticalExpand = true
        };

        root.AddChild(_rows);
    }

    public void SetRows(IReadOnlyList<TrainingDummyRow> rows)
    {
        _rows.RemoveAllChildren();

        if (rows.Count == 0)
        {
            _rows.AddChild(new Label
            {
                Text = "No RiSandboxTrainingDummy entities are currently replicated to this client."
            });

            return;
        }

        foreach (var row in rows)
        {
            var line = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                SeparationOverride = 6
            };

            line.AddChild(new Label
            {
                Text = $"{row.Name} — Hits: {row.Hits}",
                HorizontalExpand = true,
                ClipText = true
            });

            var hitButton = new Button
            {
                Text = "Hit"
            };

            hitButton.OnPressed += _ => HitRequested?.Invoke(row.Entity);

            line.AddChild(hitButton);
            _rows.AddChild(line);
        }
    }

    public void ShowToast(string message)
    {
        _status.Text = message;
    }
}