using System.Text.RegularExpressions;

/// <summary>
/// Checks player names against the banned words list.
/// Supports fuzzy matching for leetspeak and character substitutions.
/// </summary>
public static class NameChecker
{
    // Normalize leetspeak and common substitutions
    private static string Normalize(string input)
    {
        string s = input.ToLower().Trim();

        // Remove spaces and special characters between letters
        s = Regex.Replace(s, @"[\s\-_\.\*]", "");

        // Leetspeak substitutions
        s = s.Replace("@", "a")
             .Replace("4", "a")
             .Replace("3", "e")
             .Replace("1", "i")
             .Replace("!", "i")
             .Replace("0", "o")
             .Replace("5", "s")
             .Replace("$", "s")
             .Replace("7", "t")
             .Replace("+", "t")
             .Replace("9", "g")
             .Replace("6", "g")
             .Replace("8", "b")
             .Replace("(", "c")
             .Replace("<", "c")
             .Replace("|", "l")
             .Replace("2", "z");

        // Remove repeated characters (e.g. "niiigger" -> "niger")
        s = Regex.Replace(s, @"(.)\1+", "$1");

        return s;
    }

    public static bool IsBanned(string name, out string id, out BannedNameList.Severity severity)
    {
        string normalized = Normalize(name);

        foreach (var (word, wordId, wordSeverity) in BannedNameList.BannedWords)
        {
            string normalizedWord = Normalize(word);
            if (normalized.Contains(normalizedWord))
            {
                id = wordId;
                severity = wordSeverity;
                return true;
            }
        }

        id = string.Empty;
        severity = BannedNameList.Severity.Insult;
        return false;
    }

    public static string GetChatLabel(string id, BannedNameList.Severity severity)
    {
        return severity switch
        {
            BannedNameList.Severity.Slur          => $"[Slur {id}]",
            BannedNameList.Severity.NSFW           => $"[NSFW {id}]",
            BannedNameList.Severity.Insult         => $"[Insult {id}]",
            BannedNameList.Severity.SelfHarm       => $"[Slur {id}]",
            BannedNameList.Severity.HatefulFigure  => $"[Hateful Figure {id}]",
            _                                      => $"[Banned {id}]"
        };
    }
}
