using System;
using System.Collections.Generic;
using System.Linq;

// Пошук і фільтрація
public class MediaSearchService
{
    public List<Media> SearchByTitle(List<Media> library, string title)
    {
        return library.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Media> FilterByGenre(List<Media> library, string genre)
    {
        return library.Where(m => m.Genres.Contains(genre, StringComparer.OrdinalIgnoreCase)).ToList();
    }

    public List<Media> AdvancedSearch(List<Media> library, string genre, int minYear, int maxYear)
    {
        return library.Where(m => m.Genres.Contains(genre, StringComparer.OrdinalIgnoreCase)
                                && m.ReleaseYear >= minYear
                                && m.ReleaseYear <= maxYear).ToList();
    }

    public List<Media> SearchByActor(List<Media> library, string actorName)
    {
        return library.Where(m => m.Cast.Any(a => a.FullName == actorName)).ToList();
    }

    public List<Media> GetTopRated(List<Media> library, int count)
    {
        return library.OrderByDescending(m => m.Vibe?.CalculateAverage() ?? 0).Take(count).ToList();
    }
}

// рек схоже на те, що сподобалось
public class RecommendationService
{
    public List<Media> FindSimilar(List<Media> library, Media reference, int count)
    {
        return library.Where(m => m != reference)
                       .OrderByDescending(m => m.CountSharedGenres(reference))
                       .ThenByDescending(m => m.Vibe?.CalculateAverage() ?? 0)
                       .Take(count)
                       .ToList();
    }
}

// критерії для рулетки окремий об'єкт 
public class RouletteCriteria
{
    public int MaxMinutes { get; init; }
    public string? Tag { get; init; }
}

public class RouletteService
{
    public Media? Choose(List<Media> library, RouletteCriteria criteria)
    {
        var filtered = library.Where(m => m.CalculateTimeDebt() <= criteria.MaxMinutes);

        if (!string.IsNullOrEmpty(criteria.Tag))
        {
            filtered = filtered.Where(m => m.Tags.Contains(criteria.Tag) || m.Genres.Contains(criteria.Tag));
        }

        var list = filtered.ToList();
        if (list.Count == 0) return null;

        var random = new Random();
        return list[random.Next(list.Count)];
    }
}

// розпізнати нотатку і перенести в підбірку
public class CaptureService
{
    public void ResolveCapture(CaptureEntry capture, Media identifiedMedia)
    {
        capture.Resolve(identifiedMedia);
    }

    public bool MoveToWatchlist(CaptureEntry capture, Watchlist watchlist)
    {
        if (capture.Status != CaptureStatus.Resolved || capture.ResolvedMedia == null) return false;
        watchlist.AddEntry(capture.ResolvedMedia, capture.RawNote);
        capture.Archive();
        return true;
    }
}