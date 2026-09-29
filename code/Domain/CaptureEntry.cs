using System;

public enum CaptureStatus
{
    Unresolved,
    Resolved,
    Archived
}

// Сюди падає все, що зачепило користувача, ще до того, як стало структурованим Media.
public class CaptureEntry
{
    public string RawNote { get; private set; }
    public string? Source { get; private set; }
    public DateTime CapturedAt { get; private set; }
    public CaptureStatus Status { get; private set; }
    public Media? ResolvedMedia { get; private set; }

    public CaptureEntry(string rawNote, string? source)
    {
        if (string.IsNullOrWhiteSpace(rawNote))
            throw new ArgumentException("Нотатка не може бути порожньою.", nameof(rawNote));

        RawNote = rawNote;
        Source = source;
        CapturedAt = DateTime.Now;
        Status = CaptureStatus.Unresolved;
    }

    public void Resolve(Media identifiedMedia)
    {
        ResolvedMedia = identifiedMedia ?? throw new ArgumentNullException(nameof(identifiedMedia));
        Status = CaptureStatus.Resolved;
    }

    public void Archive() => Status = CaptureStatus.Archived;

    public string GetStatusSummary()
    {
        return Status switch
        {
            CaptureStatus.Resolved => $"✓ Розпізнано: {ResolvedMedia!.Title} (з нотатки: \"{RawNote}\")",
            CaptureStatus.Archived => $"[архів] {RawNote}",
            _ => $"? Ще не розпізнано: \"{RawNote}\" (джерело: {Source})"
        };
    }
}