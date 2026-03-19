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
        ("beaner",       "S13", Severity.Slur),
        ("zipperhead",   "S14", Severity.Slur),
        ("coon",         "S15", Severity.Slur),
        ("nip",          "S16", Severity.Slur),
        ("greaseball",   "S17", Severity.Slur),
        ("hymie",        "S18", Severity.Slur),
        ("darkie",       "S19", Severity.Slur),
        ("sambo",        "S20", Severity.Slur),
        ("pickaninny",   "S21", Severity.Slur),
        ("redskin",      "S22", Severity.Slur),
        ("squaw",        "S23", Severity.Slur),
        ("retard",       "S24", Severity.Slur),

        // — Homophobic & Transphobic Slurs —
        ("faggot",       "S25", Severity.Slur),
        ("fag",          "S26", Severity.Slur),
        ("fgt",          "S27", Severity.Slur),
        ("dyke",         "S28", Severity.Slur),
        ("tranny",       "S29", Severity.Slur),
        ("shemale",      "S30", Severity.Slur),

        // — Hate & Extremism —
        ("nazi",         "S31", Severity.Slur),
        ("kkk",          "S32", Severity.Slur),
        ("neonazi",      "S33", Severity.Slur),
        ("skinhead",     "S34", Severity.Slur),
        ("supremacist",  "S35", Severity.Slur),
        ("heil",         "S36", Severity.Slur),
        ("88",           "S37", Severity.Slur),
        ("1488",         "S38", Severity.Slur),

        // — Self-Harm —
        ("kys",          "S39", Severity.SelfHarm),
        ("kms",          "S40", Severity.SelfHarm),

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
        ("cum",          "N15", Severity.NSFW),
        ("whore",        "N16", Severity.NSFW),
        ("slut",         "N17", Severity.NSFW),
        ("skank",        "N18", Severity.NSFW),
        ("harlot",       "N19", Severity.NSFW),
        ("hooker",       "N20", Severity.NSFW),
        ("pornstar",     "N21", Severity.NSFW),
        ("gooner",       "N22", Severity.NSFW),
        ("masturbate",   "N23", Severity.NSFW),
        ("masturbation", "N24", Severity.NSFW),
        ("orgasm",       "N25", Severity.NSFW),
        ("jerkoff",      "N26", Severity.NSFW),
        ("fap",          "N27", Severity.NSFW),

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
        ("cracker",      "I19", Severity.Insult),
        ("homo",         "I20", Severity.Insult),
        ("queer",        "I21", Severity.Insult),
        ("jap",          "I22", Severity.Insult),
    };
}
