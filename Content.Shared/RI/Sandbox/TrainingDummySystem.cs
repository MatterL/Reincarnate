namespace Content.Shared.RI.Sandbox;

/// <summary>
/// Shared helped logic for the training dummy spike.
///
/// The server calls TryAddHit because the server owns final truth.
/// The client can use FormatHits for presentation.
/// </summary>
public abstract class SharedTrainingDummySystem : EntitySystem
{
    public const string PopupPrefix = "Training hits";

    public bool TryAddHit(EntityUid uid, TrainingDummyComponent component, out int hits)
    {
        if (component.Hits >= component.MaxHits)
        {
            hits = component.Hits;
            return false;
        }

        component.Hits += 1;
        Dirty(uid, component);

        hits = component.Hits;
        return true;
    }

    public static string FormatHits(int hits)
    {
        return $"{PopupPrefix}: {hits}";
    }
}