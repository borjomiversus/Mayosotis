using System;

public class Series : Media
{
    public int TotalSeasons;
    public int AverageEpisodeLength;
    public int UnwatchedEpisodes;
    public HiatusRating PauseStatus;

    public Series(string title, int releaseYear, int averageEpisodeLength, int unwatchedEpisodes)
        : base(title, releaseYear)
    {
        AverageEpisodeLength = averageEpisodeLength;
        UnwatchedEpisodes = unwatchedEpisodes;
    }

    // Для серіалу борг часу — це залишок серій, помножений на їхню тривалість
    public override int CalculateTimeDebt()
    {
        return AverageEpisodeLength * UnwatchedEpisodes;
    }

    // Перевірка, чи переглянуті всі доступні серії
    public bool IsCaughtUp()
    {
        return UnwatchedEpisodes == 0;
    }

    public string GetWatchProgressSummary()
    {
        return $"{TotalSeasons} сезонів, залишилось {UnwatchedEpisodes} серій ({CalculateTimeDebt()} хв)";
    }
}