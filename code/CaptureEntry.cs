using System;

// "Вхідні двері" застосунку — сюди падає все, що зачепило користувача,
// ще до того, як стало структурованим об'єктом Media.
// Три можливі стани: розпізнано одразу / назва+нотатка / не розпізнано.
public class CaptureEntry
{
    public string RawNote;       // напр. "той фільм про хлопця який застряг в часовій петлі"
    public string Source;        // напр. "TikTok", "порадила подруга", "Instagram"
    public DateTime CapturedAt;
    public bool IsResolved;
    public Media ResolvedMedia;  // заповнюється, коли вдалось ідентифікувати тайтл

    public CaptureEntry(string rawNote, string source)
    {
        RawNote = rawNote;
        Source = source;
        CapturedAt = DateTime.Now;
        IsResolved = false;
    }

    // Викликається, коли тайтл вдалось знайти (вручну, або пізніше — через API/ШІ)
    public void Resolve(Media identifiedMedia)
    {
        ResolvedMedia = identifiedMedia;
        IsResolved = true;
    }

    public string GetStatusSummary()
    {
        return IsResolved
            ? $"✓ Розпізнано: {ResolvedMedia.Title} (з нотатки: \"{RawNote}\")"
            : $"? Ще не розпізнано: \"{RawNote}\" (джерело: {Source})";
    }
}
