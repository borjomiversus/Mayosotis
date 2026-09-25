using System;
using System.Collections.Generic;

public class FranchiseTimeLine
{
	public string UniverseName;
	public List<Media> ChronologicalList;

    
    public FranchiseTimeLine(string universeName)
    {
        UniverseName = universeName;
        ChronologicalList = new List<Media>();
    }

    public void AddToTimeline(Media item)
    {
        ChronologicalList.Add(item);
    }

    public void CalculateUniverseProgress(int watchedCount)
    {
        if (watchedCount < 0 || watchedCount > ChronologicalList.Count)
        {
            Console.WriteLine("Неправильне значення кількості переглянутих елементів.");
            return;
        }
        double progress = (double)watchedCount / ChronologicalList.Count * 100;
        Console.WriteLine($"Ви подивилися {progress:F1}% всесвіту {UniverseName}.");
    }
}
