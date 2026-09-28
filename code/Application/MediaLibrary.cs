using System;
using System.Collections.Generic;
using System.Linq;

public class MediaLibrary
{
    public List<Media> AllMedia;

    public MediaLibrary()
    {
        AllMedia = new List<Media>();
    }

    public void AddMedia(Media item)
    {
        AllMedia.Add(item);
    }

    // частковий збіг, ігноруючи регістр)
    public List<Media> SearchByTitle(string query)
    {
        return AllMedia.Where(m => m.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Фільтр за жанром
    public List<Media> FilterByGenre(string genre)
    {
        return AllMedia.Where(m => m.Genres.Contains(genre)).ToList();
    }

    // жанр + діапазон років
    public List<Media> AdvancedSearch(string genre, int minYear, int maxYear)
    {
        return AllMedia.Where(m => m.Genres.Contains(genre)
                                && m.ReleaseYear >= minYear
                                && m.ReleaseYear <= maxYear).ToList();
    }

    // Пошук за актором
    public List<Media> SearchByActor(string actorName)
    {
        return AllMedia.Where(m => m.Cast.Any(a => a.FullName == actorName)).ToList();
    }

    // Топ за вайб-оцінкою
    public List<Media> GetTopRated(int count)
    {
        // Відсортувати за спаданням середньої оцінки
        return AllMedia.OrderByDescending(m => m.Vibe != null ? m.Vibe.CalculateAverage() : 0)
                       .Take(count).ToList();
    }

    public List<Media> FindSimilar(Media reference, int count)
    {
        return AllMedia.Where(m => m != reference)
                        .OrderByDescending(m => m.CountSharedGenres(reference))
                        .ThenByDescending(m => m.Vibe != null ? m.Vibe.CalculateAverage() : 0)
                        .Take(count)
                        .ToList();
    }
}