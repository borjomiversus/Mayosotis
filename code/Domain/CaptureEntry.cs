using System;

// сюди падає все, що зачепило користувача, ще до того, як стало структурованим об'єктом Media
// стани: розпізнано одразу / назва+нотатка / не розпізнано
public class CaptureEntry
{
    public string RawNote;       
    public string Source;        
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

    // Викликається, коли тайтл вдалось знайти 
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
