using System;
using System.Collections.Generic;

public abstract class Media : IComparable<Media>
{
    public string Title;
    public int ReleaseYear;
    public string ProductionStudio;
    public string PosterUrl;
    public string TrailerUrl;
    public string Description;
    public string Rating;
    public string Country;
    public int ageRestriction;
    public string type;
    public VibeRating Vibe; // Кожне медіа має свою вайб-оцінку
    public Director MediaDirector; // Кожне медіа має режисера
    public List<Actor> Cast = new List<Actor>(); // Список акторів
    public List<string> ExternalReviews = new List<string>();


    // Динамічні масиви для зберігання кількох значень
    public List<string> Genres;
    public List<string> Tags;

    // Конструктор: те, що спрацьовує при створенні об'єкта
    public Media(string title, int releaseYear)
    {
        Title = title;
        ReleaseYear = releaseYear;
        Genres = new List<string>();
        Tags = new List<string>();
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }

    public void SetProductionDetails(string studio, string country)
    {
        ProductionStudio = studio;
        Country = country;
    }

    public void ApplyAgeRestriction(int age, string rating)
    {
        ageRestriction = age;
        Rating = rating;
    }

    // Методи для тегів і жанрів
    public void AddTag(string tag)
    {
        Tags.Add(tag);
    }

    public void AddGenre(string genre)
    {
        Genres.Add(genre);
    }
    // Перевірка батьківського контролю
    public bool IsAgeAppropriate(int viewerAge)
    {
        return viewerAge >= ageRestriction;
    }

    // Аналог "More like this" - рахує кількість спільних жанрів з іншим медіа
    public int CountSharedGenres(Media other)
    {
        int sharedCount = 0;
        foreach (var genre in Genres)
        {
            if (other.Genres.Contains(genre))
            {
                sharedCount++;
            }
        }
        return sharedCount;
    }

    // Третій поліморфізм (параметричний) - сортування списків
    public int CompareTo(Media other)
    {
        if (other == null) return 1;
        return this.ReleaseYear.CompareTo(other.ReleaseYear);
    }

    // Додаткові методи для кількості
    public bool MatchesFilter(string tag)
    {
        return Tags.Contains(tag) || Genres.Contains(tag);
    }

    public string GetFormattedSummary()
    {
        return $"{Title} ({ReleaseYear}) - {Description}";
    }

    // Абстрактний метод для поліморфізму
    public abstract int CalculateTimeDebt();
}