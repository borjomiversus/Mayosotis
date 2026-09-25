using System.Collections.Generic;

public class Actor : Person
{
    public string RoleName; // Ким був у конкретному фільмі
    public List<string> Filmography; // Список назв фільмів/серіалів, де зіграв

    public Actor(string fullName, int birthYear, string roleName) : base(fullName, birthYear)
    {
        RoleName = roleName;
        Filmography = new List<string>();
    }

    public void AddToFilmography(string title)
    {
        Filmography.Add(title);
    }

    public int GetWorksCount()
    {
        return Filmography.Count;
    }

    public bool HasWorkedOn(string title)
    {
        return Filmography.Contains(title);
    }
}