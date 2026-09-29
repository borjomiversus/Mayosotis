// Одна пісня з саундтреку конкретного тайтла
// Дані сюди мають підтягуватись з реального джерела 

public class Song
{
    public string Title { get; private set; }
    public string Artist { get; private set; }
    public string? SpotifyUrl { get; private set; }

    public Song(string title, string artist, string? spotifyUrl = null)
    {
        Title = title;
        Artist = artist;
        SpotifyUrl = spotifyUrl;
    }

    public string GetDisplayName()
    {
        return $"{Title} — {Artist}";
    }
}
