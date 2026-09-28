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

    public override int CalculateTimeDebt()
    {
        return DurationMinutes;
    }
}