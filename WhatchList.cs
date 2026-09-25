using System;
using System.Collections.Generic;
using System.Linq;

// Підбірка, яку створює сам користувач: "Плани", "Похмурий вайб", "Подивитись з подругою" тощо.
// IsPrivate визначає, чи видно її іншим користувачам (якщо колись з'явиться профіль/шеринг).
public class Watchlist
{
    public string Name;
    public bool IsPrivate;
    public List<WatchlistEntry> Entries;

    public Watchlist(string name, bool isPrivate = true)
    {
        Name = name;
        IsPrivate = isPrivate;
        Entries = new List<WatchlistEntry>();
    }

    public void AddEntry(Media item, string personalNote)
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
        return Entries.Where(e => e.Item != null).Sum(e => e.Item.CalculateTimeDebt());
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
        {
            Console.WriteLine(entry.GetSummary());
        }
    }
}
