using System;

// тайтл + мій особистий коментар до нього в межах конкретної підбірки

public class WatchlistEntry
{
    public Media Item;
    public string PersonalNote;
    public DateTime AddedDate;

    public WatchlistEntry(Media item, string personalNote)
    {
        Item = item;
        PersonalNote = personalNote;
        AddedDate = DateTime.Now;
    }

    public void UpdateNote(string newNote)
    {
        PersonalNote = newNote;
    }

    public string GetSummary()
    {
        string title = Item != null ? Item.Title : "?";
        return $"{title} — {PersonalNote} (додано {AddedDate:d})";
    }
}