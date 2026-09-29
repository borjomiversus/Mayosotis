using System;
using System.Collections.Generic;

// Пошук і фільтрація


public class MediaSearchService
{
    public List<Media> SearchByTitle(List<Media> library, string title)
    {
        List<Media> results = new List<Media>();
        foreach (var m in library)
        {
            if (m.Title.ToLower().Contains(title.ToLower()))
            {
                results.Add(m);
            }
        }
        return results;
    }
    public List<Media> FilterByGenre(List<Media> library, string genre)
    {
        List<Media> results = new List<Media>();
        foreach (var m in library)
        {
            if (m.Genres.Contains(genre))
            {
                results.Add(m);
            }
        }
        return results;
    }

    public List<Media> AdvancedSearch(List<Media> library, string genre, int minYear, int maxYear)
    {
        List<Media> results = new List<Media>();
        foreach (var m in library)
        {
            if (m.Genres.Contains(genre) && m.ReleaseYear >= minYear && m.ReleaseYear <= maxYear)
            {
                results.Add(m);
            }
        }
        return results;
    }

    public List<Media> SearchByActor(List<Media> library, string actorName)
    {
        List<Media> results = new List<Media>();
        foreach (var m in library)
        {
            foreach (var actor in m.Cast)
            {
                if (actor.FullName == actorName)
                {
                    results.Add(m);
                    break;
                }
            }
        }
        return results;
    }

    public List<Media> GetTopRated(List<Media> library, int count)
    {
        List<Media> sorted = new List<Media>(library);

        sorted.Sort((a, b) => {
            double rateA = a.Vibe != null ? a.Vibe.CalculateAverage() : 0;
            double rateB = b.Vibe != null ? b.Vibe.CalculateAverage() : 0;
            return rateB.CompareTo(rateA);
        });

        List<Media> result = new List<Media>();
        for (int i = 0; i < sorted.Count && i < count; i++)
        {
            result.Add(sorted[i]);
        }
        return result;
    }
}

// рек схоже на те, що сподобалось
public class RecommendationService
    {
        public List<Media> FindSimilar(List<Media> library, Media reference, int count)
        {
            List<Media> similar = new List<Media>();

            // всі, крім того самого фільму
            foreach (var m in library)
            {
                if (m != reference) similar.Add(m);
            }

            similar.Sort((a, b) => {
                int sharedA = a.CountSharedGenres(reference);
                int sharedB = b.CountSharedGenres(reference);
                return sharedB.CompareTo(sharedA); 
            });

            List<Media> result = new List<Media>();
            for (int i = 0; i < Math.Min(count, similar.Count); i++)
            {
                result.Add(similar[i]);
            }
            return result;
        }
    }

// критерії для рулетки окремий об'єкт 
public class RouletteCriteria
{
    public int MaxMinutes { get; private set; }
    public string? Tag { get; private set; }
    public RouletteCriteria(int maxMinutes, string? tag = null)
    {
        if (maxMinutes < 0) throw new ArgumentOutOfRangeException(nameof(maxMinutes));
        MaxMinutes = maxMinutes;
        Tag = tag;
    }
}

public class RouletteService
{
    public Media? Choose(List<Media> library, RouletteCriteria criteria)
    {
        List<Media> filtered = new List<Media>();

        foreach (var m in library)
        {
            if (m.CalculateTimeDebt() <= criteria.MaxMinutes)
            {
                if (string.IsNullOrEmpty(criteria.Tag))
                {
                    filtered.Add(m);
                }
                else if (m.Tags.Contains(criteria.Tag) || m.Genres.Contains(criteria.Tag))
                {
                    filtered.Add(m);
                }
            }
        }

        if (filtered.Count == 0) return null;

        Random random = new Random();
        int index = random.Next(filtered.Count);
        return filtered[index];
    }
}

// розпізнати нотатку і перенести в підбірку
public class CaptureService
{
    // щоб зв'язати швидку нотатку з конкретним знайденим фільмом
    public void ResolveCapture(CaptureEntry capture, Media identifiedMedia)
    {
        if (capture == null || identifiedMedia == null)
        {
            return;
        }
        capture.Resolve(identifiedMedia);
    }

    // Перенос розпізнаної нотатки в обраний список (Watchlist)
    public bool MoveToWatchlist(CaptureEntry capture, Watchlist watchlist)
    {
        if (capture.Status != CaptureStatus.Resolved)
        {
            return false;
        }

        if (capture.ResolvedMedia == null)
        {
            return false;
        }

        watchlist.AddEntry(capture.ResolvedMedia, capture.RawNote);
        capture.Archive();
        return true;
    }
}