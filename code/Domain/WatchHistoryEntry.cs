using System;

// що дивилась + коли — на відміну від RecentlyViewed (просто останні 10)
public class WatchHistoryEntry
{
    public Media WatchedItem;
    public DateTime WatchDate;

    public WatchHistoryEntry(Media watchedItem, DateTime watchDate)
    {
        WatchedItem = watchedItem;
        WatchDate = watchDate;
    }
}
