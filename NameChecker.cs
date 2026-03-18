using System.Text.RegularExpressions;

namespace NameFilter
{
    /// <summary>
    /// Checks player names against the hardcoded banned word list.
    /// Uses fuzzy matching to catch variants like "1d10t".
    /// Short words (under 5 letters) use exact matching.
    /// Long words (5+ letters) use prefix matching to catch inflections.
    /// </summary>
    public static class NameChecker
    {
        private const int PrefixMatchMinLength = 5;

        /// <summary>
        /// Normalizes a name by replacing common character substitutions.
        /// e.g. "1" -> "i", "3" -> "e", "@" -> "a" etc.
        /// </summary>
        private static string Normalize(string input)
        {
            input = input.ToLower();
            input = input.Replace("1", "i")
                         .Replace("!", "i")
                         .Replace("3", "e")
                         .Replace("4", "a")
                         .Replace("@", "a")
                         .Replace("0", "o")
                         .Replace("5", "s")
                         .Replace("$", "s")
                         .Replace("7", "t")
                         .Replace("+", "t")
                         .Replace("8", "b")
                         .Replace("6", "g")
                         .Replace("9", "g");

            // Remove all remaining non-letter characters
            input = Regex.Replace(input, @"[^a-z]", "");

            return input;
        }

        /// <summary>
        /// Checks if the normalized name matches a banned word.
        /// Short words: exact match only.
        /// Long words: prefix match to catch inflections like "idiots", "idiotic".
        /// Also checks word boundaries to avoid false positives like "assassin".
        /// </summary>
        private static bool MatchesBannedWord(string normalizedName, string normalizedBanned)
        {
            if (normalizedBanned.Length < PrefixMatchMinLength)
            {
                // Short word: must be an exact standalone match
                // Use word boundary via regex to avoid matching inside longer words
                return Regex.IsMatch(normalizedName, $@"\b{Regex.Escape(normalizedBanned)}\b");
            }
            else
            {
                // Long word: prefix match — catches inflections
                // e.g. banned = "idiot" matches "idiots", "idiotic"
                int index = normalizedName.IndexOf(normalizedBanned);
                if (index < 0) return false;

                // Make sure it's not in the middle of a completely unrelated word
                // by checking that whatever comes before is not a letter
                bool validStart = index == 0 || !char.IsLetter(normalizedName[index - 1]);
                return validStart;
            }
        }

        /// <summary>
        /// Returns true if the player name contains a banned word.
        /// </summary>
        public static bool IsBanned(string playerName, out string matchedWord)
        {
            string normalized = Normalize(playerName);

            foreach (string banned in BannedNameList.BannedWords)
            {
                string normalizedBanned = Normalize(banned);

                if (MatchesBannedWord(normalized, normalizedBanned))
                {
                    matchedWord = banned;
                    return true;
                }
            }

            matchedWord = null;
            return false;
        }
    }
}
