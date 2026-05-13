namespace Content.Shared.RI.Stats;

public readonly record struct RiStatChoiceValidationResult(bool Valid, string? Error)
{
    public static RiStatChoiceValidationResult Ok => new(true, null);

    public static RiStatChoiceValidationResult Fail(string error)
    {
        return new RiStatChoiceValidationResult(false, error);
    }
}