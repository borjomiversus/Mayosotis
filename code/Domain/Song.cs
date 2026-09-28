// Одна пісня з саундтреку конкретного тайтла
// Дані сюди мають підтягуватись з реального джерела TheAudioDB/Tunefind

public class Song
{
    public string Title;
    public string Artist;
    public string SpotifyUrl;

    public Song(string title, string artist, string spotifyUrl = null)
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
