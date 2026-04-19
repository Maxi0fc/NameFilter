using System;
using System.Text;



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

    private static readonly (string Encoded, string Id, Severity Severity)[] EncodedWords =
    {
        ("bmlnZ2Vy", "S1", Severity.Slur),
        ("bmlnZ2E=", "S2", Severity.Slur),
        ("bmln", "S3", Severity.Slur),
        ("bmlnYQ==", "S4", Severity.Slur),
        ("bmdy", "S5", Severity.Slur),
        ("Y2hpbms=", "S6", Severity.Slur),
        ("Z29vaw==", "S7", Severity.Slur),
        ("c3BpYw==", "S8", Severity.Slur),
        ("d2V0YmFjaw==", "S9", Severity.Slur),
        ("a2lrZQ==", "S10", Severity.Slur),
        ("cmFnaGVhZA==", "S11", Severity.Slur),
        ("dG93ZWxoZWFk", "S12", Severity.Slur),
        ("YmVhbmVy", "S13", Severity.Slur),
        ("emlwcGVyaGVhZA==", "S14", Severity.Slur),
        ("Y29vbg==", "S15", Severity.Slur),
        ("bmlw", "S16", Severity.Slur),
        ("Z3JlYXNlYmFsbA==", "S17", Severity.Slur),
        ("aHltaWU=", "S18", Severity.Slur),
        ("ZGFya2ll", "S19", Severity.Slur),
        ("c2FtYm8=", "S20", Severity.Slur),
        ("cGlja2FuaW5ueQ==", "S21", Severity.Slur),
        ("cmVkc2tpbg==", "S22", Severity.Slur),
        ("c3F1YXc=", "S23", Severity.Slur),
        ("cmV0YXJk", "S24", Severity.Slur),
        ("ZmFnZ290", "S25", Severity.Slur),
        ("ZmFn", "S26", Severity.Slur),
        ("Zmd0", "S27", Severity.Slur),
        ("ZHlrZQ==", "S28", Severity.Slur),
        ("dHJhbm55", "S29", Severity.Slur),
        ("c2hlbWFsZQ==", "S30", Severity.Slur),
        ("bmF6aQ==", "S31", Severity.Slur),
        ("a2tr", "S32", Severity.Slur),
        ("bmVvbmF6aQ==", "S33", Severity.Slur),
        ("c2tpbmhlYWQ=", "S34", Severity.Slur),
        ("c3VwcmVtYWNpc3Q=", "S35", Severity.Slur),
        ("aGVpbA==", "S36", Severity.Slur),
        ("ODg=", "S37", Severity.Slur),
        ("MTQ4OA==", "S38", Severity.Slur),
        ("a3lz", "S39", Severity.SelfHarm),
        ("a21z", "S40", Severity.SelfHarm),
        ("cmFwZQ==", "N1", Severity.NSFW),
        ("cmFwaXN0", "N2", Severity.NSFW),
        ("cHVzc3k=", "N3", Severity.NSFW),
        ("Y29jaw==", "N4", Severity.NSFW),
        ("Y3VudA==", "N5", Severity.NSFW),
        ("cGVuaXM=", "N6", Severity.NSFW),
        ("dmFnaW5h", "N7", Severity.NSFW),
        ("ZGlsZG8=", "N8", Severity.NSFW),
        ("Ym9uZXI=", "N9", Severity.NSFW),
        ("ZXJlY3Rpb24=", "N10", Severity.NSFW),
        ("Ymxvd2pvYg==", "N11", Severity.NSFW),
        ("aGFuZGpvYg==", "N12", Severity.NSFW),
        ("Y3Vtc2hvdA==", "N13", Severity.NSFW),
        ("Y3Vtc2x1dA==", "N14", Severity.NSFW),
        ("Y3Vt", "N15", Severity.NSFW),
        ("d2hvcmU=", "N16", Severity.NSFW),
        ("c2x1dA==", "N17", Severity.NSFW),
        ("c2thbms=", "N18", Severity.NSFW),
        ("aGFybG90", "N19", Severity.NSFW),
        ("aG9va2Vy", "N20", Severity.NSFW),
        ("cG9ybnN0YXI=", "N21", Severity.NSFW),
        ("Z29vbmVy", "N22", Severity.NSFW),
        ("bWFzdHVyYmF0ZQ==", "N23", Severity.NSFW),
        ("bWFzdHVyYmF0aW9u", "N24", Severity.NSFW),
        ("b3JnYXNt", "N25", Severity.NSFW),
        ("amVya29mZg==", "N26", Severity.NSFW),
        ("ZmFw", "N27", Severity.NSFW),
        ("aGltbWxlcg==", "HF1", Severity.HatefulFigure),
        ("Z29lYmJlbHM=", "HF2", Severity.HatefulFigure),
        ("ZWljaG1hbm4=", "HF3", Severity.HatefulFigure),
        ("bWVuZ2VsZQ==", "HF4", Severity.HatefulFigure),
        ("Z29lcmluZw==", "HF5", Severity.HatefulFigure),
        ("b3NhbWE=", "HF6", Severity.HatefulFigure),
        ("YnJlaXZpaw==", "HF7", Severity.HatefulFigure),
        ("bWN2ZWlnaA==", "HF8", Severity.HatefulFigure),
        ("ZXBzdGVpbg==", "HF9", Severity.HatefulFigure),
        ("c3RhbGlu", "HF10", Severity.HatefulFigure),
        ("bXVzc29saW5p", "HF11", Severity.HatefulFigure),
        ("cG9scG90", "HF12", Severity.HatefulFigure),
        ("aWRpYW1pbg==", "HF13", Severity.HatefulFigure),
        ("YnVuZHk=", "HF14", Severity.HatefulFigure),
        ("ZGFobWVy", "HF15", Severity.HatefulFigure),
        ("Z2FjeQ==", "HF16", Severity.HatefulFigure),
        ("bWFuc29u", "HF17", Severity.HatefulFigure),
        ("Y29zYnk=", "HF18", Severity.HatefulFigure),
        ("bWFv", "HF19", Severity.HatefulFigure),
        ("YXNzaG9sZQ==", "I1", Severity.Insult),
        ("YmFzdGFyZA==", "I2", Severity.Insult),
        ("Yml0Y2g=", "I3", Severity.Insult),
        ("ZGlja2hlYWQ=", "I4", Severity.Insult),
        ("ZHVtYmFzcw==", "I5", Severity.Insult),
        ("ZGlwc2hpdA==", "I6", Severity.Insult),
        ("ZG91Y2hlYmFn", "I7", Severity.Insult),
        ("ZnVja2ZhY2U=", "I8", Severity.Insult),
        ("amFja2Fzcw==", "I9", Severity.Insult),
        ("bW90aGVyZnVja2Vy", "I10", Severity.Insult),
        ("cHJpY2s=", "I11", Severity.Insult),
        ("c2N1bWJhZw==", "I12", Severity.Insult),
        ("c2hpdGhlYWRz", "I13", Severity.Insult),
        ("c2hpdGhlYWQ=", "I14", Severity.Insult),
        ("d2Fua2Vy", "I15", Severity.Insult),
        ("dHdhdA==", "I16", Severity.Insult),
        ("bW9yb24=", "I17", Severity.Insult),
        ("aWRpb3Q=", "I18", Severity.Insult),
        ("Y3JhY2tlcg==", "I19", Severity.Insult),
        ("aG9tbw==", "I20", Severity.Insult),
        ("cXVlZXI=", "I21", Severity.Insult),
        ("amFw", "I22", Severity.Insult),
    };

    private static (string Word, string Id, Severity Severity)[]? _decoded;

    public static (string Word, string Id, Severity Severity)[] BannedWords
    {
        get
        {
            if (_decoded == null)
            {
                _decoded = new (string, string, Severity)[EncodedWords.Length];
                for (int i = 0; i < EncodedWords.Length; i++)
                {
                    var (enc, id, sev) = EncodedWords[i];
                    string word = Encoding.UTF8.GetString(Convert.FromBase64String(enc));
                    _decoded[i] = (word, id, sev);
                }
            }
            return _decoded;
        }
    }
}
