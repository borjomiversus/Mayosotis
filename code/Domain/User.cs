using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public string Username;
    public List<Watchlist> Watchlists;
    public List<CaptureEntry> Captures; 
    public List<string> RecentSearches;
    public List<Media> RecentlyViewed;
    public List<WatchHistoryEntry> WatchHistory;

    public User(string username)
    {
        Username = username;
        Watchlists = new List<Watchlist>();
        Captures = new List<CaptureEntry>();
        RecentSearches = new List<string>();
        RecentlyViewed = new List<Media>();
        WatchHistory = new List<WatchHistoryEntry>();
    }

    public Watchlist CreateWatchlist(string name, bool isPrivate = true)
    {
        var list = new Watchlist(name, isPrivate);
        Watchlists.Add(list);
        return list;
    }

    public void AddCapture(string rawNote, string source)
    {
        Captures.Add(new CaptureEntry(rawNote, source));
    }

    // Переносить уже розпізнану нотатку у конкретну підбірку
    public bool MoveResolvedCaptureToWatchlist(CaptureEntry capture, string watchlistName)
    {
        if (!capture.IsResolved) return false;
        var target = Watchlists.FirstOrDefault(w => w.Name == watchlistName);
        if (target == null) return false;
        target.AddEntry(capture.ResolvedMedia, capture.RawNote);
        Captures.Remove(capture);
        return true;
    }

    public void LogSearch(string query)
    {
        RecentSearches.Insert(0, query);
        if (RecentSearches.Count > 10) RecentSearches.RemoveAt(RecentSearches.Count - 1);
    }

    public void LogView(Media item)
    {
        RecentlyViewed.Remove(item);
        RecentlyViewed.Insert(0, item);
        if (RecentlyViewed.Count > 10) RecentlyViewed.RemoveAt(RecentlyViewed.Count - 1);
        WatchHistory.Add(new WatchHistoryEntry(item, DateTime.Now));
    }

    public List<CaptureEntry> GetUnresolvedCaptures()
    {
        return Captures.Where(c => !c.IsResolved).ToList();
    }

    public int GetMonthlyStats(int month)
    {
        return WatchHistory
            .Where(h => h.WatchDate.Month == month)
            .Sum(h => h.WatchedItem.CalculateTimeDebt());
    }

    // статистика за жанром за конкретний місяць
    public Dictionary<string, int> GetMonthlyGenreBreakdown(int month)
    {
        return WatchHistory
            .Where(h => h.WatchDate.Month == month)
            .SelectMany(h => h.WatchedItem.Genres.Select(g => new { Genre = g, Item = h.WatchedItem }))
            .GroupBy(x => x.Genre)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}