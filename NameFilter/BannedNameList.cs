namespace NameFilter
{
    /// <summary>
    /// Hardcoded list of banned words.
    /// Add or remove words here and recompile the mod to update the filter.
    /// WARNING:  This file contains offensive slurs for filtering purposes.
    /// </summary>
    public static class BannedNameList
    {
        public static readonly string[] BannedWords =
        {
            // ── Racial & Ethnic Slurs ────────────────────────────────────────
            "nigger",
            "nigga",
            "chink",
            "gook",
            "spic",
            "wetback",
            "kike",
            "raghead",
            "towelhead",
            "cracker",
            "beaner",
            "zipperhead",
            "coon",
            "jap",
            "nip",
            "greaseball",
            "hymie",
            "darkie",
            "sambo",
            "pickaninny",
            "redskin",
            "squaw",

            // ── Homophobic & Transphobic Slurs ───────────────────────────────
            "faggot",
            "fag",
            "dyke",
            "tranny",
            "shemale",
            "queer",     // context-dependent but often used as a slur in games
            "homo",

            // ── Sexual & Explicit Words ──────────────────────────────────────
            "pussy",
            "cock",
            "cunt",
            "penis",
            "vagina",
            "dildo",
            "boner",
            "erection",
            "blowjob",
            "handjob",
            "cumshot",
            "cumslut",
            "whore",
            "slut",
            "skank",
            "harlot",
            "hooker",
            "pornstar",

            // ── General Insults ──────────────────────────────────────────────
            "asshole",
            "bastard",
            "bitch",
            "dickhead",
            "dumbass",
            "dipshit",
            "douchebag",
            "fuckface",
            "jackass",
            "motherfucker",
            "prick",
            "scumbag",
            "shitheads",
            "shithead",
            "wanker",
            "twat",
            "moron",
            "retard",
            "idiot",

            // ── Hate & Extremism ─────────────────────────────────────────────
            "nazi",
            "hitler",
            "kkk",
            "neonazi",
            "skinhead",
            "supremacist",
        };
    }
}
