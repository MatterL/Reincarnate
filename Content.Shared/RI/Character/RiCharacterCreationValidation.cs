using System.Text.RegularExpressions;

namespace Content.Shared.RI.Character;

public static partial class RiCharacterCreationValidation
{
    public const int NameMinLength = 3;
    public const int NameMaxLength = 32;

    private static readonly Regex ValidNameRegex = new(
        "^[A-Za-z][A-Za-z0-9 '\\-]{2,31}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool TryValidateName(
        string rawName,
        out string normalizedName,
        out RiCharacterCreationErrorDto? error)
    {
        normalizedName = rawName.Trim();

        if (normalizedName.Length == 0)
        {
            error = new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.NameMissing,
                "name",
                "Enter a character name.");
            return false;
        }

        if (normalizedName.Length < NameMinLength)
        {
            error = new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.NameTooShort,
                "name",
                $"Name must be at least {NameMinLength} characters.");
            return false;
        }

        if (normalizedName.Length > NameMaxLength)
        {
            error = new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.NameTooLong,
                "name",
                $"Name must be no more than {NameMaxLength} characters.");
            return false;
        }

        if (!ValidNameRegex.IsMatch(normalizedName))
        {
            error = new RiCharacterCreationErrorDto(
                RiCharacterCreationErrorCode.NameInvalidCharacters,
                "name",
                "Name must start with a letter and may only contain letters, numbers, spaces, apostrophes, or hyphens.");
            return false;
        }

        error = null;
        return true;
    }
}