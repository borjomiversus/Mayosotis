using System;
using System.Collections.Generic;
using System.Linq;

public class Watchlist
{
    public string Name { get; private set; }
    public bool IsPrivate { get; private set; }
    public List<WatchlistEntry> Entries { get; private set; }

    public Watchlist(string name, bool isPrivate = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва підбірки не може бути порожньою.", nameof(name));

        Name = name;
        IsPrivate = isPrivate;
        Entries = new List<WatchlistEntry>();
    }

    public void AddEntry(Media item, string? personalNote)
    {
        Entries.Add(new WatchlistEntry(item, personalNote));
    }

    public bool RemoveEntry(Media item)
    {
        var entry = Entries.FirstOrDefault(e => e.Item == item);
        if (entry == null) return false;
        Entries.Remove(entry);
        return true;
    }

    public int GetTotalTimeDebt()
    {
        return Entries.Sum(e => e.Item.CalculateTimeDebt());
    }

    public void PrintContents()
    {
        Console.WriteLine($"--- Підбірка \"{Name}\" ({(IsPrivate ? "приватна" : "публічна")}) ---");
        if (Entries.Count == 0)
        {
            Console.WriteLine("(порожньо)");
            return;
        }
        foreach (var entry in Entries)
            Console.WriteLine(entry.GetSummary());
    }
}