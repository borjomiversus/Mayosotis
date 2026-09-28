using System;

// рядки в Media.ExternalReviews на структуровані відгуки
public class Review
{
    public string Author;
    public DateTime Date;
    public string Text;
    public bool IsSpoiler;

    public Review(string author, string text, bool isSpoiler = false)
    {
        Author = author;
        Text = text;
        Date = DateTime.Now;
        IsSpoiler = isSpoiler;
    }

    // Приховує текст, якщо це спойлер, поки явно не попросили показати
    public string GetDisplayText(bool revealSpoilers = false)
    {
        if (IsSpoiler && !revealSpoilers)
        {
            return $"{Author}: [містить спойлери — приховано]";
        }
        return $"{Author}: {Text}";
    }
}
