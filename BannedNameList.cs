/// <summary>
/// Contains the list of banned words/names for the NameFilter plugin.
/// Add or remove words here and recompile the mod to update the filter.
/// WARNING: This file contains offensive slurs for filtering purposes.
/// </summary>
public static class BannedNameList
{
    public enum Severity
    {
        Slur,
        NSFW,
        Insult,
        SelfHarm,
        HatefulFigure
    }

    public static readonly (string Word, string Id, Severity Severity)[] BannedWords =
    {
        // — Racial & Ethnic Slurs —
        ("nigger",       "S1",  Severity.Slur),
        ("nigga",        "S2",  Severity.Slur),
        ("nig",          "S3",  Severity.Slur),
        ("niga",         "S4",  Severity.Slur),
        ("ngr",          "S5",  Severity.Slur),
        ("chink",        "S6",  Severity.Slur),
        ("gook",         "S7",  Severity.Slur),
        ("spic",         "S8",  Severity.Slur),
        ("wetback",      "S9",  Severity.Slur),
        ("kike",         "S10", Severity.Slur),
        ("raghead",      "S11", Severity.Slur),
        ("towelhead",    "S12", Severity.Slur),
        ("cracker",      "S13", Severity.Slur),
        ("beaner",       "S14", Severity.Slur),
        ("zipperhead",   "S15", Severity.Slur),
        ("coon",         "S16", Severity.Slur),
        ("jap",          "S17", Severity.Slur),
        ("nip",          "S18", Severity.Slur),
        ("greaseball",   "S19", Severity.Slur),
        ("hymie",        "S20", Severity.Slur),
        ("darkie",       "S21", Severity.Slur),
        ("sambo",        "S22", Severity.Slur),
        ("pickaninny",   "S23", Severity.Slur),
        ("redskin",      "S24", Severity.Slur),
        ("squaw",        "S25", Severity.Slur),
        ("retard",       "S26", Severity.Slur),

        // — Homophobic & Transphobic Slurs —
        ("faggot",       "S27", Severity.Slur),
        ("fag",          "S28", Severity.Slur),
        ("fgt",          "S29", Severity.Slur),
        ("dyke",         "S30", Severity.Slur),
        ("tranny",       "S31", Severity.Slur),
        ("shemale",      "S32", Severity.Slur),
        ("queer",        "S33", Severity.Slur),
        ("homo",         "S34", Severity.Slur),

        // — Hate & Extremism —
        ("nazi",         "S35", Severity.Slur),
        ("hitler",       "S36", Severity.Slur),
        ("kkk",          "S37", Severity.Slur),
        ("neonazi",      "S38", Severity.Slur),
        ("skinhead",     "S39", Severity.Slur),
        ("supremacist",  "S40", Severity.Slur),
        ("heil",         "S41", Severity.Slur),
        ("88",           "S42", Severity.Slur),
        ("1488",         "S43", Severity.Slur),

        // — Self-Harm —
        ("kys",          "S44", Severity.SelfHarm),
        ("kms",          "S45", Severity.SelfHarm),

        // — NSFW —
        ("rape",         "N1",  Severity.NSFW),
        ("rapist",       "N2",  Severity.NSFW),
        ("pussy",        "N3",  Severity.NSFW),
        ("cock",         "N4",  Severity.NSFW),
        ("cunt",         "N5",  Severity.NSFW),
        ("penis",        "N6",  Severity.NSFW),
        ("vagina",       "N7",  Severity.NSFW),
        ("dildo",        "N8",  Severity.NSFW),
        ("boner",        "N9",  Severity.NSFW),
        ("erection",     "N10", Severity.NSFW),
        ("blowjob",      "N11", Severity.NSFW),
        ("handjob",      "N12", Severity.NSFW),
        ("cumshot",      "N13", Severity.NSFW),
        ("cumslut",      "N14", Severity.NSFW),
        ("whore",        "N15", Severity.NSFW),
        ("slut",         "N16", Severity.NSFW),
        ("skank",        "N17", Severity.NSFW),
        ("harlot",       "N18", Severity.NSFW),
        ("hooker",       "N19", Severity.NSFW),
        ("pornstar",     "N20", Severity.NSFW),

        // — Hateful Figures —
        ("himmler",      "HF1",  Severity.HatefulFigure),
        ("goebbels",     "HF2",  Severity.HatefulFigure),
        ("eichmann",     "HF3",  Severity.HatefulFigure),
        ("mengele",      "HF4",  Severity.HatefulFigure),
        ("goering",      "HF5",  Severity.HatefulFigure),
        ("osama",        "HF6",  Severity.HatefulFigure),
        ("breivik",      "HF7",  Severity.HatefulFigure),
        ("mcveigh",      "HF8",  Severity.HatefulFigure),
        ("epstein",      "HF9",  Severity.HatefulFigure),
        ("stalin",       "HF10", Severity.HatefulFigure),
        ("mussolini",    "HF11", Severity.HatefulFigure),
        ("polpot",       "HF12", Severity.HatefulFigure),
        ("idiamin",      "HF13", Severity.HatefulFigure),
        ("bundy",        "HF14", Severity.HatefulFigure),
        ("dahmer",       "HF15", Severity.HatefulFigure),
        ("gacy",         "HF16", Severity.HatefulFigure),
        ("manson",       "HF17", Severity.HatefulFigure),
        ("cosby",        "HF18", Severity.HatefulFigure),
        ("mao",          "HF19", Severity.HatefulFigure),

        // — General Insults —
        ("asshole",      "I1",  Severity.Insult),
        ("bastard",      "I2",  Severity.Insult),
        ("bitch",        "I3",  Severity.Insult),
        ("dickhead",     "I4",  Severity.Insult),
        ("dumbass",      "I5",  Severity.Insult),
        ("dipshit",      "I6",  Severity.Insult),
        ("douchebag",    "I7",  Severity.Insult),
        ("fuckface",     "I8",  Severity.Insult),
        ("jackass",      "I9",  Severity.Insult),
        ("motherfucker", "I10", Severity.Insult),
        ("prick",        "I11", Severity.Insult),
        ("scumbag",      "I12", Severity.Insult),
        ("shitheads",    "I13", Severity.Insult),
        ("shithead",     "I14", Severity.Insult),
        ("wanker",       "I15", Severity.Insult),
        ("twat",         "I16", Severity.Insult),
        ("moron",        "I17", Severity.Insult),
        ("idiot",        "I18", Severity.Insult),
    };
}
