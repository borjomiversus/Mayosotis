using System;

public class Movie : Media
{
    public int DurationMinutes;

    public Movie(string title, int releaseYear, int durationMinutes, Director director)
        : base(title, releaseYear)
    {
        DurationMinutes = durationMinutes;
        MediaDirector = director;
    }

    // Для фільму борг часу — це просто його тривалість
    public override int CalculateTimeDebt()
    {
        return DurationMinutes;
    }
}