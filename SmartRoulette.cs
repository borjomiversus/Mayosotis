using System;
using System.Collections.Generic;

public class SmartRoulette
{
    public int AvailableTimeMinutes;
    public string MoodTag;

    public SmartRoulette(int availableTimeMinutes, string moodTag)
    {
        AvailableTimeMinutes = availableTimeMinutes;
        MoodTag = moodTag;
    }

    // Той самий статичний поліморфізм (Generics)
    // T — це універсальний тип (може бути int, string тощо)
    public List<Media> FilterList<T>(List<Media> library, T criterion)
    {
        List<Media> filtered = new List<Media>();

        foreach (var item in library)
        {
            // Якщо критерій виявився текстом (шукаємо за тегами або жанрами)
            if (criterion is string tagCriterion)
            {
                if (item.Tags.Contains(tagCriterion) || item.Genres.Contains(tagCriterion))
                {
                    filtered.Add(item);
                }
            }
            // Якщо критерій виявився числом (фільтруємо за наявним часом)
            else if (criterion is int timeCriterion)
            {
                if (item.CalculateTimeDebt() <= timeCriterion)
                {
                    filtered.Add(item);
                }
            }
        }

        return filtered;
    }

    // Метод для випадкового вибору з відфільтрованого списку
    public Media SpinRoulette(List<Media> filteredList)
    {
        if (filteredList.Count == 0)
        {
            Console.WriteLine("На жаль, під ці критерії нічого не знайдено.");
            return null;
        }

        Random random = new Random();
        int index = random.Next(filteredList.Count);
        return filteredList[index];
    }
}