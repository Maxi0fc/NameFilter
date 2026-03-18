/// <summary>
/// Checks player names against the banned words list.
/// </summary>
public static class NameChecker
{
    public static bool IsBanned(string name, out string id, out BannedNameList.Severity severity)
    {
        string lowerName = name.ToLower().Trim();

        foreach (var (word, wordId, wordSeverity) in BannedNameList.BannedWords)
        {
            if (lowerName.Contains(word.ToLower()))
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
