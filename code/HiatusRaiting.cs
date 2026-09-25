using System;

public class HiatusRating	
{
	public Series PausedSeries;
    public int Season, Episode;
    public string Reason;
    public DateTime PauseDate;
    public string Timecode;

    public HiatusRating(Series series, int season, int episode, string reason, DateTime pausedate, string timecode)
    {
        PausedSeries = series;
        Season = season;
        Episode = episode;
        Reason = reason;
        PauseDate = pausedate;
        Timecode = timecode;
    }
    public void ResumeWatching()
    {
        Console.WriteLine($"Серіал '{PausedSeries.Title}' заморожено на S{Season}E{Episode}, таймкод: {Timecode}. Причина: {Reason}");
        Console.WriteLine($"Статус заморозки знято.");
    }
}

