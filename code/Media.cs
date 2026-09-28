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
    public VibeRating Vibe;
    public Director MediaDirector; 
    public List<Actor> Cast = new List<Actor>();
    public List<Review> ExternalReviews = new List<Review>();
    public List<Song> Soundtracks = new List<Song>();


    public List<string> Genres;
    public List<string> Tags;

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

    public void AddTag(string tag)
    {
        Tags.Add(tag);
    }

    public void AddGenre(string genre)
    {
        Genres.Add(genre);
    }

    public void AddSong(Song song)
    {
        Soundtracks.Add(song);
    }

    public bool IsAgeAppropriate(int viewerAge)
    {
        return viewerAge >= ageRestriction;
    }

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

    // сортування списків
    public int CompareTo(Media other)
    {
        if (other == null) return 1;
        return this.ReleaseYear.CompareTo(other.ReleaseYear);
    }

    public bool MatchesFilter(string tag)
    {
        return Tags.Contains(tag) || Genres.Contains(tag);
    }

    public string GetFormattedSummary()
    {
        return $"{Title} ({ReleaseYear}) - {Description}";
    }

    public abstract int CalculateTimeDebt();
}