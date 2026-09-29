using System;
using System.Collections.Generic;

public class User
{
    public string Username { get; private set; }
    public List<Watchlist> Watchlists { get; private set; }
    public List<CaptureEntry> Captures { get; private set; }
    public List<string> RecentSearches { get; private set; }
    public List<Media> RecentlyViewed { get; private set; }
    public List<WatchHistoryEntry> WatchHistory { get; private set; }
    public List<UserMediaState> MediaStates { get; private set; }

    public User(string username)
    {
        Username = username;
        Watchlists = new List<Watchlist>();
        Captures = new List<CaptureEntry>();
        RecentSearches = new List<string>();
        RecentlyViewed = new List<Media>();
        WatchHistory = new List<WatchHistoryEntry>();
        MediaStates = new List<UserMediaState>();
    }

    public Watchlist CreateWatchlist(string name, bool isPrivate = true)
    {
        var list = new Watchlist(name, isPrivate);
        Watchlists.Add(list);
        return list;
    }

    public void AddCapture(string rawNote, string? source)
    {
        Captures.Add(new CaptureEntry(rawNote, source));
    }

    public bool MoveResolvedCaptureToWatchlist(CaptureEntry capture, string watchlistName)
    {
        if (capture.Status != CaptureStatus.Resolved || capture.ResolvedMedia == null)
            return false;

        Watchlist? target = null;
        foreach (var w in Watchlists)
        {
            if (w.Name == watchlistName)
            {
                target = w;
                break;
            }
        }
        if (target == null) return false;

        target.AddEntry(capture.ResolvedMedia, capture.RawNote);
        capture.Archive();

        return true;
    }

    public void LogSearch(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;

        RecentSearches.Insert(0, query);

        if (RecentSearches.Count > 10)
            RecentSearches.RemoveAt(RecentSearches.Count - 1);
    }

    public void RecordView(Media item)
    {
        RecentlyViewed.Remove(item);
        RecentlyViewed.Insert(0, item);

        if (RecentlyViewed.Count > 10)
            RecentlyViewed.RemoveAt(RecentlyViewed.Count - 1);
    }

    public void MarkAsWatched(Media item)
    {
        WatchHistory.Add(new WatchHistoryEntry(item, DateTime.Now));
    }

    public List<CaptureEntry> GetUnresolvedCaptures()
    {
        List<CaptureEntry> unresolved = new List<CaptureEntry>();
        foreach (var c in Captures)
        {
            if (c.Status == CaptureStatus.Unresolved)
                unresolved.Add(c);
        }
        return unresolved;
    }

    public int GetMonthlyStats(int year, int month)
    {
        int totalDebt = 0;
        foreach (var h in WatchHistory)
        {
            if (h.WatchDate.Year == year && h.WatchDate.Month == month)
            {
                totalDebt += h.WatchedItem.CalculateTimeDebt();
            }
        }
        return totalDebt;
    }

    public Dictionary<string, int> GetMonthlyGenreBreakdown(int year, int month)
    {
        var breakdown = new Dictionary<string, int>();
        foreach (var historyEntry in WatchHistory)
        {
            if (historyEntry.WatchDate.Year == year && historyEntry.WatchDate.Month == month)
            {
                foreach (var genre in historyEntry.WatchedItem.Genres)
                {
                    if (breakdown.ContainsKey(genre))
                    {
                        breakdown[genre]++; 
                    }
                    else
                    {
                        breakdown[genre] = 1;
                    }
                }
            }
        }

        return breakdown;
    }
}